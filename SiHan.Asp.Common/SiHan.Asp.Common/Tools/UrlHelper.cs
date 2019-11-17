using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Tools
{
    /// <summary>
    /// URL帮助类
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// 构建URL路径
        /// </summary>
        public static string Build(string baseUrl, Dictionary<string, string> queryParams)
        {
            if (queryParams == null)
            {
                queryParams = new Dictionary<string, string>();
            }
            return QueryHelpers.AddQueryString(baseUrl, queryParams);
        }
    }
}
