namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    public enum ConfirmedStatuses
    {
        /// <summary>
        /// Заказ не был подтвержден поставщиком (отказ).
        /// </summary>
        Rejected = 0,

        /// <summary>
        /// Заказ был подтвержден и принят на выполнение поставщиком.
        /// </summary>
        Committed = 1,

        /// <summary>
        /// Заказ поставлен в очередь
        /// </summary>
        AddToQueue = 2
    }
}