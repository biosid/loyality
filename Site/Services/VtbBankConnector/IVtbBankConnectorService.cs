using Vtb24.Site.Services.VtbBankConnector.Models.Inputs;
using Vtb24.Site.Services.VtbBankConnector.Models.Outputs;

namespace Vtb24.Site.Services.VtbBankConnector
{
    public interface IVtbBankConnectorService
    {
        /// <summary>
        /// Проверяет зарегистрованна ли карта
        /// </summary>
        /// <returns></returns>
        bool IsCardRegistered(string clientId);

        /// <summary>
        /// Регистрирует карту клиента
        /// </summary>
        void RegisterCard(string clientId);

        /// <summary>
        /// Регистрирует клиента
        /// </summary>
        void RegisterClient(RegisterClientParams parameters);

        /// <summary>
        /// Блокировка клиента для дальнейшего удаления
        /// </summary>
        void BlockClientToDelete(string clientId);

        /// <summary>
        /// Получение параметров для регистрации карты
        /// </summary>
        CardRegistrationParameters GetCardRegistrationParameters(string clientId);

        /// <summary>
        /// Проверить статус регистрации карты по ID запроса на регистрацию
        /// </summary>
        bool VerifyCardRegistration(string orderId);

        /// <summary>
        /// Проверка статуса клиента
        /// </summary>
        bool IsClientOnBlocking(string clientId);

        /// <summary>
        /// Обновелние email клиента
        /// </summary>
        void UpdateClientEmail(string clientId, string email);
    }
}
