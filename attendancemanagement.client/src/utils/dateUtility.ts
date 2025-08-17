import TimeZoneConst from "../consts/types/timeZoneConst"
import TimeZoneConsts from "../consts/timeZoneConsts"

/** 日時型ユーティリティ */
export default class DateUtility {

    // Start of Javascript Date
    private static readonly unixEpoch: Date = new Date(Date.UTC(1970, 0, 1, 0, 0, 0));

    /** UNIX時間係数 */
    private static UNIXTIME_COEFFICIENT: number = 1000;

    /** フォーマット：日付 */
    static FORMAT_DATE: string = "YYYY/MM/DD";

    /** フォーマット：日付曜日 */
    static FORMAT_DATE_WEEKDAY: string = "YYYY年M月D日(曜)";

    /** フォーマット：年月日時刻 */
    static FORMAT_DATE_TIME: string = "YYYY/MM/DD HH:mm:ss";

    /** フォーマット：月日時刻 */
    static FORMAT_DATE_TIME_OUT_YEAR: string = "MM/DD HH:mm:ss";

    /** フォーマット：日時曜日 */
    static FORMAT_DATE_TIME_WEEKDAY: string = "YYYY年M月D日(曜) HH:mm:ss";

    /**
     * ユーザー時刻を取得します。
     * @param timeZoneConst タイムゾーン情報
     * @returns ユーザー時刻
     */
    public static getUserDateTimeNow(timeZoneConst: TimeZoneConst): Date {
        return new Date(new Date().toLocaleString(timeZoneConst.locale, { timeZone: timeZoneConst.timeZone }));
    }

    /** UNIX時間を取得します */
    public static getUnixTime(): number {
        return new Date(Date.now()).getTime() / DateUtility.UNIXTIME_COEFFICIENT;
    }

    /**
     * ユーザー時刻の年月を取得します
     * @param timeZoneConst タイムゾーン情報
     * @returns ユーザー時刻
     */
    public static getUserTimeNowForMonth(timeZoneConst: TimeZoneConst): Date {
        let date = new Date(new Date().toLocaleString(timeZoneConst.locale, { timeZone: timeZoneConst.timeZone }));
        return new Date(date.getFullYear(), date.getMonth(), 1);
    }

    /**
     * DateTimeから指定フォーマット文字列に変換します。
     * @param dateTime 日時
     * @param format フォーマット
     * @returns フォーマットされた文字列
     */
    public static convertToString(dateTime: Date, format: string): string {
        return DateUtility.formatDate(dateTime, format);
    }

    /**
     * stringからDateTimeに変換します。
     * @param dateTimeString 日時型文字列
     * @param format フォーマット
     * @returns 日時
     */
    public static convertToDateTime(dateTimeString: string): Date {
        return new Date(dateTimeString);
    }

    /**
     * 日付からUTC時刻のUNIX時間（秒）を取得します
     * @param date 日付
     * @returns UNIX時間（秒）
     */
    public static convertDateTimeToUnixTimeWithUtc(date: Date): number {
        return DateUtility.convertDateTimeToUnixTimeWithTimeZone(date, true);
    }

    /**
     * 日付からUNIX時間（秒）を取得します
     * @param date 日付
     * @param isConvertTimezone 標準タイムゾーンに変換するか
     * @returns UNIX時間（秒）
     */
    public static convertDateTimeToUnixTimeWithTimeZone(date: Date, isConvertTimezone: boolean): number {
        if (isConvertTimezone) {
            date = new Date(date.toUTCString());
        }

        const duration = date.getTime() - DateUtility.unixEpoch.getTime();
        return Math.floor(duration / DateUtility.UNIXTIME_COEFFICIENT);
    }

    /**
     * ユーザー時刻からUTC時刻に変換します
     * @param userDateTime ユーザー時刻
     * @param timeZoneConst タイムゾーン情報
     * @returns UTC時刻
     */
    public static convertUserDateTimeToUtcDateTIme(userDateTime: Date): Date {
        const utcTime = new Date(
            Date.UTC(userDateTime.getUTCFullYear(),
                userDateTime.getUTCMonth(),
                userDateTime.getUTCDate(),
                userDateTime.getUTCHours(),
                userDateTime.getUTCMinutes(),
                userDateTime.getUTCSeconds()));

        return utcTime;
    }

    /**
     * UTC時刻からユーザ時刻を取得します
     * @param utcDateTime UTC時刻
     * @param timeZoneConst タイムゾーン情報
     * @returns ユーザ時刻
     */
    public static convertUtcDateTimeToUserDateTime(utcDateTime: Date, timeZoneConst: TimeZoneConst): Date {
        return new Date(utcDateTime.toLocaleString(timeZoneConst.locale, { timeZone: timeZoneConst.timeZone }));
    }

    /**
     * UNIX時間（秒）からUTC時刻を取得します
     * @param unixTimeSeconds UNIX時間（秒）
     * @returns UTC時刻
     */
    public static convertUnixTimeToUtcDateTime(unixTimeSeconds: string): Date | null {
        try {
            const seconds = parseFloat(unixTimeSeconds);
            return new Date(DateUtility.unixEpoch.getTime() + seconds * 1000);
        } catch (error) {
            console.error(error);
            return null;
        }
    }

    /**
     * UNIX時間（秒）からユーザ時刻を取得します。
     * @param unixTimeSeconds UNIX時間（秒）
     * @param timeZoneConst タイムゾーン情報
     * @returns ユーザ時刻
     */
    static convertUnixTimeToUserDateTime(unixTimeSeconds: string, timeZoneConst: TimeZoneConst): Date | null {
        let date: Date | null = null;

        try {
            date = new Date(new Date(Number(unixTimeSeconds) * DateUtility.UNIXTIME_COEFFICIENT)
                .toLocaleString(timeZoneConst.locale, { timeZone: timeZoneConst.timeZone }));
        } catch (ex) {
            console.log(ex);
        }

        return date;
    }

    /**
     * Dateを指定フォーマットに変換します
     * @param date Date
     * @param format フォーマット
     * @returns フォーマット後の文字列
     */
    private static formatDate(date: Date, format: string): string {

        if (DateUtility.FORMAT_DATE == format) {
            return date.toLocaleDateString(TimeZoneConsts.Tokyo.locale, {
                year: "numeric",
                month: "2-digit",
                day: "2-digit"
            });
        } else if (DateUtility.FORMAT_DATE_WEEKDAY == format) {
            return date.toLocaleDateString(TimeZoneConsts.Tokyo.locale, {
                year: "numeric",
                month: "long",
                day: "numeric",
                weekday: "short"
            });
        } else if (DateUtility.FORMAT_DATE_TIME == format) {
            return date.toLocaleDateString(TimeZoneConsts.Tokyo.locale, {
                year: "numeric",
                month: "2-digit",
                day: "2-digit",
                hour: "2-digit",
                minute: "2-digit",
                second: "2-digit"
            });
        } else if (DateUtility.FORMAT_DATE_TIME_OUT_YEAR == format) {
            return date.toLocaleDateString(TimeZoneConsts.Tokyo.locale, {

                month: "2-digit",
                day: "2-digit",
                hour: "2-digit",
                minute: "2-digit",
                second: "2-digit"
            });
        } else if (DateUtility.FORMAT_DATE_TIME_WEEKDAY == format) {
            return date.toLocaleDateString(TimeZoneConsts.Tokyo.locale, {
                year: "numeric",
                month: "long",
                day: "numeric",
                weekday: "short",
                hour: "2-digit",
                minute: "2-digit",
                second: "2-digit"
            });
        } else {
            throw new Error(`NotFound: ${format}`);
        }
    }
}