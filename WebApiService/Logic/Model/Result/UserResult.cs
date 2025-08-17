using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using WebApiService.Resource;

namespace WebApiService.Logic
{
    /// <summary>
    /// ユーザー情報結果
    /// </summary>
    public class UserResult
    {
        /// <summary>
        /// ユーザー名。例: 山田 太郎
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredUserName))]
        [SwaggerSchema(Description = "ユーザー名。例: 山田 太郎", Format = "string")]
        public string UserName { get; set; } = string.Empty;
    }
}