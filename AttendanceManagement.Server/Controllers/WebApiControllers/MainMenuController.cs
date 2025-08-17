using AttendanceManagement.Server.Services;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Server.Controllers
{
    /// <summary>
    /// メニューコントローラー（分割クラス）
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public partial class MainMenuController : ApiControllerBase
    {
        #region constructor

        /// <summary>
        ///  コンストラクタ
        /// </summary>
        /// <param name="userManager">ユーザー管理</param>
        /// <param name="authService">認証サービス</param>
        public MainMenuController(
            UserManager<Users> userManager,
            AuthServiceJWT authService)
            : base(
                  userManager,
                  authService)
        {
        }

        #endregion
    }
}
