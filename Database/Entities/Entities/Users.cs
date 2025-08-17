using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities
{
    /// <summary>
    /// IdentityUser拡張ユーザエンティティ
    /// </summary>
    public class Users : IdentityUser
    {
        /// <summary>
        /// ニックネーム
        /// </summary>
        [Required]
        [StringLength(256)]
        public string NickName { get; set; } = string.Empty;
    }
}
