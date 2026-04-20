using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Styling;

namespace Leapay.Client.Helpers;

public class SmoothAnimation
{
    public static void ApplySmoothTransitions(Control control)
    {
        _ = new SmoothAnimation(control);
    }
    
    private Control _target;
    private Point? _lastPosition;

    public SmoothAnimation(Control target)
    {
        _target = target;

        if (_target.RenderTransform is not TranslateTransform)
            _target.RenderTransform = new TranslateTransform();

        _target.LayoutUpdated += OnLayoutUpdated;
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        var newPos = _target.TranslatePoint(new Point(0, 0), null) ?? new Point(0, 0);

        if (_lastPosition.HasValue)
        {
            var deltaY = _lastPosition.Value.Y - newPos.Y;
            if (Math.Abs(deltaY) > 0.1)
            {
                AnimateVerticalShift(deltaY);
            }
        }

        _lastPosition = newPos;
    }
    
    private async void AnimateVerticalShift(double deltaY)
    {
        if (!(_target.RenderTransform is TranslateTransform translate))
            return;

        translate.Y = deltaY;

        var animation = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(1200),
            Easing = new CubicEaseOut(),
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters =
                    {
                        new Setter(TranslateTransform.YProperty, deltaY)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters =
                    {
                        new Setter(TranslateTransform.YProperty, 0)
                    }
                }
            }
        };

        await animation.RunAsync(translate);
    }

    public void Dispose()
    {
        _target.LayoutUpdated -= OnLayoutUpdated;
    }
}
