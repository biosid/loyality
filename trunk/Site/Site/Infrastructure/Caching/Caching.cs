using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.Caching;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Infrastructure.Caching
{
    public class Caching
    {
        protected Caching(IUnityContainer container, int memoryLimitMb = 0)
        {
            _container = container;
            _interfaces = new List<string>();
            _settings = new Dictionary<string, CachingPolicy>();
            _evicts = new Dictionary<string, EvictPolicy>();
            container.RegisterType<CacheInterceptor, CacheInterceptor>(new HierarchicalLifetimeManager());
            _memoryLimit = memoryLimitMb;
        }

        private readonly IUnityContainer _container;
        private readonly List<string> _interfaces;
        private readonly IDictionary<string, CachingPolicy> _settings;
        private readonly Dictionary<string, EvictPolicy> _evicts;

        private ObjectCache _store;
        private readonly object _storeLock = new object();
        private readonly int _memoryLimit;

        #region API

        public Caching Add<T>(Expression<Action<T>> exp, TimeSpan duration, Func<VaryByContext, string> varyBy = null, EvictTriggers evicts = null)
        {
            var type = typeof (T);
            AssertIsInterface(type);
            EnsureInterception(type);

            var name = KeysUtil.GetMethodFullName(exp);
            
            AssertExists(name);
            
            var policy = new CachingPolicy { Method = name, VaryBy = varyBy, Duration = duration };
            _settings[name] = policy;

            if (evicts != null)
            {
                foreach (var evict in evicts)
                {
                    AddEvict(policy, evict.Key, evict.Value);
                }
            }

            return this;
        }

        public Caching Add<T>(Expression<Action<T>> exp, TimeSpan duration, TimeSpan expiration, TimeSpan delay, TimeSpan? preloadOffset = null, Func<VaryByContext, string> varyBy = null, Action<Exception> onSilentException = null, EvictTriggers evicts = null)
        {
            var type = typeof(T);
            AssertIsInterface(type);
            EnsureInterception(type);

            var name = KeysUtil.GetMethodFullName(exp);

            AssertExists(name);

            var policy = new CachingPolicy
            {
                Method = name, 
                VaryBy = varyBy, 
                Duration = duration, 
                ExpirationTimeSpan = expiration, 
                DelayExpirationOnExceptionTimeSpan = delay,
                BackgroundPreloadTimespan = expiration - preloadOffset,
                OnSilentException = onSilentException
            };
            _settings[name] = policy;

            if (evicts != null)
            {
                foreach (var evict in evicts)
                {
                    AddEvict(policy, evict.Key, evict.Value);
                }
            }

            return this;
        }

        public void Purge()
        {
            lock (_storeLock)
            {
                ((MemoryCache)_store).Dispose();
                _store = null;
            }
        }

        #endregion


        #region Внутренние API

        internal CachingPolicy GetPolicy(string methodKey)
        {
            return _settings.ContainsKey(methodKey) ? _settings[methodKey] : null;
        }

        internal EvictPolicy GetEvicts(string methodKey)
        {
            var evict = _evicts.ContainsKey(methodKey) ? _evicts[methodKey] : null;
            return evict;
        } 


        internal ObjectCache Store
        {
            get
            {
                lock (_storeLock)
                {
                    if (_store == null)
                    {
                        _store = new MemoryCache("Caching", new NameValueCollection { { "CacheMemoryLimitMegabytes", _memoryLimit.ToString(CultureInfo.InvariantCulture) } });
                    }
                }
                return _store;
            }
        }

        #endregion


        #region Статическое API

        public static Caching Configure(IUnityContainer container, int memoryLimitMb = 0)
        {
            if (Current != null)
            {
                throw new InvalidOperationException("Кэш уже сконфигурирован. Нельзя повторно сконфигурировать кэш");
            }

            var caching = new Caching(container, memoryLimitMb);
            Current = caching;
            return caching;
        }

        public static Caching Current { get; private set; }

        #endregion


        #region Приватные методы

        private void AddEvict(CachingPolicy cachePolicy, string whenKey, Type whenType)
        {

            AssertIsInterface(whenType);
            EnsureInterception(whenType);

            EvictPolicy policy;
            if (!_evicts.ContainsKey(whenKey))
            {
                policy = new EvictPolicy();
                _evicts[whenKey] = policy;
            }
            else
            {
                policy = _evicts[whenKey];
            }

            policy.Methods.Add(new KeyValuePair<string, Func<VaryByContext, string>>(cachePolicy.Method, cachePolicy.VaryBy));

            if (policy.Duration < cachePolicy.Duration)
            {
                policy.Duration = cachePolicy.Duration;
            }
        }

        private static void AssertIsInterface(Type type)
        {
            if (!type.IsInterface)
            {
                throw new InvalidOperationException("Кэш возможно настроить только для интерфейсов");
            }
        }

        private void EnsureInterception(Type type)
        {
            if (_interfaces.Contains(type.Name))
            {
                return;
            }

            _interfaces.Add(type.Name);
            _container.RegisterType(
                type,
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<CacheInterceptor>()
            );
        }

        private void AssertExists(string name)
        {
            if (_settings.ContainsKey(name))
            {
                throw new InvalidOperationException(
                    string.Format("Кэш для метода {0} уже настроен", name)
                );
            }
        }

        #endregion
    }
}