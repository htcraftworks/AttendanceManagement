using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using WebApiService.Resource;

namespace WebApiService.Logic.Model
{
    /// <summary>
    /// 勤怠削除パラメータモデル
    /// </summary>
    public class AttendanceDeleteParam
    {
        /// <summary>
        /// 勤怠ID。例: d290f1ee-6c54-4b01-90e6-d701748f0851
        /// </summary>
        /// [Required]
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredAttendanceId))]
        [DataType(DataType.Text)]
        [SwaggerSchema(Description = "勤怠ID。例: d290f1ee-6c54-4b01-90e6-d701748f0851", Format = "string")]
        public string AttendanceId { get; set; } = string.Empty;

        /// <summary>
        /// 更新理由。誤登録のため削除
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredReason))]
        [DataType(DataType.Text)]
        [SwaggerSchema(Description = "更新理由。例: 誤登録のため削除")]
        public string Reason { get; set; } = string.Empty;
    }
}
