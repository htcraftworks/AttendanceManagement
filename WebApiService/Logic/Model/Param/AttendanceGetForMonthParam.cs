using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using WebApiService.Resource;

namespace WebApiService.Logic.Model
{
    /// <summary>
    /// 月間勤怠取得パラメータモデル
    /// </summary>
    public class AttendanceGetForMonthParam
    {
        /// <summary>
        /// 対象月（UTC）のUNIX時間（秒）。例: 1672531200
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredTargetDateUtcUnixTimeSec))]
        [DataType(DataType.Text)]
        [SwaggerSchema(Description = "対象月（UTC）のUNIX時間（秒）。例: 1672531200", Format = "string")]
        public string TargetDateUtcUnixTimeSec { get; set; } = string.Empty;
    }
}
