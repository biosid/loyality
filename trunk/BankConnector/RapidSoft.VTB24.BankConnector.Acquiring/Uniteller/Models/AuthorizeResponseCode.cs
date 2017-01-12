namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models
{
    internal enum AuthorizeResponseCode
    {
        Unknown = 0,

        // ReSharper disable InconsistentNaming

        // АВТОРИЗАЦИЯ УСПЕШНО ЗАВЕРШЕНА
        AS000,

        // ОТКАЗ В АВТОРИЗАЦИИ
        AS100,

        // ОТКАЗ В АВТОРИЗАЦИИ. Ошибочный номер карты
        AS101,

        // ОТКАЗ В АВТОРИЗАЦИИ. Недостаточно средств 
        AS102,

        // ОТКАЗ В АВТОРИЗАЦИИ. Неверный срок действия карты
        AS104,

        // ОТКАЗ В АВТОРИЗАЦИИ. Превышен лимит
        AS105,

        // ОТКАЗ В АВТОРИЗАЦИИ. Ошибка приёма данных.
        AS107,

        // ОТКАЗ В АВТОРИЗАЦИИ. Подозрение на мошенничество
        AS108,

        // ОТКАЗ В АВТОРИЗАЦИИ. Превышен лимит операций Uniteller
        AS109,

        // ПОВТОРИТЕ АВТОРИЗАЦИЮ
        AS200,

        // ОШИБКА СИСТЕМЫ. Свяжитесь с Uniteller
        AS998

        // ReSharper restore InconsistentNaming
    }
}
