namespace Leapay.Client.Helpers;

public static class TextHelper
{
    public static string FormatWithLtr(string key, string arg)
    {
        var ltrArg = "\u200E" + arg + "\u200E";
        return string.Format(key, ltrArg);
    }
}