using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Vtb24.Arms.Helpers
{
    public static class EnumDropdownHelper
    {
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = GetNonNullableModelType(metadata.ModelType);
            var values = Enum.GetValues(enumType).Cast<TEnum>();

            var items = from value in values
                        select new SelectListItem
                        {
                            Text = value.EnumDescription(),
                            Value = value.ToString(),
                            Selected = value.Equals(metadata.Model)
                        };

            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, Func<TEnum, bool> filter, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = GetNonNullableModelType(metadata.ModelType);
            var values = Enum.GetValues(enumType).Cast<TEnum>();

            var items = from value in values where filter(value)
                        select new SelectListItem
                        {
                            Text = value.EnumDescription(),
                            Value = value.ToString(),
                            Selected = value.Equals(metadata.Model)
                        };

            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name, TEnum val, object htmlAttributes = null)
        {
            var enumType = GetNonNullableModelType(typeof(TEnum));
            var values = Enum.GetValues(enumType).Cast<TEnum>();

            var items = from value in values
                        select new SelectListItem
                        {
                            Text = value.EnumDescription(),
                            Value = value.ToString(),
                            Selected = value.Equals(val)
                        };

            return htmlHelper.DropDownList(name, items, htmlAttributes);
        }

        private static Type GetNonNullableModelType(Type realModelType)
        {
            var underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }
    }
}
