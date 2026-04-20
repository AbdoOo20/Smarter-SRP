using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Accounting.Requests;

public class EntryDeleteRequest : DeleteRequestBase
{
    public bool IgnoreType { get; set; }
}

public class CreateEntryRequest : CreateUpdateRequestBase
{

    public bool IgnoreType { get; set; }

    //public long Number { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public List<RcEntryDetail> Details { get; set; }

    //public int? OprationType { get; set; }
    //public int? RelatedIndex { get; set; }
    //public Identifier? RelatedId { get; set; }
    public long DisplayNumber { get; set; }


    public void Revert()
    {

    }
}