import { type ServiceResult } from "./result/ServiceResult";

/** APIの共通応答モデル */
export interface ApiResultModel<T> {

    /** HTTPステータスコード */
    statusCode: number,

    /** 応答データ */
    data: ServiceResult<T>
}