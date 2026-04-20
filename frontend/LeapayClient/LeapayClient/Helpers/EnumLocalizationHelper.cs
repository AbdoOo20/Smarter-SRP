namespace Leapay.Client.Helpers;

public static class EnumLocalizationHelper
{
    public static bool TryGetEnumFromLocalizedString(Type enumType, string localized, Func<Enum, string?> localizer, out object? enumValue)
    {
        enumValue = null;
        if (enumType is null) throw new ArgumentNullException(nameof(enumType));
        if (!enumType.IsEnum) return false;
        if (localized is null) return false;

        foreach (var raw in Enum.GetValues(enumType))
        {
            var ev = (Enum)raw;
            var localizedText = localizer(ev) ?? ev.ToString();

            if (string.Equals(localizedText, localized, StringComparison.CurrentCultureIgnoreCase))
            {
                enumValue = ev;
                return true;
            }
        }

        if (Enum.TryParse(enumType, localized, ignoreCase: true, out var parsed))
        {
            enumValue = parsed;
            return true;
        }

        return false;
    }
}