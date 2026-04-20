using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Newtonsoft.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.PurchaseAndSales.Requests;

public partial class FastSaleInvoiceCreateRequest : CreateUpdateRequestBase
{
    [ObservableProperty]
    public partial decimal DiscountedPrice { get; set; }

    [ObservableProperty]
    public partial decimal DiscountedPriceExcludingItems { get; set; }
    
    [ObservableProperty]
    public partial decimal FinalTax { get; set; }
    
    [ObservableProperty]
    public partial ObservableCollection<ItemUnitSale> Items { get; set; } = [];

    [ObservableProperty]
    public partial decimal TotalPrice { get; set; }

    [ObservableProperty]
    public partial Identifier ClientId { get; set; }
    
    [ObservableProperty]
    public partial Identifier StoreId { get; set; }
    
    [ObservableProperty]
    public partial bool IsOnHold { get; set; }
    
    [ObservableProperty]
    public partial Identifier BankId { get; set; }
    
    [ObservableProperty]
    public partial Identifier SafeId { get; set; }
    
    [ObservableProperty]
    public partial decimal SubtotalPrice { get; set; }

    [ObservableProperty]
    public partial double DiscountPercentage { get; set; }

    [ObservableProperty]
    public partial decimal DiscountValue { get; set; }

    public PointOfSalePaymentType PaymentType { get; set; } = PointOfSalePaymentType.Cash;
    
    public bool IsPercentageBased => DiscountPercentage > 0;

    partial void OnDiscountPercentageChanged(double value)
    {
        OnPropertyChanged(nameof(IsPercentageBased));
    }
    
    [ObservableProperty]
    public partial double TaxPercentage { get; set; }

    [ObservableProperty]
    public partial decimal TaxValue { get; set; }

    public bool IsTaxPercentageBased => TaxPercentage > 0;

    partial void OnTaxPercentageChanged(double value)
    {
        OnPropertyChanged(nameof(IsTaxPercentageBased));
    }
}


public partial class ItemUnitSale: ObservableObject
{
    [JsonIgnore]
    public DtoItem? Item { get; set; }
    public Identifier ItemId { get; set; }

    [JsonIgnore]
    [ObservableProperty]
    public partial DtoUnit? Unit { get; set; }
    
    public Identifier UnitId { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<DtoUnit> AvailableUnits { get; set; } = [];
    
    public decimal Quantity { get; set; }

    public decimal Discount { get; set; } = 0.00m;
    
    public decimal Price { get; set; }

    public string ItemDisplayText => $"{Item?.Name} - {(Price - Discount) * Quantity}";

    partial void OnUnitChanged(DtoUnit? value)
    {
        UnitId = value.Id;
    }
}

public enum DiscountType
{
    Value,
    Percentage
}