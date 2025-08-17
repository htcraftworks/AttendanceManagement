namespace WebApiService.Common.Const
{
    /// <summary>
    /// <para>勤務形態定義</para>
    /// </summary>
    public class WorkingStyleStates
    {
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static WorkingStyleStates()
        {
            Instance = new WorkingStyleStates();
        }

        /// <summary>
        /// プライベートコンストラクタ
        /// </summary>
        private WorkingStyleStates()
        {
        }

        /// <summary>
        /// インスタンス
        /// </summary>
        public static WorkingStyleStates Instance { get; private set; }

        /// <summary>
        /// 出社
        /// </summary>
        public static WorkingStyleState Office { get; } = new WorkingStyleState(nameof(Office), 1);

        /// <summary>
        /// 外出
        /// </summary>
        public static WorkingStyleState GoOut { get; } = new WorkingStyleState(nameof(GoOut), 2);

        /// <summary>
        /// 在宅
        /// </summary>
        public static WorkingStyleState Home { get; } = new WorkingStyleState(nameof(Home), 3);

        /// <summary>
        /// 一致する勤務形態を取得します
        /// </summary>
        /// <param name="state">勤務形態コード</param>
        /// <returns>勤務形態</returns>
        public static WorkingStyleState? GetState(int state)
        {
            return WorkingStyleState.GetAll().FirstOrDefault(x => x.State == state);
        }
    }
}
