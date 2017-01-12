using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vtb24.Arms.Helpers
{
    public static class QueryHelper
    {
        #region Статические методы

        public static string ToQuery(object obj)
        {
            var properties = obj.GetType()
                .GetProperties()
                .Where(p => HasSetter(p) && !IsDefaultValue(p, obj))
                .Select(p => MapToQuery(p, obj))
                .ToArray();

            return string.Join("&", properties);
        }

        public static RouteValueDictionary ToRouteValue(object obj)
        {
            var properties = obj.GetType()
                .GetProperties()
                .Where(p => HasSetter(p) && !IsDefaultValue(p, obj))
                .ToArray();

            var route = new RouteValueDictionary();

            foreach (var item in properties)
            {
                if (item.PropertyType.IsArray)
                {
                    var arr = (Array)item.GetValue(obj, null);
                    var values = arr.Cast<object>()
                        .Select(i => i.ToString())
                        .ToArray();

                    for (int i = 0; i < values.Length; i++)
                    {
                        route.Add(String.Format("{0}[{1}]", item.Name, i), values[i]);
                    }

                    continue;
                }

                if (item.PropertyType == typeof(DateTime?))
                {
                    var date = (DateTime?) item.GetValue(obj, null);

                    if (date.HasValue)
                    {
                        route.Add(item.Name, date.Value.ToShortDateString());
                    }
                    continue;
                }

                route.Add(item.Name, item.GetValue(obj, null).ToString());
            }

            return route;
        }

        public static T MixQueryTo<T>(T obj, string query, bool overwrite = false)
        {
            var properties = obj.GetType()
                .GetProperties()
                .Where(p => HasSetter(p) && (overwrite || IsDefaultValue(p, obj)))
                .ToArray();

            var qs = HttpUtility.ParseQueryString(query);

            foreach (var p in properties.Where(p => qs.AllKeys.Contains(p.Name)))
            {
                var val = ConvertTo(p.PropertyType, qs.GetValues(p.Name));
                p.SetValue(obj, val);
            }

            return obj;
        }

        #endregion


        #region Extension для UrlHelper

        public static string ToQuery(this UrlHelper helper, object obj)
        {
            return ToQuery(obj);
        }

        public static T MixQueryTo<T>(this UrlHelper helper, T obj, string query, bool overwrite = false)
        {
            var actualQuery = query;
            if (string.IsNullOrEmpty(query))
            {
                actualQuery = HttpContext.Current.Request.QueryString.ToString();
            }

            return MixQueryTo(obj, actualQuery, overwrite);
        }

        #endregion


        #region Приватные методы

        private static bool HasSetter(PropertyInfo p)
        {
            return p.GetSetMethod() != null;
        }

        private static bool IsDefaultValue(PropertyInfo p, object obj)
        {
            var val = p.GetValue(obj, null);
            return val == null ||
                   val.GetType().IsValueType
                   && Equals(val, Activator.CreateInstance(val.GetType()));
        }

        private static string MapToQuery(PropertyInfo p, object obj)
        {
            if (p.PropertyType.IsArray)
            {
                var arr = (Array) p.GetValue(obj, null);
                var values = arr.Cast<object>()
                    .Select(i => p.Name + "=" + HttpUtility.UrlEncode(i.ToString()))
                    .ToArray();
                return string.Join("&", values);
            } 
            
            return p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());   
        }

        private static object ConvertTo(Type type, params string[] values)
        {
            var underlying = Nullable.GetUnderlyingType(type);

            if (underlying != null)
            {
                if (values == null)
                {
                    return null;
                }
                type = underlying;
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, values[0]);
            }

            if (type.IsArray)
            {
                type = type.GetElementType();
                var arr = Array.CreateInstance(type, values.Length);
                for (var i = 0; i < values.Length; i++)
                {
                    arr.SetValue(ConvertTo(type, values[i]), i);
                }
                return arr;
            }

            return Convert.ChangeType(values[0], type);
        }

        #endregion
    }
}