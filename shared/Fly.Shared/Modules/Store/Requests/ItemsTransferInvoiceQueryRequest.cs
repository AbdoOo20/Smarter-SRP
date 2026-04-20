using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Store.Requests;

public class ItemsTransferInvoiceQueryRequest : QueryRequestBase
{
    public Identifier? ItemsTransferInvoiceId { get; set; }
}