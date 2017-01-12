namespace RapidSoft.Loaylty.PartnersConnector.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var results = new List<ValidationResult>();

            if (value is IEnumerable)
            {
                IEnumerable enumerable = (IEnumerable)value;
                
                foreach (object item in enumerable)
                {
                    Validate(item, results);
                }
            }
            else
            {
                Validate(value, results);
            }

            if (results.Count > 0)
            {
                var errorMessage = string.Join(Environment.NewLine, results.Select(r => string.Format("member:{0} error:{1}", validationContext.MemberName, r.ErrorMessage)).ToArray());

                return new ValidationResult(errorMessage)
                {
                    ErrorMessage = errorMessage
                };
            }

            return ValidationResult.Success;
        }

        private static void Validate(object item, List<ValidationResult> results)
        {
            if (item == null)
            {
                return;
            }

            var context = new ValidationContext(item, null, null);
            Validator.TryValidateObject(item, context, results, true);
        }
    }
}