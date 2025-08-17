using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Common.Const;

namespace Library;

/// <summary>
/// ログ出力クラス
/// </summary>
public static class Logger
{
    /// <summary>
    /// ロックオブジェクト
    /// </summary>
    private static readonly object _lock = new object();

    /// <summary>
    /// インスタンス
    /// </summary>
    private static TraceSource? _logger;

    /// <summary>
    /// 自身のインスタンス
    /// </summary>
    private static TraceSource LoggerInstance => _logger ?? throw new InvalidOperationException("Logger is not initialized.");

    /// <summary>
    /// デバッグ出力
    /// </summary>
    /// <param name="msg">メッセージ</param>
    public static void Debug(string msg) => WriteLog(TraceEventType.Verbose, LoggerEventCodes.Debug.EventCode, msg);

    /// <summary>
    /// 情報出力
    /// </summary>
    /// <param name="msg">メッセージ</param>
    public static void Info(string msg) => WriteLog(TraceEventType.Information, LoggerEventCodes.Info.EventCode, msg);

    /// <summary>
    /// 警告出力
    /// </summary>
    /// <param name="msg">メッセージ</param>
    public static void Warn(string msg) => WriteLog(TraceEventType.Warning, LoggerEventCodes.Warn.EventCode, msg);

    /// <summary>
    /// エラー出力
    /// </summary>
    /// <param name="msg">メッセージ</param>
    public static void Error(string msg) => WriteLog(TraceEventType.Error, LoggerEventCodes.Error.EventCode, msg);

    /// <summary>
    /// 致命的エラー出力
    /// </summary>
    /// <param name="msg">メッセージ</param>
    public static void Fatal(string msg) => WriteLog(TraceEventType.Critical, LoggerEventCodes.Fatal.EventCode, msg);

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="loggerPrefix">ロガープレフィックス</param>
    public static void Initialize(string loggerPrefix)
    {
        lock (_lock)
        {
            if (_logger != null)
            {
                return;
            }

            _logger = new TraceSource(loggerPrefix, SourceLevels.All);

            if (_logger.Listeners.Count == 1 && _logger.Listeners[0] is DefaultTraceListener)
            {
                _logger.Listeners.Clear();
                _logger.Listeners.Add(new ConsoleTraceListener());

                string logPath = Path.Combine(AppContext.BaseDirectory, "Logs", $"{loggerPrefix}_{DateTime.Now:yyyyMMdd}.log");
                Directory.CreateDirectory(Path.GetDirectoryName(logPath) ?? string.Empty);
                _logger.Listeners.Add(new TextWriterTraceListener(logPath));
            }
        }
    }

    /// <summary>
    /// ログ出力本体
    /// </summary>
    private static void WriteLog(TraceEventType level, int eventId, string msg)
    {
        string formattedMessage = FormatMessage(msg);

        lock (_lock)
        {
            LoggerInstance.TraceEvent(level, eventId, formattedMessage);
            LoggerInstance.Flush();
        }
    }

    /// <summary>
    /// フォーマット済みメッセージを返します。
    /// </summary>
    /// <param name="msg">メッセージ</param>
    /// <returns>フォーマット済みメッセージ</returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string FormatMessage(string msg)
    {
        MethodBase? method = new StackFrame(2).GetMethod();
        if (method == null ||
            method.ReflectedType == null ||
            method.ReflectedType.FullName == null)
        {
            return "MethodBase is null.";
        }

        string fullName = method.ReflectedType.FullName;
        string name = method.Name;
        return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + " ThreadId->" + Thread.CurrentThread.ManagedThreadId + ", Class->" + fullName + ", Method->" + name + " : " + msg;
    }
}