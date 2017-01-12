using System;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Vtb24.Site.Infrastructure.Caching
{
    internal class CachedMethod
    {
        public CachedMethod(Func<IMethodReturn> method)
        {
            _method = method;
            _time = DelayedTime =  DateTime.UtcNow;
        }

        public readonly object SyncRoot = new object();

        private readonly object _lock = new object();
        private readonly Func<IMethodReturn> _method;
        private readonly DateTime _time;

        private IMethodReturn _value;
        private bool _valueCreated;

        public bool IsValueCreated { 
            get
            {
                lock (_lock)
                {
                    return _valueCreated;   
                }
            } 
        }

        public IMethodReturn Value
        {
            get
            {
                lock (_lock)
                {
                    if (_valueCreated)
                    {
                        return _value;
                    }
                    _value = _method();
                    _valueCreated = true;
                }
                return _value;
            }
        }

        public DateTime CreateTimeUtc { get { return _time; } }

        public DateTime DelayedTime { get; set; }

        public bool StaleFlag { get; set; }

        public bool PreloadingFlag { get; set; }
    }
}