using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SiHan.Asp.Common.Extensions;
using SiHan.Asp.Common.Upload;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Base
{
    /// <summary>
    /// Startup基类
    /// </summary>
    public abstract class BaseStartup
    {
        /// <summary>
        /// 应用程序配置
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// Web托管环境
        /// </summary>
        protected IHostingEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// 日志工厂
        /// </summary>
        protected ILoggerFactory LoggerFactory { get; set; }

        public BaseStartup(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            this.Configuration = configuration;
            this.HostingEnvironment = hostingEnvironment;
            this.LoggerFactory = loggerFactory;
        }

        /// <summary>
        /// 配置服务
        /// </summary>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor(); // 添加HTTP上下文
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>(); //注入上下文访问器，供taghelper使用
            services.AddSiteSession(); // 添加cookie和session服务
            services.AddHttpClient();
            services.AddCoreDatabase(this.HostingEnvironment); // 添加数据基础服务
            services.AddEmail(this.Configuration); // 添加email服务
            services.AddScoped<IUploadService, DefaultUploadService>(); // 添加上传服务
            services.AddCookieAuth(); // 添加cookie认证
            services.AddResponseCompression(); // 添加响应压缩服务
            services.AddResponseCaching(); // 响应缓存
            this.ConfigureCustomizeServices(services); // 添加自定义服务
            services.AddSiteMvc();
        }

        /// <summary>
        /// 配置自定义服务
        /// </summary>
        public virtual void ConfigureCustomizeServices(IServiceCollection services) { }

        /// <summary>
        /// 配置中间件
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseResponseCaching(); // 响应缓存
            app.UseResponseCompression(); // 响应压缩
            app.UseStaticFilesWithCache();
            app.UseSession();
            app.UseCookieAuth();
            app.UseMvc(routes =>
            {
                routes.MapRoute("area", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                this.UseCustomizeRoute(routes);
            });
        }

        /// <summary>
        /// 加载自定义路由
        /// </summary>
        protected virtual void UseCustomizeRoute(IRouteBuilder routes) { }
    }
}
