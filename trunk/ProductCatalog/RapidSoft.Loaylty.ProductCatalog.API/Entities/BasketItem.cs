namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// ������� �������.
    /// </summary>
    [DataContract]
    public class BasketItem
    {
        /// <summary>
        /// ������������� �������� �������.
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// ������������� �������.
        /// </summary>
        [DataMember]
        public string ClientId { get; set; }

        /// <summary>
        /// ������������� ������ ��������� �������.
        /// </summary>
        [DataMember]
        public int? BasketItemGroupId { get; set; }

        /// <summary>
        /// ���������� ������������� ������.
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// ���������� �����.
        /// </summary>
        [DataMember]
        public Product Product { get; set; }

        /// <summary>
        /// ���� ���������� ������ � �������
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// ���������� ������� � �������
        /// </summary>
        [DataMember]
        public int ProductsQuantity { get; set; }

        /// <summary>
        /// ����� ��������� �������� ������� � ������
        /// </summary>
        [DataMember]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// ����� ��������� �������� ������� � ������
        /// </summary>
        [DataMember]
        public decimal TotalPriceRur { get; set; }

        /// <summary>
        /// ��������� �������� ������� � ������
        /// </summary>
        [DataMember]
        public decimal ItemPrice { get; set; }

        /// <summary>
        /// ���������� ���� �� ������� ������ � �������� ������� � ������
        /// </summary>
        [DataMember]
        public string FixedPrice { get; set; }

        /// <summary>
        /// ������ ����������� �������� ������ ��� ������
        /// </summary>
        [DataMember]
        public ProductAvailabilityStatuses AvailabilityStatus { get; set; }
    }
}