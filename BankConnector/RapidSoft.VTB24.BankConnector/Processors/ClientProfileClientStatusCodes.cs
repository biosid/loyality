namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System.Diagnostics.CodeAnalysis;

    public enum ClientProfileClientStatusCodes
    {
		/// <summary>
		/// Активирован
		/// </summary>
		Activated = 1,

		/// <summary>
		/// Создан
		/// </summary>
		Created = 20,

		/// <summary>
		/// Деактивирован
		/// </summary>
		Deactivated = 25,
		
		/// <summary>
		/// Блокирован
		/// </summary>
		Blocked = 30,

		/// <summary>
		/// Удален
		/// </summary>
		Deleted = 40,
    }
}
