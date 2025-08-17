namespace WebApiService.Logic.Logic
{
    /// <summary>
    /// アカウントロジック
    /// </summary>
    public static class AccountLogic
    {
        /// <summary>
        /// アカウント作成の入力値検証を行います。
        /// </summary>
        /// <param name="account">アカウント</param>
        /// <returns>検証結果辞書</returns>
        public static Dictionary<string, string> CreateUserValidate(UserCreateParam account)
        {
            Dictionary<string, string> validateDic = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(account.UserCode))
            {
                validateDic.Add(nameof(account.UserCode), "empty data.");
            }

            if (string.IsNullOrWhiteSpace(account.UserName))
            {
                validateDic.Add(nameof(account.UserName), "empty data.");
            }

            if (string.IsNullOrWhiteSpace(account.Password))
            {
                validateDic.Add(nameof(account.Password), "empty data.");
            }

            return validateDic;
        }
    }
}
