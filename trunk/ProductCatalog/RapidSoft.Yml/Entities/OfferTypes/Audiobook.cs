using System;

namespace RapidSoft.YML.Entities.OfferTypes
{
    /// <summary>
    /// �����, �������������� ��� ����������� audiobook (����������)
    /// </summary>
    public class Audiobook : BaseOffer
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
        /// ����� ����
        /// </summary>
        public int? Volume { get; set; }

        /// <summary>
        /// ����� �����
        /// </summary>
        public int? Part { get; set; }

        /// <summary>
        /// ���� ������������
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// �����������. ���� �� ���������, ������������� ����� �������
        /// </summary>
        public string PerformedBy
        {
            get; set;
        }

        /// <summary>
        /// ��� ���������� (��������������, ������������ ��������, ...)
        /// </summary>
        public string PerformanceType
        {
            get; set;
        }

        /// <summary>
        /// ����������.
        /// ��������� ���������� � ������������� ������������, ���� ��� ������� ��������� ��� ������.
        /// </summary>
        public string TableOfContents { get; set; }

        /// <summary>
        /// ��������, �� ������� ������������ ����������.
        /// </summary>
        public string Storage
        {
            get; set;
        }

        /// <summary>
        /// ������ ����������
        /// </summary>
        public string Format
        {
            get; set;
        }

        /// <summary>
        /// ����� �������� �������� � ������� mm.ss (������.�������)
        /// </summary>
        public string RecordingLength
        {
            get; set;
        }

        public override string DisplayName
        {
            get
            {
                return String.IsNullOrEmpty(Author) ? Name : String.Format(@"{0} ""{1}""", Author, Name);
            }
        }
    }
}