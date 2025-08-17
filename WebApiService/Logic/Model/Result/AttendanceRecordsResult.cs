using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using WebApiService.Resource;

namespace WebApiService.Logic
{
    /// <summary>
    /// 勤怠情報結果モデル
    /// </summary>
    public class AttendanceRecordsResult
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
        [SwaggerSchema(Description = "ユーザーコード。例: user123", Format = "string")]
        public string UserCode { get; set; } = string.Empty;

        /// <summary>
        /// 勤務日のUNIX時間（秒）。例: 1672531200
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredWorkDateUserUnixTimeSec))]
        [SwaggerSchema(Description = "勤務日のUNIX時間（秒）。例: 1672531200", Format = "string")]
        public string WorkDateUserUnixTimeSec { get; set; } = string.Empty;

        /// <summary>
        /// 出勤時刻（UTC）のUNIX時間（秒）。例: 1672531200
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredClockInDateUtcUnixTimeSec))]
        [SwaggerSchema(Description = "出勤時刻（UTC）のUNIX時間（秒）。例: 1672531200", Format = "string")]
        public string ClockInDateUtcUnixTimeSec { get; set; } = string.Empty;

        /// <summary>
        /// 退勤時刻（UTC）のUNIX時間（秒）。例: 1672531200
        /// </summary>
        [SwaggerSchema(Description = "退勤時刻（UTC）のUNIX時間（秒）。例: 1672560000", Format = "string", Nullable = true)]
        public string? ClockOutDateUtcUnixTimeSec { get; set; }

        /// <summary>
        /// 休憩開始時刻（UTC）のUNIX時間（秒）。例: 1672531200
        /// </summary>
        [SwaggerSchema(Description = "休憩開始時刻（UTC）のUNIX時間（秒）。例: 1672542000", Format = "string", Nullable = true)]
        public string? BreakInDateUtcUnixTimeSec { get; set; }

        /// <summary>
        /// 休憩終了時刻（UTC）のUNIX時間（秒）。例: 1672531200
        /// </summary>
        [SwaggerSchema(Description = "休憩終了時刻（UTC）のUNIX時間（秒）。例: 1672545600", Format = "string", Nullable = true)]
        public string? BreakOutDateUtcUnixTimeSec { get; set; }

        /// <summary>
        /// 勤務形態。1:出社、2:在宅、3:外出
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredWorkingStyleState))]
        [SwaggerSchema(Description = "勤務形態。例: 0")]
        public int WorkingStyleState { get; set; } = 0;

        /// <summary>
        /// 更新履歴
        /// </summary>
        [SwaggerSchema(Description = "更新履歴")]
        public List<AttendanceRecordsHistoryResult>? History { get; set; }
    }
}
