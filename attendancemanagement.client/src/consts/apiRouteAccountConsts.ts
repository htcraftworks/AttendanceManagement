/** アカウント関連APIルート */
export default class ApiRouteAccountConsts {

    /** 親ルート */
    private static ACCOUNT_ROUTE: string = "/account";

    /** ログイン */
    static readonly Login: string = ApiRouteAccountConsts.ACCOUNT_ROUTE + "/login";

    /** ログアウト */
    static readonly Logout: string = ApiRouteAccountConsts.ACCOUNT_ROUTE + "/logout";

    /** アカウント作成 */
    static readonly UserCreate: string = ApiRouteAccountConsts.ACCOUNT_ROUTE + "/usercreate";

    /** 有効認証ユーザ取得 */
    static readonly UserCurrent: string = ApiRouteAccountConsts.ACCOUNT_ROUTE + "/usercurrent";
}