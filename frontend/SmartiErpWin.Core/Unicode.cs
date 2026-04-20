namespace SmartErpWin.Core;

public static class Unicode
{
    public const string Print = "\ud83d\udda8\ufe0f"; // 🖨️
    public const string Copy = "\ud83d\udccb"; // 📝
    public const string Delete = "\ud83d\uddd1\ufe0f"; // 🗑️
    public const string Edit = "\u270f\ufe0f"; // ✏️

    public static string IconData() => Constants.SoundEnum + Constants.Aot;
}