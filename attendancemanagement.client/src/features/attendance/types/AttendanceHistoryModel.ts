/** 勤怠履歴画面モデル */
export interface AttendanceHistoryModel {

    /** 登録日時 */
    generateDateTime: Date | null;

    /** 理由 */
    reason: string;
}