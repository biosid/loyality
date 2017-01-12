namespace RapidSoft.VTB24.BankConnector.DataModels
{
    /// <summary>
    /// Ограничения полей бд
    /// </summary>
    public class FieldLenght
    {
        // Общие поля
        public const int ProductIdMaxLen = 50;
        public const int ProductNameMaxLen = 500;
        public const int ExternalOrderIdMaxLen = 50;
        
        // Клиент
        public const int ClientIdMaxLen = 255;
        public const int ContactNameMaxLen = 255;
        public const int ContactPhoneMaxLen = 20;
        public const int ContactEmailMaxLen = 255;
        
        // Заказ
        public const int DeliveryRegionMaxLen = 50;
        public const int DeliveryCityMaxLen = 50;
        public const int DeliveryAddressMaxLen = 500;

        public const int UnitellerItemsShopIdMaxLen = 50;
    }
}