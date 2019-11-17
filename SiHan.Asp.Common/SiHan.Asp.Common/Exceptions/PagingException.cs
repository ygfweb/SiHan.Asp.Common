using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Exceptions
{
    /// <summary>
    /// 分页异常
    /// </summary>
    public class PagingException : Exception
    {
        public PagingException(string msg) : base(msg)
        {
        }
    }
}
