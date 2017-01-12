using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Vtb24.Arms.Helpers
{
    public static class PagerHelpers
    {
        public static MvcHtmlString Pager(this HtmlHelper html, int total, int page = 1, int items = 10, string template = "_Pager", string url = null)
        {
            if (page <= 0)
            {
                page = 1;
            }

            if (url == null)
            {
                url = GetUrlTemplateFromUrl(
                    html.ViewContext.RequestContext.HttpContext.Request.Url.ToString()
                );
            }

            var model = new MkIModel();

            if (total <= 1)
            {
                model.Pages = 
                    new[]
                    {
                        new Page
                        {
                            Url = string.Format(url, 1),
                            IsActive = true,
                            Number = 1
                        }
                    };
            } else
            {
                model.Prev = page > 1
                                 ? new Page { Number = page - 1, Url = string.Format(url, page - 1) }
                                 : null;

                model.Next = page < total
                                 ? new Page { Number = page + 1, Url = string.Format(url, page + 1) }
                                 : null;

                model.Pages = GenerateMkIPages(page, total, items)
                    .Select(p => 
                        p.HasValue
                            ? new Page
                            {
                                Number = p.Value, IsActive = p.Value == page, Url = string.Format(url, p.Value)
                            }
                            : new Page
                            {
                                IsPlaceholder = true
                            }
                    ).ToArray();
            }


            return html.Partial(template, model);
        }


        #region Приватные хелперы

        private static string GetUrlTemplateFromUrl(string url)
        {
            url = Regex.Replace(url, @"&?page=(\d+)", "");

            if (url.IndexOf("?", StringComparison.InvariantCulture) == -1)
            {
                url += "?page={0}";
            }
            else
            {
                url += "&page={0}";
            }

            return url;
        }

        private static IEnumerable<int?> GenerateMkIPages(int page, int total, int items)
        {
            if (total <= 0)
                return Enumerable.Empty<int?>();

            if (total <= items)
                return NullableRange(1, total);

            if (items < 5)
                items = 5;

            if (page < items - 2)
                return NullableRange(1, items - 2)
                    .Concat(new int?[] { null, total });

            if (page > total - items + 3)
                return new int?[] { 1, null }
                    .Concat(NullableRange(total - items + 3, items - 2));

            return new int?[] { 1, null }
                .Concat(NullableRange(page - (items - 5) / 2, items - 4))
                .Concat(new int?[] { null, total });
        }

        private static IEnumerable<int?> NullableRange(int start, int count)
        {
            return Enumerable.Range(start, count).Select(i => (int?)i);
        }

        #endregion


        #region Модельки

        public class MkIModel
        {
            public Page Next { get; set; }
            
            public Page Prev { get; set; }

            public Page[] Pages { get; set; }
        }

        public class Page
        {
            public long Number { get; set; }

            public string Url { get; set; }

            public bool IsActive { get; set; }

            public bool IsPlaceholder { get; set; }
        }

        #endregion
    }
}