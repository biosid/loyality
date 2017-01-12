namespace RapidSoft.Loaylty.PromoAction.Api
{
    using System;
    using System.ServiceModel;

    /// <summary>
	/// Интерфейс мониторинга сервиса.
	/// </summary>
	[ServiceContract]
	public interface IServiceInfo
	{
		/// <summary>
		/// Метод проверки связнанных ресурсов.
		/// </summary>
		[OperationContract]
		void Ping();

		/// <summary>
		/// Метод получения версии компонента.
		/// </summary>
		/// <returns>
		/// The <see cref="Version"/>.
		/// </returns>
		[OperationContract]
		Version GetServiceVersion();
	}
}
