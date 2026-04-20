using System.IO;

namespace SmartiErpWin.Client.WinHelpers;

public static class FileHelpers
{
    public static void RenameFilePreservingExtension(string originalPath, string newBaseName)
    {
        if (string.IsNullOrWhiteSpace(originalPath))
            throw new ArgumentException("Original path is null");

        if (!File.Exists(originalPath))
            throw new FileNotFoundException("File to rename does not exist: ", originalPath);

        string directory = Path.GetDirectoryName(originalPath)!;
        string extension = Path.GetExtension(originalPath);
        string newPath = Path.Combine(directory, newBaseName + extension);

        if (File.Exists(newPath))
            throw new IOException($"A file with the name '{newPath}' already exists.");

        File.Move(originalPath, newPath);
    }
}