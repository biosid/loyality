using System.Configuration;
using System.Web.Mvc;
using Vtb24.Site.Content.Pages;
using Vtb24.Site.Content.Pages.Models;
using Vtb24.Site.Content.Pages.Models.Inputs;

namespace Vtb24.Site.Filters
{
    public class SeoFilter : IResultFilter
    {
        public SeoFilter(IPagesManagement pages)
        {
            _pages = pages;
        }

        private static readonly string DefaultPageKeywords = ConfigurationManager.AppSettings["content_default_page_keywords"];
        private static readonly string DefaultPageDescription = ConfigurationManager.AppSettings["content_default_page_description"];

        private readonly IPagesManagement _pages;

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var url = filterContext.HttpContext.Request.Url;
            if (url == null)
            {
                SetDefaultSeoValues(filterContext.Controller.ViewBag);
                return;
            }

            var pageUrl = url.LocalPath;
            var options = new GetPlainPagesOptions { Status = PageStatus.Active };
            var page = _pages.GetPageByUrl(pageUrl, options);
            if (page == null)
            {
                SetDefaultSeoValues(filterContext.Controller.ViewBag);
                return;
            }

            var pageData = page.CurrentVersion.Data;

            filterContext.Controller.ViewBag.SeoTitle = pageData.Title;
            filterContext.Controller.ViewBag.SeoKeywords = pageData.Keywords;
            filterContext.Controller.ViewBag.SeoDescription = pageData.Description;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        private static void SetDefaultSeoValues(dynamic viewBag)
        {
            viewBag.SeoKeywords = DefaultPageKeywords;
            viewBag.SeoDescription = DefaultPageDescription;
        }
    }
}
