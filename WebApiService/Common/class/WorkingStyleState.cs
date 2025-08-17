using Common.Const;

namespace WebApiService.Common.Const
{
    /// <summary>
    /// 勤務形態
    /// </summary>
    public class WorkingStyleState : ConstBaseWithDictionary<WorkingStyleState>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="state">勤務形態</param>
        internal WorkingStyleState(string id, int state) 
            : base(id)
        {
            this.State = state;
            Dictionary.Add(id, this);
        }

        /// <summary>
        /// 勤務形態
        /// </summary>
        public int State { get; private set; }
    }
}