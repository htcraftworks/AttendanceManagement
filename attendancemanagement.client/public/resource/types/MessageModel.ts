/** メッセージ定義の型 */
export interface MessageModel {

    /** メッセージ格納配列 */
    [locale: string]: { key: string, value: string }[]
};