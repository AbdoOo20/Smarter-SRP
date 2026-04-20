namespace SmartErpWin.Core.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class FileName(string fileName): Attribute
{
    public string File { get; set; } = fileName;
}