using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Vtb24.Site.Infrastructure.Caching
{
    internal class CacheInterceptor : IInterceptionBehavior
    {
        public CacheInterceptor()
        {
            _config = Caching.Current;
        }

        private readonly Caching _config;

        private ObjectCache Store
        {
            get { return _config.Store; }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var methodKey = KeysUtil.GetMethodFullName(input.MethodBase);
            
            TriggerEvicts(methodKey);

            var policy = _config.GetPolicy(methodKey);
            if (policy == null)
            {
                return getNext()(input, getNext);
            }

            var parameters = new object[input.Inputs.Count];
            input.Inputs.CopyTo(parameters, 0);
            var key = methodKey + KeysUtil.GetInvokationArgumentsString(parameters);

            var data = new CachedMethod(() => getNext()(input, getNext));
            var result = AddOrGetExisting(policy, key, data);
            return result.Value;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return (Type.EmptyTypes);
        }

        public bool WillExecute
        {
            get
            {
                return true;
            }
        }

        private void TriggerEvicts(string methodKey)
        {
            var evicts = _config.GetEvicts(methodKey);
            if (evicts == null)
            {
                return;
            }

            var expiration = DateTimeOffset.Now.Add(evicts.Duration);
            var now = DateTime.UtcNow;

            foreach (var evict in evicts.Methods)
            {
                string varyBy = null;
                if (evict.Value != null)
                {
                    var context = new VaryByContext();
                    varyBy = evict.Value(context);
                    if (context.SuppressEvictTriggers)
                    {
                        continue;
                    }
                }

                var evictKey = GetEvictKey(evict.Key, varyBy);
                Store.Set(evictKey, now, expiration);
            }
        }

        private CachedMethod AddOrGetExisting(CachingPolicy policy, string key, CachedMethod value)
        {
            var varyBy = policy.VaryBy == null ? null : policy.VaryBy(new VaryByContext());
            var cacheKey = GetCacheKey(key, varyBy);
            var offset = DateTimeOffset.Now.Add(policy.Duration);
            var cached = (CachedMethod) Store.AddOrGetExisting(cacheKey, value, offset) ?? value;

            lock (cached.SyncRoot)
            {
                if (value == cached)
                {
                    // холодный старт, у нас самое актуальное значение
                }
                else if (cached.StaleFlag)
                {
                    // параллельный тред попросил запросить более свежее значение, пока мы сто€ли в очереди (в lock'е)
                    cached = (CachedMethod) Store.Get(cacheKey);
                }
                else
                {
                    // запускаем процесс инвалидации кэша
                    var now = DateTime.UtcNow;
                    var evictKey = GetEvictKey(policy.Method, varyBy);
                    var evictTime = Store[evictKey] as DateTime?;

                    var isEvicted = evictTime.HasValue && cached != value && cached.CreateTimeUtc < evictTime.Value;
                    var isStale = policy.ExpirationTimeSpan.HasValue &&
                                  cached.DelayedTime + policy.ExpirationTimeSpan.Value < now;
                    var shouldPreload = policy.BackgroundPreloadTimespan.HasValue &&
                                  cached.DelayedTime + policy.BackgroundPreloadTimespan < now;

                    if (isEvicted)
                    {
                        // необходимо инвалидировать кэш
                        cached.StaleFlag = true; // просьба к сто€щим в очереди тредам получить значение заново
                        Store.Set(cacheKey, value, offset);
                        cached = value;
                    }
                    else if (isStale && !cached.PreloadingFlag)
                    {
                        // необходимо попытатьс€ вз€ть новое значение
                        if (value.Value.Exception == null)
                        {
                            // значение успешно получено
                            cached.StaleFlag = true; // просьба к сто€щим в очереди тредам получить значение заново
                            Store.Set(cacheKey, value, offset);
                            cached = value;
                        }
                        else
                        {
                            // произошЄл сбой, необходимо отложить актуализацию существующего кэша
                            cached.DelayedTime =
                                DateTime.UtcNow 
                                + policy.DelayExpirationOnExceptionTimeSpan 
                                - policy.ExpirationTimeSpan.Value;
                            if (policy.OnSilentException != null)
                            {
                                policy.OnSilentException(value.Value.Exception);
                            } 
                        }
                    }
                    else if (shouldPreload && !cached.PreloadingFlag)
                    {
                        // необходимо начать подгружать данные фоном
                        cached.PreloadingFlag = true;
                        new BackgroundRunner(value).Run(m =>
                        {
                            if (m.Value.Exception == null)
                            {
                                cached.StaleFlag = true; // на случай, если кто-то ещЄ ждЄт, что маловеро€тно
                                Store.Set(cacheKey, m, offset);
                            }
                            else
                            {
                                cached.DelayedTime = 
                                    DateTime.UtcNow 
                                    + policy.DelayExpirationOnExceptionTimeSpan 
                                    - policy.BackgroundPreloadTimespan.Value;
                                cached.PreloadingFlag = false;
                                policy.OnSilentException(m.Value.Exception);
                            }
                        });
                    }
                }
            }

            // проверка текущего закэшированного значени€ на ошибки
            if (cached.Value.Exception != null)
            {
                // —ценарий:

                // * N одновременных запросов
                // * все получают один и тот же экземпл€р CachedMethod
                // * одновременно обращаютс€ к CachedMethod.Value (итого N обращений)
                // * ждут в lock'е M секунд на выполнение операции (обрабатываетс€ только первое обращение)
                // * одновременно получают одно на всех исключение
                
                // “.о. долгие операции с исключени€ми выполн€ютс€ один раз и не создают нагрузку на сервер.
                Store.Remove(cacheKey);
            }

            return cached;
        }

        private static string GetCacheKey(string key, string varyBy)
        {
            return varyBy != null ? varyBy + key : key;
        }

        private static string GetEvictKey(string key, string varyBy)
        {
            return "evict:" + GetCacheKey(key, varyBy);
        }
    }
}