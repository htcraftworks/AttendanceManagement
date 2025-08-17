using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using WebApiService.Common.Const;
using WebApiService.Resource;

namespace WebApiService.Logic
{
    /// <summary>
    /// ログインパラメータモデル
    /// </summary>
    public class LoginParam
    {
        /// <summary>
        /// ユーザーコード。例: user123
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredUserCode))]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(MessageResource), Name = nameof(MessageResource.KeyIdUserId))]
        [StringLength(maximumLength: AuthConst.MaximumUserCodeLength, ErrorMessageResourceType = typeof(ApplicationErrorResource), ErrorMessageResourceName = nameof(ApplicationErrorResource.EnterWithInMaxLengthError))]
        [SwaggerSchema(Description = "ユーザーコード。例: user123", Format = "string")]
        public string UserCode { get; set; } = string.Empty;

        /// <summary>
        /// パスワード。例: Password123!
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MessageResource), ErrorMessageResourceName = nameof(MessageResource.SWRequiredPassword))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(MessageResource), Name = nameof(MessageResource.KeyIdPassword))]
        [StringLength(maximumLength: AuthConst.MaximumPasswordLength, ErrorMessageResourceType = typeof(ApplicationErrorResource), ErrorMessageResourceName = nameof(ApplicationErrorResource.EnterWithInMaxLengthError))]
        [SwaggerSchema(Description = "パスワード。例: Password123!")]
        public string Password { get; set; } = string.Empty;
    }
}
