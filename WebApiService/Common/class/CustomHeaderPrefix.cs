using Common.Const;

namespace WebApiService.Common.Const
{
    /// <summary>
    /// カスタムヘッダーのプレフィックス定義
    /// </summary>
    public class CustomHeaderPrefix : ConstBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="headerName">カスタムヘッダー名</param>
        /// <param name="prefix">プレフィックス</param>
        internal CustomHeaderPrefix(string id, string headerName, string prefix) 
            : base(id)
        {
            this.HeaderName = headerName;
            this.Prefix = prefix;
        }

        /// <summary>
        /// カスタムヘッダー名
        /// </summary>
        public string HeaderName { get; private set; }

        /// <summary>
        /// プレフィックス
        /// </summary>
        public string Prefix { get; private set; }
    }
}