using AttendanceManagement.Server.Services;
using Data.Entities;
using Library;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiService.Common.Const;
using WebApiService.Logic;
using WebApiService.Logic.Logic;

namespace AttendanceManagement.Server.Controllers
{
    /// <summary>
    /// アカウントコントローラー
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ApiControllerBase
    {
        #region constructor

        /// <summary>
        ///  コンストラクタ
        /// </summary>
        /// <param name="userManager">ユーザー管理</param>
        /// <param name="authService">認証サービス</param>
        public AccountController(
            UserManager<Users> userManager,
            AuthServiceJWT authService)
            : base(
                  userManager,
                  authService)
        {
        }

        #endregion

        #region API

        /// <summary>
        /// JWT認証の有効ユーザがあれば応答します。
        /// </summary>
        /// <returns>応答データ</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(AccountControllerActionsRoutes.UserCurrent)]
        [Tags("認証状態確認")]
        [ProducesResponseType(typeof(ServiceResult<UserResult?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ServiceResult<>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult<>), StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> UserCurrent()
        {
            return this.ExecuteLogicWithStatus<UserResult?>(async userCode =>
            {
                if (this.UserManager == null)
                {
                    Logger.Error($"{nameof(this.UserManager)} is null.");
                    return (ServiceResult<UserResult?>.CreateError(ApplicationErrorCodes.UnexpectedError), null);
                }

                var identityUser = await this.UserManager.FindByNameAsync(userCode);
                if (identityUser == null)
                {
                    Logger.Warn($"FindByIdAsync Failed.");
                    return (ServiceResult<UserResult?>.CreateError(ApplicationErrorCodes.NotActiveUserError), null);
                }

                return (ServiceResult<UserResult?>.CreateSuccess(this.CreateUserObject(identityUser)), identityUser);
            });
        }

        /// <summary>
        /// アカウントログインを行います。
        /// </summary>
        /// <param name="model">ログイン情報</param>
        /// <returns>応答データ</returns>
        [AllowAnonymous]
        [HttpPost(AccountControllerActionsRoutes.Login)]
        [Tags("ログイン")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResult<UserResult?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult<>), StatusCodes.Status500InternalServerError)]
        public Task<ActionResult<ServiceResult<UserResult?>>> Login([FromBody] LoginParam model)
        {
            return this.ExecuteLogic<ServiceResult<UserResult?>>(async () =>
            {
                if (this.UserManager == null)
                {
                    Logger.Error($"{nameof(this.UserManager)} is null.");
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        ServiceResult<UserResult?>.CreateError(ApplicationErrorCodes.UnexpectedError));
                }

                Users? identityUser = await this.UserManager.FindByNameAsync(model.UserCode);
                if (identityUser == null)
                {
                    return BadRequest(ServiceResult<UserResult?>.CreateError(ApplicationErrorCodes.LoginError));
                }

                var passwordValid = await this.UserManager.CheckPasswordAsync(identityUser, model.Password);
                if (!passwordValid)
                {
                    Logger.Warn("Login failed.");
                    return BadRequest(ServiceResult<UserResult?>.CreateError(ApplicationErrorCodes.NotActiveUserError));
                }

                this.SetCommonCustomHeader(identityUser);

                return Ok(ServiceResult<UserResult?>.CreateSuccess(this.CreateUserObject(identityUser)));
            });
        }

        /// <summary>
        /// アカウントログアウトを行います。
        /// NOTE:JWT認証なのでクライアント側で対応する必要がある
        /// </summary>
        /// <returns>応答データ</returns>
        [AllowAnonymous]
        [HttpGet(AccountControllerActionsRoutes.Logout)]
        [Tags("ログアウト")]
        [ProducesResponseType(typeof(ServiceResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ServiceResult<bool>> Logout()
        {
            return Ok(ServiceResult<bool>.CreateSuccess(true));
        }

        /// <summary>
        /// アカウント作成を行います。
        /// </summary>
        /// <param name="account">アカウント</param>
        /// <returns>応答データ</returns>
        [AllowAnonymous]
        [HttpPost(AccountControllerActionsRoutes.UserCreate)]
        [Tags("アカウント作成")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResult<bool?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<bool?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult<bool?>), StatusCodes.Status500InternalServerError)]
        public Task<ActionResult<ServiceResult<bool?>>> CreateUser([FromBody] UserCreateParam account)
        {
            return this.ExecuteLogic<ServiceResult<bool?>>(async () =>
            {
                if (this.UserManager == null)
                {
                    Logger.Error($"{nameof(this.UserManager)} is null.");
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        ServiceResult<bool?>.CreateError(ApplicationErrorCodes.UnexpectedError));
                }

                foreach (KeyValuePair<string, string> validate in AccountLogic.CreateUserValidate(account))
                {
                    this.ModelState.AddModelError(validate.Key, validate.Value);
                }

                if (this.ModelState.ErrorCount > 0)
                {
                    return BadRequest(ServiceResult<bool?>.CreateError(ApplicationErrorCodes.ValidateParamError));
                }

                if (await this.UserManager.Users.AnyAsync(x => x.UserName == account.UserCode))
                {
                    this.ModelState.AddModelError(nameof(account.UserCode), "Already exists");
                    return BadRequest(ServiceResult<bool?>.CreateError(ApplicationErrorCodes.AlreadyExitsAccountError));
                }

                // NOTE:Identityにおいて、UserNameはDB上はKEYではないがCreateAsyncのValidateで重複チェックしているためKEYとなる
                var identityUser = new Users
                {
                    UserName = account.UserCode,
                    NickName = account.UserName
                };

                var result = await this.UserManager.CreateAsync(identityUser, account.Password);
                if (!result.Succeeded)
                {
                    Logger.Error(string.Join(", ", result.Errors.Select(e => $"[ErrorCode:{e.Code}]=>{e.Description}")));
                    return StatusCode(StatusCodes.Status500InternalServerError, ServiceResult<bool?>.CreateError(ApplicationErrorCodes.AccounCreatFailedError));
                }

                return Ok(ServiceResult<bool?>.CreateSuccess(null));
            });
        }

        #endregion

        #region private

        /// <summary>
        ///  ユーザーオブジェクトを作成します。
        /// </summary>
        /// <param name="identityUser">ユーザー情報</param>
        /// <returns>ユーザーオブジェクト</returns>
        private UserResult CreateUserObject(Users identityUser)
        {
            return new UserResult()
            {
                UserName = identityUser.NickName,
            };
        }

        #endregion
    }
}
