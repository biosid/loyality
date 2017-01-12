using System;

namespace RapidSoft.YML.Entities.OfferTypes
{
    /// <summary>
    /// Класс, представляющий тип предложения book (книга)
    /// </summary>
    public class Book : BaseOffer
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
        /// Количество томов
        /// </summary>
        public int? Volumes { get; set; }

        /// <summary>
        /// Номер тома
        /// </summary>
        public int? Part { get; set; }

        /// <summary>
        /// Язык произведения
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Переплёт
        /// </summary>
        public string Binding { get; set; }

        /// <summary>
        /// Количествово страниц в книге
        /// </summary>
        public int? PageExtent { get; set; }

        /// <summary>
        /// Оглавление.
        /// Выводится информация о наименованиях произведений, если это сборник рассказов или стихов.
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