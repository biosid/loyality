namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Задача импортирования тарифов доставки
    /// </summary>
    [DataContract]
    public class DeliveryRateImportTask
    {
        /// <summary>
        /// Уникальный идентификатор задачи.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public DateTime InsertDateTime { get; set; }

        /// <summary>
        /// Дата начала обработки задачи.
        /// </summary>
        [DataMember]
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Дата окончания обработки задачи.
        /// </summary>
        [DataMember]
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Идентификатор партнера, для которого была инициирована загрузка.
        /// </summary>
        [DataMember]
        public int PartnerId { get; set; }

        /// <summary>
        /// HTTP путь к загруженному файлу, указывает путь к файле на web-сервре, 
        /// так как файл сначала загружается на web-сервер на которомы выполняется АРМ для инициализации импорта.
        /// </summary>
        [DataMember]
        public string FileUrl { get; set; }

        /// <summary>
        /// Статус выполнения задачи.
        /// </summary>
        [DataMember]
        public ImportTaskStatuses Status { get; set; }

        /// <summary>
        /// Количество успешно загруженных товаров.
        /// </summary>
        [DataMember]
        public int CountSuccess { get; set; }

        /// <summary>
        /// Количество не загруженных товаров.
        /// </summary>
        [DataMember]
        public int CountFail { get; set; }

        /// <summary>
        /// Имя пользователя, инициировавшего загрузку.
        /// </summary>
        [DataMember]
        public string InsertedUserId { get; set; }
    }
}