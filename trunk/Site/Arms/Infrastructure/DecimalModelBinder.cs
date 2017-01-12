using System;
using System.Globalization;
using System.Web.Mvc;

namespace Vtb24.Arms.Infrastructure
{
    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            var valueStr = valueResult.AttemptedValue;

            if (string.IsNullOrEmpty(valueStr))
                return null;

            object actualValue = null;
            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue, 
                    valueResult.AttemptedValue.Contains(".") ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
            }

            return actualValue;
        }
    }
}