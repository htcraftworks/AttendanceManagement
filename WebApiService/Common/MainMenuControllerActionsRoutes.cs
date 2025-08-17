namespace WebApiService.Common.Const
{
    /// <summary>
    /// MainMenuRouteコントローラー定義
    /// </summary>
    public static class MainMenuControllerActionsRoutes
    {
        /// <summary>
        /// 出勤登録状況
        /// </summary>
        public const string AttendanceClockInState = nameof(AttendanceClockInState);

        /// <summary>
        /// 出勤データ登録
        /// </summary>
        public const string AttendanceClockIn = nameof(AttendanceClockIn);

        /// <summary>
        /// 退勤データ登録
        /// </summary>
        public const string AttendanceClockOut = nameof(AttendanceClockOut);

        /// <summary>
        /// 勤怠取得
        /// </summary>
        public const string AttendanceGet = nameof(AttendanceGet);

        /// <summary>
        /// 指定月の勤怠取得
        /// </summary>
        public const string AttendanceGetForMonth = nameof(AttendanceGetForMonth);

        /// <summary>
        /// 勤怠編集
        /// </summary>
        public const string AttendanceUpdate = nameof(AttendanceUpdate);

        /// <summary>
        /// 出退勤削除
        /// </summary>
        public const string AttendanceDelete = nameof(AttendanceDelete);
    }
}
