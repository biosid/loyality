using System.Web.Mvc;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Helpers
{
    public static class PluralizeHtmlHelper
    {
        #region Pluralize

        public static string Pluralize(this HtmlHelper html, long num, string singular, string plural, string plural2=null)
        {
            return num.Pluralize(singular, plural, plural2);
        }

        public static string Pluralize(this HtmlHelper html, double num, string singular, string plural, string plural2 = null)
        {
            return num.Pluralize(singular, plural, plural2);
        }

        public static string Pluralize(this HtmlHelper html, decimal num, string singular, string plural, string plural2 = null)
        {
            return num.Pluralize(singular, plural, plural2);
        }

        #endregion

        #region RawPluralize

        public static MvcHtmlString RawPluralize(this HtmlHelper html, long num, string singular, string plural, string plural2 = null)
        {
            return new MvcHtmlString(num.Pluralize(singular, plural, plural2));
        }

        public static MvcHtmlString RawPluralize(this HtmlHelper html, double num, string singular, string plural, string plural2 = null)
        {
            return new MvcHtmlString(num.Pluralize(singular, plural, plural2));
        }

        public static MvcHtmlString RawPluralize(this HtmlHelper html, decimal num, string singular, string plural, string plural2 = null)
        {
            return new MvcHtmlString(num.Pluralize(singular, plural, plural2));
        }

        #endregion
    }
}