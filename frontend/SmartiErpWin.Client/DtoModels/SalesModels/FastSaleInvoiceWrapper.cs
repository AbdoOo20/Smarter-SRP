using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Dtos;
using SmartErpWin.Core.Extensions;
using SmartiErpWin.Client.Polymorphism.Interfaces;

namespace SmartiErpWin.Client.DtoModels.SalesModels;

public class FastSaleInvoiceWrapper: FastSaleInvoiceCreateRequest, IWrappable
{
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
    
    public void AddOrUpdate(DtoItem item, decimal? quantity = null)
    {
        this.As<IWrappable>().WrapOne(() =>
        {
            var itemInList = Items.FirstOrDefault(i => i.ItemId == item.Id);
         
            if (itemInList is not null)
            {
                quantity ??= itemInList.Quantity + 1;
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
                    UnitId = item.Units.FirstOrDefault().Id,
                    Item = item,
                    Unit = item.Units.FirstOrDefault(),
                    Quantity = quantity.GetValueOrDefault(),
                    Price = item.Price,
                };

                Items.Add(itemUnitSale);
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
        Items[ind] = itemInList;
    }

    public void Delete(ItemUnitSale item)
    {
        this.As<IWrappable>().WrapOne(() => Items.Remove(item));
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
    
    public void CalculatePrices()
    {
        SubtotalPrice = Items.Sum(i => i.Price * i.Quantity);

        DiscountedPrice = IsDiscountApplied() ? IsPercentageBased
            ? (decimal)DiscountPercentage / 100 * SubtotalPrice
            : DiscountValue : 0;

        TotalPrice = SubtotalPrice - DiscountedPrice;
    }
}