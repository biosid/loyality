using System.ComponentModel;
using System.Web.Mvc;

namespace Rapidsoft.VTB24.Reports.Site.Helpers
{
    public static class EnumDescriptionHelper
    {
        public static string DescriptionFor<TEnum>(this HtmlHelper htmlHelper, TEnum value)
        {
            return value.GetEnumDescription();
        }

        public static string GetEnumDescription<TEnum>(this TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}
