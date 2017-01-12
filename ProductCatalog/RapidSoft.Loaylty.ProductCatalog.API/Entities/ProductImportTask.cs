namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Задача импортирования каталога продуктов
    /// </summary>
    [DataContract]
    public class ProductImportTask : IValidatableObject
    {
        public ProductImportTask()
        {
            this.InsertedDate = DateTime.Now;
        }

        public ProductImportTask(
            int partnerId, 
            string fileUrl, 
            string userId, 
            WeightProcessTypes? weightProcessType = null, 
            ParamsProcessTypes? paramsProcessTypes = null)
        {
            this.PartnerId = partnerId;
            this.FileUrl = fileUrl;
            this.InsertedUserId = userId;
            this.InsertedDate = DateTime.Now;
            this.WeightProcessType = weightProcessType ?? WeightProcessTypes.AcceptEmptyWeight;
            this.ParamsProcessType = paramsProcessTypes ?? ParamsProcessTypes.NotAcceptParamDuplicate;
        }

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
        public ParamsProcessTypes ParamsProcessType { get; set; }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        /// <param name="validationContext">The validation context.</param>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!this.StartDateTime.HasValue && this.Status != ImportTaskStatuses.Waiting)
            {
                yield return new ValidationResult("Дата начала обработки должна быть заполнена если статус не Waiting");
            }

            if (!this.EndDateTime.HasValue && this.Status.IsFinalStatus())
            {
                yield return new ValidationResult("Дата окончания обработки задачи должна быть заполнена если статус финальный");
            }
        }
    }
}