using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using WebApiService.Resource;

namespace WebApiService.Logic.Model
{
    /// <summary>
    /// 勤怠更新パラメータモデル
    /// </summary>
    public class AttendanceUpdateParam
    {
        /// <summary>
        /// 勤怠ID。例: d290f1ee-6c54-4b01-90e6-d701748f0851
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredAttendanceId))]
        [SwaggerSchema(Description = "勤怠ID。例: d290f1ee-6c54-4b01-90e6-d701748f0851", Format = "string")]
        public Guid AttendanceId { get; set; }

        /// <summary>
        /// ユーザーコード。例: user123
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredUserCode))]
        [DataType(DataType.Text)]
        [SwaggerSchema(Description = "ユーザーコード。例: user123", Format = "string")]
        public string UserCode { get; set; } = string.Empty;

        /// <summary>
        /// 出勤時刻（UTC）のUNIX時間（秒）。例: 1672531200
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredClockInDateUtcUnixTimeSec))]
        [DataType(DataType.Text)]
        [SwaggerSchema(Description = "出勤時刻（UTC）のUNIX時間（秒）。例: 1672531200", Format = "string", Nullable = true)]
        public string? ClockInUtcDateUnixTimeSec { get; set; }

        /// <summary>
        /// 退勤時刻（UTC）のUNIX時間（秒）。例: 1672560000
        /// </summary>
        [DataType(DataType.Text)]
        [SwaggerSchema(Description = "退勤時刻（UTC）のUNIX時間（秒）。例: 1672560000", Format = "string", Nullable = true)]
        public string? ClockOutUtcDateUnixTimeSec { get; set; }

        /// <summary>
        /// 休憩開始時刻（UTC）のUNIX時間（秒）。例: 1672542000
        /// </summary>
        [DataType(DataType.Text)]
        [SwaggerSchema(Description = "休憩開始時刻（UTC）のUNIX時間（秒）。例: 1672542000", Format = "string", Nullable = true)]
        public string? BreakInDateUtcUnixTimeSec { get; set; }

        /// <summary>
        /// 休憩終了時刻（UTC）のUNIX時間（秒）。例: 1672545600
        /// </summary>
        [DataType(DataType.Text)]
        [SwaggerSchema(Description = "休憩終了時刻（UTC）のUNIX時間（秒）。例: 1672545600", Format = "string", Nullable = true)]
        public string? BreakOutDateUtcUnixTimeSec { get; set; }

        /// <summary>
        /// 勤務形態。1:出社、2:在宅、3:外出
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredWorkingStyleState))]
        [SwaggerSchema(Description = "勤務形態。1:出社、2:在宅、3:外出")]
        public int WorkingStyleState { get; set; }

        /// <summary>
        /// 更新理由。例: 勤怠修正のため。
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredWorkingReason))]
        [SwaggerSchema(Description = "更新理由。例: 勤怠修正のため")]
        public string Reason { get; set; } = string.Empty;
    }
}
