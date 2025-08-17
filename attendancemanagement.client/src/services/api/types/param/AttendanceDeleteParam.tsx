/** 勤怠削除パラメータ */
export interface AttendanceDeleteParam {

    /** 勤怠ID */
    attendanceId: string;

    /** 削除理由 */
    reason: string;
}