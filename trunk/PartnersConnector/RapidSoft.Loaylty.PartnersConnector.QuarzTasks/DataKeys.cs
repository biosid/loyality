using RapidSoft.Loaylty.PartnersConnector.Interfaces;

namespace RapidSoft.Loaylty.PartnersConnector.QuarzTasks
{
    using Quartz;

    using PartnersConnector.Interfaces;

    /// <summary>
    /// Перечисления ключей под которыми храняться данные в <see cref="JobDataMap"/>.
    /// </summary>
    public enum DataKeys
    {
        /// <summary>
        /// Ключ сервиса реализующего <see cref="IOrderManagementService"/>.
        /// </summary>
        Service,

        /// <summary>
        /// Ключ уведомления о изменении статуса заказа.
        /// </summary>
        NotifyOrders,

        /// <summary>
        /// Ключ заказа или коллекции заказов.
        /// </summary>
        Orders,

        /// <summary>
        /// Ключ идентификатора партнера
        /// </summary>
        PartnerId,

        /// <summary>
        /// Ключ HTTPS адреса yml-файла каталога подарков на сервер партнера
        /// </summary>
        RemoteFileUrl,

        /// <summary>
        /// Ключ пути по которому сохраняется файл каталога порадков.
        /// NOTE: LocalFileUrl и LocalFilePath должны указывать на один и тотже файл для одного партнера
        /// </summary>
        LocalFilePath,

        /// <summary>
        /// Ключ HTTP адреса yml-файла каталога подарков доступный для компонента "Каталог порадков (ProductCatalog)"
        /// NOTE: LocalFileUrl и LocalFilePath должны указывать на один и тотже файл для одного партнера
        /// </summary>
        LocalFileUrl,

        /// <summary>
        /// Ключ пути по которому сохраняются файлы для продукта каталога подарков
        /// </summary>
        GiftFilePath
    }
}