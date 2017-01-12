using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Vtb24.Arms.Infrastructure
{
    public class MaskAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public static readonly Dictionary<string, string> Definitions = new Dictionary<string, string>
        {
            { "9", "[0-9]" },
            { "a", "[A-Za-zА-Яа-яёЁ]" },
            { "*", "[A-Za-zА-Яа-яёЁ0-9]" }
        };

        public string Mask { get; protected set; }

        public string RegexOverride { get; protected set; }

        public MaskAttribute(string mask, string regexOverride = null)
            : base(GenerateRegex(mask, regexOverride))
        {
            Mask = mask;
            RegexOverride = regexOverride;
        }

        private static string GenerateRegex(string mask, string regexOverride = null)
        {
            return !string.IsNullOrEmpty(regexOverride) ? regexOverride : MaskToRegex(mask);
        }

        private static string MaskToRegex(string mask)
        {
            var rx = "";

            for (var i = 0; i < mask.Length; i++)
            {
                var key = mask[i].ToString(CultureInfo.InvariantCulture);
                rx += !Definitions.ContainsKey(key) ? Regex.Escape(key) : Definitions[key];
            }

            return rx;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "mask",
                ValidationParameters =
                {
                    { "pattern", Mask }
                }
            };
        }
    }
}