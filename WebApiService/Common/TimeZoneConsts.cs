namespace WebApiService.Common.Const
{
    /// <summary>
    /// <para>タイムゾーン情報定義</para>
    /// </summary>
    public class TimeZoneConsts
    {
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static TimeZoneConsts()
        {
            Instance = new TimeZoneConsts();
        }

        /// <summary>
        /// プライベートコンストラクタ
        /// </summary>
        private TimeZoneConsts()
        {
        }

        /// <summary>
        /// インスタンス
        /// </summary>
        public static TimeZoneConsts Instance { get; private set; }

        /// <summary>
        /// 東京
        /// </summary>
        public static TimeZoneConst Tokyo { get; } = new TimeZoneConst(nameof(Tokyo), "Tokyo Standard Time");

        /// <summary>
        /// 一致する勤務形態を取得します
        /// </summary>
        /// <param name="timeZone">タイムゾーン</param>
        /// <returns>勤務形態</returns>
        public static TimeZoneConst? GetState(string timeZone)
        {
            return TimeZoneConst.GetAll().FirstOrDefault(x => x.TimeZone == timeZone);
        }
    }
}
