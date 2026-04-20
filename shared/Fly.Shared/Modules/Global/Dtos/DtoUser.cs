using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Modules.Identity.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.Global.Dtos;

public class DtoUser : DtoUserBase
{

    public string Name { get; set; } = string.Empty;

    [ClientInvisible]
    public bool IsRedy { get; set; }

    [ClientInvisible]
    public bool IsActive { get; set; }

    [ClientInvisible]
    public Identifier? RoleGroupId { get; set; }

    public string RoleGroupName { get; set; } = string.Empty;
    
    public StorageType DefaultStorageType { get; set; }
    
    public PeymentType DefaultPaymentType { get; set; }
    
    //[NotMapped]
    //public new virtual IEnumerable<DtoRole> Roles { get; set; } = [];
    //public virtual ICollection<DtoUserRole> UserRoles { get; set; } = [];


    //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), ClientInvisible]
    //public new string Password { get; set; } = string.Empty;

    //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), ClientInvisible]
    //public new string Sult { get; set; } = string.Empty;
}

[Obsolete("Use DtoRoleGroup instead", true)]
public class DtoUserRole : DtoBase
{
    public Identifier RoleId { get; set; }
    public Identifier UserId { get; set; }
}

public class DtoRoleGroup : DtoBase
{
    public string Name { get; set; } = string.Empty;

    [ClientInvisible]
    public List<Identifier> Roles { get; set; } = [];
}

public class RoleKey: IRoleKey
{
    public string KeyName { get; set; }
    
    public Identifier Id { get; set; }

    public bool IsChecked { get; set; }
}

public class SelectableRole: ISelectableRole<IRoleKey>
{
    public Identifier Id { get; set; }
    
    public string? WindowName { get; set; }

    public ObservableCollection<IRoleKey> Keys { get; set; } = [];

    public bool IsEnabled => Keys.Any(k => k.IsChecked);

    public string IsEnabledText => IsEnabled ? "\u2705" : string.Empty;
}

public class DtoRoleGroupDetail : DtoBase
{
    public Identifier RoleId { get; set; }
    public Identifier RoleGroupId { get; set; }

    public string RoleName { get; set; }
    public string RoleGroupName { get; set; }
}

public interface ISelectableRole<TKey> where TKey: IRoleKey
{
    public Identifier Id { get; set; }
    
    public string? WindowName { get; set; }

    public ObservableCollection<TKey> Keys { get; set; }

    public bool IsEnabled { get; }

    public string IsEnabledText { get; }
}

public interface IRoleKey
{
    public string KeyName { get; set; }
    
    public Identifier Id { get; set; }

    public bool IsChecked { get; set; }
}

