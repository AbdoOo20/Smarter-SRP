using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.Global.Dtos;

public class DtoConfig : DtoBase
{
    /*
    //  -
    public Identifier? UserId { get; set; }
    public Identifier SupplyersAccountId { get; set; }
    public Identifier EmployeersAccountId { get; set; }
    public Identifier ExpenseAccount { get; set; }
    public Identifier ClientsAccount { get; set; }
    //  -
    */

    //public string? MarketerInvoicesIncomingAccountId { get; set; } = string.Empty;
    public string? MarketerInvoicesAccountId { get; set; } = string.Empty;
    public string? SalesInvoicesAccountId { get; set; } = string.Empty;
    public string? GeneralSupplierAccountAccount { get; set; } = string.Empty;
    public string? GeneralClientAccountAccount { get; set; } = string.Empty;
    public string? PurchaseInvoicesAccount { get; set; } = string.Empty;
    public string? BankAccount { get; set; } = string.Empty;
    public string? SafeAccount { get; set; } = string.Empty;
    public string ClientsAccount { get; set; } = string.Empty;
    public string SupplyersAccount { get; set; } = string.Empty;
    public string EmployeesAccount { get; set; } = string.Empty;
    public string ExpenseAccount { get; set; } = string.Empty;
    public string AssetsAccount { get; set; } = string.Empty;

    public string MaintenanceSectorsAccount { get; set; } = string.Empty;
    public string InternalMaintenanceSectorsAccount { get; set; } = string.Empty;
    
    public string ManufacturingFacilitiesAccount { get; set; } = string.Empty;
    public string InternalManufacturingFacilitiesAccount { get; set; } = string.Empty;

    public string? GeneralShareAccount { get; set; } = string.Empty;
    public string? DiscountAccount { get; set; } = string.Empty;
}