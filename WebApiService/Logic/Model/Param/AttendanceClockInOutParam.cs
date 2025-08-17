using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using Swashbuckle.AspNetCore.Annotations;
using WebApiService.Resource;

namespace WebApiService.Logic.Model
{
    /// <summary>
    /// 出退勤登録パラメータモデル
    /// </summary>
    public class AttendanceClockInOutParam
    {
        /// <summary>
        /// 出退勤日時（UTC）のUNIX時間（秒）。例: 1672531200
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredClockInOutDateTimeUserUnixTimeSec))]
        [DataType(DataType.Text)]
        [SwaggerSchema(Description = "出退勤日時（UTC）のUNIX時間（秒）。例: 1672531200", Format = "string")]
        public string ClockInOutDateTimeUserUnixTimeSec { get; set; } = string.Empty;
    }
}
