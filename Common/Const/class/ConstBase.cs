namespace Common.Const
{
    /// <summary>
    /// 定義ベースクラス
    /// </summary>
    public class ConstBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        protected ConstBase(string id)
        {
            this.Id = id;
        }

        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; }
    }
}
