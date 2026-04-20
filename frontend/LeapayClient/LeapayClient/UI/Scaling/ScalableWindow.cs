using Avalonia.Input;
using SukiUI.Controls;

namespace Leapay.Client.UI.Scaling
{
    /// <summary>
    /// Represents an avalonia <see cref="Window"/> that can be resized and scales it's content accordingly.
    /// </summary>
    public abstract class ScalableWindow : SukiWindow
    {
        public abstract event EventHandler<PointerPressedEventArgs> BeginResize;
        public abstract event EventHandler<PointerEventArgs> Resize;
        public abstract event EventHandler<PointerReleasedEventArgs> EndResize;
    }
}
