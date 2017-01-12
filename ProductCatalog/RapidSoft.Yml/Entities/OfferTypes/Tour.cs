using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidSoft.YML.Entities.OfferTypes
{
    /// <summary>
    /// Класс, представляющий тип предложения tour (туры)
    /// </summary>
    public class Tour : BaseOffer
    {
        /// <summary>
        /// Часть света
        /// </summary>
        public string WorldRegion { get; set; }

        /// <summary>
        /// Страна
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Курорт или город
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Количество дней тура
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Даты заездов
        /// </summary>
        public IList<string> TourDates { get; set; }

        /// <summary>
        /// Название отеля (в некоторых случаях название тура)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Звезды отеля
        /// </summary>
        public string HotelStars { get; set; }

        /// <summary>
        /// Тип комнаты (SNG, DBL, ...)
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Тип питания (All, HB, ...)
        /// </summary>
        public string Meal { get; set; }

        /// <summary>
        /// Что включено в стоимость тура
        /// </summary>
        public string Included { get; set; }

        /// <summary>
        /// Транспорт
        /// </summary>
        public string Transport { get; set; }

        public override string DisplayName
        {
            get
            {
                var text = new List<string>
                {
                   "Тур",                                   
                   WorldRegion,
                   Country,
                   Region,
                   Name,
                   HotelStars,
                   Room,
                   Meal,
                   "на",
                   Days.ToString(),
                   Days%10 == 1 && Days%100 != 11 ? "д." : "дн."                   
                };
                return String.Join(" ", text.Where(s => !String.IsNullOrEmpty(s)).ToArray());
            }
        }
    }
}