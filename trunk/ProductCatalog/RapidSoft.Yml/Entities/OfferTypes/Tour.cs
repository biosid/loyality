using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidSoft.YML.Entities.OfferTypes
{
    /// <summary>
    /// �����, �������������� ��� ����������� tour (����)
    /// </summary>
    public class Tour : BaseOffer
    {
        /// <summary>
        /// ����� �����
        /// </summary>
        public string WorldRegion { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// ������ ��� �����
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// ���������� ���� ����
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// ���� �������
        /// </summary>
        public IList<string> TourDates { get; set; }

        /// <summary>
        /// �������� ����� (� ��������� ������� �������� ����)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ������ �����
        /// </summary>
        public string HotelStars { get; set; }

        /// <summary>
        /// ��� ������� (SNG, DBL, ...)
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// ��� ������� (All, HB, ...)
        /// </summary>
        public string Meal { get; set; }

        /// <summary>
        /// ��� �������� � ��������� ����
        /// </summary>
        public string Included { get; set; }

        /// <summary>
        /// ���������
        /// </summary>
        public string Transport { get; set; }

        public override string DisplayName
        {
            get
            {
                var text = new List<string>
                {
                   "���",                                   
                   WorldRegion,
                   Country,
                   Region,
                   Name,
                   HotelStars,
                   Room,
                   Meal,
                   "��",
                   Days.ToString(),
                   Days%10 == 1 && Days%100 != 11 ? "�." : "��."                   
                };
                return String.Join(" ", text.Where(s => !String.IsNullOrEmpty(s)).ToArray());
            }
        }
    }
}