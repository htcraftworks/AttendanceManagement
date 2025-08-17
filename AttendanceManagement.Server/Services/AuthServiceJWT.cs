using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceManagement.Server.Services
{
    /// <summary>
    /// JWT認証サービス
    /// </summary>
    public class AuthServiceJWT
    {
        /// <summary>
        /// アプリケーション構成オブジェクト
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">アプリケーション構成</param>
        public AuthServiceJWT(IConfiguration config)
        {
            this._config = config;
        }

        /// <summary>
        /// JWT認証トークンを作成します。
        /// </summary>
        /// <param name="claimList">トークン作成用データ</param>
        /// <returns>JWT認証トークン</returns>
        /// <exception cref="ArgumentNullException">パラメータ異常</exception>
        public string CreateToken(List<Claim> claimList)
        {
            string? tokenKey = this._config["JwtSettings:TokenKey"];
            if (string.IsNullOrEmpty(tokenKey))
            {
                throw new ArgumentNullException(nameof(tokenKey));
            }

            int expiryMinutes = int.Parse(_config["JwtSettings:TokenExpiryMinutes"] ?? "1");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.Now.AddDays(expiryMinutes),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}