
using Vtb24.Site.Services.CardRegistration.Models.Outputs;

namespace Vtb24.Site.Services
{
    public interface ICardRegistration
    {
        /// <summary>
        /// Зарегистрировать карту текущего пользователя
        /// </summary>
        void RegisterCard();

        /// <summary>
        /// Проверить, что у текущего пользователя уже зарегистрирована карта
        /// </summary>
        bool IsCardRegistered();

        /// <summary>
        /// Получить параметры регистрации карты текущего пользователя
        /// </summary>
        RegistrationParameters GetRegistrationParameters();

        /// <summary>
        /// Проверить статус регистрации карты по ID запроса на регистрацию
        /// </summary>
        bool VerifyRegistration(string orderId);
    }
}
