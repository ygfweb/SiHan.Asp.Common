using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SiHan.Asp.Common.Extensions
{
    /// <summary>
    /// ClaimsPrincipal扩展类
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// 获取用户的Id
        /// </summary>
        public static string GetUserId(this ClaimsPrincipal user)
        {
            Claim claim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return "";
            }
            else
            {
                return claim.Value;
            }
        }

        /// <summary>
        /// 获取用户的账号名称
        /// </summary>
        public static string GetUserName(this ClaimsPrincipal user)
        {
            Claim claim = user.FindFirst(ClaimTypes.Name);
            if (claim == null)
            {
                return "";
            }
            else
            {
                return claim.Value;
            }
        }

        /// <summary>
        /// 获取用户的角色名称
        /// </summary>
        public static string GetUserRoleName(this ClaimsPrincipal user)
        {
            Claim claim = user.FindFirst(ClaimTypes.Role);
            if (claim == null)
            {
                return "";
            }
            else
            {
                return claim.Value;
            }
        }
    }
}
