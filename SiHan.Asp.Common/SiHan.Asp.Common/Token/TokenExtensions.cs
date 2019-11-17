using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SiHan.Asp.Common.Token
{
    /// <summary>
    /// IServiceCollection的Token扩展
    /// </summary>
    public static class TokenExtensions
    {
        /// <summary>
        /// 添加token身份验证
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="validateLifetime">是否验证有效期</param>
        /// <param name="key">token密钥，如果为空，则使用默认密钥</param>
        /// <returns></returns>
        public static IServiceCollection AddToken(this IServiceCollection services, bool validateLifetime = true, string key = "")
        {
            SymmetricSecurityKey securityKey = null;
            if (string.IsNullOrWhiteSpace(key))
            {
                securityKey = TokenHelper.GetDefaultKey();
            }
            else
            {
                securityKey = TokenHelper.GetKey(key);
            }
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = validateLifetime,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = null,
                        ValidAudience = null,
                        IssuerSigningKey = securityKey
                    };
                });
            return services;
        }

        /// <summary>
        /// 从HTTP请求获取通过Token验证的用户ID
        /// </summary>
        public static TokenUser GetCurrentTokenUser(this HttpContext httpContext)
        {
            ClaimsPrincipal user = httpContext.User;
            TokenUser tokenUser = new TokenUser
            {
                JwtIdentifier = user.FindFirstValue(JwtRegisteredClaimNames.Jti),
                UserId = user.FindFirstValue("id"),
                UserName = user.FindFirstValue("name"),
                Password = user.FindFirstValue("up")
            };
            return tokenUser;
        }
    }
}
