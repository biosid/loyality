using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Vtb24.Site.Helpers
{
    public static class EnumDropdownHelper
    {
         public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes = null)
         {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = GetNonNullableModelType(metadata);
            var values = Enum.GetValues(enumType).Cast<TEnum>();

            var items = from value in values select new SelectListItem
            {
                Text = GetEnumDescription(value),
                Value = value.ToString(),
                Selected = value.Equals(metadata.Model)
            };

            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
         }

         public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, TEnum[] values, object htmlAttributes = null)
         {
             var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

             var items = from value in values
                         select new SelectListItem
                         {
                             Text = GetEnumDescription(value),
                             Value = value.ToString(),
                             Selected = value.Equals(metadata.Model)
                         };

             return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
         }

         public static string GetEnumDescription<TEnum>(TEnum value)
         {
             var fi = value.GetType().GetField(value.ToString());

             var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

             return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
         }

         private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
         {
             var realModelType = modelMetadata.ModelType;

             var underlyingType = Nullable.GetUnderlyingType(realModelType);
             if (underlyingType != null)
             {
                 realModelType = underlyingType;
             }
             return realModelType;
         }
    }
}