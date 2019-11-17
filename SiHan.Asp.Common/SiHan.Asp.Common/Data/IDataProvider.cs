using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Data
{
    /// <summary>
    /// 数据提供器接口
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// 获取数据提供器
        /// </summary>
        IOrmLiteDialectProvider GetProvider();
    }
}
