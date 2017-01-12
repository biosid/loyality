using System.ComponentModel;

namespace Rapidsoft.VTB24.Reports.Etl.EtlReports.Models
{
    public enum InteractionType
    {
        [Description("3.1. Регистрация клиентов в Системе лояльности")]
        LoyaltyRegistrations,

        [Description("3.2. Регистрация клиентов на стороне Банка")]
        BankRegistrations,

        [Description("3.3. Активация клиентов в Системе лояльности")]
        Activations,

        [Description("3.4. Отключение клиентов от Системы лояльности")]
        Detachments,

        [Description("3.5. Изменение анкетных данных клиентов Банком")]
        BankClientUpdates,

        [Description("3.6. Начисление бонусов на бонусные счета клиентов")]
        Accruals,

        [Description("3.7. Формирование кампаний")]
        PromoActions,

        [Description("3.8. Формирование списка участников целевых кампаний")]
        Audiences,

        [Description("3.9. Формирование персональных сообщений")]
        Messages,

        [Description("3.10. Отправка реестра совершенных заказов")]
        Orders,

        [Description("3.11. Оповещение об обновлении анкетных данных клиентов")]
        LoyaltyClientUpdates,

        [Description("3.12. Изменение номера мобильного телефона клиента Банком")]
        LoginUpdates,

        [Description("3.13. Сброс пароля клиента")]
        PasswordResets,

        [Description("3.14. Получение персональных банковских предложений")]
        BankOffers
    }
}
