using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo]
public class TCategory : TEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [Column("CategoryId")]
    public Identifier? CateguryId { get; set; }
    public TCategory? Category { get; set; }
}