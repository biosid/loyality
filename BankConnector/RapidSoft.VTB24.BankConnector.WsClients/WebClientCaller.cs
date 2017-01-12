using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace RapidSoft.VTB24.BankConnector.WsClients
{
    public static class WebClientCaller
	{
		[DebuggerStepThrough]
		public static void CallService<TService>(Action<TService> action)
			where TService : ICommunicationObject, IDisposable, new()
		{
			Contract.Requires(action != null);

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
		public static TResult CallService<TService, TResult>(Func<TService, TResult> func)
			where TService : ICommunicationObject, IDisposable, new()
		{
			var result = default(TResult);

			CallService<TService>(service => { result = func(service); });

			return result;
		}
	}
}