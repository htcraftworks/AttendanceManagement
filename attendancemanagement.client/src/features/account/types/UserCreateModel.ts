/** types */
import { type LoginModel } from "./LoginModel";

/** ユーザ作成画面モデル */
export interface UserCreateModel extends LoginModel {

    /** ユーザー名 */
    userName: string;
}