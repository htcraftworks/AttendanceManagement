namespace Data.Accessor
{
    /// <summary>
    /// 勤怠履歴データ
    /// </summary>
    public class AttendanceRecordsHistoryResult
    {
        /// <summary>
        /// 履歴登録日時
        /// </summary>
        public DateTime GenerateDateTime { get; set; }

        /// <summary>
        /// 更新理由
        /// </summary>
        public string Reason { get; set; } = string.Empty;
    }
}
