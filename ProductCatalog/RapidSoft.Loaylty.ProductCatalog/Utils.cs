namespace RapidSoft.Loaylty.ProductCatalog
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    using API.Entities;

    public static class Utils
    {
        /// <summary>
        /// throws ArgumentException if predicate is true
        /// </summary>
        /// <typeparam name="T">type of object</typeparam>
        /// <param name="predicate">проверка условия</param>
        /// <param name="param">object to check</param>
        /// <param name="message">object name</param>
        public static void CheckArgument<T>(Func<T, bool> predicate, T param, string message)
        {
            if (predicate(param))
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// throws ArgumentException if predicate is true
        /// </summary>
        /// <param name="predicate">проверка условия</param>
        /// <param name="message">object name</param>
        public static void CheckArgument(bool predicate, string message)
        {
            if (predicate)
            {
                throw new ArgumentException(message);
            }
        }

        public static void CheckParameters<TParameters>(TParameters parameters)
        {
            var supportedTypes = new List<Type>() { typeof(StringLengthAttribute), typeof(RequiredAttribute) };

            foreach (var property in typeof(TParameters).GetProperties()
                                                  .Where(pi => pi.CustomAttributes.Any(a => supportedTypes.Contains(a.AttributeType))).ToList())
            {
                foreach (var attributeData in property.GetCustomAttributesData().Where(a => supportedTypes.Contains(a.AttributeType)))
                {
                    if (attributeData.AttributeType == typeof(StringLengthAttribute))
                    {
                        if (!attributeData.ConstructorArguments.Any())
                        {
                            return;
                        }

                        var maxLen = (int)attributeData.ConstructorArguments[0].Value;

                        var value = (string)property.GetValue(parameters);

                        if (string.IsNullOrEmpty(value))
                        {
                            continue;
                        }

                        var errorMessage = attributeData.ConstructorArguments.Count >= 2
                            ? attributeData.ConstructorArguments[1].ToString()
                            : null;

                        if (value.Length > maxLen)
                        {
                            throw new ArgumentOutOfRangeException(string.IsNullOrEmpty(errorMessage) ? property.Name : errorMessage);
                        }
                    }

                    if (attributeData.AttributeType == typeof(RequiredAttribute))
                    {
                        var name = property.Name;
                        var value = property.GetValue(parameters);

                        CheckArgument(value == null, name);

                        if (value is string)
                        {
                            CheckArgument(string.IsNullOrEmpty((string)value), name);
                        }
                        
                        if (value is int)
                        {
                            CheckArgument((int)value < 0, name);
                        }

                        if (value is decimal)
                        {
                            CheckArgument((decimal)value < 0, name);
                        }

                        if (value is DateTime)
                        {
                            CheckArgument((DateTime)value <= DateTime.MinValue, name);
                        }

                        if (value is IEnumerable<object>)
                        {
                            CheckArgument(!((IEnumerable<object>)value).Any(), name);
                        }
                    }
                }
            }
        }

        public static ProductParam[] EscapeQuotes(ProductParam[] productParam)
        {
            var result = new List<ProductParam>();

            foreach (var param in productParam)
            {
                var escapedParam = new ProductParam() { Name = param.Name, Unit = param.Unit, Value = param.Value };

                if (!string.IsNullOrEmpty(escapedParam.Name))
                {
                    escapedParam.Name = escapedParam.Name
                         .Replace("\"", "\"\"")
                         .Replace("'", "''");
                }

                if (!string.IsNullOrEmpty(escapedParam.Unit))
                {
                    escapedParam.Unit = escapedParam.Unit
                        .Replace("\"", "\"\"")
                        .Replace("'", "''");
                }

                if (!string.IsNullOrEmpty(escapedParam.Value))
                {
                    escapedParam.Value = escapedParam.Value
                        .Replace("\"", "\"\"")
                        .Replace("'", "''");
                }

                result.Add(escapedParam);
            }

            return result.ToArray();
        }
    }
}