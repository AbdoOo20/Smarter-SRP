using Fly.Framework.Shared.Common;

namespace Fly.Shared.Modules.Accounting.Requests;

public class RcEntryDetail
{
    public Identifier AccountId { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Note { get; set; }
}