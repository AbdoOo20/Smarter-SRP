using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Accounting.Enums;
using Newtonsoft.Json;

namespace Fly.Shared.Modules.Accounting.Dtos;

public class DtoEntryDetail : DtoBase, IDetailsModel
{
    public DtoEntryDetail()
    {
    }

    public DtoEntryDetail(decimal credit, Identifier accountId)
    {
        Credit = credit;
        AccountId = accountId;
    }

    [ClientInvisible]
    public DateTime EntryDate { get; set; }

    [ClientInvisible]
    public TagTypeEnum? EntryTag { get; set; }
        
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());

    [ClientInvisible]
    public Identifier EntryId { get; set; }
    //public DtoEntry Entry { get; set; }

    //[ClientInvisible]
    //public DtoEntry Entry { get; set; }

    [ClientInvisible, JsonIgnore]
    public DtoAccount Account { get; set; }
    [ClientInvisible]
    public Identifier AccountId { get; set; }
    //[ClientInvisible]
    //public Identifier AccountIdAccountId { get; set; }

    [ClientInvisible]
    public OprationTypeEnum EntryOprationType { get; set; }

    public string? AccountName { get; set; } = string.Empty;

    [ClientInvisible]
    public long EntryNumber { get; set; }

    [ClientInvisible]
    public long EntryDisplayNumber { get; set; }

    [ClientInvisible]
    public string EntryNote{ get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }

    [ClientInvisible]
    public string? Note { get; set; }
}