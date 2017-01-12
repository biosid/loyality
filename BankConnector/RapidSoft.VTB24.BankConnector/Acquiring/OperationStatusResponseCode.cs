namespace RapidSoft.VTB24.BankConnector.Acquiring
{
    public class OperationStatusResponseCode
    {
        // АВТОРИЗАЦИЯ УСПЕШНО ЗАВЕРШЕНА
        public const string AS000 = "AS000";

        // ОТКАЗ В АВТОРИЗАЦИИ
        public const string AS100 = "AS100";

        // ОТКАЗ В АВТОРИЗАЦИИ. Ошибочный номер карты
        public const string AS101 = "AS101";

        // ОТКАЗ В АВТОРИЗАЦИИ. Недостаточно средств 
        public const string AS102 = "AS102";

        // ОТКАЗ В АВТОРИЗАЦИИ. Неверный срок действия карты
        public const string AS104 = "AS104";

        // ОТКАЗ В АВТОРИЗАЦИИ. Превышен лимит
        public const string AS105 = "AS105";

        // ОТКАЗ В АВТОРИЗАЦИИ. Ошибка приёма данных.
        public const string AS107 = "AS107";

        // ОТКАЗ В АВТОРИЗАЦИИ. Подозрение на мошенничество
        public const string AS108 = "AS108";

        // ОТКАЗ В АВТОРИЗАЦИИ. Превышен лимит операций Uniteller
        public const string AS109 = "AS109";

        // ПОВТОРИТЕ АВТОРИЗАЦИЮ
        public const string AS200 = "AS200";

        // ОШИБКА СИСТЕМЫ. Свяжитесь с Uniteller
        public const string AS998 = "AS998";
    }
}
