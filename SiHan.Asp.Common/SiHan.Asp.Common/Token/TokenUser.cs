using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Token
{
    /// <summary>
    /// token用户
    /// </summary>
    public class TokenUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; } = "";

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// JWT唯一标识
        /// </summary>
        public string JwtIdentifier { get; set; } = "";
    }
}
