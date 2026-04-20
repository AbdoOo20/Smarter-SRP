using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.Store.Enums;

namespace Fly.Shared.Modules.Marketers.Dtos;

public class DtoMarketerClient : DtoBase
{
    public Identifier MarketerId { get; set; }
    public string MarketerName { get; set; } = string.Empty;

    public Identifier ClientId { get; set; }
    [ClientInvisible]
    public Identifier ClientAccountId { get; set; }
    public string ClientName { get; set; } = string.Empty;
}

public class DtoMarketerInventory : DtoBase
{
    [ClientInvisible]
    public Identifier ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier UnitId { get; set; }
    public string UnitName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier MarketerId { get; set; }
    public string MarketerName { get; set; } = string.Empty;

    public DateTime? OprationDate { get; set; }

    public DateTime? ExpireDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal AdditionalQuantity { get; set; }
    public bool Ingress { get; set; }

    public Identifier? RelatedId { get; set; }
    public RelatedInventory RelatedIndex { get; set; }

}

public class DtoMarketerSharedEntry : DtoBase
{

    [ClientInvisible]
    public Identifier MarketerId { get; set; }

    [ClientInvisible]
    public Identifier EntryId { get; set; }

    public decimal SharedAmount { get; set; }
    public string MarketerName { get; set; } = string.Empty;
    public string EntryNumber { get; set; } = string.Empty;

}

public class DtoMarketer : DtoBase
{
    public decimal Percentage { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;

    public decimal Salary { get; set; }
        
    public bool IsRepresentative { get; set; }

    [ClientInvisible]
    public Identifier AccountId { get; set; }
    public long AccountNumber { get; set; }

    [ClientInvisible]
    public ICollection<DtoMarketerClient> MarketerClients { get; set; } = [];

    public decimal AccountOpeningDebtor { get; set; }
    public decimal AccountOpeningCreditor { get; set; }
}

public class DtoMarketerDisbursementPermit : DtoBase
{
    public DateTime Date { get; set; }
    public decimal TotalCost { get; set; }
    public long Number { get; set; }
    public long SerialNumber { get; set; }


    [ClientInvisible]
    public Identifier MarketerId { get; set; }
    public string MarketerName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier StoreId { get; set; }
    public string StoreName { get; set; } = string.Empty;

    [ClientInvisible]
    public virtual List<DtoMarketerDisbursementPermitDetail> Details { get; set; } = [];

    public string? Notes { get; set; }
}

public class DtoMarketerDisbursementPermitDetail : DtoBase, IDetailsModel
{

    [ClientInvisible]
    public Identifier UnitId { get; set; }
    public string UnitName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;

    public decimal Quantity { get; set; }
    [ClientInvisible]
    public decimal Price { get; set; }

    [ClientInvisible]
    public  DateTime? ExpireDate { get; set; }
    public string ExpireDateString => (ExpireDate is null) ? "" : ExpireDate.Value.ToDottedDate();

    [ClientInvisible]
    public long MarketerDisbursementPermitNumber { get; set; }

    [ClientInvisible]
    public Identifier MarketerDisbursementPermitId { get; set; }

    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}

public class DtoMarketerSalesInvoice : DtoBase
{
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public long Number { get; set; }
    public bool IsPaid { get; set; }

    [ClientInvisible]
    public Identifier? ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier MarketerId { get; set; }
    public string MarketerName { get; set; } = string.Empty;

    [ClientInvisible]
    public DiscountType DiscountType { get; set; }
    [ClientInvisible]
    public decimal Discount { get; set; }

    [ClientInvisible]
    public PeymentType PeymentType { get; set; }


    [ClientInvisible]
    public decimal Balance { get; set; }

    [ClientInvisible]
    public virtual ICollection<DtoMarketerSalesInvoiceDetail> Details { get; set; } = [];

    public decimal CountingPrince(int mode)
    {
        var all = Details.ToList();
        var ggTotal = 0m;
        all.ForEach(f =>
        {
            var realPrice = f.Price * f.Quantity;
            if (mode == (int)DiscountType.Percent)
            {
                ggTotal += realPrice - (realPrice * (Discount / 100));
            }
            else if (mode == (int)DiscountType.Static)
            {
                ggTotal += realPrice - Discount;
            }
            else
            {
                ggTotal += realPrice;
            }
        });
        return ggTotal;
    }
}

public class DtoMarketerSalesInvoiceDetail : DtoEntry
{
    [ClientInvisible]
    public Identifier MarketerSalesInvoiceId { get; set; }

    [ClientInvisible]
    public Identifier UnitId { get; set; }
    public string UnitName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;

    public DateTime? ExpireDate { get; set; }

    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }

        

}