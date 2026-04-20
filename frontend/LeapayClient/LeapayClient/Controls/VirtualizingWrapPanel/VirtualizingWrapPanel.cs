/*
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.VisualTree;

namespace Leapay.Client.Controls.VirtualizingWrapPanel
{
    public class VirtualizingWrapPanel : VirtualizingPanelBase
    {
        #region Properties

        public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<VirtualizingWrapPanel, Orientation>(nameof(Orientation), Orientation.Horizontal);
        public static readonly StyledProperty<Size> ItemSizeProperty = AvaloniaProperty.Register<VirtualizingWrapPanel, Size>(nameof(ItemSize));
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

        #endregion

        private static readonly AttachedProperty<object?> RecycleKeyProperty = AvaloniaProperty.RegisterAttached<VirtualizingWrapPanel, Control, object?>("RecycleKey");
        private static readonly object s_itemIsItsOwnContainer = new();
        
        private static readonly Size InfiniteSize = new(double.PositiveInfinity, double.PositiveInfinity);
        private static readonly Size FallbackItemSize = new(48, 48);

        private Dictionary<object, Stack<Control>>? _recyclePool;
        private Size _sizeOfFirstItem;
        private readonly Dictionary<object, Size> _itemSizesCache = new();
        private Size _averageItemSizeCache;

        private int _startItemIndex = -1;
        private int _endItemIndex = -1;
        private double _startItemOffsetX;
        private double _startItemOffsetY;

        static VirtualizingWrapPanel()
        {
            AffectsMeasure<VirtualizingWrapPanel>(OrientationProperty, ItemSizeProperty, AllowDifferentSizedItemsProperty, ItemSizeProviderProperty);
            AffectsArrange<VirtualizingWrapPanel>(ItemAlignmentProperty, SpacingModeProperty, StretchItemsProperty);
        }

        public void ClearItemSizeCache()
        {
            _itemSizesCache.Clear();
            _averageItemSizeCache = default;
        }

        #region Layout Overrides

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Items.Count == 0)
            {
                return default;
            }
            
            _averageItemSizeCache = default;

            RealizeAndVirtualizeItems(availableSize);
            
            foreach (var child in Children)
            {
                child.Measure(InfiniteSize);
            }

            UpdateExtent(availableSize);

            return Extent;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0)
            {
                return finalSize;
            }

            double x = _startItemOffsetX - Offset.X;
            double y = _startItemOffsetY - Offset.Y;
            double rowHeight = 0;
            var rowChildren = new List<Control>();
            var childSizes = new List<Size>();

            foreach (var child in Children)
            {
                var itemIndex = ItemContainerGenerator.IndexFromContainer(child);
                if (itemIndex < 0)
                {
                    continue;
                }
                var item = Items[itemIndex]!;

                Size childSize = GetUpfrontKnownItemSizeOrEmpty(item);
                if (childSize == default)
                {
                    childSize = child.DesiredSize;
                }

                if (rowChildren.Count > 0 && x + GetWidth(childSize) > GetWidth(finalSize))
                {
                    ArrangeLine(GetWidth(finalSize), rowChildren, childSizes, y);
                    x = 0;
                    y += rowHeight;
                    rowHeight = 0;
                    rowChildren.Clear();
                    childSizes.Clear();
                }

                x += GetWidth(childSize);
                rowHeight = Math.Max(rowHeight, GetHeight(childSize));
                rowChildren.Add(child);
                childSizes.Add(childSize);
            }

            if (rowChildren.Any())
            {
                ArrangeLine(GetWidth(finalSize), rowChildren, childSizes, y);
            }

            UpdateScrollInfo(finalSize, Extent);

            return finalSize;
        }

        #endregion

        #region Item Virtualization

        protected override void OnItemsChanged(IReadOnlyList<object?> items, NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(items, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex <= _endItemIndex)
                    {
                        InvalidateMeasure();
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex <= _endItemIndex)
                    {
                        InvalidateMeasure();
                    }
                    if (AllowDifferentSizedItems)
                    {
                        foreach (var item in e.OldItems!)
                        {
                            _itemSizesCache.Remove(item!);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (AllowDifferentSizedItems)
                    {
                        foreach (var item in e.OldItems!)
                        {
                            _itemSizesCache.Remove(item!);
                        }
                    }
                    InvalidateMeasure();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _itemSizesCache.Clear();
                    if (_recyclePool != null)
                    {
                        foreach (var pool in _recyclePool.Values)
                        {
                            foreach (var item in pool)
                            {
                                RemoveInternalChild(item);
                            }
                        }
                        _recyclePool.Clear();
                    }
                    InvalidateMeasure();
                    break;
                default:
                    InvalidateMeasure();
                    break;
            }
        }

        private void RealizeAndVirtualizeItems(Size availableSize)
        {
            FindStartIndexAndOffset(availableSize);
            FindEndIndex(availableSize);

            var children = Children;
            var generator = ItemContainerGenerator;

            for (var i = children.Count - 1; i >= 0; --i)
            {
                var child = children[i];
                var index = generator.IndexFromContainer(child);

                if (index >= 0 && (index < _startItemIndex || index > _endItemIndex))
                {
                    RecycleElement(child);
                }
            }

            for (var i = _startItemIndex; i <= _endItemIndex; ++i)
            {
                var child = generator.ContainerFromIndex(i);
                if (child is null)
                {
                    child = GetOrCreateElement(i);
                }
                child.IsVisible = true;
            }
        }

        private Control GetOrCreateElement(int index)
        {
            var items = Items;
            var item = items[index];
            var generator = ItemContainerGenerator;

            if (generator.NeedsContainer(item, index, out var recycleKey))
            {
                var recycled = GetRecycledElement(item, index, recycleKey);
                if (recycled != null)
                {
                    return recycled;
                }
                return CreateElement(item, index, recycleKey);
            }
            else
            {
                return GetItemAsOwnContainer(item, index);
            }
        }

        private Control GetItemAsOwnContainer(object? item, int index)
        {
            var controlItem = (Control)item!;
            var generator = ItemContainerGenerator;

            if (!controlItem.IsSet(RecycleKeyProperty))
            {
                generator.PrepareItemContainer(controlItem, item, index);
                if (controlItem.GetVisualParent() == null)
                {
                    AddInternalChild(controlItem);
                }
                controlItem.SetValue(RecycleKeyProperty, s_itemIsItsOwnContainer);
                generator.ItemContainerPrepared(controlItem, item, index);
            }

            return controlItem;
        }

        private Control? GetRecycledElement(object? item, int index, object? recycleKey)
        {
            if (recycleKey is null) return null;
            var generator = ItemContainerGenerator;

            if (_recyclePool?.TryGetValue(recycleKey, out var recyclePool) == true && recyclePool.Count > 0)
            {
                var recycled = recyclePool.Pop();
                generator.PrepareItemContainer(recycled, item, index);
                generator.ItemContainerPrepared(recycled, item, index);
                return recycled;
            }

            return null;
        }

        private Control CreateElement(object? item, int index, object? recycleKey)
        {
            var generator = ItemContainerGenerator;
            var container = generator.CreateContainer(item, index, recycleKey);

            container.SetValue(RecycleKeyProperty, recycleKey);
            generator.PrepareItemContainer(container, item, index);
            AddInternalChild(container);
            generator.ItemContainerPrepared(container, item, index);

            return container;
        }

        private void RecycleElement(Control element)
        {
            var recycleKey = element.GetValue(RecycleKeyProperty);
            element.IsVisible = false;

            if (recycleKey is not null && recycleKey != s_itemIsItsOwnContainer)
            {
                ItemContainerGenerator.ClearItemContainer(element);
                PushToRecyclePool(recycleKey, element);
            }
        }

        private void PushToRecyclePool(object recycleKey, Control element)
        {
            _recyclePool ??= new();

            if (!_recyclePool.TryGetValue(recycleKey, out var pool))
            {
                pool = new();
                _recyclePool.Add(recycleKey, pool);
            }

            pool.Push(element);
        }

        #endregion

        #region Layout Calculation

        private Size GetAverageItemSize()
        {
            if (ItemSize != default) return ItemSize;
            if (!AllowDifferentSizedItems) return _sizeOfFirstItem != default ? _sizeOfFirstItem : FallbackItemSize;
            if (_averageItemSizeCache == default) _averageItemSizeCache = CalculateAverageItemSize();
            return _averageItemSizeCache;
        }

        private void FindStartIndexAndOffset(Size availableSize)
        {
            if (availableSize.Width == 0 && availableSize.Height == 0 || Items.Count == 0)
            {
                _startItemIndex = -1;
                _startItemOffsetX = 0;
                _startItemOffsetY = 0;
                return;
            }

            double startOffsetY = Math.Max(GetY(Offset), 0);
            if (startOffsetY <= 0)
            {
                _startItemIndex = 0;
                _startItemOffsetX = 0;
                _startItemOffsetY = 0;
                return;
            }

            if (!AllowDifferentSizedItems && (ItemSize != default || _sizeOfFirstItem != default))
            {
                Size uniformItemSize = ItemSize != default ? ItemSize : _sizeOfFirstItem;
                if (GetHeight(uniformItemSize) <= 0)
                {
                     _startItemIndex = 0;
                     return;
                }
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
                    Size itemSize = GetItemSizeOrAverage(Items[i]!);
                    if (x + GetWidth(itemSize) > GetWidth(availableSize) && x != 0)
                    {
                        x = 0;
                        y += rowHeight;
                        rowHeight = 0;
                        indexOfFirstRowItem = i;
                    }
                    x += GetWidth(itemSize);
                    rowHeight = Math.Max(rowHeight, GetHeight(itemSize));
                    if (y + rowHeight > startOffsetY)
                    {
                        _startItemIndex = indexOfFirstRowItem;
                        _startItemOffsetX = 0;
                        _startItemOffsetY = y;
                        break;
                    }
                }
                if (_startItemIndex == -1)
                {
                    _startItemIndex = Items.Count > 0 ? 0 : -1;
                }
            }
        }

        private void FindEndIndex(Size availableSize)
        {
            if (_startItemIndex < 0)
            {
                _endItemIndex = -1;
                return;
            }
            
            double endOffsetY = Math.Max(GetY(Offset), 0) + GetHeight(availableSize);

            if (!AllowDifferentSizedItems && (ItemSize != default || _sizeOfFirstItem != default))
            {
                Size uniformItemSize = ItemSize != default ? ItemSize : _sizeOfFirstItem;
                if (GetHeight(uniformItemSize) <= 0)
                {
                    _endItemIndex = Items.Count - 1;
                    return;
                }
                int itemsPerRow = CalculateItemsPerRow(uniformItemSize, availableSize.Width);
                int rows = (int)Math.Ceiling(endOffsetY / GetHeight(uniformItemSize));
                _endItemIndex = Math.Min((itemsPerRow * rows) - 1, Items.Count - 1);
            }
            else
            {
                double x = _startItemOffsetX, y = _startItemOffsetY, rowHeight = 0;
                _endItemIndex = Items.Count - 1;

                for (int i = _startItemIndex; i < Items.Count; i++)
                {
                    var item = Items[i]!;
                    var containerSize = GetItemSizeOrAverage(item);

                    if (x != 0 && x + GetWidth(containerSize) > GetWidth(availableSize))
                    {
                        x = 0;
                        y += rowHeight;
                        rowHeight = 0;
                    }

                    x += GetWidth(containerSize);
                    rowHeight = Math.Max(rowHeight, GetHeight(containerSize));

                    if (y > endOffsetY)
                    {
                        _endItemIndex = i;
                        break;
                    }
                }
            }
        }

        private void UpdateExtent(Size availableSize)
        {
            Size extent;
            if (Items.Count == 0)
            {
                extent = default;
            }
            else if (!AllowDifferentSizedItems)
            {
                extent = CalculateExtentForUniformSizedItems(availableSize);
            }
            else
            {
                extent = CalculateExtentForDifferentSizedItems(availableSize);
            }
            UpdateScrollInfo(availableSize, extent);
        }

        private Size CalculateExtentForUniformSizedItems(Size availableSize)
        {
            var uniformItemSize = ItemSize != default ? ItemSize : _sizeOfFirstItem;
            if (uniformItemSize == default)
            {
                var firstChild = Children.FirstOrDefault();
                if (firstChild != null)
                {
                    uniformItemSize = _sizeOfFirstItem = firstChild.DesiredSize;
                }
                else
                {
                    return availableSize;
                }
            }
            
            int itemsPerRow = CalculateItemsPerRow(uniformItemSize, availableSize.Width);
            double extentY = Math.Ceiling((double)Items.Count / itemsPerRow) * GetHeight(uniformItemSize);
            return CreateSize(availableSize.Width, extentY);
        }

        private Size CalculateExtentForDifferentSizedItems(Size availableSize)
        {
            double y = 0, x = 0, rowHeight = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                var itemSize = GetItemSizeOrAverage(Items[i]!);
                if (x + itemSize.Width > availableSize.Width)
                {
                    y += rowHeight;
                    x = 0;
                    rowHeight = 0;
                }
                x += itemSize.Width;
                rowHeight = Math.Max(rowHeight, itemSize.Height);
            }
            y += rowHeight;
            return CreateSize(availableSize.Width, y);
        }

        private Size GetUpfrontKnownItemSizeOrEmpty(object item)
        {
            if (ItemSize != default) return ItemSize;
            if (!AllowDifferentSizedItems && _sizeOfFirstItem != default) return _sizeOfFirstItem;
            return ItemSizeProvider?.GetSizeForItem(item) ?? default;
        }

        private Size GetItemSizeOrAverage(object item)
        {
            var size = ItemSizeProvider?.GetSizeForItem(item);
            if (size.HasValue) return size.Value;
            if (_itemSizesCache.TryGetValue(item, out Size cachedItemSize)) return cachedItemSize;
            return GetAverageItemSize();
        }

        private void ArrangeLine(double rowWidth, List<Control> children, List<Size> childSizes, double linePositionCrossAxis)
        {
            double summedUpChildWidth, extraWidth = 0;
            if (AllowDifferentSizedItems)
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
                    var firstChild = children[0];
                    double maxWidth = Orientation == Orientation.Horizontal ? firstChild.MaxWidth : firstChild.MaxHeight;
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

        private double GetArrangePositionCrossAxis(Control child, double childArrangeCrossAxisSize, double linePositionCrossAxis, double lineSizeCrossAxis)
        {
            return ItemAlignment switch
            {
                ItemAlignment.End => linePositionCrossAxis + (lineSizeCrossAxis - childArrangeCrossAxisSize),
                ItemAlignment.Center => linePositionCrossAxis + (lineSizeCrossAxis - childArrangeCrossAxisSize) / 2,
                _ => linePositionCrossAxis,
            };
        }

        private double GetArrangeSizeCrossAxis(Control child, Size childSize, double lineSizeCrossAxis)
        {
            if (ItemAlignment == ItemAlignment.Stretch)
            {
                if (Orientation == Orientation.Horizontal && double.IsNaN(child.Height)) return Math.Min(lineSizeCrossAxis, child.MaxHeight);
                if (Orientation == Orientation.Vertical && double.IsNaN(child.Width)) return Math.Min(lineSizeCrossAxis, child.MaxWidth);
            }
            return GetHeight(childSize);
        }

        private void CalculateRowSpacing(double rowWidth, List<Control> children, double summedUpChildWidth, out double innerSpacing, out double outerSpacing)
        {
            int childCount = AllowDifferentSizedItems ? children.Count : (int)Math.Max(1, Math.Floor(rowWidth / GetWidth(_sizeOfFirstItem)));
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
            if (_itemSizesCache.Any()) return new Size(Math.Round(_itemSizesCache.Values.Average(s => s.Width)), Math.Round(_itemSizesCache.Values.Average(s => s.Height)));
            return FallbackItemSize;
        }

        private int CalculateItemsPerRow(Size uniformItemSize, double availableWidth)
        {
            return uniformItemSize.Width > 0 ? Math.Max(1, (int)Math.Floor(GetWidth(new Size(availableWidth, 0)) / GetWidth(uniformItemSize))) : 1;
        }

        #endregion

        #region Orientation Helpers

        private double GetX(Vector vector) => Orientation == Orientation.Horizontal ? vector.X : vector.Y;
        private double GetY(Vector vector) => Orientation == Orientation.Horizontal ? vector.Y : vector.X;
        private double GetWidth(Size size) => Orientation == Orientation.Horizontal ? size.Width : size.Height;
        private double GetHeight(Size size) => Orientation == Orientation.Horizontal ? size.Height : size.Width;
        private Point CreatePoint(double x, double y) => Orientation == Orientation.Horizontal ? new Point(x, y) : new Point(y, x);
        private Size CreateSize(double width, double height) => Orientation == Orientation.Horizontal ? new Size(width, height) : new Size(height, width);
        private Rect CreateOrientedRect(double x, double y, double width, double height) => Orientation == Orientation.Horizontal ? new Rect(x, y, width, height) : new Rect(y, x, height, width);
        
        #endregion

        #region Abstract Member Implementation

        public override Control? ScrollIntoView(int index)
        {
            if (index < 0 || index >= Items.Count)
            {
                return null;
            }

            var offset = FindItemOffset(index);
            var point = new Point(offset.X, offset.Y);
            var rect = new Rect(point, ItemSize != default ? ItemSize : GetAverageItemSize());
            
            if (rect.Y < Offset.Y)
            {
                Offset = new Vector(Offset.X, rect.Y);
            }
            else if (rect.Bottom > Offset.Y + Viewport.Height)
            {
                Offset = new Vector(Offset.X, rect.Bottom - Viewport.Height);
            }

            return ContainerFromIndex(index);
        }

        public override Control? ContainerFromIndex(int index)
        {
            return ItemContainerGenerator.ContainerFromIndex(index);
        }

        public override int IndexFromContainer(Control container)
        {
            return ItemContainerGenerator.IndexFromContainer(container);
        }

        public override IEnumerable<Control>? GetRealizedContainers()
        {
            return Children;
        }

        public override IInputElement? GetControl(NavigationDirection direction, IInputElement? from, bool wrap)
        {
            int index = from is Control fromControl ? IndexFromContainer(fromControl) : -1;

            switch (direction)
            {
                case NavigationDirection.First: index = 0; break;
                case NavigationDirection.Last: index = Items.Count - 1; break;
                case NavigationDirection.Next: index++; break;
                case NavigationDirection.Previous: index--; break;
                case NavigationDirection.Left: index--; break;
                case NavigationDirection.Right: index++; break;
                case NavigationDirection.Up: index -= CalculateItemsPerRow(GetAverageItemSize(), Viewport.Width); break;
                case NavigationDirection.Down: index += CalculateItemsPerRow(GetAverageItemSize(), Viewport.Width); break;
                default: break;
            }

            if (wrap)
            {
                if (index < 0) index = Items.Count - 1;
                if (index >= Items.Count) index = 0;
            }
            else
            {
                if (index < 0) index = 0;
                if (index >= Items.Count) index = Items.Count - 1;
            }

            return ContainerFromIndex(index);
        }

        #endregion
    }
}
*/
