using Common.Const;

namespace WebApiService.Common.Const
{
    /// <summary>
    /// アプリケーションエラーコード定義
    /// </summary>
    public class ApplicationErrorCode : ConstBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">エラーコード</param>
        /// <param name="resourceKey">リソースファイルのキー</param>
        internal ApplicationErrorCode(string id, string resourceKey) 
            : base(id)
        {
            this.ResourceKey = resourceKey;
        }

        /// <summary>
        /// リソースファイルのキー
        /// </summary>
        public string ResourceKey { get; private set; }
    }
}