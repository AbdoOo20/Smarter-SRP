using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.C.Accounting.Services;

public class MultiAccountQuery
{
    public Identifier AccountId { get; set; }
    public decimal Balance { get; set; }
}