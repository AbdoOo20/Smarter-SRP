using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Common.Markers;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Enums;

namespace Fly.Core.Persistence.Ef.Models;

[MapTo]
public class TEntry : TEntity, INumberEntity
{
    public long Number { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public virtual List<TEntryDetail> Details { get; set; } = [];

    public OprationTypeEnum? OprationType { get; set; }
    public int? RelatedIndex { get; set; }
    public Identifier? RelatedId { get; set; }
    public long DisplayNumber { get; set; }
    public long? SerialNumber { get; set; }

    public TagTypeEnum? Tag { get; set; }
}