using System;
using System.Globalization;
using System.Web.Mvc;

namespace Vtb24.Arms.Infrastructure
{
    public class DateTimeModelBinder : IModelBinder
    {
        // ReSharper disable InconsistentNaming

        private const string FORMAT = "dd.MM.yyyy";
        private const DateTimeStyles STYLES = DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite;

        // ReSharper restore InconsistentNaming

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            var valueStr = valueResult.AttemptedValue;

            if (string.IsNullOrEmpty(valueStr))
                return null;

            try
            {
                DateTime value;
                if (TryParse(valueStr, out value))
                    return value;
                
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Неверный формат даты");
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
            }

            return null;
        }

        private static bool TryParse(string valueStr, out DateTime value)
        {
            return DateTime.TryParseExact(valueStr, FORMAT, CultureInfo.InvariantCulture, STYLES, out value);
        }
    }
}
