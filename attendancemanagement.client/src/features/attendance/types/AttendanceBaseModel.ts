/** 勤怠モデル */
export interface AttendanceBaseModel {

    /** 勤怠ID */
    attendanceId: string;

    /** ユーザー */
    userCode: string;

    /** 勤務日 */
    workDate: Date | null;

    /** 出勤時刻 */
    clockInDateTime: Date | null;

    /** 退勤時刻 */
    clockOutDateTime: Date | null;

    /** 休憩開始時刻 */
    breakInDateTime: Date | null;

    /** 休憩終了時刻 */
    breakOutDateTime: Date | null;

    /** 勤務形態 */
    workingStyleState: number;
}