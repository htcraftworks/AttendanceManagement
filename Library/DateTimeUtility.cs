namespace Library;

/// <summary>
/// 日時型ユーティリティ
/// </summary>
public static class DateTimeUtility
{
    /// <summary>
    /// Start of Javascript Date
    /// </summary>
    private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// フォーマット
    /// </summary>
    public static string FormatInputDateTime { get; } = "yyyy/MM/dd HH:mm:ss";

    /// <summary>
    /// ユーザー時刻を取得します。
    /// </summary>
    /// <param name="timeZoneInfo">タイムゾーン情報</param>
    /// <returns> ユーザー時刻</returns>
    public static DateTime GetUserDateTimeNow(TimeZoneInfo timeZoneInfo)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
    }

    /// <summary>
    /// UTC時刻を取得します。
    /// </summary>
    /// <returns> ユーザー時刻</returns>
    public static DateTime GetUtcDateTimeNow()
    {
        return DateTime.UtcNow;
    }

    /// <summary>
    ///  DateTimeから指定フォーマット文字列に変換します。
    /// </summary>
    /// <param name="dateTime">日時</param>
    /// <param name="format">フォーマット</param>
    /// <returns>フォーマットされた文字列</returns>
    public static string ConvertToString(DateTime dateTime, string format)
    {
        return dateTime.ToString(format);
    }

    /// <summary>
    /// stringからDateTimeに変換します。
    /// </summary>
    /// <param name="dateTimeString">日時型文字列</param>
    /// <param name="format">フォーマット</param>
    /// <returns>日時</returns>
    public static DateTime ConvertToDateTime(string dateTimeString, string format)
    {
        return DateTime.ParseExact(dateTimeString, format, null);
    }

    /// <summary>
    /// DateTimeからDateOnlyに変換します。
    /// </summary>
    /// <param name="dateTime">日時</param>
    /// <returns>日付型</returns>
    public static DateOnly ConvertDateTimeToDateOnly(DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime);
    }

    /// <summary>
    /// DateOnlyからDateTimeに変換
    /// </summary>
    /// <param name="date">日時</param>
    /// <returns>日付型</returns>
    public static DateTime ConvertDateOnlyToDate(DateOnly date)
    {
        return date.ToDateTime(TimeOnly.MinValue);
    }

    /// <summary>
    /// 日付に時刻を足した日時を取得する
    /// </summary>
    /// <param name="dateTime">日時</param>
    /// <param name="addHour">時</param>
    /// <param name="addTime">分</param>
    /// <returns>日付型</returns>
    public static DateTime ConvertToDateToAddTime(DateTime dateTime, int addHour, int addTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, addHour, addTime, 0);
    }

    /// <summary>
    /// 指定月の開始と終了日時を取得する
    /// </summary>
    /// <param name="dateTime">指定月</param>
    /// <returns>Item1:開始日 Item2:終了日</returns>
    public static Tuple<DateOnly, DateOnly> ConvertToTargetMonthWithStartEnd(DateTime dateTime)
    {
        DateOnly startDate = new DateOnly(dateTime.Year, dateTime.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
        DateOnly endDate = new DateOnly(dateTime.Year, dateTime.Month, daysInMonth);

        return Tuple.Create(startDate, endDate);
    }

    /// <summary>
    /// 日付からUTC時刻のUNIX時間（秒）を取得します
    /// </summary>
    /// <param name="date">日付</param>
    /// <returns>UNIX時間（秒）</returns>
    public static double ConvertDataTimeToUnixTimeWithUtc(DateTime date)
    {
        return DateTimeUtility.ConvertDataTimeToUnixTimeWithTimeZone(date, true);
    }

    /// <summary>
    /// 日付からUNIX時間（秒）を取得します
    /// </summary>
    /// <param name="date">日付</param>
    /// <param name="isConvertTimezone">標準タイムゾーンに変換するか</param>
    /// <returns>UNIX時間（秒）</returns>
    public static double ConvertDataTimeToUnixTimeWithTimeZone(DateTime date, bool isConvertTimezone)
    {
        if (isConvertTimezone)
        {
            date = date.ToUniversalTime();
        }

        TimeSpan duration = date.Subtract(DateTimeUtility._unixEpoch);

        return Math.Floor(duration.TotalSeconds);
    }

    /// <summary>
    /// ユーザー時刻からUTC時刻に変換します
    /// </summary>
    /// <param name="userDateTime">ユーザー時刻</param>
    /// <param name="timeZoneInfo">タイムゾーン情報</param>
    /// <returns>UTC時刻</returns>
    public static DateTime ConvertUserDateTimeToUtcDateTIme(DateTime userDateTime, TimeZoneInfo timeZoneInfo)
    {
        return TimeZoneInfo.ConvertTimeToUtc(userDateTime, timeZoneInfo);
    }

    /// <summary>
    /// UTC時刻からユーザ時刻を取得します
    /// </summary>
    /// <param name="utcDateTime">UTC時刻</param>
    /// <param name="timeZoneInfo">タイムゾーン情報</param>
    /// <returns>ユーザ時刻</returns>
    public static DateTime ConvertUtcDateTimeToUserDateTime(DateTime utcDateTime, TimeZoneInfo timeZoneInfo)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZoneInfo);
    }

    /// <summary>
    /// UNIX時間（秒）からUTC時刻を取得します
    /// </summary>
    /// <param name="unixTimeSeconds">UNIX時間</param>
    /// <returns>UTC時刻</returns>
    public static DateTime? ConvertUnixTimeToUtcDateTime(string unixTimeSeconds)
    {
        DateTime? result = null;

        try
        {
            var seconds = double.Parse(unixTimeSeconds);
            result = DateTimeUtility._unixEpoch.AddSeconds(seconds);
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }

        return result;
    }

    /// <summary>
    /// UNIX時間からユーザ時刻を取得します
    /// </summary>
    /// <param name="unixTimeSeconds">UNIX時間（秒）</param>
    /// <param name="timeZoneInfo">タイムゾーン情報</param>
    /// <returns>ユーザ時刻</returns>
    public static DateTime? ConvertUnixTimeToUserDateTime(string unixTimeSeconds, TimeZoneInfo timeZoneInfo)
    {
        DateTime? result = null;

        try
        {
            var milliSeconds = double.Parse(unixTimeSeconds);
            result = TimeZoneInfo.ConvertTime(DateTimeUtility._unixEpoch.AddSeconds(milliSeconds), timeZoneInfo);
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }

        return result;
    }
}