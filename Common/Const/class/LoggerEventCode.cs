namespace Common.Const
{
    /// <summary>
    /// 勤務形態
    /// </summary>
    public class LoggerEventCode : ConstBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="eventCode">イベントコード</param>
        internal LoggerEventCode(string id, int eventCode) 
            : base(id)
        {
            this.EventCode = eventCode;
        }

        /// <summary>
        /// イベントコード
        /// </summary>
        public int EventCode { get; private set; }
    }
}