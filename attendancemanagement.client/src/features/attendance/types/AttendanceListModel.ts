/** types */
import { type AttendanceBaseModel } from "./AttendanceBaseModel"
import { type AttendanceHistoryModel } from "./AttendanceHistoryModel"

/** 勤怠一覧モデル */
export interface AttendanceListModel extends AttendanceBaseModel {

    /** 更新履歴配列 */
    history: AttendanceHistoryModel[] | null;
}