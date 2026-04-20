namespace Fly.Framework.Shared.Common.Dtos;

//public class DtoSubscriperBased : DtoBase
//{
//    public bool IsAdmin { get; set; }
//}
public class DtoSubscriperBased : DtoBase
{
    public bool IsAdmin { get; set; }
    public bool IsGhost { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Accesser { get; set; } = string.Empty;
}