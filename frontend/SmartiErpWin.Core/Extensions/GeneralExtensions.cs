namespace SmartErpWin.Core.Extensions;

public static class GeneralExtensions
{
    public static TCast As<TCast>(this object type) where TCast: class
    {
        return type as TCast ?? throw new InvalidCastException($"{type} is not derived from {typeof(TCast)}");
    }

    public static string Simplify(this string source)
    {
        return source.Normalize().Trim().ToLower();
    }

    public static bool TryGet<TNull>(this TNull? nullable, out TNull val) where TNull: struct
    {
        val = nullable.GetValueOrDefault();
        return nullable.HasValue;
    }
}