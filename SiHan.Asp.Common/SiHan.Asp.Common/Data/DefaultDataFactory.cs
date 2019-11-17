using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SiHan.Asp.Common.Data
{
    /// <summary>
    /// 默认的数据工厂，从配置文件中获取连接字符串
    /// </summary>
    public class DefaultDataFactory : IDataFactory
    {
        protected IConfiguration Configuration { get; }
        protected ILoggerFactory LoggerFactory { get; }
        protected IHostingEnvironment HostingEnvironment { get; }
        protected OrmLiteConnectionFactory ConnectionFactory { get; }

        protected IDataProvider DataProvider { get; }

        public DefaultDataFactory(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment, IDataProvider dataProvider)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            HostingEnvironment = hostingEnvironment;
            DataProvider = dataProvider;
            string connStr = configuration.GetConnectionString("db");
            this.ConnectionFactory = new OrmLiteConnectionFactory(connStr, dataProvider.GetProvider());
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            return this.ConnectionFactory.Open();
        }
    }
}
