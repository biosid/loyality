using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

namespace Vtb24.Site.Infrastructure.Caching
{
    internal static class KeysUtil
    {
        public static string GetMethodFullName<T>(Expression<Action<T>> exp)
        {
            var method = ((MethodCallExpression)exp.Body).Method;
            var name = typeof(T).Name + '.' + method.Name;
            return name + GetParametersString(method.GetParameters());
        }

        public static string GetMethodFullName(MethodBase method)
        {
            var name = method.DeclaringType.UnderlyingSystemType.Name + '.' + method.Name;
            return name + GetParametersString(method.GetParameters());
        }

        public static string GetParametersString(IEnumerable<ParameterInfo> parameters)
        {
            return '(' + string.Join(",", parameters.Select(p => GetTypeStringRecursive(p.ParameterType))) + ')';
        }

        public static string GetInvokationArgumentsString(IEnumerable arguments)
        {
            return JsonConvert.SerializeObject(arguments, Formatting.None);
        }

        private static string GetTypeStringRecursive(Type type)
        {
            return type.IsGenericType ? 
                string.Format(
                    "{0}<{1}>", 
                    type.Name, 
                    string.Join(",", type.GenericTypeArguments.Select(GetTypeStringRecursive))
                ) 
                : type.Name;
        }
    }
}