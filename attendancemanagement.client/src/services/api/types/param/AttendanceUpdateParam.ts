/** 勤怠更新送信パラメータ */
export interface AttendanceUpdateParam {

    /** 勤怠ID */
    attendanceId: string;

    /** ユーザーコード */
    userCode: string;

    /** 出勤時刻（UTC）のUNIX時間（秒） */
    clockInUtcDateUnixTimeSec: string | null;

    /** 退勤時刻（UTC）のUNIX時間（秒） */
    clockOutUtcDateUnixTimeSec: string | null;

    /** 休憩開始時刻（UTC）のUNIX時間（秒） */
    breakInDateUtcUnixTimeSec: string | null;

    /** 休憩終了時刻（UTC）のUNIX時間（秒） */
    breakOutDateUtcUnixTimeSec: string | null;

    /** 勤務形態 */
    workingStyleState: number;

    /** 更新理由 */
    reason: string;
}