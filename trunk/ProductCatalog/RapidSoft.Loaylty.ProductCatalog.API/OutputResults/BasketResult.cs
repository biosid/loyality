namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// Результат получения состояния корзины.
    /// </summary>
    [DataContract]
    public class BasketResult : ResultBase
    {
        /// <summary>
        /// Общее количество записей, соответствующим входным параметрам, 
        /// кроме параметров «Количество пропущенных записей» и «Максимальное количество возвращаемых записей». 
        /// </summary>
        [DataMember]
        public int? TotalCount { get; set; }

        /// <summary>
        /// Набор элементов корзины.
        /// </summary>
        [DataMember]
        public BasketItem[] Items { get; set; }

        /// <summary>
        /// Общая стоимость всех элементов корзины в баллах
        /// </summary>
        [DataMember]
        public decimal TotalPrice
        {
            get;
            set;
        }

        public static BasketResult BuildError(string errorMessage)
        {
            return new BasketResult { ResultCode = ResultCodes.SUCCESS, ResultDescription = errorMessage };
        }
    }
}