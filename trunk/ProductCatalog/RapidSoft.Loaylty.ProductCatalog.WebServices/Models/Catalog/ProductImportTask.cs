namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Задача импортирования каталога продуктов
    /// </summary>
    [DataContract]
    public class ProductImportTask
    {
        /// <summary>
        /// Уникальный идентификатор задачи.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

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

        /// <summary>
        /// Дата и время создания задачи, для сортировки, так как <see cref="StartDateTime"/> может быть <c>null</c>.
        /// </summary>
        [DataMember]
        public DateTime InsertedDate { get; set; }

        /// <summary>
        /// Тип обработки веса.
        /// </summary>
        [DataMember]
        public WeightProcessTypes WeightProcessType { get; set; }

        /// <summary>
        /// Тип обработки параметров (поведение при обнаружении дубликатов по имени с разными значениями)
        /// </summary>
        [DataMember]
        public ParameterssProcessTypes ParametersProcessType { get; set; }
    }
}
