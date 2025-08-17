using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using WebApiService.Resource;

namespace WebApiService.Logic.Model
{
    /// <summary>
    /// 勤怠取得パラメータモデル
    /// </summary>
    public class AttendanceGetParam
    {
        /// <summary>
        /// 勤怠ID。例: d290f1ee-6c54-4b01-90e6-d701748f0851
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredAttendanceId))]
        [SwaggerSchema(Description = "勤怠ID。例: d290f1ee-6c54-4b01-90e6-d701748f0851", Format = "string")]
        public Guid AttendanceId { get; set; }
    }
}
