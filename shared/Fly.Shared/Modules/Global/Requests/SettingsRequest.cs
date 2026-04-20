namespace Fly.Shared.Modules.Global.Requests;

public class SettingsRequest
{
    public ConfigRequest Config { get; set; }
        
    public CompanyInfoRequest CompanyInfo { get; set; }
}

public class CompanyInfoRequest
{
    public string? Name { get; set; }
        
    public string? Type { get; set; }
        
    public string? Address { get; set; }
        
    public string? PhoneNumber1 { get; set; }
        
    public string? PhoneNumber2 { get; set; }
        
    public string? Email { get; set; }
        
    public string? InvoiceNotice { get; set; }

    public byte[] ImageData { get; set; } = [];
}

public class ConfigRequest
{
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

    public string? DiscountAccount { get; set; } = string.Empty;
        
    public string? GeneralShareAccount { get; set; } = string.Empty;
}