using System;

namespace RapidSoft.YML.Entities.OfferTypes
{
    /// <summary>
    /// Класс, представляющий тип предложения audiobook (аудиокнига)
    /// </summary>
    public class Audiobook : BaseOffer
    {
        /// <summary>
        /// Автор произведения
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Наименование произведения
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Издательство
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Серия
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Год издания
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Код книги, если их несколько, то указываются через запятую
        /// </summary>
        public string Isbn { get; set; }

        /// <summary>
        /// Номер тома
        /// </summary>
        public int? Volume { get; set; }

        /// <summary>
        /// Номер части
        /// </summary>
        public int? Part { get; set; }

        /// <summary>
        /// Язык произведения
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Исполнитель. Если их несколько, перечисляются через запятую
        /// </summary>
        public string PerformedBy
        {
            get; set;
        }

        /// <summary>
        /// Тип аудиокниги (радиоспектакль, произведение начитано, ...)
        /// </summary>
        public string PerformanceType
        {
            get; set;
        }

        /// <summary>
        /// Оглавление.
        /// Выводится информация о наименованиях произведений, если это сборник рассказов или стихов.
        /// </summary>
        public string TableOfContents { get; set; }

        /// <summary>
        /// Носитель, на котором поставляется аудиокнига.
        /// </summary>
        public string Storage
        {
            get; set;
        }

        /// <summary>
        /// Формат аудиокниги
        /// </summary>
        public string Format
        {
            get; set;
        }

        /// <summary>
        /// Время звучания задается в формате mm.ss (минуты.секунды)
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