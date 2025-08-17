using System.Text;
using AttendanceManagement.Server.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiService.Common.Const;
using WebApiService.Resource;

namespace AttendanceManagement.Server.Schema
{
    /// <summary>
    /// Swaggerのエラー時の例を表示するスキーマフィルタークラス
    /// </summary>
    public class SwaggerResultExampleFilter : IOperationFilter
    {
        /// <summary>
        /// エラー時の例を表示する。
        /// </summary>
        /// <param name="operation">操作オブジェクト</param>
        /// <param name="context">コンテキスト</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var response in operation.Responses)
            {
                var responseType = context.ApiDescription.SupportedResponseTypes.FirstOrDefault(r => r.StatusCode.ToString() == response.Key)?.Type;
                if (responseType == null || !IsServiceResultType(responseType))
                {
                    continue;
                }

                List<ApplicationErrorCode> appErrorCode = new List<ApplicationErrorCode>();

                var actionMethod = context.MethodInfo;
                var controllerType = actionMethod.DeclaringType;

                var methodName = actionMethod.Name;
                var controllerName = controllerType?.Name;

                if (controllerName == nameof(AccountController))
                {
                    if (methodName == nameof(AccountController.UserCurrent))
                    {
                        if (response.Key == StatusCodes.Status400BadRequest.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.NotActiveUserError);
                        }
                        else if (response.Key == StatusCodes.Status500InternalServerError.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.UnexpectedError);
                        }
                    }
                    else if (methodName == nameof(AccountController.Login))
                    {
                        if (response.Key == StatusCodes.Status400BadRequest.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.LoginError);
                        }
                        else if (response.Key == StatusCodes.Status500InternalServerError.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.UnexpectedError);
                        }
                    }
                    else if (methodName == nameof(AccountController.Logout))
                    {
                        if (response.Key == StatusCodes.Status500InternalServerError.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.UnexpectedError);
                        }
                    }
                    else if (methodName == nameof(AccountController.CreateUser))
                    {
                        if (response.Key == StatusCodes.Status400BadRequest.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.ValidateParamError);
                            appErrorCode.Add(ApplicationErrorCodes.AlreadyExitsAccountError);
                        }
                        else if (response.Key == StatusCodes.Status500InternalServerError.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.UnexpectedError);
                            appErrorCode.Add(ApplicationErrorCodes.AccounCreatFailedError);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (controllerName == nameof(MainMenuController))
                {
                    if (methodName == nameof(MainMenuController.AttendanceClockInState))
                    {
                        if (response.Key == StatusCodes.Status500InternalServerError.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.UnexpectedError);
                        }
                    }
                    else if (methodName == nameof(MainMenuController.AttendanceClockIn))
                    {
                        if (response.Key == StatusCodes.Status400BadRequest.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.ValidateParamError);
                            appErrorCode.Add(ApplicationErrorCodes.NotDataAttendanceError);
                        }
                        else if (response.Key == StatusCodes.Status500InternalServerError.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.UnexpectedError);
                        }
                    }
                    else if (methodName == nameof(MainMenuController.AttendanceClockOut))
                    {
                        if (response.Key == StatusCodes.Status400BadRequest.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.ValidateParamError);
                            appErrorCode.Add(ApplicationErrorCodes.NotDataAttendanceError);
                        }
                        else if (response.Key == StatusCodes.Status500InternalServerError.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.UnexpectedError);
                        }
                    }
                    else if (methodName == nameof(MainMenuController.AttendanceGet))
                    {
                        if (response.Key == StatusCodes.Status400BadRequest.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.ValidateParamError);
                            appErrorCode.Add(ApplicationErrorCodes.NotDataAttendanceTargetError);
                        }
                        else if (response.Key == StatusCodes.Status500InternalServerError.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.UnexpectedError);
                        }
                    }
                    else if (methodName == nameof(MainMenuController.AttendanceGetForMonth))
                    {
                        if (response.Key == StatusCodes.Status400BadRequest.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.ValidateParamError);
                            appErrorCode.Add(ApplicationErrorCodes.NotDataAttendanceError);
                        }
                        else if (response.Key == StatusCodes.Status500InternalServerError.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.UnexpectedError);
                        }
                    }
                    else if (methodName == nameof(MainMenuController.AttendanceUpdate))
                    {
                        if (response.Key == StatusCodes.Status400BadRequest.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.ValidateParamError);
                            appErrorCode.Add(ApplicationErrorCodes.NotDataAttendanceError);
                        }
                        else if (response.Key == StatusCodes.Status500InternalServerError.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.UnexpectedError);
                        }
                    }
                    else if (methodName == nameof(MainMenuController.AttendanceDelete))
                    {
                        if (response.Key == StatusCodes.Status400BadRequest.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.ValidateParamError);
                            appErrorCode.Add(ApplicationErrorCodes.NotDataAttendanceError);
                        }
                        else if (response.Key == StatusCodes.Status500InternalServerError.ToString())
                        {
                            appErrorCode.Add(ApplicationErrorCodes.UnexpectedError);
                            appErrorCode.Add(ApplicationErrorCodes.NotDataAttendanceUpdateError);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (appErrorCode.Count == 0)
                {
                    continue;
                }

                var errorCode = new StringBuilder();
                var errorMessage = new StringBuilder();

                int index = 1;
                foreach (var error in appErrorCode)
                {
                    string split = string.Empty;
                    if (index > 1)
                    {
                        split = ", ";
                    }
                    errorCode.Append($"{split}パターン{index}:{error.Id}");
                    errorMessage.Append($"{split}パターン{index}:{ApplicationErrorResource.ResourceManager.GetString(error.ResourceKey) ?? string.Empty}");
                    index++;
                }

                var example = new OpenApiObject
                {
                    ["isSuccess"] = new OpenApiBoolean(false),
                    ["errorCode"] = new OpenApiString(errorCode.ToString()),
                    ["errorMessage"] = new OpenApiString(errorMessage.ToString()),
                    ["value"] = new OpenApiNull(),
                };

                foreach (var mediaType in response.Value.Content)
                {
                    mediaType.Value.Example = example;
                }
            }
        }

        /// <summary>
        /// WebApiService.Logic.ServiceResultクラスか判別します。
        /// </summary>
        /// <param name="type">型</param>
        /// <returns>結果</returns>
        private bool IsServiceResultType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Name.Contains("ServiceResult");
        }

        /// <summary>
        /// 例を作成します。
        /// </summary>
        /// <param name="type">型</param>
        /// <returns>例</returns>
        private IOpenApiAny CreateValueExample(Type? type)
        {
            if (type == null)
            {
                return new OpenApiNull();
            }

            return type switch
            {
                _ when type == typeof(string) => new OpenApiString("example"),
                _ when type == typeof(int) => new OpenApiInteger(123),
                _ when type == typeof(bool) => new OpenApiBoolean(true),
                _ when type == typeof(DateTime) => new OpenApiString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                _ => new OpenApiObject()
            };
        }
    }
}
