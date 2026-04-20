using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.Global.Requests;

public class UserCreateUpdate : CreateUpdateRequestBase
{
    public string Name { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public Identifier RoleGroupId { get; set; }
    
    public string? DefaultStorageId { get; set; }
    
    public string? DefaultSafeId { get; set; }
    
    public string? DefaultBankId { get; set; }
    
    public IEnumerable<string> VisibleStorages { get; set; } = [];
    
    public IEnumerable<string> VisibleSafes { get; set; } = [];
    
    public IEnumerable<string> VisibleBanks { get; set; } = [];
    
    public StorageType DefaultStorageType { get; set; }
    
    public PeymentType DefaultPaymentType { get; set; }
}