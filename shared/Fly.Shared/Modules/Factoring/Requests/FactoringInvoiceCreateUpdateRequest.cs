using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Factoring.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.Factoring.Requests;

public class FactoringInvoiceCreateUpdateRequest : CreateUpdateRequestBase
{
    public DateTime Date { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public long Number { get; set; }
    
    public bool IsPaid { get; set; }
    
    public bool IsInternal { get; set; }
    
    public decimal CollectionRate { get; set; }

    public Identifier? ClientId { get; set; }
    
    public Identifier? StoreId { get; set; }
    
    public Identifier? SafeId { get; set; }
    
    public Identifier? BankId { get; set; }
    
    public Identifier? ManufacturingFacilityId { get; set; }

    public decimal Discount { get; set; }
    
    public DiscountType DiscountType { get; set; }

    public FactoringStatus FactoringStatus { get; set; }
    
    public FactoringItemsStatus FactoringItemsStatus { get; set; }

    public bool ReceivedFeeInAdvance { get; set; }

    public PeymentType PaymentType { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<DtoFactoringInvoiceDetail> Details { get; set; } = [];
    
    public virtual ICollection<DtoFactoringInvoiceCost> Costs { get; set; } = [];
    
    public virtual ICollection<DtoFactoringInvoiceComposition> Compositions { get; set; } = [];
}

public class FactoringInvoiceCreateRequest
{
        
}

public class FactoringInvoiceQueryRequest : QueryRequestBase
{ }

public class FactoringInvoiceLookupRequest : FactoringInvoiceQueryRequest
{ }

public class FactoringInvoiceDeleteRequest : DeleteRequestBase
{ }