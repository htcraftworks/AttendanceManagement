namespace Data.Accessor
{
    /// <summary>
    /// 勤怠情報削除パラメータ
    /// </summary>
    public class DeleteAttendanceParam
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
        /// 更新理由
        /// </summary>
        public string Reason { get; set; } = string.Empty;
    }
}
