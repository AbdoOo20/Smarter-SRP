using System.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.Store.Dtos;

public class DtoCategory : DtoNamesBase
{
    [ClientInvisible]
    public Identifier? CateguryId { get; set; }
    
    [DisplayName("ParentCategory")]
    public string? CategoryName { get; set; } = "";
}

public class DtoItemQuntityStatement
{
    public int Quantity { get; set; }
    public int AddtionalQuantity { get; set; }
}
