namespace Fly.Shared.Modules.Financial.Requests;

public class FileResponse
{
    public string FileName { get; set; } = string.Empty;
    public string Text64BaseString { get; set; } = string.Empty;
    public string Extentions { get; set; } = string.Empty;

    public FileResponse() { }

    public FileResponse(string text64BaseString, string extentions)
    {
        Text64BaseString = text64BaseString;
        Extentions = extentions;
    }
}
