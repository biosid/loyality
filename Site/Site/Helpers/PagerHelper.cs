using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Vtb24.Site.Models.Shared;

namespace Vtb24.Site.Helpers
{
    public static class PagerHtmlHelper
    {
        public const int MIN_ITEMS = 5;

        /// <summary>
        /// Пейджер формата "туда 1 .. 10 [11] 12 .. 25 сюда"
        /// <code>
        /// <![CDATA[
        ///     <!-- По умолчанию. Использует partial "~/Views/Shared/_Pager" -->
        ///     @Html.Pager(Model.TotalPages, Model.Page)
        ///     
        ///     <!-- С пользовательским шаблоном -->
        ///     @Html.Pager(Model.TotalPages, Model.Page, template: "~/Views/Users/_UsersPager")
        /// 
        ///     <!-- C нестандартным URL "/users?index=1" -->
        ///     @Html.Pager(Model.TotalPages, Model.Page, url: Url.Action("Index", "Users", new{ index="{0}" }))
        /// 
        ///     <!-- C хэш навигацией "#page=1", на манер одностраничных приложений -->
        ///     @Html.Pager(Model.TotalPages, Model.Page, url: "#page={0}")
        /// ]]>
        /// </code>
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="total">Общее количество страниц</param>
        /// <param name="page">Текущая страница</param>
        /// <param name="items">Отображаемое количество страниц, минимум 5 (первая, последняя, выбранная и две заглушки)</param>
        /// <param name="template">Имя partial-шаблона для отображения пейджера</param>
        /// <param name="url">Шаблон URL для String.Format</param>
        public static IHtmlString Pager(this HtmlHelper html, int total, int page = 1, int items = 7, string template = "_Pager", string url = null)
        {
            page = page <= 0 ? 1 : page;
            items = items < MIN_ITEMS ? MIN_ITEMS : items;
            url = url ?? GetUrlTemplateFromUrl(
                // ReSharper disable PossibleNullReferenceException
                html.ViewContext.RequestContext.HttpContext.Request.RawUrl
                // ReSharper restore PossibleNullReferenceException
            );

            var model = new PagerModel
            {
                Prev = page > 1
                            ? new PageModel { Number = page - 1, Url = string.Format(url, page - 1) }
                            : null,
                Next = page < total
                            ? new PageModel { Number = page + 1, Url = string.Format(url, page + 1) }
                            : null,
                Pages = Generate(page, total, items)
                    .Select(p =>
                            p == -1
                                ?
                                new PageModel
                                {
                                    IsPlaceholder = true
                                }
                                :
                                new PageModel
                                {
                                    Number = p,
                                    IsActive = p == page,
                                    Url = string.Format(url, p)
                                }
                    ).ToArray()
            };


            return html.Partial(template, model);
        }


        #region Приватные методы

        private static string GetUrlTemplateFromUrl(string url)
        {
            url = Regex.Replace(url, @"(?<=&|\?)page=[^&]*&?", "").TrimEnd('&');
            // ReSharper disable StringIndexOfIsCultureSpecific.1
            return url + (url.IndexOf("?") == -1 ? '?' : '&') + "page={0}";
            // ReSharper restore StringIndexOfIsCultureSpecific.1
        }

        private static IEnumerable<int> Generate(int page, int total, int items)
        {
            // 01 02 03 04 05 .. 25      -- left: []         right: [-1,25]
            // 01 .. 09 10 11 .. 25      -- left: [1,-1]     right: [-1,25]
            // 01 .. 21 22 23 25 25      -- left: [1,-1]     right: []
            // 01 02 03 04 05              -- left: []         right: []

            var isFit = items >= total;
            var left = isFit || page <= items - 3 ? new int[] { } : new[] { 1, -1 };
            var right = isFit || page >= total - items + 4 ? new int[] { } : new[] { -1, total };
            var length = Math.Min(total, items - left.Length - right.Length);

            int start;
            if (left.Length == 0)
            {
                start = 1; // от начала
            }
            else if (right.Length == 0)
            {
                start = total - length + 1; // от конца
            }
            else
            {
                start = page - (length / 2); // выбранная страница посередине пейджера
            }

            return left.Concat(Enumerable.Range(start, length)).Concat(right);
        }

        #endregion


        #region Модели представления

        public class PagerModel
        {
            public PageModel Next { get; set; }

            public PageModel Prev { get; set; }

            public PageModel[] Pages { get; set; }
        }

        public class PageModel
        {
            public int Number { get; set; }

            public string Url { get; set; }

            public bool IsActive { get; set; }

            public bool IsPlaceholder { get; set; }
        }

        #endregion
    }
}