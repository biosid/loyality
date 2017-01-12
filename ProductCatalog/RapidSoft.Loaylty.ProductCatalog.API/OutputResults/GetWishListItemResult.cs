namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    /// <summary>
    /// ��������� ��������� �������� WishList.
    /// </summary>
    [DataContract]
    public class GetWishListItemResult : WishListResult
    {
        /// <summary>
        /// ������� �������.
        /// </summary>
        [DataMember]
        public WishListItem Item { get; set; }

        /// <summary>
        /// ����������� ����������� ���������� �� ��������� ����������.
        /// </summary>
        /// <param name="errorMessage">
        /// �������� ������
        /// </param>
        /// <returns>
        /// ��������� �� ��������� ���������
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