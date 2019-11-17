using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.WebEncoders;
using Newtonsoft.Json.Serialization;
using ServiceStack.Logging;
using SiHan.Asp.Common.Data;
using SiHan.Asp.Common.Providers;
using SiHan.Libs.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SiHan.Asp.Common.Extensions
{
    /// <summary>
    /// IServiceCollection扩展类
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 增加Session服务
        /// </summary>
        public static void AddSiteSession(this IServiceCollection services, int IdleTimeout = 60 * 30)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false; //关闭GDPR规范    
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDistributedMemoryCache(); // 使用内存作为session的缓存
            services.AddSession(options =>
            {
                options.Cookie.Name = ".app.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(IdleTimeout);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; //强制存储cookie
            });
        }


        /// <summary>
        /// 添加 ASP.NET CORE
        /// </summary>
        public static void AddSiteMvc(this IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddMvcOptions(o =>
            {
                // 模型绑定字符串默认为null，改为默认空字符
                o.ModelMetadataDetailsProviders.Add(new BlankMetadataProvider());
            });
            services.AddMvc().AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });

            // 解决中文的title被编码问题
            // https://www.cnblogs.com/dudu/p/5879913.html
            services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.BasicLatin,UnicodeRanges.CjkUnifiedIdeographs));
        }

        /// <summary>
        /// 添加cookie认证
        /// </summary>
        public static void AddCookieAuth(this IServiceCollection services)
        {
            services.AddAuthentication(options => { options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => { options.Cookie.Name = "auth_cookie"; });
        }
        /// <summary>
        /// 添加EMAIL服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddEmail(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("Email"));
            services.AddScoped<IEmailSender, DefaultEmailSender>(); //邮件发送
        }

        /// <summary>
        /// 添加核心数据服务
        /// </summary>
        public static void AddCoreDatabase(this IServiceCollection services, IHostingEnvironment environment)
        {
            services.AddScoped<IDataProvider, PostgreSqlProvider>(); // 数据提供器
            services.AddScoped<IDataFactory, DefaultDataFactory>(); //添加数据工厂
            services.AddScoped<IDataInitService, DefaultDataInitService>(); //添加数据种子服务
            if (environment.IsDevelopment())
            {
                LogManager.LogFactory = new ConsoleLogFactory(debugEnabled: true);
            }
        }
    }
}
