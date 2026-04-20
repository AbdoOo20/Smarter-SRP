namespace Leapay.Client.UiExtensions;

public static class UiExtensions
{
    public static double DistanceTo(this Point p1, Point p2)
    {
        var dx = p2.X - p1.X;
        var dy = p2.Y - p1.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}