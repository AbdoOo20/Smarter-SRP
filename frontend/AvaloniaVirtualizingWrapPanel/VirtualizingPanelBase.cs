using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Avalonia.VirtualizingWrapPanel;

public abstract class VirtualizingPanelBase : VirtualizingPanel, IScrollable
{
    public static readonly StyledProperty<double> ScrollLineDeltaProperty = AvaloniaProperty.Register<VirtualizingPanelBase, double>(nameof(ScrollLineDelta), 16.0);
    public static readonly StyledProperty<double> MouseWheelDeltaProperty = AvaloniaProperty.Register<VirtualizingPanelBase, double>(nameof(MouseWheelDelta), 48.0);
    public static readonly StyledProperty<int> ScrollLineDeltaItemProperty = AvaloniaProperty.Register<VirtualizingPanelBase, int>(nameof(ScrollLineDeltaItem), 1);
    public static readonly StyledProperty<int> MouseWheelDeltaItemProperty = AvaloniaProperty.Register<VirtualizingPanelBase, int>(nameof(MouseWheelDeltaItem), 3);

    private Size _extent;
    private Vector _offset;
    private Size _viewport;

    public double ScrollLineDelta { get => GetValue(ScrollLineDeltaProperty); set => SetValue(ScrollLineDeltaProperty, value); }
    public double MouseWheelDelta { get => GetValue(MouseWheelDeltaProperty); set => SetValue(MouseWheelDeltaProperty, value); }
    public int ScrollLineDeltaItem { get => GetValue(ScrollLineDeltaItemProperty); set => SetValue(ScrollLineDeltaItemProperty, value); }
    public int MouseWheelDeltaItem { get => GetValue(MouseWheelDeltaItemProperty); set => SetValue(MouseWheelDeltaItemProperty, value); }

    protected ScrollDirection MouseWheelScrollDirection { get; set; } = ScrollDirection.Vertical;

    protected ItemsControl ItemsControl => ItemsControl.GetItemsOwner(this);

    protected IControl ItemsOwner => this.GetVisualAncestors().OfType<ItemsPresenter>().FirstOrDefault() ?? (IControl)ItemsControl;

    protected ReadOnlyCollection<object> Items => ItemContainerGenerator.Items;

    public Size Extent => _extent;
    public Size Viewport => _viewport;
    public Vector Offset
    {
        get => _offset;
        set
        {
            if (_offset != value)
            {
                _offset = value;
                InvalidateMeasure();
            }
        }
    }

    public event EventHandler? ScrollInvalidated;

    protected void UpdateScrollInfo(Size viewport, Size extent)
    {
        var oldViewport = _viewport;
        var oldExtent = _extent;
        var oldOffset = _offset;

        _viewport = viewport;
        _extent = extent;

        var newOffset = _offset;
        var x = Math.Max(0, Math.Min(newOffset.X, extent.Width - viewport.Width));
        var y = Math.Max(0, Math.Min(newOffset.Y, extent.Height - viewport.Height));
        newOffset = new Vector(x, y);
        _offset = newOffset;

        if (oldViewport != _viewport || oldExtent != _extent || oldOffset != _offset)
        {
            ScrollInvalidated?.Invoke(this, EventArgs.Empty);
        }
    }

    protected void VerifyItemsControl()
    {
        if (!GetIsVirtualizing(ItemsControl))
        {
            throw new InvalidOperationException("VirtualizingPanel.IsVirtualizing must be true.");
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var scrollViewer = this.GetVisualAncestors().OfType<ScrollViewer>().FirstOrDefault();
        if (scrollViewer != null)
        {
            scrollViewer.ScrollChanged += OnScrollChanged;
        }
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (sender is ScrollViewer scrollViewer)
        {
            Offset = scrollViewer.Offset;
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        var scrollViewer = this.GetVisualAncestors().OfType<ScrollViewer>().FirstOrDefault();
        if (scrollViewer != null)
        {
            scrollViewer.ScrollChanged -= OnScrollChanged;
        }
    }
}
