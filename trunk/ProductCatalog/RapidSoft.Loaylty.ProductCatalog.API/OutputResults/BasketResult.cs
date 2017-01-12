namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// ��������� ��������� ��������� �������.
    /// </summary>
    [DataContract]
    public class BasketResult : ResultBase
    {
        /// <summary>
        /// ����� ���������� �������, ��������������� ������� ����������, 
        /// ����� ���������� ����������� ����������� ������� � ������������� ���������� ������������ �������. 
        /// </summary>
        [DataMember]
        public int? TotalCount { get; set; }

        /// <summary>
        /// ����� ��������� �������.
        /// </summary>
        [DataMember]
        public BasketItem[] Items { get; set; }

        /// <summary>
        /// ����� ��������� ���� ��������� ������� � ������
        /// </summary>
        [DataMember]
        public decimal TotalPrice
        {
            get;
            set;
        }

        public static BasketResult BuildError(string errorMessage)
        {
            return new BasketResult { ResultCode = ResultCodes.SUCCESS, ResultDescription = errorMessage };
        }
    }
}