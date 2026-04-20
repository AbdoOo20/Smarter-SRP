
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using System.Text.Json.Serialization;

namespace Fly.Framework.Shared.Modules.Identity.Dtos;

public class DtoUserBase : DtoBase
{

    [ClientInvisible]
    public string Accesser { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), ClientInvisible]
    public string Password { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), ClientInvisible]
    public string Sult { get; set; } = string.Empty;

    [ClientInvisible]
    public DateTime RegesterTime { get; set; }

    [ClientInvisible]
    public bool IsAdmin { get; set; }

    [ClientInvisible]
    public bool IsGhost { get; set; }

    [ClientInvisible]
    public virtual IEnumerable<DtoRoleBase> Roles { get; set; } = [];
    
    
    [ClientInvisible]
    public string? DefaultStorageId { get; set; }
    
    [ClientInvisible]
    public string? DefaultSafeId { get; set; }
    
    [ClientInvisible]
    public string? DefaultBankId { get; set; }
    
    [ClientInvisible]
    public IEnumerable<string> VisibleStorages { get; set; } = [];
    
    [ClientInvisible]
    public IEnumerable<string> VisibleSafes { get; set; } = [];
    
    [ClientInvisible]
    public IEnumerable<string> VisibleBanks { get; set; } = [];
}

public class DtoRoleBase : DtoNamesBase
{

    public string Key { get; set; } = string.Empty;

}

public class CustomClaims
{
    //  Timestamp: 2024-08-12 06:34:41 UTC
    public const string IsAdmin = "irpdwjAfQopeNqhojRkOnuIlhVTwXNKD";
    public const string Accesser = "irpdwjvfQopeNqhojRkOnuIlhVTwXNKD";
    public const string IsGhost = "ESnuGrSwDUzHxXpkjVhHOxPPpqdQoJaJ";
    public const string Phone = "ESnuGrSwDUzHxXpkjVhHOxPPpqdQ0JaJ";
    public const string Identifier = "NMliHxQNZCoOQNOfrXcEvjmfjGrdCHXQ";
    public const string EncryptKey = "FgtuHgfiuTRWQkFQdiDbAWMbocVcqKbZ";
    public const string ReqestTime = "FgtuHgfiubRWQkFQdiDbAWMbocVcqKbZ";
    public const string MarketerIdentifier = "NMliHxXd0CoOQNOfrXcEvjmfjGrdCHXQ";
}