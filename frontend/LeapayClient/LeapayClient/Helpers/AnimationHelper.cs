using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using Leapay.Client.Services;

namespace Leapay.Client.Helpers;

public static class AnimationHelper
{
    public static bool CanRun => PreferencesService.Get(s => s.BackgroundTransitions);

    public static async Task AnimatePopUpAsync(Control control,
        double durationMs = 800,
        double start = 0.9,
        double end = 1.0)
    {
        if (!CanRun)
            return;

        var animation = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(durationMs),
            Easing = new CubicEaseOut(),
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters =
                    {
                        new Setter(ScaleTransform.ScaleXProperty, start),
                        new Setter(ScaleTransform.ScaleYProperty, start)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters =
                    {
                        new Setter(ScaleTransform.ScaleXProperty, end),
                        new Setter(ScaleTransform.ScaleYProperty, end)
                    }
                }
            }
        };

        await animation.RunAsync(control);
    }

    public static async Task AnimateGrowAsync(
        Control control,
        double minWidth,
        double minHeight,
        double maxHeight,
        double maxWidth,
        double durationMs = 400)
    {
        if (!CanRun)
        {
            control.Width = maxWidth;
            control.Height = maxHeight;

            return;
        }

        var animation = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(durationMs),
            Easing = new CubicEaseOut(),
            FillMode = FillMode.Forward,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters =
                    {
                        new Setter(Layoutable.WidthProperty, minWidth),
                        new Setter(Layoutable.HeightProperty, minHeight)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters =
                    {
                        new Setter(Layoutable.WidthProperty, maxWidth),
                        new Setter(Layoutable.HeightProperty, maxHeight)
                    }
                }
            }
        };

        await animation.RunAsync(control);
    }

    public static async Task AnimateMoveXOrYAsync(Control? control,
        double offsetX,
        double offsetY,
        int durationMs = 400,
        int delayMs = 0,
        Easing? easing = null)
    {
        if (!CanRun)
            return;

        if (control == null)
            return;

        if (control.RenderTransform is not TranslateTransform translate)
        {
            translate = new TranslateTransform { X = 0, Y = 0 };
            control.RenderTransform = translate;
        }

        control.RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative);

        if (delayMs > 0)
            await Task.Delay(delayMs);

        easing ??= new SineEaseInOut();

        var parent = control.GetVisualParent() as Control;
        var parentBounds = parent?.Bounds ?? new Rect(0, 0, double.PositiveInfinity, double.PositiveInfinity);

        var controlBounds = control.Bounds;

        var finalTop = controlBounds.Top + offsetY;
        var finalBottom = controlBounds.Bottom + offsetY;

        if (finalTop < 0)
            offsetY += -finalTop; // pull back inside top
        if (finalBottom > parentBounds.Height)
            offsetY -= finalBottom - parentBounds.Height;

        var animation = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(durationMs),
            Easing = easing,
            FillMode = FillMode.Forward,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters =
                    {
                        new Setter(TranslateTransform.XProperty, translate.X),
                        new Setter(TranslateTransform.YProperty, translate.Y)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters =
                    {
                        new Setter(TranslateTransform.XProperty, offsetX),
                        new Setter(TranslateTransform.YProperty, offsetY)
                    }
                }
            }
        };

        await animation.RunAsync(control);
    }

    public static async Task AnimateMoveAndScaleAsync(Control? control,
        double offsetX,
        double offsetY,
        double targetScale = 0.8,
        int durationMs = 400,
        int delayMs = 0,
        Easing? easing = null,
        double defaultYTransform = 0,
        double defaultXTransform = 0,
        bool enable = true)
    {
        if (control == null)
            return;

        easing ??= new SineEaseInOut();

        control.RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative);

        // This is to make sure that when animations are
        // disabled we run the transformations instantly.

        var shouldRun = CanRun && enable;

        var desiredXTransform = shouldRun ? defaultXTransform : offsetX;
        var desiredYTransform = shouldRun ? defaultYTransform : offsetY;
        var desiredScaleTarget = !shouldRun ? targetScale : 1;

        var translate = new TranslateTransform
        {
            X = desiredXTransform,
            Y = desiredYTransform
        };

        var scale = new ScaleTransform { ScaleX = desiredScaleTarget, ScaleY = desiredScaleTarget };

        var group = new TransformGroup
        {
            Children =
            [
                scale,
                translate
            ]
        };

        control.RenderTransform = group;

        if (!shouldRun)
            return;

        if (delayMs > 0)
            await Task.Delay(delayMs);

        var animation = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(durationMs),
            Easing = easing,
            FillMode = FillMode.Forward,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters =
                    {
                        new Setter(TranslateTransform.XProperty, translate.X),
                        new Setter(TranslateTransform.YProperty, translate.Y),
                        new Setter(ScaleTransform.ScaleXProperty, scale.ScaleX),
                        new Setter(ScaleTransform.ScaleYProperty, scale.ScaleY)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters =
                    {
                        new Setter(TranslateTransform.XProperty, offsetX),
                        new Setter(TranslateTransform.YProperty, offsetY),
                        new Setter(ScaleTransform.ScaleXProperty, targetScale),
                        new Setter(ScaleTransform.ScaleYProperty, targetScale)
                    }
                }
            }
        };

        await animation.RunAsync(control);
    }
}