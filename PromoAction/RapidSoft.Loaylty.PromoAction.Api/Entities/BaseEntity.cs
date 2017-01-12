namespace RapidSoft.Loaylty.PromoAction.Api.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Базовая сущность.
    /// </summary>
    [DataContract]
    public abstract class BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class.
        /// </summary>
        protected BaseEntity()
        {
            this.InsertedDate = DateTime.Now;
        }

        /// <summary>
        /// Дата и время создания.
        /// </summary>
        [DataMember]
        public DateTime InsertedDate { get; set; }

        /// <summary>
        /// Дата и время обновления.
        /// </summary>
        [DataMember]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Имя пользователя в системе безопасности, который внес последнее изменение.
        /// </summary>
        [DataMember]
        public string UpdatedUserId { get; set; }
    }
}