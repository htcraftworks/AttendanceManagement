/** types */
import { type UserModel } from "../../types/UserModel";

/** アカウントContextモデル */
export interface AccountContextModel {

    /** ユーザー情報 */
    user: UserModel,

    /** ユーザー情報登録ダミーセッター */
    setUser: (user: UserModel) => void;

    /** ロケール */
    locale: string,

    /** ユーザー情報登録ダミーセッター */
    setLocale: (locale: string) => void;
}