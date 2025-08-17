namespace Data.Accessor
{
    /// <summary>
    /// 勤怠登録パラメータ
    /// </summary>
    public class InsertAttendanceParam
    {
        /// <summary>
        /// 勤怠ID
        /// </summary>
        public Guid AttendanceId { get; set; }

        /// <summary>
        /// ユーザーコード
        /// </summary>
        public string UserCode { get; set; } = string.Empty;

        /// <summary>
        /// 勤務日
        /// </summary>
        public DateOnly WorkDate { get; set; }

        /// <summary>
        /// 出勤時刻
        /// </summary>
        public DateTime ClockInDateTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 退勤時刻
        /// </summary>
        public DateTime? ClockOutDateTime { get; set; }

        /// <summary>
        /// 休憩開始時刻
        /// </summary>
        public DateTime? BreakInDateTime { get; set; }

        /// <summary>
        /// 休憩終了時刻
        /// </summary>
        public DateTime? BreakOutDateTime { get; set; }

        /// <summary>
        /// 勤務形態
        /// </summary>
        public int WorkingStyleState { get; set; } = 0;
    }
}
