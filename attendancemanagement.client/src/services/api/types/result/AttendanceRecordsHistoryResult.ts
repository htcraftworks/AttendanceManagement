/** 勤怠更新履歴結果パラメータ */
export interface AttendanceRecordsHistoryResult {

    /** 履歴時刻（UTC）のUNIX時間（秒） */
    generateDateUtcUnixTimeSec: string;

    /** 更新理由 */
    reason: string;
}