using Vtb24.Site.Services.BonusPayments.Models.Inputs;

namespace Vtb24.Site.Services.BonusPayments
{
    /// <summary>
    /// Фасад для работы с баллам (сервисы для клиентского сайта)
    /// </summary>
    public interface IBonusPayments
    {
        /// <summary>
        /// Запрос на списание баллов
        /// </summary>
        void Charge(ChargeParameters parameters);
    }
}
