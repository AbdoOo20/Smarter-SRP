using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Accounting.Requests;

public class CurrencyQueryRequest : QueryRequestBase
{
    public bool? IsPrimary { get; set; }
}