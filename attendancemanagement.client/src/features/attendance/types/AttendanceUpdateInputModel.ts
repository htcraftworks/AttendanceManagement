/** types */
import { type AttendanceBaseModel } from "./AttendanceBaseModel"

/** 勤怠更新画面入力モデル */
export interface AttendanceUpdateInputModel extends AttendanceBaseModel {

    /** 理由 */
    reason: string | null;
}