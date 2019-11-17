using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Data
{
    /// <summary>
    /// PostgreSQL数据提供器
    /// </summary>
    public class PostgreSqlProvider : IDataProvider
    {
        public IOrmLiteDialectProvider GetProvider()
        {
            return PostgreSqlDialect.Provider;
        }
    }
}
