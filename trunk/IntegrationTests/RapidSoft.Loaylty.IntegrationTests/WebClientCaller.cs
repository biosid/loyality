namespace RapidSoft.Loaylty.IntegrationTests
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.ServiceModel;
    using System.Threading;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    public static class WebClientCaller
    {
        [DebuggerStepThrough]
        public static void CallService<TService>(Action<TService> action) where TService : ICommunicationObject, IDisposable, new()
        {
            using (var service = new TService())
            {
                Exception exception = null;
                try
                {
                    action(service);
                    if (service.State != CommunicationState.Faulted)
                    {
                        service.Close();
                    }
                    else
                    {
                        service.Abort();
                    }
                }
                catch (Exception ex)
                {
                    service.Abort();
                    exception = ex;
                }

                if (exception != null)
                {
                    throw exception;
                }
            }
        }

        [DebuggerStepperBoundary]
        public static TResult CallService<TService, TResult>(Func<TService, TResult> func) where TService : ICommunicationObject, IDisposable, new()
        {
            var result = default(TResult);

            CallService<TService>(service => { result = func(service); });

            return result;
        }

        [DebuggerStepperBoundary]
        public static TResult CallService<TService, TResult>(Func<TService, TResult> func, int count, TimeSpan delay) where TService : ICommunicationObject, IDisposable, new()
        {
            Exception lastException;
            var currentDelay = delay;

            do
            {
                try
                {
                    return CallService(func);
                }
                catch (Exception e)
                {
                    lastException = e;
                }

                Thread.Sleep(currentDelay);

                currentDelay = currentDelay.Add(currentDelay);
            }
            while (--count > 0);

            throw lastException;
        }
    }
}
