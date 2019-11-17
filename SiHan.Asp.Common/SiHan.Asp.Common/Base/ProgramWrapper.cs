using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SiHan.Asp.Logger;
using Microsoft.Extensions.DependencyInjection;
using SiHan.Libs.Orm;
using SiHan.Asp.Common.Data;

namespace SiHan.Asp.Common.Base
{
    /// <summary>
    /// Program包装类
    /// </summary>
    public class ProgramWrapper<TStartup> where TStartup : BaseStartup
    {
        /// <summary>
        /// 运行
        /// </summary>
        public virtual void Run(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            ILogger logger = host.Services.GetService<ILogger<ProgramWrapper<TStartup>>>();
            try
            {
                this.Init(logger, host);
                host.Run();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init(ILogger logger, IWebHost webHost)
        {
            OrmLicense.Patch();
            bool isSuccess = OrmLicense.HasLicense();
            if (isSuccess)
            {
                using (var scope = webHost.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    IDataInitService initService = services.GetService<IDataInitService>();
                    initService.Seed();
                }
            }
            else
            {
                throw new Exception("ServiceStack crack failed!");
            }
        }

        /// <summary>
        /// 创建WEB主机
        /// </summary>
        protected virtual IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                 .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.ClearProviders();
                     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                     logging.AddColorConsole();
                     logging.AddFile();
                 })
                .UseStartup<TStartup>();
        }
    }
}
