using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using WebApiService.Resource;

namespace WebApiService.Logic
{
    /// <summary>
    /// 勤怠履歴データ
    /// </summary>
    public class AttendanceRecordsHistoryResult
    {
        /// <summary>
        /// 履歴時刻（UTC）のUNIX時間（秒）。勤怠登録忘れのため。例: 1672531200
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredGenerateDateUtcUnixTimeSec))]
        [SwaggerSchema(Description = "履歴時刻（UTC）のUNIX時間（秒）。例: 1672531200", Format = "string")]
        public string GenerateDateUtcUnixTimeSec { get; set; } = string.Empty;

        /// <summary>
        /// 更新理由。勤怠登録忘れのため。
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredReason))]
        [SwaggerSchema(Description = "更新理由。例: 勤怠登録忘れのため。")]
        public string Reason { get; set; } = string.Empty;
    }
}
