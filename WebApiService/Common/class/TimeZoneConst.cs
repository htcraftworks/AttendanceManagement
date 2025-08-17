using Common.Const;

namespace WebApiService.Common.Const
{
    /// <summary>
    /// タイムゾーン情報
    /// </summary>
    public class TimeZoneConst : ConstBaseWithDictionary<TimeZoneConst>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="timeZone">タイムゾーン</param>
        internal TimeZoneConst(string id, string timeZone) 
            : base(id)
        {
            this.TimeZone = timeZone;
            this.TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            Dictionary.Add(id, this);
        }

        /// <summary>
        /// タイムゾーン
        /// </summary>
        public string TimeZone { get; private set; }

        /// <summary>
        /// タイムゾーン情報
        /// </summary>
        public TimeZoneInfo TimeZoneInfo { get; private set; }
    }
}