using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;
using System.Web.Compilation;
using System.Globalization;

namespace RapidSoft.Etl.Monitor.Helpers
{
    public static class MvcExtensions
    {
        public static void AddErrorMessageFor<TModel, TProperty>(this Controller controller, Expression<Func<TModel, TProperty>> expression, string errorMessage)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var modelName = controller.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
            controller.ModelState.AddModelError(modelName, errorMessage);
        }

        public static bool IsInvalid<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
            return htmlHelper.ViewData.ModelState.ContainsKey(fullHtmlFieldName);
        }

        public static string GetNameFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var modelName = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
            return modelName;
        }

        public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string prefix, string postfix)
        {
            var message = htmlHelper.ValidationMessageFor(expression);
            return WrapMessage(message, prefix, postfix);
        }

        public static MvcHtmlString InfoMessageFor<TModel>(this HtmlHelper<TModel> htmlHelper, string message, string prefix, string postfix)
        {
            return WrapMessage(message, prefix, postfix);
        }

        public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string prefix, string postfix, object attributes)
        {
            var message = htmlHelper.ValidationMessageFor(expression, null, attributes);
            return WrapMessage(message, prefix, postfix);
        }

        public static MvcHtmlString ValidationMessage(this HtmlHelper htmlHelper, string name, string prefix, string postfix, object attributes = null)
        {
            var message = htmlHelper.ValidationMessage(name, attributes);
            return WrapMessage(message, prefix, postfix);
        }

        public static string ValidationMessageNoEncode<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var message = htmlHelper.ValidationMessageFor(expression);
            return message == null || string.IsNullOrEmpty(message.ToString().Trim()) ? null : HttpUtility.HtmlDecode(message.ToString());
        }

        private static MvcHtmlString WrapMessage(MvcHtmlString validationMessageFor, string prefix, string postfix)
        {
            if (validationMessageFor == null)
            {
                return validationMessageFor;
            }

            var message = validationMessageFor.ToHtmlString();

            return WrapMessage(message, prefix, postfix);
        }

        private static MvcHtmlString WrapMessage(string message, string prefix, string postfix)
        {
            if (string.IsNullOrEmpty(message))
            {
                return MvcHtmlString.Create("");
            }
            return MvcHtmlString.Create(prefix + message + postfix);
        }

        public static string StripScripts(this HtmlHelper htmlhelper, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return Regex.Replace(str, @"\</?\s*script.*?\>", "");
        }

        public static string GetVersion(this HtmlHelper html)
        {
            return HttpUtility.HtmlEncode(ConfigurationManager.AppSettings.AllKeys.Contains("Version")
                              ? ConfigurationManager.AppSettings["Version"]
                              : DateTime.Now.Ticks.ToString());
        }

        public static MvcHtmlString DropDownForEnum<TModel, TEnum>(this HtmlHelper helper, Expression<Func<TModel, TEnum>> property, Func<TEnum, string> toString, string nullText, TEnum nullValue)
        {
            var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(property));
            var currentValue = (TEnum)property.Compile().Invoke((TModel)helper.ViewContext.ViewData.Model);

            var sb = new StringBuilder();
            sb.Append("<select id=\"");
            sb.Append(name);
            sb.Append("\" ");
            sb.Append("name=\"");
            sb.Append(name);
            sb.Append("\">");

            if (!string.IsNullOrEmpty(nullText))
            {
                sb.Append("<option value=\"");
                sb.Append(nullValue.ToString());
                sb.Append("\">");
                sb.Append(nullText);
                sb.Append("</option>");
            }

            foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
            {
                sb.Append("<option value=\"");
                sb.Append(value.ToString());
                sb.Append("\"");

                if (object.Equals(value, currentValue))
                {
                    sb.Append(" selected=\"selected\"");
                }

                sb.Append(">");
                sb.Append(toString(value));
                sb.Append("</option>");
            }

            sb.Append("</select>");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString DateBoxFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, string>> property, string format)
        {
            return DateBoxFor(helper, property, format, null);
        }

        public static MvcHtmlString DateBoxFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, string>> property, string format, object htmlAttributes)
        {
            var viewData = helper.ViewContext.ViewData;
            var stringDate = property.Compile().Invoke((TModel)viewData.Model);
            DateTime date;
            string value;
            if (DateTime.TryParse(stringDate, out date))
            {
                value = date.ToString(format);
            }
            else
            {
                value = stringDate;
            }
            var name = viewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(property));

            return helper.TextBox(name, value, htmlAttributes);
        }

        public static MvcHtmlString DateBoxFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, DateTime?>> property, string format)
        {
            return DateBoxFor(helper, property, format, null);
        }

        public static MvcHtmlString DateBoxFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, DateTime?>> property, string format, object htmlAttributes)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }
            var viewData = helper.ViewContext.ViewData;
            var date = property.Compile().Invoke((TModel)viewData.Model);

            var value = string.Format(format, date);
            var name = viewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(property));

            return helper.TextBox(name, value, htmlAttributes);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty, TItem>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<TItem> list,
            Func<TItem, SelectListItem> convert,
            string nullText,
            string nullValue)
        {
            var nullItemAsArray = new[] { new SelectListItem { Text = nullText, Value = nullValue } };
            var items = nullItemAsArray.Union(list.Select(convert)).ToArray();

            var currentValue = (object)expression.Compile().Invoke((TModel)helper.ViewContext.ViewData.Model);

            var curVal = currentValue == null ? nullValue : currentValue.ToString();

            var item = items.FirstOrDefault(selectListItem => selectListItem.Value == curVal);

            if (item != null)
            {
                item.Selected = true;
            }

            var retVal = helper.DropDownListFor(expression, items);
            return retVal;
        }
    }
}