namespace Nu_Models.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ConvertDateTimeToUtc(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(dateTime);
    }
}