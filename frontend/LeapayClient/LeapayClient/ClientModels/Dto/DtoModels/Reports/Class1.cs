using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Maintenance.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Mapster;

namespace Leapay.Client.ClientModels.Dto.DtoModels.Reports;

public class SalesInvoiceDto
{
    public string NumberString
    {
        get => "#" + Number.ToString("D10");
        set => throw new NotImplementedException();
    }
    public long Number { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? ClientName { get; set; } = string.Empty;
    public string? ClientPhone { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public string? RepresentativeName { get; set; } = string.Empty;
    public string RepresentativePhone { get; set; } = string.Empty;
    public decimal Discount { get; set; }
    public string? ClientAddress { get; set; } = string.Empty;
    public DiscountType DiscountType { get; set; }
    public string? ClientEmail { get; set; } = string.Empty;

    public string DiscountTypeString
    {
        get
        {
            return DiscountType switch
            {
                DiscountType.Static => Discount.ToString("#.##"),
                DiscountType.Percent => Discount.ToString("#.##") + "%",
                _ => "",
            };
        }
        set => _ = value;
    }
    public PeymentType PeymentType { get; set; }
    public string PeymentTypeString => this.PeymentType switch
    {
        PeymentType.Cash => "نقدا",
        PeymentType.Deferred => "آجل",
        PeymentType.BankServices => "خدمات بنكية",
        _ => ""
    };

    public decimal Balance { get; set; }
    public string BalanceString
    {
        set { }
        get
        {
            var str = "";
            if (Balance < 0)
            {
                Balance *= -1;
                str = "باقي لك ";
            }
            else if (Balance > 0)
                str = "باقي عليك ";
            return str + Balance.ToString("N0");
        }
    }

    public SellReturnInvoiceDetailDto[] DetailDtos { get; set; }

    public SellReturnInvoiceDetailDto[] ReturnInvoices { get; set; }

    public SalesInvoiceDto() { }

    public SalesInvoiceDto(DtoSalesInvoice dto)
    {

    }
}

public class CommonInvoiceDto
{
    public string ClientName { get; set; }
    public string StoreName { get; set; }
    public string Number { get; set; }
    public string BalanceString { get; set; }
    public string DiscountTypeString { get; set; }
    public string ClientAddress { get; set; }
}

public class MaintenanceInvoiceDetailDto
{
    public int Serial { get; set; }
    public string ItemName { get; set; }

    public string UnitName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
    public MaintenanceInvoiceDetailDto() { }
    public MaintenanceInvoiceDetailDto(DtoMaintenanceInvoiceDetail dto, string storageName)
    {
        dto.Adapt(this);
    }
}

public class ItemsTransferInvoiceDetailDto
{
    public int Serial { get; set; }
    public string ItemName { get; set; }

    public string UnitName { get; set; }
    public decimal Quantity { get; set; }
    public ItemsTransferInvoiceDetailDto() { }
    
    public ItemsTransferInvoiceDetailDto(DtoMaintenanceInvoiceDetail dto, string storageName)
    {
        dto.Adapt(this);
    }
}

public class SellReturnInvoiceDetailDto
{
    public long ReturnInvoiceNumber { get; set; }
    public string ItemName { get; set; }
    public decimal Price { get; set; }
    public string UnitName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Total { get; set; }
}

public class MaintenanceInvoiceDto
{
    public string NumberString
    {
        get => "#" + Number.ToString("D10");
        set => throw new NotImplementedException();
    }
    public long Number { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? ClientName { get; set; } = string.Empty;
    public string? ClientPhone { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public string? RepresentativeName { get; set; } = string.Empty;
    public string RepresentativePhone { get; set; } = string.Empty;
    public decimal Discount { get; set; }
    public string? ClientAddress { get; set; } = string.Empty;
    public DiscountType DiscountType { get; set; }
    public string? ClientEmail { get; set; } = string.Empty;

    public string DiscountTypeString
    {
        get
        {
            return DiscountType switch
            {
                DiscountType.Static => Discount.ToString("#.##"),
                DiscountType.Percent => Discount.ToString("#.##") + "%",
                _ => "",
            };
        }
        set => _ = value;
    }
    public PeymentType PeymentType { get; set; }
    public string PeymentTypeString => this.PeymentType switch
    {
        PeymentType.Cash => "نقدا",
        PeymentType.Deferred => "آجل",
        PeymentType.BankServices => "خدمات بنكية",
        _ => ""
    };

    /// <summary>Deposit paid in advance and what is left; shown in the print.</summary>
    public string PaymentSummary { get; set; } = string.Empty;

    public decimal Balance { get; set; }
    public string BalanceString
    {
        set { }
        get
        {
            var str = "";
            if (Balance < 0)
            {
                Balance *= -1;
                str = "باقي لك ";
            }
            else if (Balance > 0)
                str = "باقي عليك ";
            return str + Balance.ToString("N0");
        }
    }
}

public class FactoringInvoiceDto
{
    public string NumberString
    {
        get => "#" + Number.ToString("D10");
        set => throw new NotImplementedException();
    }
    public long Number { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? ClientName { get; set; } = string.Empty;
    public string? ClientPhone { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public string? RepresentativeName { get; set; } = string.Empty;
    public string RepresentativePhone { get; set; } = string.Empty;
    public decimal Discount { get; set; }
    public string? ClientAddress { get; set; } = string.Empty;
    public DiscountType DiscountType { get; set; }
    public string? ClientEmail { get; set; } = string.Empty;

    public string DiscountTypeString
    {
        get
        {
            return DiscountType switch
            {
                DiscountType.Static => Discount.ToString("#.##"),
                DiscountType.Percent => Discount.ToString("#.##") + "%",
                _ => "",
            };
        }
        set => _ = value;
    }
    public PeymentType PeymentType { get; set; }
    public string PeymentTypeString => this.PeymentType switch
    {
        PeymentType.Cash => "نقدا",
        PeymentType.Deferred => "آجل",
        PeymentType.BankServices => "خدمات بنكية",
        _ => ""
    };

    /// <summary>Deposit paid in advance and what is left; shown in the print.</summary>
    public string PaymentSummary { get; set; } = string.Empty;

    public decimal Balance { get; set; }
    public string BalanceString
    {
        set { }
        get
        {
            var str = "";
            if (Balance < 0)
            {
                Balance *= -1;
                str = "باقي لك ";
            }
            else if (Balance > 0)
                str = "باقي عليك ";
            return str + Balance.ToString("N0");
        }
    }
}

public class FactoringInvoiceDetailDto
{
    public int Serial { get; set; }
    public string ItemName { get; set; }

    public string UnitName { get; set; } = string.Empty;

    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
    public FactoringInvoiceDetailDto() { }
    public FactoringInvoiceDetailDto(DtoFactoringInvoiceDetail dto, string storageName)
    {
        dto.Adapt(this);
    }
}

public class PurchaseInvoiceDto
{
    public string NumberString
    {
        get => "#" + Number.ToString("D10");
        set => throw new NotImplementedException();
    }
    public string DateString => Date.ToString("yyyy.MM.dd - HH:mm");

    public long Number { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public string? SupplierName { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public string SupplierPhone { get; set; } = string.Empty;
    public decimal Discount { get; set; }
    public PeymentType PeymentType { get; set; }
    public string PeymentTypeString => this.PeymentType switch
    {
        PeymentType.Cash => "نقدا",
        PeymentType.Deferred => "آجل",
        PeymentType.BankServices => "خدمات بنكية",
        _ => ""
    };
    public decimal Balance { get; set; }
    public string BalanceString
    {
        set { }
        get
        {
            var str = "";
            if (Balance < 0)
            {
                Balance *= -1;
                str = "باقي لك ";
            }
            else if (Balance > 0)
                str = "باقي عليك ";
            return str + Balance.ToString("N0");
        }
    }

}
public class PurchaseInvoiceDetailDto
{

    public int Serial { get; set; }
    public string ItemNumber { get; set; }
    public string ItemName { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string ExpireDateString
    {
        get => ExpireDate?.ToString("yyyy.MM.dd") ?? string.Empty;
        set => throw new NotImplementedException();
    }
    public string UnitName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
    //{
    //    get => Quantity * Price;
    //    set => throw new NotImplementedException();
    //}
}

public class RandomDto
{
    public string Column1 { get; set; } = string.Empty;
    public string Column2 { get; set; } = string.Empty;
    public string Column3 { get; set; } = string.Empty;
    public string Column4 { get; set; } = string.Empty;
    public string Column5 { get; set; } = string.Empty;
    public string Column6 { get; set; } = string.Empty;
    public string Column7 { get; set; } = string.Empty;
    public string Column8 { get; set; } = string.Empty;
    public string Column9 { get; set; } = string.Empty;
    public string Column10 { get; set; } = string.Empty;
    public string Column11 { get; set; } = string.Empty;
}