using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Mvc
{
    /// <summary>
    /// HTML响应结果
    /// </summary>
    public class HtmlResponseResult : ContentResult
    {
        /// <summary>
        /// HTML响应结果
        /// </summary>
        /// <param name="html">Html文本</param>
        /// <param name="statusCode">响应代码</param>
        public HtmlResponseResult(string html,int statusCode = 200)
        {
            this.Content = html;
            this.ContentType = "text/html";
            this.StatusCode = statusCode;
        }
    }
}
