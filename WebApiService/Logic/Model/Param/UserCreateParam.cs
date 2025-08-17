using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using WebApiService.Common.Const;
using WebApiService.Resource;

namespace WebApiService.Logic
{
    /// <summary>
    /// ログインモデル
    /// </summary>
    public class UserCreateParam
    {
        /// <summary>
        /// ユーザー名。例: 山田 太郎
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredUserName))]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(MessageResource), Name = nameof(MessageResource.KeyIdUserName))]
        [StringLength(maximumLength: AuthConst.MaximumUserNameLength, ErrorMessageResourceType = typeof(ApplicationErrorResource), ErrorMessageResourceName = nameof(ApplicationErrorResource.UserIdOrPasswordInvalidError))]
        [SwaggerSchema(Description = "ユーザー名。例: 山田 太郎", Format = "string")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// ユーザーコード
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredUserCode))]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(MessageResource), Name = nameof(MessageResource.KeyIdUserId))]
        [StringLength(maximumLength: AuthConst.MaximumUserCodeLength, ErrorMessageResourceType = typeof(ApplicationErrorResource), ErrorMessageResourceName = nameof(ApplicationErrorResource.UserIdOrPasswordInvalidError))]
        [SwaggerSchema(Description = "ユーザーコード。例: user123", Format = "string")]
        public string UserCode { get; set; } = string.Empty;

        /// <summary>
        /// パスワード
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredPassword))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(MessageResource), Name = nameof(MessageResource.KeyIdPassword))]
        [StringLength(maximumLength: AuthConst.MaximumPasswordLength, ErrorMessageResourceType = typeof(ApplicationErrorResource), ErrorMessageResourceName = nameof(ApplicationErrorResource.EnterWithInMaxLengthError))]
        [SwaggerSchema(Description = "パスワード。例: Password123!")]
        public string Password { get; set; } = string.Empty;
    }
}
