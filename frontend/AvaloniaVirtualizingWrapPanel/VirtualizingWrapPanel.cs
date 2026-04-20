using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Layout;

namespace Avalonia.VirtualizingWrapPanel
{
    public class VirtualizingWrapPanel : VirtualizingPanelBase
    {
        public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<VirtualizingWrapPanel, Orientation>(nameof(Orientation), Orientation.Horizontal);
        public static readonly StyledProperty<Size> ItemSizeProperty = AvaloniaProperty.Register<VirtualizingWrapPanel, Size>(nameof(ItemSize), Size.Empty);
        public static readonly StyledProperty<bool> AllowDifferentSizedItemsProperty = AvaloniaProperty.Register<VirtualizingWrapPanel, bool>(nameof(AllowDifferentSizedItems));
        public static readonly StyledProperty<IItemSizeProvider?> ItemSizeProviderProperty = AvaloniaProperty.Register<VirtualizingWrapPanel, IItemSizeProvider?>(nameof(ItemSizeProvider));
        public static readonly StyledProperty<ItemAlignment> ItemAlignmentProperty = AvaloniaProperty.Register<VirtualizingWrapPanel, ItemAlignment>(nameof(ItemAlignment), ItemAlignment.Start);
        public static readonly StyledProperty<SpacingMode> SpacingModeProperty = AvaloniaProperty.Register<VirtualizingWrapPanel, SpacingMode>(nameof(SpacingMode), SpacingMode.Uniform);
        public static readonly StyledProperty<bool> StretchItemsProperty = AvaloniaProperty.Register<VirtualizingWrapPanel, bool>(nameof(StretchItems));

        public Orientation Orientation { get => GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }
        public Size ItemSize { get => GetValue(ItemSizeProperty); set => SetValue(ItemSizeProperty, value); }
        public bool AllowDifferentSizedItems { get => GetValue(AllowDifferentSizedItemsProperty); set => SetValue(AllowDifferentSizedItemsProperty, value); }
        public IItemSizeProvider? ItemSizeProvider { get => GetValue(ItemSizeProviderProperty); set => SetValue(ItemSizeProviderProperty, value); }
        public ItemAlignment ItemAlignment { get => GetValue(ItemAlignmentProperty); set => SetValue(ItemAlignmentProperty, value); }
        public SpacingMode SpacingMode { get => GetValue(SpacingModeProperty); set => SetValue(SpacingModeProperty, value); }
        public bool StretchItems { get => GetValue(StretchItemsProperty); set => SetValue(StretchItemsProperty, value); }

        private static readonly Size InfiniteSize = new(double.PositiveInfinity, double.PositiveInfinity);
        private static readonly Size FallbackItemSize = new(48, 48);

        private ItemContainerManager? _itemContainerManager;
        private ItemContainerManager ItemContainerManager => _itemContainerManager ??= new ItemContainerManager(ItemContainerGenerator);

        private Size _sizeOfFirstItem = Size.Empty;
        private readonly Dictionary<object, Size> _itemSizesCache = new();
        private Size _averageItemSizeCache = Size.Empty;

        private int _startItemIndex = -1;
        private int _endItemIndex = -1;
        private double _startItemOffsetX;
        private double _startItemOffsetY;
        private double _knownExtendX;
        private double _knownExtendY;
        private int _bringIntoViewItemIndex = -1;
        private IControl? _bringIntoViewContainer;

        private ReadOnlyCollection<object> _items = new(Array.Empty<object>());
        private Orientation _orientation = Orientation.Horizontal;
        private Size _itemSize = Size.Empty;
        private bool _allowDifferentSizedItems;
        private IItemSizeProvider? _itemSizeProvider;

        static VirtualizingWrapPanel()
        {
            AffectsMeasure<VirtualizingWrapPanel>(OrientationProperty, ItemSizeProperty, AllowDifferentSizedItemsProperty, ItemSizeProviderProperty);
            AffectsArrange<VirtualizingWrapPanel>(ItemAlignmentProperty, SpacingModeProperty, StretchItemsProperty);
        }

        public void ClearItemSizeCache()
        {
            _itemSizesCache.Clear();
            _averageItemSizeCache = Size.Empty;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            CacheDependencyProperties();
            VerifyItemsControl();

            ItemContainerManager.IsRecycling = IsVirtualizing;

            MeasureBringIntoViewContainer(InfiniteSize);

            _averageItemSizeCache = Size.Empty;

            RealizeAndVirtualizeItems(availableSize);
            UpdateExtent(availableSize);

            return Extent;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            ArrangeBringIntoViewContainer();

            if (_startItemIndex == -1)
            {
                return finalSize;
            }

            double x = _startItemOffsetX - Offset.X;
            double y = _startItemOffsetY - Offset.Y;
            double rowHeight = 0;
            var rowChilds = new List<IControl>();
            var childSizes = new List<Size>();

            for (int i = _startItemIndex; i <= _endItemIndex; i++)
            {
                var item = Items[i];
                var child = ItemContainerManager.RealizedContainers[item];

                Size childSize = GetUpfrontKnownItemSizeOrEmpty(item);
                if (childSize.IsEmpty)
                {
                    childSize = child.DesiredSize;
                }

                if (rowChilds.Count > 0 && x + GetWidth(childSize) > GetWidth(finalSize))
                {
                    ArrangeLine(GetWidth(finalSize), rowChilds, childSizes, y);
                    x = 0;
                    y += rowHeight;
                    rowHeight = 0;
                    rowChilds.Clear();
                    childSizes.Clear();
                }

                x += GetWidth(childSize);
                rowHeight = Math.Max(rowHeight, GetHeight(childSize));
                rowChilds.Add(child);
                childSizes.Add(childSize);
            }

            if (rowChilds.Any())
            {
                ArrangeLine(GetWidth(finalSize), rowChilds, childSizes, y);
            }

            UpdateScrollInfo(finalSize, Extent);

            return finalSize;
        }

        protected override void OnItemsChanged(IReadOnlyList<object> items, NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(items, e);

            if (_bringIntoViewItemIndex >= Items.Count)
            {
                _bringIntoViewItemIndex = -1;
                _bringIntoViewContainer = null;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove or NotifyCollectionChangedAction.Replace:
                    if (AllowDifferentSizedItems)
                    {
                        foreach (var key in _itemSizesCache.Keys.Except(Items).ToList())
                        {
                            _itemSizesCache.Remove(key);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _itemSizesCache.Clear();
                    if (AllowDifferentSizedItems && ItemSizeProvider is null)
                    {
                        Offset = new Vector(0, 0);
                    }
                    break;
            }
            InvalidateMeasure();
        }

        private void MeasureBringIntoViewContainer(Size availableSize)
        {
            if (_bringIntoViewContainer is not null && !_bringIntoViewContainer.IsMeasureValid)
            {
                var upfrontKnownItemSize = GetUpfrontKnownItemSizeOrEmpty(Items[_bringIntoViewItemIndex]);
                _bringIntoViewContainer.Measure(!upfrontKnownItemSize.IsEmpty ? upfrontKnownItemSize : availableSize);

                if (!_allowDifferentSizedItems && _sizeOfFirstItem.IsEmpty)
                {
                    _sizeOfFirstItem = _bringIntoViewContainer.DesiredSize;
                }
            }
        }

        private void ArrangeBringIntoViewContainer()
        {
            if (_bringIntoViewContainer is not null)
            {
                var offset = FindItemOffset(_bringIntoViewItemIndex);
                offset = new Point(offset.X - Offset.X, offset.Y - Offset.Y);
                var upfrontKnownItemSize = GetUpfrontKnownItemSizeOrEmpty(Items[_bringIntoViewItemIndex]);
                var size = !upfrontKnownItemSize.IsEmpty ? upfrontKnownItemSize : _bringIntoViewContainer.DesiredSize;
                _bringIntoViewContainer.Arrange(new Rect(offset, size));
            }
        }

        private void RealizeAndVirtualizeItems(Size availableSize)
        {
            FindStartIndexAndOffset(availableSize);
            FindEndIndex(availableSize);
            VirtualizeItems();
            RealizeItems(availableSize);
            VirtualizeItems();
        }

        private Size GetAverageItemSize()
        {
            if (!_itemSize.IsEmpty) return _itemSize;
            if (!_allowDifferentSizedItems) return !_sizeOfFirstItem.IsEmpty ? _sizeOfFirstItem : FallbackItemSize;
            if (_averageItemSizeCache.IsEmpty) _averageItemSizeCache = CalculateAverageItemSize();
            return _averageItemSizeCache;
        }

        private Point FindItemOffset(int itemIndex)
        {
            if (!_allowDifferentSizedItems && (!_itemSize.IsEmpty || !_sizeOfFirstItem.IsEmpty))
            {
                Size uniformItemSize = !_itemSize.IsEmpty ? _itemSize : _sizeOfFirstItem;
                var itemsPerRow = CalculateItemsPerRow(uniformItemSize, Viewport.Width);
                int rowIndex = itemIndex / itemsPerRow;
                int itemIndexInRow = itemIndex % itemsPerRow;
                double x = itemIndexInRow * GetWidth(uniformItemSize);
                double y = rowIndex * GetHeight(uniformItemSize);
                return CreatePoint(x, y);
            }
            else
            {
                double x = 0, y = 0, rowHeight = 0;
                for (int i = 0; i <= itemIndex; i++)
                {
                    Size itemSize = GetItemSizeOrAverage(Items[i]);
                    if (x != 0 && x + GetWidth(itemSize) > GetWidth(Viewport))
                    {
                        x = 0;
                        y += rowHeight;
                        rowHeight = 0;
                    }
                    if (i != itemIndex) { x += GetWidth(itemSize); rowHeight = Math.Max(rowHeight, GetHeight(itemSize)); }
                }
                return CreatePoint(x, y);
            }
        }

        private void FindStartIndexAndOffset(Size availableSize)
        {
            if (availableSize.Width == 0 && availableSize.Height == 0) { _startItemIndex = -1; _startItemOffsetX = 0; _startItemOffsetY = 0; return; }

            double startOffsetY = Math.Max(GetY(Offset), 0);
            if (startOffsetY <= 0) { _startItemIndex = Items.Count > 0 ? 0 : -1; _startItemOffsetX = 0; _startItemOffsetY = 0; return; }

            if (!_allowDifferentSizedItems && (!_itemSize.IsEmpty || !_sizeOfFirstItem.IsEmpty))
            {
                Size uniformItemSize = !_itemSize.IsEmpty ? _itemSize : _sizeOfFirstItem;
                int itemsPerRow = CalculateItemsPerRow(uniformItemSize, availableSize.Width);
                int offsetInRows = (int)Math.Floor(startOffsetY / GetHeight(uniformItemSize));
                _startItemIndex = Math.Min(offsetInRows * itemsPerRow, Items.Count - 1);
                _startItemOffsetX = 0;
                _startItemOffsetY = offsetInRows * GetHeight(uniformItemSize);
            }
            else
            {
                _startItemIndex = -1;
                double x = 0, y = 0, rowHeight = 0;
                int indexOfFirstRowItem = 0;
                for (int i = 0; i < Items.Count; i++)
                {
                    Size itemSize = GetItemSizeOrAverage(Items[i]);
                    if (x + GetWidth(itemSize) > GetWidth(availableSize) && x != 0)
                    {
                        x = 0; y += rowHeight; rowHeight = 0; indexOfFirstRowItem = i;
                    }
                    x += GetWidth(itemSize);
                    rowHeight = Math.Max(rowHeight, GetHeight(itemSize));
                    if (y + rowHeight > startOffsetY) { _startItemIndex = indexOfFirstRowItem; _startItemOffsetX = 0; _startItemOffsetY = y; break; }
                }
                if (_startItemIndex == -1 && Items.Count > 0) { _startItemIndex = Items.Count - 1; _startItemOffsetX = x; _startItemOffsetY = y; }
            }
        }

        private void FindEndIndex(Size availableSize)
        {
            if (!_allowDifferentSizedItems && (!_itemSize.IsEmpty || !_sizeOfFirstItem.IsEmpty))
            {
                Size uniformItemSize = !_itemSize.IsEmpty ? _itemSize : _sizeOfFirstItem;
                double endOffsetY = Math.Max(GetY(Offset), 0) + GetHeight(availableSize);
                int itemsPerRow = CalculateItemsPerRow(uniformItemSize, availableSize.Width);
                int rows = (int)Math.Ceiling(endOffsetY / GetHeight(uniformItemSize));
                _endItemIndex = Math.Min((itemsPerRow * rows) - 1, Items.Count - 1);
            }
        }

        private void RealizeItems(Size availableSize)
        {
            if (_startItemIndex == -1) { _endItemIndex = -1; _knownExtendX = 0; return; }

            int newEndItemIndex = Items.Count - 1;
            bool endItemIndexFound = false;
            double endOffsetY = Math.Max(GetY(Offset), 0) + GetHeight(availableSize);
            double x = _startItemOffsetX, y = _startItemOffsetY, rowHeight = 0;
            _knownExtendX = 0;

            for (int i = _startItemIndex; i <= newEndItemIndex; i++)
            {
                if (i == 0) _sizeOfFirstItem = Size.Empty;
                var item = Items[i];
                var container = ItemContainerManager.Realize(i);
                if (container == _bringIntoViewContainer) { _bringIntoViewItemIndex = -1; _bringIntoViewContainer = null; }

                Size upfrontKnownItemSize = GetUpfrontKnownItemSizeOrEmpty(item);
                container.Measure(!upfrontKnownItemSize.IsEmpty ? upfrontKnownItemSize : InfiniteSize);
                var containerSize = DetermineContainerSize(item, container, upfrontKnownItemSize);

                if (!_allowDifferentSizedItems && _sizeOfFirstItem.IsEmpty) _sizeOfFirstItem = containerSize;

                if (x != 0 && x + GetWidth(containerSize) > GetWidth(availableSize))
                {
                    x = 0; y += rowHeight; rowHeight = 0;
                }

                x += GetWidth(containerSize);
                _knownExtendX = Math.Max(x, _knownExtendX);
                rowHeight = Math.Max(rowHeight, GetHeight(containerSize));

                if (!endItemIndexFound && y >= endOffsetY) { endItemIndexFound = true; newEndItemIndex = i; }

                _knownExtendY = y + rowHeight;
            }
            _endItemIndex = newEndItemIndex;
        }

        private Size DetermineContainerSize(object item, IControl container, Size upfrontKnownItemSize)
        {
            Size containerSize = !upfrontKnownItemSize.IsEmpty ? upfrontKnownItemSize : container.DesiredSize;
            if (_allowDifferentSizedItems) _itemSizesCache[item] = containerSize;
            return containerSize;
        }

        private void VirtualizeItems()
        {
            var itemsToBeRealized = Utils.HashSetOfRange(Items, _startItemIndex, _endItemIndex);
            foreach (var (item, container) in ItemContainerManager.RealizedContainers.ToList())
            {
                if (container != _bringIntoViewContainer && !itemsToBeRealized.Contains(item))
                {
                    ItemContainerManager.Virtualize(item);
                }
            }
        }

        private void UpdateExtent(Size availableSize)
        {
            Size extent;
            if (_startItemIndex == -1) extent = new Size(0, 0);
            else if (!_allowDifferentSizedItems) extent = CalculateExtentForUniformSizedItems(availableSize);
            else extent = CalculateExtentForDifferentSizedItems();
            UpdateScrollInfo(availableSize, extent);
        }

        private Size CalculateExtentForUniformSizedItems(Size availableSize)
        {
            var uniformItemSize = !_itemSize.IsEmpty ? _itemSize : _sizeOfFirstItem;
            int itemsPerRow = CalculateItemsPerRow(uniformItemSize, availableSize.Width);
            double extentY = Math.Ceiling((double)Items.Count / itemsPerRow) * GetHeight(uniformItemSize);
            return CreateSize(_knownExtendX, extentY);
        }

        private Size CalculateExtentForDifferentSizedItems()
        {
            double itemsUntilEndCount = Items.Count - (_endItemIndex + 1);
            double extendPerItem = _knownExtendY / (_endItemIndex + 1);
            double estimatedExtendY = _knownExtendY + itemsUntilEndCount * extendPerItem;
            return CreateSize(_knownExtendX, estimatedExtendY);
        }

        private Size GetUpfrontKnownItemSizeOrEmpty(object item)
        {
            if (!_itemSize.IsEmpty) return _itemSize;
            if (!_allowDifferentSizedItems && !_sizeOfFirstItem.IsEmpty) return _sizeOfFirstItem;
            return ItemSizeProvider?.GetSizeForItem(item) ?? Size.Empty;
        }

        private Size GetItemSizeOrAverage(object item)
        {
            var size = ItemSizeProvider?.GetSizeForItem(item);
            if (size.HasValue) return size.Value;
            if (_itemSizesCache.TryGetValue(item, out Size cachedItemSize)) return cachedItemSize;
            return GetAverageItemSize();
        }

        private void ArrangeLine(double rowWidth, List<IControl> children, List<Size> childSizes, double linePositionCrossAxis)
        {
            double summedUpChildWidth, extraWidth = 0;
            if (_allowDifferentSizedItems)
            {
                summedUpChildWidth = childSizes.Sum(GetWidth);
                if (StretchItems) { double unusedWidth = rowWidth - summedUpChildWidth; extraWidth = unusedWidth / children.Count; summedUpChildWidth = rowWidth; }
            }
            else
            {
                double childWidth = GetWidth(childSizes[0]);
                int itemsPerRow = Math.Max(1, (int)Math.Floor(rowWidth / childWidth));
                if (StretchItems)
                {
                    var firstChild = (Layoutable)children[0];
                    double maxWidth = _orientation == Orientation.Horizontal ? firstChild.MaxWidth : firstChild.MaxHeight;
                    double stretchedChildWidth = Math.Min(rowWidth / itemsPerRow, maxWidth);
                    stretchedChildWidth = Math.Max(stretchedChildWidth, childWidth);
                    extraWidth = stretchedChildWidth - childWidth;
                    summedUpChildWidth = itemsPerRow * stretchedChildWidth;
                }
                else { summedUpChildWidth = itemsPerRow * childWidth; }
            }

            double innerSpacing = 0, outerSpacing = 0;
            if (summedUpChildWidth < rowWidth) CalculateRowSpacing(rowWidth, children, summedUpChildWidth, out innerSpacing, out outerSpacing);

            double positionMainAxis = outerSpacing;
            double lineSizeCrossAxis = children.Select((t, i) => GetHeight(childSizes[i])).Max();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                Size childSize = childSizes[i];
                double mainAxisSize = GetWidth(childSize) + extraWidth;
                double crossAxisSize = GetArrangeSizeCrossAxis(child, childSize, lineSizeCrossAxis);
                double positionCrossAxis = GetArrangePositionCrossAxis(child, crossAxisSize, linePositionCrossAxis, lineSizeCrossAxis);
                child.Arrange(CreateOrientedRect(positionMainAxis, positionCrossAxis, mainAxisSize, crossAxisSize));
                positionMainAxis += GetWidth(childSize) + extraWidth + innerSpacing;
            }
        }

        private double GetArrangePositionCrossAxis(IControl child, double childArrangeCrossAxisSize, double linePositionCrossAxis, double lineSizeCrossAxis)
        {
            if (ItemAlignment == ItemAlignment.End) return linePositionCrossAxis + (lineSizeCrossAxis - childArrangeCrossAxisSize);
            if (ItemAlignment == ItemAlignment.Center) return linePositionCrossAxis + (lineSizeCrossAxis - childArrangeCrossAxisSize) / 2;
            return linePositionCrossAxis;
        }

        private double GetArrangeSizeCrossAxis(IControl child, Size childSize, double lineSizeCrossAxis)
        {
            if (ItemAlignment == ItemAlignment.Stretch && child is Layoutable fe)
            {
                if (_orientation == Orientation.Horizontal && double.IsNaN(fe.Height)) return Math.Min(lineSizeCrossAxis, fe.MaxHeight);
                if (_orientation == Orientation.Vertical && double.IsNaN(fe.Width)) return Math.Min(lineSizeCrossAxis, fe.MaxWidth);
            }
            return GetHeight(childSize);
        }

        private void CalculateRowSpacing(double rowWidth, List<IControl> children, double summedUpChildWidth, out double innerSpacing, out double outerSpacing)
        {
            int childCount = _allowDifferentSizedItems ? children.Count : (int)Math.Max(1, Math.Floor(rowWidth / GetWidth(_sizeOfFirstItem)));
            double unusedWidth = Math.Max(0, rowWidth - summedUpChildWidth);
            switch (SpacingMode)
            {
                case SpacingMode.Uniform: innerSpacing = outerSpacing = unusedWidth / (childCount + 1); break;
                case SpacingMode.BetweenItemsOnly: innerSpacing = unusedWidth / Math.Max(childCount - 1, 1); outerSpacing = 0; break;
                case SpacingMode.StartAndEndOnly: innerSpacing = 0; outerSpacing = unusedWidth / 2; break;
                default: innerSpacing = 0; outerSpacing = 0; break;
            }
        }

        private Size CalculateAverageItemSize()
        {
            if (_itemSizesCache.Values.Count > 0) return new Size(Math.Round(_itemSizesCache.Values.Average(s => s.Width)), Math.Round(_itemSizesCache.Values.Average(s => s.Height)));
            return FallbackItemSize;
        }

        private int CalculateItemsPerRow(Size uniformItemSize, double availableWidth)
        {
            return Math.Max(1, (int)Math.Floor(GetWidth(new Size(availableWidth, 0)) / GetWidth(uniformItemSize)));
        }

        private void CacheDependencyProperties()
        {
            _items = Items; _orientation = Orientation; _itemSize = ItemSize; _allowDifferentSizedItems = AllowDifferentSizedItems; _itemSizeProvider = ItemSizeProvider;
        }

        private double GetX(Point point) => _orientation == Orientation.Horizontal ? point.X : point.Y;
        private double GetY(Point point) => _orientation == Orientation.Horizontal ? point.Y : point.X;
        private double GetWidth(Size size) => _orientation == Orientation.Horizontal ? size.Width : size.Height;
        private double GetHeight(Size size) => _orientation == Orientation.Horizontal ? size.Height : size.Width;
        private Point CreatePoint(double x, double y) => _orientation == Orientation.Horizontal ? new Point(x, y) : new Point(y, x);
        private Size CreateSize(double width, double height) => _orientation == Orientation.Horizontal ? new Size(width, height) : new Size(height, width);
        private Rect CreateOrientedRect(double x, double y, double width, double height) => _orientation == Orientation.Horizontal ? new Rect(x, y, width, height) : new Rect(y, x, height, width);
    }
}
