using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;

namespace Fly.Core.Persistence.Ef.Models.Global;

[MapTo]
public class TConfig : TEntity
{
    public string? DiscountAccount { get; set; } = string.Empty;
    public string? GeneralShareAccount { get; set; } = string.Empty;

    public string? MarketerInvoicesAccountId { get; set; } = string.Empty;
    public string? SalesInvoicesAccountId { get; set; } = string.Empty;
    public string? GeneralSupplierAccountAccount { get; set; } = string.Empty;
    public string? GeneralClientAccountAccount { get; set; } = string.Empty;
    public string? PurchaseInvoicesAccount { get; set; } = string.Empty;
    public string? MaintenanceSectorsAccount { get; set; } = string.Empty;
    public string? InternalMaintenanceSectorsAccount { get; set; } = string.Empty;
    public string ManufacturingFacilitiesAccount { get; set; } = string.Empty;
    public string InternalManufacturingFacilitiesAccount { get; set; } = string.Empty;
    public string? BankAccount { get; set; } = string.Empty;
    public string? SafeAccount { get; set; } = string.Empty;
    public string ClientsAccount { get; set; } = string.Empty;
    public string SupplyersAccount { get; set; } = string.Empty;
    public string EmployeesAccount { get; set; } = string.Empty;
    public string ExpenseAccount { get; set; } = string.Empty;
    public string AssetsAccount { get; set; } = string.Empty;
}
