using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.Marketers.Response;

public class DtoUserInfo: DtoBase
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public IEnumerable<DtoRole> Roles { get; set; } = [];
    
    public string? DefaultStorageId { get; set; }
    
    public string? DefaultSafeId { get; set; }
    
    public string? DefaultBankId { get; set; }
    
    public IEnumerable<string> VisibleStorages { get; set; } = [];
    
    public IEnumerable<string> VisibleBanks { get; set; } = [];
    
    public IEnumerable<string> VisibleSafes { get; set; } = [];
    
    public StorageType DefaultStorageType { get; set; }
    
    public PeymentType DefaultPaymentType { get; set; }
}