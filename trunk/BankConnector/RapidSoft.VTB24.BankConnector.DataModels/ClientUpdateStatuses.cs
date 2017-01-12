namespace RapidSoft.VTB24.BankConnector.DataModels
{
    public class ClientUpdateStatuses
    {
        // Обновлён успешно
        public const int Success = 0;
        
        // Неизвестная ошибка обновления
        public const int Error = 1;

        // Клиент не найден
        public const int ClientNotFound = 2;

        // Неверный пол
        public const int WrongGender = 3;

        // Неверный сегмент
        public const int WrongSegment = 4;

        // Клиент не активирован
        public const int NotActivated = 5;
    }
}