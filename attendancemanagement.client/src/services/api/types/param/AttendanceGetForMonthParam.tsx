/** 月間勤怠取得パラメータ */
export interface AttendanceGetForMonthParam {

    /** 対象月（UTC）のUNIX時間（秒） */
    targetDateUtcUnixTimeSec: string;
}