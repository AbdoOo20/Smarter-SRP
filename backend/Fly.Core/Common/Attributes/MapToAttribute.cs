namespace Fly.Core.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class MapToAttribute : Attribute
{
    public Type DtoType { get; }

    public MapToAttribute(Type dtoType = null)
    {
        DtoType = dtoType;
    }
}
