using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    /// <summary>
    /// 勤怠記録エンティティ
    /// </summary>
    [PrimaryKey(nameof(AttendanceId), nameof(UserCode))]
    [Index(nameof(UserCode), Name = "IX_AttendanceRecords_UserCode")]
    [Index(nameof(WorkDate), Name = "IX_AttendanceRecords_WorkDate")]
    [Index(nameof(UserCode), nameof(WorkDate), Name = "IX_AttendanceRecords_UserCode_WorkDate")]
    [Index(nameof(AttendanceId), nameof(UserCode), Name = "IX_AttendanceRecords_AttendanceId_UserCode")]

    public class AttendanceRecords
    {
        /// <summary>
        /// 勤怠ID
        /// </summary>
        public Guid AttendanceId { get; set; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        [Required]
        [StringLength(10)]
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
        /// 更新日時
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 論理削除フラグ
        /// </summary>
        [Required]
        public bool IsDelete { get; set; }
    }
}
