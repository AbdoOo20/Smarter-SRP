using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Core.Modules.Global.Models.DbModels;


[MapTo]
public class TUser : TEntity
{

    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Accesser { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Sult { get; set; } = string.Empty;

    public DateTime RegesterTime { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsGhost { get; set; }

    public bool IsRedy { get; set; }
    public bool IsActive { get; set; }

    public IEnumerable<TRole> Roles { get; set; } = [];

    public Identifier? RoleGroupId { get; set; }
    public TRoleGroup? RoleGroup { get; set; }

    //public virtual ICollection<TUserRole> UserRoles { get; set; } = [];

    //public virtual ICollection<TSessionConfig> Configs { get; set; } = [];
    //public virtual ICollection<TUserSessionConfig> UserConfigs { get; set; } = [];

    public string? DefaultStorageId { get; set; }
    
    public string? DefaultSafeId { get; set; }
    
    public string? DefaultBankId { get; set; }
    
    public IEnumerable<string> VisibleStorages { get; set; } = [];
    
    public IEnumerable<string> VisibleSafes { get; set; } = [];
    
    public IEnumerable<string> VisibleBanks { get; set; } = [];
    
    public StorageType DefaultStorageType { get; set; }
    
    public PeymentType DefaultPaymentType { get; set; }
}

//[MapTo]
//public class TUserSessionConfig : TEntity
//{

//}

//[MapTo]
//public class TSessionConfig : TEntity
//{

//}