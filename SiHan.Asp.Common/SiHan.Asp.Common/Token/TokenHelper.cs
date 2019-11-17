using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SiHan.Asp.Common.Token
{
    /// <summary>
    /// Token帮助类
    /// </summary>
    public class TokenHelper
    {
        /// <summary>
        /// 创建token
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">用户登陆密码</param>
        /// <param name="expiredMinutes">过期时间，单位：分钟</param>
        /// <param name="securityKey">密钥，如果为空则使用默认密钥</param>
        /// <returns></returns>
        public static string CreateToken(string userId, string userName, string password, int expiredMinutes = 10, SymmetricSecurityKey securityKey = null)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("id",userId),
                new Claim("name",userName),
                new Claim("up",password),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            if (securityKey == null)
            {
                securityKey = GetDefaultKey();
            }
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: DateTime.Now.AddMinutes(expiredMinutes), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 获取Token密钥
        /// </summary>
        public static SymmetricSecurityKey GetKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        /// <summary>
        /// 获取默认密钥
        /// </summary>
        public static SymmetricSecurityKey GetDefaultKey()
        {
            string key = "8aSa7H9w463Zws1wilxnChZ4fN2ckyBiuTtpt6t3XUKoqqMCbU";
            return GetKey(key);
        }
    }
}
