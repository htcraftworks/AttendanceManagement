namespace WebApiService.Common.Const
{
    /// <summary>
    /// AccountRouteコントローラー定義
    /// </summary>
    public static class AccountControllerActionsRoutes
    {
        /// <summary>
        /// ログイン
        /// </summary>
        public const string Login = nameof(Login);

        /// <summary>
        /// ログアウト
        /// </summary>
        public const string Logout = nameof(Logout);

        /// <summary>
        /// カレントユーザー
        /// </summary>
        public const string UserCurrent = nameof(UserCurrent);

        /// <summary>
        /// ユーザ作成
        /// </summary>
        public const string UserCreate = nameof(UserCreate);
    }
}
