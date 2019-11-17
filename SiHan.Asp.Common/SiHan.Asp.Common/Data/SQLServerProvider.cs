using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Data
{
    /// <summary>
    /// SQL2008数据提供器
    /// </summary>
    public class SQLServerProvider : IDataProvider
    {
        public IOrmLiteDialectProvider GetProvider()
        {
            return SqlServer2008Dialect.Provider;
        }
    }
}
