using System;

namespace RapidSoft.YML.Entities.OfferTypes
{
    /// <summary>
    /// Класс, представляющий тип предложения event-ticket (билеты на мероприятие)
    /// </summary>
    public class EventTicket : BaseOffer
    {
        /// <summary>
        /// Название мероприятия
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Место проведения
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// Зал
        /// </summary>
        public Hall Hall { get; set; }

        /// <summary>
        /// Сектор
        /// </summary>
        public string HallPart { get; set; }

        /// <summary>
        /// Дата и время сеанса
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Признак премьерности мероприятия
        /// </summary>
        public bool? IsPremiere { get; set; }

        /// <summary>
        /// Признак детского мероприятия
        /// </summary>
        public bool? IsKids { get; set; }

        public override string DisplayName
        {
            get { return Name; }
        }
    }
}