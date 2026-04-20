using System.Diagnostics.CodeAnalysis;

namespace Fly.Framework.Shared.Common;

//[JsonConverter(typeof(IdentifierConverter))]
public struct Identifier : IEquatable<Identifier>, IParsable<Identifier>, IIdentifier
{
    public static readonly Identifier Empty = Guid.Empty;
    public Guid Value { get; set; }
    
    public Identifier() { }
    private Identifier(Guid id) { Value = id; }

    public bool Equals(Identifier other)
    {
        // TODO: Check if empity guid may equals to other empity guid
        return other.Value == Value;
    }

    public bool IsNullOrEmpty()
    {
        var v = Value as Guid?;
        return v is null || Value == Guid.Empty;
    }
    public Identifier GetValueOrNull()
    {
        SetNullIfEmpty();
        return this;
    }
    public void SetNullIfEmpty()
    {
        if(Value == Guid.Empty)
        {
            Value = default;
        }
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Identifier id && id.Value.Equals(Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode() * 41;
    }

    public override string ToString()
    {
        return $"{Value}";
    }

    #region Static Methods

    public static Identifier Create(Guid id)
    {
        return new Identifier(id);
    }

    public static Identifier New()
    {
        return new Identifier();
    }

    public static Identifier Parse(string? guid)
    {
        var isParsed = Guid.TryParse(guid, out var id);
        if (isParsed)
        {
            return new Identifier(id);
        }
        return New();
    }

    public static Identifier Parse(Guid guid)
    {
        return new Identifier(guid);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Identifier result)
    {
        try
        {
            if (s is null)
            {
                result = Empty;
                return false;
            }
            result = Parse(s);
            return true;
        }
        catch (Exception)
        {
            result = Empty;
            return false;
        }
    }

    public static Identifier Parse(string s, IFormatProvider? provider)
    {
        return Parse(s);
    }

    public static implicit operator Identifier(Guid value) => Create(value);
    public static implicit operator Guid(Identifier value) => value.Value; // TODO : Check this issue
    //public static implicit operator Guid(Identifier? value) =>  value?.Value ?? Guid.NewGuid(); // TODO : Check this issue
    public static bool operator ==(Identifier left, Identifier right) => left.Value.Equals(right.Value);
    public static bool operator !=(Identifier left, Identifier right) => !(left == right);

    //public static bool operator ==(Identifier? left, Identifier right) => left.HasValue ? left.Value.Equals(right.Value) : false;
    //public static bool operator !=(Identifier? left, Identifier right) => !(left == right);
    //public static bool operator ==(Identifier left, Identifier? right) => right.HasValue ? left.Value.Equals(right.Value) : false;
    //public static bool operator !=(Identifier left, Identifier? right) => !(right == left);

    #endregion

}

public interface IIdentifier
{
    public Guid Value { get; set; }
}
