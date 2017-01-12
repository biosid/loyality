using System;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Vtb24.Site.Infrastructure.Caching
{
    internal class BackgroundRunner : IRegisteredObject
    {
        internal BackgroundRunner(CachedMethod method)
        {
            HostingEnvironment.RegisterObject(this);
            _method = method;
        }

        private readonly CachedMethod _method;

        public void Stop(bool immediate)
        {

            HostingEnvironment.UnregisterObject(this);
        }

        public void Run(Action<CachedMethod> callback)
        {
            Task.Factory.StartNew(() => {
                // тавтология, конечно, но геттер проще всего вызвать так
                if (_method.Value.Exception == null)
                {
                    callback(_method);
                }
                else
                {
                    callback(_method);
                }
            });
        }
    }
}