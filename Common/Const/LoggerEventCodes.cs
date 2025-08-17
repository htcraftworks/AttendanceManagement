namespace Common.Const
{
    /// <summary>
    /// <para>ログイベントコード定義</para>
    /// </summary>
    public static class LoggerEventCodes
    {
        /// <summary>
        /// デバッグ
        /// </summary>
        public static LoggerEventCode Debug { get; } = new LoggerEventCode(nameof(Debug), 0);

        /// <summary>
        /// 情報
        /// </summary>
        public static LoggerEventCode Info { get; } = new LoggerEventCode(nameof(Info), 0);

        /// <summary>
        /// 警告
        /// </summary>
        public static LoggerEventCode Warn { get; } = new LoggerEventCode(nameof(Warn), 8001);

        /// <summary>
        /// エラー
        /// </summary>
        public static LoggerEventCode Error { get; } = new LoggerEventCode(nameof(Error), 9001);

        /// <summary>
        /// 異常
        /// </summary>
        public static LoggerEventCode Fatal { get; } = new LoggerEventCode(nameof(Fatal), 10001);
    }
}
