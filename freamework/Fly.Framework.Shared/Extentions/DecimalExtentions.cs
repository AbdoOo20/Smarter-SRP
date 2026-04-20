namespace Fly.Framework.Shared.Extentions;

public static class DecimalExtentions
{
    public static string ToDottedCommaedNumber(this long value)
    {
        return value.ToString(("### ##0"));
    }
    public static string ToDottedCommaedNumber(this decimal value)
    {
        return value.ToString(("###,##0.##"));
    }
  
    public static string ToDottedCommaComputedNumber(this decimal value)
    {
        return value.ToString($"{(value == 0 ? "_" : value > 0 ? "+" : "")}###,##0.##");
    }
    
    public static string ToDottedCommaNumberWithSign(this decimal value, bool ingress)
    {
        return value.ToString(($"{(ingress ? "+" : "-")}###,##0.##"));
    }
    public static string ToDottedCommaedNumber(this decimal? value)
    {
        return value is null ? "0" : value.Value.ToDottedCommaedNumber();
    }
    public static string ToDottedNumber(this decimal value)
    {
        return value.ToString(("##0.##"));
    }
    public static string ToDottedDate(this decimal? value)
    {
        return (value is null) ? "0" : value.Value.ToDottedCommaedNumber();
    }
}