/** types */
import CustomHeaderPrefix from "./types/customHeaderPrefix"

/** 
 * 勤務形態定義
 */
export default class CustomHeaderPrefixConsts {

    /// <summary>
    /// 認証トークン
    /// </summary>
    public static AuthToken: CustomHeaderPrefix = new CustomHeaderPrefix("1", "X-Auth-Token", "x-auth-token", "Bearer ");
}