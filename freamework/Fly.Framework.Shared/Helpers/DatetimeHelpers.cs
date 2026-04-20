namespace Fly.Framework.Shared.Helpers;

public static class DatetimeHelpers
{
    public static DateTime GetUTC() => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

    public static DateTime TOUtc(this DateTime dateTime, TimeSpan? span = null) => dateTime.Kind == DateTimeKind.Utc ? dateTime : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);


    private static DateTime? SetKindUtc(this DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return dateTime.Value.SetKindUtc();
        }
        else
        {
            return null;
        }
    }

    private static DateTime SetKindUtc(this DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc ? dateTime : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

}
