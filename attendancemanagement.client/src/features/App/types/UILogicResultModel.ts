/** UIロジック応答モデル*/
export interface UILogicResultModel<T> {

    /** 結果 */
    isSuccess: boolean;

    /** メッセージタイトル */
    messageTitle: string | null;

    /** メッセージ */
    message: string | null;

    /** 応答データ */
    value: T | null;
}