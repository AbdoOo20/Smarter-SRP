using Avalonia.Controls.Primitives;

namespace Leapay.Client.Controls.VirtualizingWrapPanel;

public abstract class VirtualizingPanelBase : VirtualizingPanel, IScrollable
{
    private Size _extent;
    private Vector _offset;
    private Size _viewport;

    public Size Extent
    {
        get => _extent;
        private set
        {
            if (_extent == value) return;
            _extent = value;
            ScrollInvalidated?.Invoke(this, EventArgs.Empty);
        }
    }

    public Vector Offset
    {
        get => _offset;
        set
        {
            if (_offset == value) return;
            _offset = value;
            InvalidateMeasure();
            ScrollInvalidated?.Invoke(this, EventArgs.Empty);
        }
    }

    public Size Viewport
    {
        get => _viewport;
        private set
        {
            if (_viewport == value) return;
            _viewport = value;
            ScrollInvalidated?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? ScrollInvalidated;

    protected void UpdateScrollInfo(Size viewport, Size extent)
    {
        Viewport = viewport;
        Extent = extent;
        
        var newOffset = Offset;
        var x = Math.Max(0, Math.Min(newOffset.X, extent.Width - viewport.Width));
        var y = Math.Max(0, Math.Min(newOffset.Y, extent.Height - viewport.Height));
        newOffset = new Vector(x, y);
        Offset = newOffset;
    }
}
