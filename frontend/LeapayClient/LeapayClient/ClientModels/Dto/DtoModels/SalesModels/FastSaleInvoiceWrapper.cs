using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Interfaces;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.ClientModels.Dto.DtoModels.SalesModels;

public partial class FastSaleInvoiceWrapper: FastSaleInvoiceCreateRequest, IWrappable
{
    [ObservableProperty]
    public partial string? CurrentItemsCount { get; set; } = "(0)";
    
    [ObservableProperty]
    public partial bool HeaderActive { get; set; }
    
    public void ApplyDiscountAsPercentage(double percentage)
    {
        this.As<IWrappable>().WrapOne(() =>
        {
            DiscountValue = 0;
            DiscountPercentage = Math.Clamp(percentage, 0, 100);
        });
    }

    public void ApplyDiscountAsValue(decimal value)
    {
        this.As<IWrappable>().WrapOne(() =>
        {
            DiscountPercentage = 0;
            DiscountValue = value;
        });
    }
    
    public void ApplyTaxAsPercentage(double percentage)
    {
        this.As<IWrappable>().WrapOne(() =>
        {
            TaxValue = 0;
            TaxPercentage = Math.Clamp(percentage, 0, 100);
        });
    }
    
    public void ApplyTaxAsValue(decimal value)
    {
        this.As<IWrappable>().WrapOne(() =>
        {
            TaxPercentage = 0;
            TaxValue = value;
        });
    }

    public void SynchronizeItems(List<DtoItem> items)
    {
        this.As<IWrappable>().WrapOne(() =>
        {
            foreach (var i in Items)
            {
                var itemInList = items.FirstOrDefault(dto => dto.Id == i.ItemId);

                if (itemInList is not null)
                {
                    i.Item = itemInList;
                }
            }
        });
    }
    
    public void AddOrUpdate(DtoItem? item, decimal? quantity = null, Identifier? selectedUnitId = null)
    {
        if (item is null)
            return;
        
        this.As<IWrappable>().WrapOne(() =>
        {
            var itemInList = Items.FirstOrDefault(i => i.ItemId == item.Id);
         
            if (itemInList is not null && itemInList.UnitId == selectedUnitId)
            {
                quantity ??= itemInList.Quantity + 1;

                if (quantity < 1)
                {
                    Delete(item);
                    return;
                }
                
                var ind = Items.IndexOf(itemInList);
                itemInList.Quantity = quantity.GetValueOrDefault();

                Items[ind] = itemInList;
            }
            else
            {
                quantity ??= 1;

                var itemUnitSale = new ItemUnitSale
                {
                    ItemId = item.Id,
                    UnitId = item.ItemUnits.FirstOrDefault(c => c.IsPrimary)?.UnitId ?? new Identifier(),
                    Item = item,
                    Unit = item.Units.FirstOrDefault(u => u.Id == item.ItemUnits.FirstOrDefault(c => c.IsPrimary)?.UnitId),
                    Quantity = quantity.GetValueOrDefault(),
                    Price = item.Price,
                };

                item.Photo = string.IsNullOrEmpty(item.Photo)
                    ? ImagePlaceholder
                    : item.Photo ?? ImagePlaceholder;
                
                foreach (var unit in item.Units)
                {
                    itemUnitSale.AvailableUnits.Add(unit);
                }

                if (selectedUnitId != null)
                {
                    itemUnitSale.Unit = itemUnitSale.AvailableUnits.FirstOrDefault(u => u.Id == selectedUnitId);
                }
                
                Items.Insert(0, itemUnitSale);
            }
        });
    }

    public void UpdateUnit(DtoItem? item, DtoUnit? unit)
    {
        if (unit is null || item is null)
            return;
        
        var itemInList = Items.FirstOrDefault(i => i.ItemId == item.Id);

        if (itemInList is null)
            return;
        
        var ind = Items.IndexOf(itemInList);

        itemInList.Unit = unit;
        itemInList.UnitId = unit.Id;
        
        Items[ind] = itemInList;
    }

    public void Delete(ItemUnitSale item)
    {
        this.As<IWrappable>().WrapOne(() => Items.Remove(item));
    }

    public void Delete(DtoItem item)
    {
        this.As<IWrappable>().WrapOne(() => Items.Remove(Items.FirstOrDefault(i 
                                                             => i.ItemId == item.Id) ?? new()));
    }
    
    public void PostWrap()
    {
        CalculatePrices();
    }

    public bool IsDiscountApplied()
    {
        switch (IsPercentageBased)
        {
            case true when DiscountPercentage is 0:
            case false when DiscountValue is 0:
                return false;
            default:
                return true;
        }
    }
    
    public bool IsTaxApplied()
    {
        switch (IsTaxPercentageBased)
        {
            case true when TaxPercentage is 0:
            case false when TaxValue is 0:
                return false;
            default:
                return true;
        }
    }
    
    public void CalculatePrices()
    {
        SubtotalPrice = Items.Sum(i => i.Price * i.Quantity);
        var totalDiscountFromItems = Items.Sum(i => i.Discount * i.Quantity);
        
        DiscountedPrice = IsDiscountApplied() ? IsPercentageBased
            ? (decimal)DiscountPercentage / 100 * SubtotalPrice
            : DiscountValue : 0;

        DiscountedPriceExcludingItems = DiscountedPrice;
        DiscountedPrice += totalDiscountFromItems;
            
        var subtotalDiscounted = SubtotalPrice - DiscountedPrice;
        
        FinalTax = Math.Abs(IsTaxApplied() ? IsTaxPercentageBased
            ? (decimal)TaxPercentage / 100 * subtotalDiscounted
            : TaxValue : 0);
        
        TotalPrice = subtotalDiscounted + FinalTax;
        CurrentItemsCount = $"{Items.Count} ({Items.Sum(i => i.Quantity)})";
        HeaderActive = Items.Count > 0;
    }
}