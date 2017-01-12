using System;

namespace RapidSoft.YML.Entities.OfferTypes
{
    /// <summary>
    /// Класс, представляющий тип artist.title (аудио и видеопродукция)
    /// </summary>
    public class ArtistTitle : BaseOffer
    {
        /// <summary>
        /// Исполнитель
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Год выпуска
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Носитель
        /// </summary>
        public string Media { get; set; }

        /// <summary>
        /// Актеры
        /// </summary>
        public string Starring { get; set; }

        /// <summary>
        /// Режиссер
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// Оригинальное название
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Страна
        /// </summary>
        public string Country { get; set; }

        public override string DisplayName
        {
            get
            {
                return String.IsNullOrEmpty(Artist) ? Title : String.Format(@"{0} ""{1}""", Artist, Title);
            }
        }
    }
}