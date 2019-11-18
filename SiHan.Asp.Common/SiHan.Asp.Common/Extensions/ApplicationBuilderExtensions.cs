using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.Extensions
{
    /// <summary>
    /// IApplicationBuilder扩展类
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加cookie认证中间件
        /// </summary>
        public static void UseCookieAuth(this IApplicationBuilder app)
        {
            app.UseCors(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
                policy.AllowCredentials();
            });
            app.UseAuthentication();
        }

        /// <summary>
        /// 添加异常处理中间件
        /// </summary>
        public static void UseAppException(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("Error", "?statusCode={0}");
            }
        }

        /// <summary>
        /// 使用包含区域的默认路由
        /// </summary>
        public static void UseSiteMvcWithRoute(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute("area", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 为静态文件添加缓存
        /// </summary>
        public static void UseStaticFilesWithCache(this IApplicationBuilder app,int cacheSeconds = 604800)
        {
            //https://andrewlock.net/adding-cache-control-headers-to-static-files-in-asp-net-core/
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + cacheSeconds;
                }
            });
        }
    }
}
