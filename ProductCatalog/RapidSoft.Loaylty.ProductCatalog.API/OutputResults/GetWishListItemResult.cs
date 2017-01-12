namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Результат получения элемента WishList.
    /// </summary>
    [DataContract]
    public class GetWishListItemResult : WishListResult
    {
        /// <summary>
        /// Элемент корзины.
        /// </summary>
        [DataMember]
        public WishListItem Item { get; set; }

        /// <summary>
        /// Статический конструктор результата не успешного выполнения.
        /// </summary>
        /// <param name="errorMessage">
        /// Описание ошибки
        /// </param>
        /// <returns>
        /// Результат не успешного выполения
        /// </returns>
        public static new GetWishListItemResult BuildError(string errorMessage)
        {
            return new GetWishListItemResult { ResultCode = ResultCodes.UNKNOWN_ERROR, ResultDescription = errorMessage };
        }

        public static GetWishListItemResult BuildError(int code, string errorMessage)
        {
            return new GetWishListItemResult { ResultCode = code, ResultDescription = errorMessage };
        }
    }
}