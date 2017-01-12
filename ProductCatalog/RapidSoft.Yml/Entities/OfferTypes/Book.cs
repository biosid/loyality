using System;

namespace RapidSoft.YML.Entities.OfferTypes
{
    /// <summary>
    /// �����, �������������� ��� ����������� book (�����)
    /// </summary>
    public class Book : BaseOffer
    {
        /// <summary>
        /// ����� ������������
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// ������������ ������������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// ��� �������
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// ��� �����, ���� �� ���������, �� ����������� ����� �������
        /// </summary>
        public string Isbn { get; set; }

        /// <summary>
        /// ���������� �����
        /// </summary>
        public int? Volumes { get; set; }

        /// <summary>
        /// ����� ����
        /// </summary>
        public int? Part { get; set; }

        /// <summary>
        /// ���� ������������
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public string Binding { get; set; }

        /// <summary>
        /// ������������ ������� � �����
        /// </summary>
        public int? PageExtent { get; set; }

        /// <summary>
        /// ����������.
        /// ��������� ���������� � ������������� ������������, ���� ��� ������� ��������� ��� ������.
        /// </summary>
        public string TableOfContents { get; set; }

        public override string DisplayName
        {
            get
            {
                return String.IsNullOrEmpty(Author) ? Name : String.Format(@"{0} ""{1}""", Author, Name);
            }
        }
    }
}