using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.Store.Dtos;

public class DtoItemComposition : DtoNamesBase
{
    public new string? Name { get; set; }
    
    public Identifier CategoryId { get; set; }
    
    public string? CategoryName { get; set; }

    [ClientInvisible]
    public IEnumerable<DtoItemCompositionDetail> Details { get; set; } = [];
    
    public string DetailsString
    {
        get => string.Join(", ", Details.Select(x => x.ItemName));
        set { } // This setter is just to avoid serialization issues
    }
    
    public decimal Count()
    {
        var realTotal = Details.Sum(s => s.CountPrice());
        return realTotal;
    }
}