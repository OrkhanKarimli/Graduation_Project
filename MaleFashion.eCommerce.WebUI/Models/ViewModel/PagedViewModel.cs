using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.ViewModel
{
    public class PagedViewModel<T>
        where T : class
    {
        public PagedViewModel(IQueryable<T> query,
            int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = query.Count();

            this.Items = query
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize);
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int MaxPageCount
        {
            get
            {
                return (int)Math.Ceiling(TotalCount * 1.0 / PageSize);
            }
        }

        public IEnumerable<T> Items { get; private set; }


        public IHtmlContent GetPagenation(IUrlHelper url, string action)
        {
            if (TotalCount <= PageSize)
                return HtmlString.Empty;

            // Max 5 dənə düymə göstər
            int pageCount = 5;
            int start = 1, end = MaxPageCount;

            if (end - start > 5)
            {
                if (PageIndex - pageCount / 2 > 1)
                {
                    start = PageIndex - pageCount / 2;
                }

                if (end > start + pageCount - 1)
                {
                    end = start + pageCount - 1;

                    if (end > MaxPageCount)
                    {
                        end = MaxPageCount;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");

            if (PageIndex > 1)
            {
                var link = url.Action(action, values: new
                {
                    pageIndex = PageIndex - 1,
                    PageSize = this.PageSize
                });

                sb.Append($"<li class='prev'><a href='{link}'>" +
                    $"<i class='fa fa-chevron-left'></i></a></li>");
            }
            else
            {
                sb.Append(" <li class='prev disabled'>" +
                    "<a><i class='fa fa-chevron-left'></i></a></li>");
            }

            for (int i = start; i <= end; i++)
            {
                if (i == PageIndex)
                {
                    sb.Append($"<li class='active'><a>{i}</a></li>");
                }
                else
                {
                    var link = url.Action(action, values: new
                    {
                        pageIndex = i,
                        PageSize = this.PageSize
                    });

                    sb.Append($"<li><a href='{link}'>{i}</a></li>");
                }
            }

            if (PageIndex < MaxPageCount)
            {
                var link = url.Action(action, values: new
                {
                    pageIndex = PageIndex + 1,
                    PageSize = this.PageSize
                });

                sb.Append($"<li class='next'><a href='{link}'>" +
                    $"<i class='fa fa-chevron-right'></i></a></li>");
            }
            else
            {
                sb.Append(" <li class='next disabled'>" +
                    "<a><i class='fa fa-chevron-right'></i></a></li>");
            }

            sb.Append("</ul>");


            return new HtmlString(sb.ToString());
        }
    }
}