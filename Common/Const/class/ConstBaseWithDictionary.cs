namespace Common.Const
{
    /// <summary>
    /// 辞書付き定義ベースクラス
    /// </summary>
    /// <typeparam name="T">型</typeparam>
    public abstract class ConstBaseWithDictionary<T> : ConstBase
    {
        /// <summary>
        /// 辞書
        /// </summary>
        private static Dictionary<string, T>? _dic;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        protected ConstBaseWithDictionary(string id) 
            : base(id)
        {
        }

        /// <summary>
        /// 定義辞書
        /// </summary>
        protected static Dictionary<string, T> Dictionary
        {
            get
            {
                LazyInitializer.EnsureInitialized(ref _dic, () => new Dictionary<string, T>());
                return _dic;
            }
        }

        /// <summary>
        /// 全定義を取得します
        /// </summary>
        /// <returns>定義リスト</returns>
        public static List<T> GetAll() => Dictionary.Values.ToList();
    }
}
