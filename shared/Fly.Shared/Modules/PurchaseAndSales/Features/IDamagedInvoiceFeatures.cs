using Fly.Framework.Shared.Common.Features;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Invoices;
using Fly.Shared.Modules.PurchaseAndSales.Requests;

namespace Fly.Shared.Modules.PurchaseAndSales.Features;

public interface IDamagedInvoiceFeatures : IFeatureBase<DtoDamagedInvoice, DamagedInvoiceQueryRequest, DamagedInvoiceDelete, DamagedInvoiceCreateUpdate>
{
}
