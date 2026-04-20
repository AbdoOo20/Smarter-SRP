using Newtonsoft.Json;

namespace Fly.Framework.Shared.Extentions;

public static class ObjectExtentions
{
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
}
public static class DateTimeExtentions
{
    public static string ToDottedDate(this DateTimeOffset dateTime)
    {
        return dateTime.ToString("yyyy.MM.dd");
    }
        
    public static string ToDashedDate(this DateTimeOffset dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }
        
    public static string ToDashedDate(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }
        
    public static string ToDottedDate(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy.MM.dd");
    }
        
    public static string ToISO8601(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
    }
        
    public static string ToDottedDate(this DateTimeOffset? dateTime)
    {
        return (dateTime is null) ? "" : dateTime.Value.ToDottedDate();
    }
        
    public static string ToDottedDate(this DateTime? dateTime)
    {
        return (dateTime is null) ? "" : dateTime.Value.ToDottedDate();
    }
        
    public static string ToDashedDate(this DateTimeOffset? dateTime)
    {
        return (dateTime is null) ? "" : dateTime.Value.ToDashedDate();
    }
        
    public static string ToDashedDate(this DateTime? dateTime)
    {
        return (dateTime is null) ? "" : dateTime.Value.ToDashedDate();
    }
        
    public static string ToISO8601(this DateTime? dateTime)
    {
        return (dateTime is null) ? "" : dateTime.Value.ToISO8601();
    }
}