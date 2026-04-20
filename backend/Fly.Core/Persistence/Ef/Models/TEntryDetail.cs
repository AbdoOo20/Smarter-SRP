using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Persistence.Ef.Models;

[MapTo]
public class TEntryDetail : TEntity
{
    public Identifier EntryId { get; set; }
    public virtual TEntry? Entry { get; set; }
    public Identifier AccountId { get; set; }
    public virtual TAccount? Account { get; set; }
    //public long EntryNumber { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Note { get; set; }
}