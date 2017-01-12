namespace RapidSoft.VTB24.BankConnector.DataModels
{
    public class RegClientStatuses
    {
        // Зарегистрирован успешно
        public const int Registred = 0;
        
        // Отменён успешно
        public const int Cancelled = 1;
        
        // Заявка на регистрацию не найдена
        public const int RequestNotFound = 2;
        
        // Неверный сегмент
        public const int InvalidSegment = 3;
        
        // Уже зарегистрирован
        public const int AlreadyRegistred = 4;

        // Неизвестная ошибка
        public const int Error = 7;
    }
}