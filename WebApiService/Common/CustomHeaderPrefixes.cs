namespace WebApiService.Common.Const
{
    /// <summary>
    /// カスタムヘッダーのプレフィックス定義
    /// </summary>
    public static class CustomHeaderPrefixes
    {
        /// <summary>
        /// 認証トークン
        /// </summary>
        public static CustomHeaderPrefix AuthToken = new CustomHeaderPrefix("1", "X-Auth-Token", "Bearer ");
    }
}