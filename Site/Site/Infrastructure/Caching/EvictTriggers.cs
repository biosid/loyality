using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Vtb24.Site.Infrastructure.Caching
{
    public class EvictTriggers : IEnumerable<KeyValuePair<string, Type>>
    {
        public EvictTriggers()
        {
            Methods = new Dictionary<string, Type>();
        }

        public Dictionary<string, Type> Methods { get; private set; }


        public void Add<T>(Expression<Action<T>> exp)
        {
            var type = typeof (T);
            var methodName = KeysUtil.GetMethodFullName(exp);

            if (!Methods.ContainsKey(methodName))
            {
                Methods[methodName] = type;
            }
        }

        public IEnumerator<KeyValuePair<string, Type>> GetEnumerator()
        {
            return Methods.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}