using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using WebApiService.Common.Const;
using WebApiService.Resource;

namespace WebApiService.Logic
{
    /// <summary>
    /// 汎用結果クラス
    /// </summary>
    /// <typeparam name="T">応答データ型</typeparam>
    [Serializable]
    [DataContract]
    public class ServiceResult<T>
    {
        /// <summary>
        /// 結果。True:成功、False:失敗
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredAttendanceId))]
        [SwaggerSchema(Description = "勤怠ID。例: d290f1ee-6c54-4b01-90e6-d701748f0851", Format = "string")]

        public bool IsSuccess { get; private set; }

        /// <summary>
        /// エラーコード。例：9001
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredAttendanceId))]
        [SwaggerSchema(Description = "エラーコード。例: 9001", Format = "string")]

        public string ErrorCode { get; private set; } = string.Empty;

        /// <summary>
        /// エラーメッセージ。　例: ログインに失敗しました。
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredAttendanceId))]
        [SwaggerSchema(Description = "エラーメッセージ。例: ログインに失敗しました。", Format = "string")]

        public string ErrorMessage { get; private set; } = string.Empty;

        /// <summary>
        /// 応答データ型
        /// </summary>
        [SwaggerSchema(Description = "応答データの型", Format = "string")]

        public T? Value { get; set; }

        /// <summary>
        /// 成功時の応答データ作成
        /// </summary>
        /// <param name="value">型</param>
        /// <returns>応答データ</returns>
        public static ServiceResult<T> CreateSuccess(T value)
        {
            return ServiceResult<T>.CreateSuccess(value, string.Empty, string.Empty);
        }

        /// <summary>
        /// 成功時にエラー付き応答データ作成
        /// </summary>
        /// <param name="value">型</param>
        /// <param name="error">エラーコード定義</param>
        /// <returns>応答データ</returns>
        public static ServiceResult<T> CreateSuccessWithError(T value, ApplicationErrorCode error)
        {
            return ServiceResult<T>.CreateSuccess(value, error.Id, ApplicationErrorResource.ResourceManager.GetString(error.ResourceKey) ?? string.Empty);
        }

        /// <summary>
        /// 成功時の応答データ作成
        /// </summary>
        /// <param name="value">型</param>
        /// <param name="errorCode">エラーコード</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        /// <returns>応答データ</returns>
        public static ServiceResult<T> CreateSuccess(T value, string errorCode, string errorMessage)
        {
            var instance = new ServiceResult<T>();
            instance.IsSuccess = true;
            instance.Value = value;
            instance.ErrorCode = errorCode;
            instance.ErrorMessage = errorMessage;

            return instance;
        }

        /// <summary>
        /// エラー時の応答データ作成
        /// </summary>
        /// <param name="error">エラーコード定義</param>
        /// <returns>応答データ</returns>
        public static ServiceResult<T> CreateError(ApplicationErrorCode error)
        {
            return ServiceResult<T>.CreateError(error.Id, ApplicationErrorResource.ResourceManager.GetString(error.ResourceKey) ?? string.Empty);
        }

        /// <summary>
        /// エラー時の応答データ作成
        /// </summary>
        /// <param name="errorCode">エラーコード</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        /// <returns>応答データ</returns>
        public static ServiceResult<T> CreateError(string errorCode, string errorMessage)
        {
            var instance = new ServiceResult<T>();
            instance.ErrorCode = errorCode;
            instance.ErrorMessage = errorMessage;

            return instance;
        }
    }
}
