/** 汎用結果パラメータ */
export interface ServiceResult<T> {

    /** 結果 */
    isSuccess: boolean;

    /** エラーコード */
    errorCode: string;

    /** エラーメッセージ */
    errorMessage: string;

    /** 応答データ */
    value: T;
}