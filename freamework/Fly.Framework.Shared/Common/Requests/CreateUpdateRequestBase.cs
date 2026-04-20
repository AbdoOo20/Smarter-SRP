namespace Fly.Framework.Shared.Common.Requests;

public class CreateUpdateRequestBase : UpdateRequestBase
{
    public bool IsNew() => Id == Identifier.Empty;
}

public class CreateUpdateNamedRequestBase : CreateUpdateRequestBase
{
    public Identifier? CateguryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}