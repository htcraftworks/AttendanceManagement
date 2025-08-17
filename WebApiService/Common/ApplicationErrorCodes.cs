using WebApiService.Resource;

namespace WebApiService.Common.Const
{
    /// <summary>
    /// アプリケーションエラーコード定義
    /// NOTE:多言語化用にリソースのキーを設定
    /// </summary>
    public static class ApplicationErrorCodes
    {
        /// <summary>
        /// 成功
        /// </summary>
        public static ApplicationErrorCode Success = new ApplicationErrorCode("9001", nameof(ApplicationErrorResource.Success));

        /// <summary>
        /// 予期せぬエラー
        /// </summary>
        public static ApplicationErrorCode UnexpectedError = new ApplicationErrorCode("9002", nameof(ApplicationErrorResource.UnexpectedError));

        /// <summary>
        /// アカウント作成失敗
        /// </summary>
        public static ApplicationErrorCode AccounCreatFailedError = new ApplicationErrorCode("9003", nameof(ApplicationErrorResource.AccounCreatFailedError));

        /// <summary>
        /// 認証ユーザなし
        /// </summary>
        public static ApplicationErrorCode NotActiveUserError = new ApplicationErrorCode("9004", nameof(ApplicationErrorResource.NotActiveUserError));

        /// <summary>
        /// ログインエラー
        /// </summary>
        public static ApplicationErrorCode LoginError = new ApplicationErrorCode("9005", nameof(ApplicationErrorResource.NotActiveUserError));

        /// <summary>
        /// 有効なユーザではない
        /// </summary>
        public static ApplicationErrorCode NotValidUserError = new ApplicationErrorCode("9006", nameof(ApplicationErrorResource.NotValidUserError));

        /// <summary>
        /// 本日未退勤データあり
        /// </summary>
        public static ApplicationErrorCode NotRegistAttendanceClockOutError = new ApplicationErrorCode("9007", nameof(ApplicationErrorResource.NotRegistAttendanceClockOutError));

        /// <summary>
        /// 勤怠登録エラー：対象データなし
        /// </summary>
        public static ApplicationErrorCode NotDataAttendanceTargetError = new ApplicationErrorCode("9008", nameof(ApplicationErrorResource.NotDataAttendanceTargetError));

        /// <summary>
        /// 勤怠登録エラー：勤怠データなし
        /// </summary>
        public static ApplicationErrorCode NotDataAttendanceError = new ApplicationErrorCode("9009", nameof(ApplicationErrorResource.NotDataAttendanceError));

        /// <summary>
        /// パラメータ不正エラー
        /// </summary>
        public static ApplicationErrorCode ValidateParamError = new ApplicationErrorCode("9010", nameof(ApplicationErrorResource.ValidateParamError));

        /// <summary>
        /// 勤怠更新データなし
        /// </summary>
        public static ApplicationErrorCode NotDataAttendanceUpdateError = new ApplicationErrorCode("9011", nameof(ApplicationErrorResource.NotDataAttendanceUpdateError));

        /// <summary>
        /// ユーザーIDまたはパスワード無効。
        /// </summary>
        public static ApplicationErrorCode UserIdOrPasswordInvalidError = new ApplicationErrorCode("9012", nameof(ApplicationErrorResource.UserIdOrPasswordInvalidError));

        /// <summary>
        /// {1}文字以内で入力
        /// </summary>
        public static ApplicationErrorCode EnterWithInMaxLengthError = new ApplicationErrorCode("9013", nameof(ApplicationErrorResource.EnterWithInMaxLengthError));

        /// <summary>
        /// 既にアカウントが登録済みエラー
        /// </summary>
        public static ApplicationErrorCode AlreadyExitsAccountError = new ApplicationErrorCode("9014", nameof(ApplicationErrorResource.AlreadyExitsAccountError));

        /// <summary>
        /// エラー用のフォーマットを返します
        /// </summary>
        /// <param name="code">エラー</param>
        /// <returns>変換した文字列</returns>
        public static string ConvertErrorToString(ApplicationErrorCode code)
        {
            return $"[ErrorCode:{code.Id}]{code.ResourceKey}";
        }
    }
}
