using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Data.Entities
{
    /// <summary>
    /// 勤怠記録履歴エンティティ
    /// </summary>
    [PrimaryKey(nameof(AttendanceId), nameof(UserCode), nameof(GenerateDateTime))]
    [Index(nameof(UserCode), Name = "IX_AttendanceRecordsHistory_UserCode")]
    [Index(nameof(AttendanceId), nameof(UserCode), Name = "IX_AttendanceRecordsHistory_AttendanceId_UserCode")]

    public class AttendanceRecordsHistory
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
        /// 登録日時
        /// </summary>
        [Required]
        public DateTime GenerateDateTime { get; set; }

        /// <summary>
        /// 理由
        /// </summary>
        [Required]
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// 論理削除フラグ
        /// </summary>
        [Required]
        public bool IsDelete { get; set; }
    }
}
