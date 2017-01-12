namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Объект-результат выполения операции добавления в wishlist.
    /// </summary>
    [DataContract]
    public class WishListResult : ResultBase
    {
        /// <summary>
        /// Статический конструктор результата успешного выполнения.
        /// </summary>
        /// <returns>
        /// Результат успешного выполения
        /// </returns>
        public static new WishListResult BuildSuccess()
        {
            return new WishListResult { ResultCode = ResultCodes.SUCCESS, ResultDescription = null };
        }

        /// <summary>
        /// Статический конструктор результата не успешного выполнения.
        /// </summary>
        /// <param name="errorMessage">
        /// Описание ошибки
        /// </param>
        /// <returns>
        /// Результат не успешного выполения
        /// </returns>
        public static WishListResult BuildError(string errorMessage)
        {
            return new WishListResult { ResultCode = ResultCodes.UNKNOWN_ERROR, ResultDescription = errorMessage };
        }
    }
}