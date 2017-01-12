using System;

namespace Vtb24.Site.Infrastructure.Caching
{
    internal class CachingPolicy
    {
        public CachingPolicy()
        {
            DelayExpirationOnExceptionTimeSpan = TimeSpan.FromMinutes(5);
        }

        public string Method { get; set; }

        public Func<VaryByContext, string> VaryBy { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan? ExpirationTimeSpan { get; set; }

        public TimeSpan DelayExpirationOnExceptionTimeSpan { get; set; }

        public TimeSpan? BackgroundPreloadTimespan { get; set; }

        public Action<Exception> OnSilentException { get; set; }
    }
}