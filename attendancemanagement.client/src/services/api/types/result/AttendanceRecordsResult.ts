/** types */
import { type AttendanceRecordsHistoryResult } from "./AttendanceRecordsHistoryResult"

/** 勤怠情報結果パラメータ */
export interface AttendanceRecordsResult {

    /** 勤怠ID */
    attendanceId: string;

    /** ユーザー */
    userCode: string;

    /** 勤務日のUNIX時間（秒） */
    workDateUserUnixTimeSec: string;

    /** 出勤時刻（UTC）のUNIX時間（秒） */
    clockInDateUtcUnixTimeSec: string;

    /** 退勤時刻（UTC）のUNIX時間（秒） */
    clockOutDateUtcUnixTimeSec: string | null;

    /** 休憩開始時刻（UTC）のUNIX時間（秒） */
    breakInDateUtcUnixTimeSec: string | null;

    /** 休憩終了時刻（UTC）のUNIX時間（秒） */
    breakOutDateUtcUnixTimeSec: string | null;

    /** 勤務形態 */
    workingStyleState: number;

    /** 更新履歴配列 */
    history: AttendanceRecordsHistoryResult[] | null;
}