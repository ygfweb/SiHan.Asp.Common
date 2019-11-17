using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SiHan.Asp.Common.Extensions
{
    /// <summary>
    /// HttpContext扩展类
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 获取客户端的IP
        /// </summary>
        public static string GetClientUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        /// <summary>
        /// 获取推荐网站
        /// </summary>
        public static string GetReferer(this HttpContext context)
        {
            return context.Request.Headers[HeaderNames.Referer].ToString();
        }

        /// <summary>
        /// 获取HTTP请求头UserAgent
        /// </summary>
        public static string GetUserAgent(this HttpContext context)
        {
            return context.Request.Headers[HeaderNames.UserAgent].ToString();
        }

        private const string VerifyCodeName = "_VerifyCode";

        /// <summary>
        /// 设置验证码
        /// </summary>
        public static async Task SetVerifyCodeAsync(this ISession session, string code)
        {
            session.SetString(VerifyCodeName, code);
            await session.CommitAsync();
        }

        /// <summary>
        /// 刷新验证码
        /// </summary>
        public static async Task RefreshVerifyCodeAsync(this ISession session)
        {
            await SetVerifyCodeAsync(session, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        public static string GetVerifyCode(this ISession session)
        {
            var str = session.GetString(VerifyCodeName);
            return string.IsNullOrWhiteSpace(str) ? "" : str;
        }

        /// <summary>
        /// 网站登录（用于cookie认证）
        /// </summary>
        public static async Task LoginAsync(this HttpContext context, string userId, string userName, string roleName, bool isRememberMe = false)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
            var principal = new ClaimsPrincipal(identity);
            if (isRememberMe)
            {
                AuthenticationProperties properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(15) // cookie保存15天
                };
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
            }
            else
            {
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public static async Task LogoutAsync(this HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
