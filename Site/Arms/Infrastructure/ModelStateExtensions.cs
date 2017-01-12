using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Text;

namespace Vtb24.Arms.Infrastructure
{
    public static class ModelStateExtensions
    {
        public static IDictionary<string, string> GetAllErrors(this ModelStateDictionary modelState)
        {
            var errors = new Dictionary<string, string>();

            foreach (var value in modelState.Where(s => s.Value.Errors.Any()))
            {
                errors[value.Key] = string.Join("\n\n", value.Value.Errors.Select(e => e.ErrorMessage).ToArray());
            }

            return errors;
        }

        public static string GetErrorsText(this ModelStateDictionary modelState, string key, string delimiter)
        {
            var keyState = modelState[key];
            if (keyState == null)
            {
                return string.Empty;
            }

            var text = new StringBuilder();

            foreach (var error in keyState.Errors)
            {
                text.Append(error.ErrorMessage);
                if (delimiter != null)
                {
                    text.Append(delimiter);
                }
            }

            return text.ToString();
        }

        /// <summary>
        /// Удалить ошибки по ключу.
        /// </summary>
        /// <param name="modelState">ModelStateDictionary</param>
        /// <param name="key">ключ</param>
        public static void RemoveErrors(this ModelStateDictionary modelState, string key)
        {
            if (modelState.ContainsKey(key))
            {
                modelState[key].Errors.Clear();
            }
        }
    }
}