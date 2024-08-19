public static class DateTimeManager
{
    public static string GetCurrentDateTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static bool TryParseDateTime(string input, out DateTime result)
    {
        return DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out result);
    }
}
