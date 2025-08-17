/** ログイン画面モデル */
export interface LoginModel {

    /** ユーザーコード */
    userCode: string;

    /** パスワード */
    password: string;

    /** エラー */
    error: null;
}