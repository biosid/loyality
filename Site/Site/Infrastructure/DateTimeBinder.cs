using System;
using System.Globalization;
using System.Web.Mvc;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Infrastructure
{
    public class DateTimeRangeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext", "controllerContext is null.");
            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext", "bindingContext is null.");

            var from = TryGet(bindingContext, "from");
            var to = TryGet(bindingContext, "to");

            return new DateTimeRange(from, to);
        }

        private DateTime? TryGet(ModelBindingContext bindingContext, string key)
        {
            if (String.IsNullOrEmpty(key))
                return null;

            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "." + key);
            if (valueResult == null && bindingContext.FallbackToEmptyPrefix)
                valueResult = bindingContext.ValueProvider.GetValue(key);

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);
            
            if (valueResult == null)
                return null;

            try
            {
                if (!string.IsNullOrEmpty(valueResult.AttemptedValue))
                {
                    const string format = "dd.MM.yyyy";
                    DateTime date;
                    if (DateTime.TryParseExact(valueResult.AttemptedValue,
                                               format,
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out date))
                    {
                        return date;
                    }
                }
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
            }
            return null;
        }
    }
}