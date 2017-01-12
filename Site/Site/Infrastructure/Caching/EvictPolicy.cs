using System;
using System.Collections.Generic;

namespace Vtb24.Site.Infrastructure.Caching
{
    internal class EvictPolicy
    {
        public EvictPolicy()
        {
            Methods = new List<KeyValuePair<string, Func<VaryByContext, string>>>();
        }

        public IList<KeyValuePair<string, Func<VaryByContext, string>>> Methods { get; private set; }

        public TimeSpan Duration { get; set; }
    }
}