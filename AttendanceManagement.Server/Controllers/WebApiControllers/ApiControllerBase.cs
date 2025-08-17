using System.Security.Claims;
using AttendanceManagement.Server.Services;
using Data.Entities;
using Library;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiService.Common.Const;
using WebApiService.Logic;

namespace AttendanceManagement.Server
{
    /// <summary>
    /// Controllerのベースクラス
    /// </summary>
    public class ApiControllerBase : Controller
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected ApiControllerBase()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userManager">ユーザー認証マネージャー</param>
        /// <param name="authService">認証トークンサービス</param>
        protected ApiControllerBase(
            UserManager<Users> userManager,
            AuthServiceJWT authService)
        {
            this.UserManager = userManager;
            this.AuthService = authService;
        }

        /// <summary>
        /// ユーザー管理
        /// </summary>
        protected UserManager<Users>? UserManager { get; private set; }

        /// <summary>
        /// 認証サービス
        /// </summary>
        protected AuthServiceJWT? AuthService { get; private set; }

        /// <summary>
        /// ログインユーザーのユーザーコードを取得します
        /// </summary>
        /// <returns>ユーザーコード</returns>
        protected string? GetLoginUserCode()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }

        /// <summary>
        /// 共通ロジック（ログイン不要処理）
        /// </summary>
        /// <param name="func">ロジック処理 引数1:戻り値の型</param>
        /// <typeparam name="T">型</typeparam>
        /// <returns>結果</returns>
        protected async Task<ActionResult<T>> ExecuteLogic<T>(Func<Task<ActionResult<T>>> func)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return BadRequest(this.ModelState);
                }

                return await func();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ServiceResult<T>.CreateError(ApplicationErrorCodes.UnexpectedError));
            }
        }

        /// <summary>
        /// 共通ロジック（ユーザ情報参照なし）
        /// </summary>
        /// <typeparam name="T">ServiceResultの中身の型</typeparam>
        /// <param name="func">ユーザーコードを引数に取る非同期ロジック</param>
        /// <returns>ActionResultでServiceResultをラップして返す</returns>
        protected async Task<IActionResult> ExecuteLogicWithStatus<T>(Func<string, Task<ServiceResult<T>>> func)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? userCode = null;

            try
            {
                userCode = GetLoginUserCode();
                if (string.IsNullOrEmpty(userCode))
                {
                    return Unauthorized(ServiceResult<T>.CreateError(ApplicationErrorCodes.NotActiveUserError));
                }

                var result = await func(userCode);

                return ConvertServiceResult<T>(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ServiceResult<T>.CreateError(ApplicationErrorCodes.UnexpectedError));
            }
            finally
            {
                await this.SetCommonCustomHeader(userCode);
            }
        }

        /// <summary>
        /// 共通ロジック（ユーザ情報参照あり）
        /// </summary>
        /// <typeparam name="T">ServiceResultの中身の型</typeparam>
        /// <param name="func">ユーザーコードを引数に取る非同期ロジック</param>
        /// <returns>結果</returns>
        protected async Task<IActionResult> ExecuteLogicWithStatus<T>(Func<string, Task<(ServiceResult<T> Result, Users? IdentityUser)>> func)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userCode = GetLoginUserCode();
            if (string.IsNullOrEmpty(userCode))
            {
                return Unauthorized(ServiceResult<T>.CreateError(ApplicationErrorCodes.NotActiveUserError));
            }

            Users? identityUser = null;

            try
            {
                var (result, user) = await func(userCode);
                identityUser = user;

                return ConvertServiceResult(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ServiceResult<T>.CreateError(ApplicationErrorCodes.UnexpectedError));
            }
            finally
            {
                if (identityUser != null)
                {
                    SetCommonCustomHeader(identityUser);
                }
                else
                {
                    await SetCommonCustomHeader(userCode);
                }
            }
        }

        /// <summary>
        /// ServiceResult に応じて適切な HTTP ステータスコードを返す
        /// </summary>
        /// <param name="result">応答結果</param>
        /// <typeparam name="T">型</typeparam>
        /// <returns>結果</returns>
        protected IActionResult ConvertServiceResult<T>(ServiceResult<T> result)
        {
            // 異常
            if (!result.IsSuccess)
            {
                if (result.ErrorCode == ApplicationErrorCodes.UnexpectedError.Id
                    || result.ErrorCode == ApplicationErrorCodes.NotDataAttendanceUpdateError.Id)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
                }

                return BadRequest(result);
            }

            // 正常
            if (result.ErrorCode == ApplicationErrorCodes.NotRegistAttendanceClockOutError.Id)
            {
                return Accepted(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// 共通カスタムヘッダーを設定します。
        /// </summary>
        /// <param name="userCode">ユーザーコード</param>
        /// <returns>void</returns>
        /// <exception cref="ArgumentException">パラメータ異常</exception>
        protected async Task SetCommonCustomHeader(string? userCode)
        {
            try
            {
                if (this.UserManager == null)
                {
                    throw new ArgumentException($"{nameof(AuthService)} or {nameof(UserManager)} is null.");
                }

                if (string.IsNullOrWhiteSpace(userCode))
                {
                    return;
                }

                Users? identityUser = await this.UserManager.FindByNameAsync(userCode);
                if (identityUser == null
                    || string.IsNullOrEmpty(identityUser.UserName))
                {
                    return;
                }

                this.SetCommonCustomHeader(identityUser);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        /// <summary>
        /// 共通カスタムヘッダーを設定します。
        /// </summary>
        /// <param name="identityUser">ユーザー情報</param>
        /// <exception cref="ArgumentException">パラメータ異常</exception>
        protected void SetCommonCustomHeader(Users identityUser)
        {
            try
            {
                if (this.AuthService == null)
                {
                    throw new ArgumentException($"{nameof(AuthService)} or {nameof(UserManager)} is null.");
                }

                if (identityUser == null
                    || string.IsNullOrEmpty(identityUser.UserName))
                {
                    return;
                }

                string token = this.AuthService.CreateToken(new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
                    new Claim(ClaimTypes.Name, identityUser.UserName)
                });

                var headerName = CustomHeaderPrefixes.AuthToken.HeaderName;
                var headerValue = CustomHeaderPrefixes.AuthToken.Prefix + token;

                // カスタムヘッダー設定
                if (Response.Headers.ContainsKey(headerName))
                {
                    Response.Headers[headerName] = headerValue;
                }
                else
                {
                    Response.Headers.Append(headerName, headerValue);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }
    }
}
