using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SiHan.Asp.Common.Exceptions;
using SiHan.Libs.Utils.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Asp.Common.TagHelpers
{
    /// <summary>
    /// bootstrap 4 分页
    /// </summary>
    [HtmlTargetElement("bs4-pager", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PaginationTagHelper : TagHelper
    {
        // https://gunnarpeipman.com/aspnet/pager-tag-helper/
        private readonly IUrlHelper _urlHelper;
        private int CurrentPage { get; set; }
        private Pager _pager { get; set; }

        [ViewContext] [HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("for_page_size")] public int PageSize { get; set; } = 10;

        [HtmlAttributeName("for_row_count")] public long RowCount { get; set; } = 0;

        public PaginationTagHelper(IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        private int ParseCurrentPage(HttpContext context)
        {
            var request = context.Request;
            int currentPage = 1;
            if (request.Query.ContainsKey("page"))
            {
                try
                {
                    currentPage = Convert.ToInt32(request.Query["page"].ToString());
                }
                catch (Exception ex)
                {
                    throw new PagingException(ex.Message);
                }
            }
            else
            {
                currentPage = 1;
            }

            if (currentPage < 1)
            {
                throw new PagingException("当前网址的分页参数小于1");
            }

            return currentPage;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (RowCount < 0 || CurrentPage < 0 || PageSize < 0 || PageSize < 0)
            {
                return;
            }

            this.CurrentPage = ParseCurrentPage(this.ViewContext.HttpContext);
            this._pager = new Pager(this.RowCount, this.CurrentPage, this.PageSize);
            output.TagName = "ul"; // 覆盖标签pager为ul
            output.Attributes.Add("class", "pagination"); //添加bs4样式
            BuildFirstPageLink(output);
            BuildPreviousPage(output);
            BuildNumPageLink(output);
            BuildNextPage(output);
            BuildLastPage(output);
        }

        /// <summary>
        /// 构建首页链接按钮
        /// </summary>
        private void BuildFirstPageLink(TagHelperOutput output)
        {
            if (this.CurrentPage > 1)
            {
                BuildLink(output, "首页", 1, false, false);
            }
            else
            {
                BuildLink(output, "首页", 1, true, false);
            }
        }

        /// <summary>
        /// 构建上一页连接
        /// </summary>
        private void BuildPreviousPage(TagHelperOutput output)
        {
            if (this._pager.HasPreviousPage)
            {
                BuildLink(output, "上页", _pager.PreviousPage, false, false);
            }
            else
            {
                BuildLink(output, "上页", 0, true, false);
            }
        }

        /// <summary>
        /// 构建下一页
        /// </summary>
        private void BuildNextPage(TagHelperOutput output)
        {
            if (this._pager.HasNextPage)
            {
                BuildLink(output, "下页", _pager.NextPage, false, false);
            }
            else
            {
                BuildLink(output, "下页", 0, true, false);
            }
        }

        /// <summary>
        /// 构建尾页
        /// </summary>
        private void BuildLastPage(TagHelperOutput output)
        {
            if (CurrentPage >= this._pager.TotalPages)
            {
                BuildLink(output, "尾页", 0, true, false);
            }
            else
            {
                BuildLink(output, "尾页", _pager.TotalPages, false, false);
            }
        }


        /// <summary>
        /// 构建分页数连接
        /// </summary>
        private void BuildNumPageLink(TagHelperOutput output)
        {
            Pager pager = new Pager(this.RowCount, this.CurrentPage);

            if (this.RowCount == 0)
            {
                BuildLink(output, "1", 1, false, false);
            }
            else
            {
                for (int i = pager.StartPage; i <= pager.EndPage; i++)
                {
                    if (i == this.CurrentPage)
                    {
                        BuildLink(output, i.ToString(), i, false, true);
                    }
                    else
                    {
                        BuildLink(output, i.ToString(), i, false, false);
                    }
                }
            }
        }

        /// <summary>
        /// 构建分页连接，如果isDisabled为true，则渲染链接为#
        /// </summary>
        private void BuildLink(TagHelperOutput output, string text, int pageNum, bool isDisabled, bool isActive)
        {
            string url;
            List<string> list = new List<string>();
            list.Add("page-item");
            if (isDisabled)
            {
                list.Add("disabled");
                url = "#";
            }
            else
            {
                url = BuildQueryUrl(pageNum);
            }

            if (isActive)
            {
                list.Add("active");
            }

            string css = string.Join(" ", list.ToArray());
            string html = $"<li class=\"{css}\"><a class=\"page-link\" href=\"{url}\">{text}</a></li>";
            output.Content.AppendHtml(html);
        }

        private string BuildQueryUrl(int num)
        {
            var path = ViewContext.HttpContext.Request.Path;
            //            var action = ViewContext.RouteData.Values["action"].ToString();
            //            string url = WebUtility.UrlDecode(_urlHelper.Action(action, new {page = num}));
            string url = path + "?page=" + num;
            var request = this.ViewContext.HttpContext.Request;
            // 将Url中的查询参数复制到新构建的Url
            foreach (var key in request.Query.Keys)
            {
                if (key.ToLower() == "page")
                {
                    continue; // 如果当前网址存在page参数，则跳过，避免重复
                }

                if (key.ToLower() == "clicksort")
                {
                    continue; // 不合并点击排序参数
                }

                url += "&" + key + "=" + request.Query[key];
            }

            return url;
        }
    }
}
