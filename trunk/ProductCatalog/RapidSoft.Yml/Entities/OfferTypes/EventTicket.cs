using System;

namespace RapidSoft.YML.Entities.OfferTypes
{
    /// <summary>
    /// �����, �������������� ��� ����������� event-ticket (������ �� �����������)
    /// </summary>
    public class EventTicket : BaseOffer
    {
        /// <summary>
        /// �������� �����������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ����� ����������
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// ���
        /// </summary>
        public Hall Hall { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string HallPart { get; set; }

        /// <summary>
        /// ���� � ����� ������
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// ������� ������������ �����������
        /// </summary>
        public bool? IsPremiere { get; set; }

        /// <summary>
        /// ������� �������� �����������
        /// </summary>
        public bool? IsKids { get; set; }

        public override string DisplayName
        {
            get { return Name; }
        }
    }
}