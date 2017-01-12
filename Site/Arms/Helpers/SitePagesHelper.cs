using System;
using System.Configuration;
using System.Web.Mvc;

namespace Vtb24.Arms.Helpers
{
    public static class SitePagesHelper
    {
        private static readonly string SiteUrl = ConfigurationManager.AppSettings["content_site_url"];
        private static readonly string PlainPreviewUrlTemplate = ConfigurationManager.AppSettings["content_preview_plain_page_url_template"];
        private static readonly string OfferPreviewUrlTemplate = ConfigurationManager.AppSettings["content_preview_offer_page_url_template"];
        private static readonly string PreviewVersionUrlTemplate = ConfigurationManager.AppSettings["content_preview_page_version_url_template"];

        public static string SiteRoot
        {
            get { return SiteUrl; }
        }

        public static string SitePlainPagePreviewUrl(this HtmlHelper html, Guid pageId)
        {
            return string.Format(PlainPreviewUrlTemplate, pageId);
        }

        public static string SiteOfferPagePreviewUrl(this HtmlHelper html, int partnerId)
        {
            return string.Format(OfferPreviewUrlTemplate, partnerId);
        }

        public static string SitePageVersionPreviewUrl(this HtmlHelper html, Guid pageVersionId)
        {
            return string.Format(PreviewVersionUrlTemplate, pageVersionId);
        }
    }
}
