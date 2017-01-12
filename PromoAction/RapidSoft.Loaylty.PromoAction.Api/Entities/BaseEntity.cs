namespace RapidSoft.Loaylty.PromoAction.Api.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// ������� ��������.
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
        /// ���� � ����� ��������.
        /// </summary>
        [DataMember]
        public DateTime InsertedDate { get; set; }

        /// <summary>
        /// ���� � ����� ����������.
        /// </summary>
        [DataMember]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// ��� ������������ � ������� ������������, ������� ���� ��������� ���������.
        /// </summary>
        [DataMember]
        public string UpdatedUserId { get; set; }
    }
}