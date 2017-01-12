using System;
using System.Text;

namespace RapidSoft.Loaylty.PartnersConnector.Queue.Entities
{
    /// <summary>
    /// Элемент очереди на подтверждение заказов.
    /// </summary>
    public class CommitOrderQueueItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitOrderQueueItem"/> class.
        /// </summary>
        public CommitOrderQueueItem()
        {
            this.InsertedDate = DateTime.Now;
            this.ProcessStatus = Statuses.NotProcessed;
        }

        /// <summary>
        /// Уникальный идентификатор элемента очереди
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор партнера.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Xml данные заказа.
        /// NOTE: Так как сущность храниться в MS SQL базе в поле с типом xml который не признает utf-8, должно быть сериализовано в utf-16 (<see cref="Encoding.Unicode"/>)
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Дата и время постановки в очередь.
        /// </summary>
        public DateTime InsertedDate { get; set; }

        /// <summary>
        /// Статус обработки.
        /// </summary>
        public Statuses ProcessStatus { get; set; }

        /// <summary>
        /// Дополнительное описание статуса.
        /// </summary>
        public string ProcessStatusDescription { get; set; }
    }
}