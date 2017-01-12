using System;
using System.Web.Routing;

namespace Vtb24.Site.Infrastructure
{
    public class BaseQueryModel : IQueryModel
    {
        public RouteValueDictionary ToRouteValueDictionary()
        {
            var dict = new RouteValueDictionary();
            
            var currentClass = GetType();
            var properties = currentClass.GetProperties();

            foreach (var propertyInfo in properties)
            {
                var value = propertyInfo.GetValue(this);
                if (!IsDefaultValue(value))
                    dict.Add(propertyInfo.Name, value);
            }

            return dict;
        }

        private bool IsDefaultValue(object val) 
        {
            return val == null || 
                val.GetType().IsValueType 
                && Equals(val, Activator.CreateInstance(val.GetType()));
        }
    }
}