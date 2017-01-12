namespace RapidSoft.GeoPoints.Entities
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Страна.
    /// </summary>
    [DataContract]
    public class Country
    {
        /// <summary>
        /// Цифровой код страны по ОКСМ.
        /// </summary>
        [DataMember]
        public int NumberCode { get; set; }

        /// <summary>
        /// Краткое наименование страны согласно ОКСМ.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Полное наименование страны по ОКСМ.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Буквенный код страны альфа-2 по ОКСМ (двухбуквеннный код).
        /// </summary>
        [DataMember]
        public string Alpha2Code { get; set; }

        /// <summary>
        /// Буквенный код страны альфа-3 по ОКСМ.
        /// </summary>
        [DataMember]
        public string Alpha3Code { get; set; }
    }
}