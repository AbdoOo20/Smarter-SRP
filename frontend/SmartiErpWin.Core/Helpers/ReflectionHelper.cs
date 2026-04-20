namespace SmartErpWin.Core.Helpers;

public static class ReflectionHelper
{
    public static T? GetAttributeOfType<T>(Enum enumVal) where T: Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        
        return attributes.Length > 0 ? (T)attributes[0] : null;
    }
}