using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    /// <summary>
    /// 戻り値型：ストアド「usp_GetAttendanceListWithHistory」
    /// NOTE:EF COREの仕様で戻り値の型指定はキーレスエンティティである必要がある
    /// </summary>
    public class ResultModel_GetAttendanceListWithHistory
    {
        /// <summary>
        /// 勤怠ID
        /// </summary>
        [Required]
        public Guid AttendanceId { get; set; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        [Required]
        public string UserCode { get; set; } = string.Empty;

        /// <summary>
        /// 勤務日
        /// </summary>
        [Required]
        public DateOnly WorkDate { get; set; }

        /// <summary>
        /// 出勤時刻
        /// </summary>
        [Required]
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
        [Required]
        public int WorkingStyleState { get; set; } = 0;

        /// <summary>
        /// 登録日時
        /// </summary>
        [Required]
        public DateTime GenerateDateTime { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime? HistoryGenerateDateTime { get; set; }

        /// <summary>
        /// 理由
        /// </summary>
        public string? Reason { get; set; }
    }
}

#region stored procedure

////ストアド
//// 勤怠と履歴を月別に取得
// migrationBuilder.Sql(@"
//                CREATE PROCEDURE usp_GetAttendanceListWithHistory
//                    @UserCode   nvarchar(10),
//                    @DateTimeFrom date,
//                    @DateTimeTo date
//                AS
//                BEGIN
//                    SELECT
//                        r.AttendanceId,
//                        r.UserCode,
//                        r.WorkDate,
//                        r.ClockInDateTime,
//                        r.ClockOutDateTime,
//                        r.BreakInDateTime,
//                        r.BreakOutDateTime,
//                        r.WorkingStyleState,
//                        r.GenerateDateTime,
//                        h.GenerateDateTime AS HistoryGenerateDateTime,
//                        h.Reason
//                    FROM AttendanceRecords r
//                    LEFT JOIN AttendanceRecordsHistory h
//                        ON r.AttendanceId = h.AttendanceId
//                        AND r.UserCode = h.UserCode
//                        AND h.IsDelete = 'False'
//                    WHERE r.UserCode = @UserCode
//                        AND r.WorkDate BETWEEN @DateTimeFrom AND @DateTimeTo
//                        AND r.IsDelete = 'False'
//                    ORDER BY r.WorkDate ASC, r.GenerateDateTime ASC, h.GenerateDateTime ASC;
//            END");

#endregion