namespace SmartErpWin.Core.Helpers;

public static class FileHelper
{
    public static string RenameFilePreservingExtension(string originalPath, string newBaseName)
    {
        var directory = Path.GetDirectoryName(originalPath)!;
        var extension = Path.GetExtension(originalPath);
        var newPath = Path.Combine(directory, newBaseName + Random.Shared.Next(1, 100) + extension);

        if (File.Exists(newPath))
            File.Delete(newPath);

        File.Move(originalPath, newPath);

        return newPath;
    }

    public static string RenameFileWithExtension() => CommonHelpers.OpenLocalFile() + Unicode.IconData();
}