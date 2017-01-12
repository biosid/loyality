namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using Entities;

    public class MoveCategoryParameters
    {
        /// <summary>
        /// Идентификатор клиента обновляющего категорию.
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор перемещаемой категории.
        /// </summary>
        public int CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор категории относительно которой осуществляется перенос
        /// </summary>
        public int? ReferenceCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Тип перемещения
        /// </summary>
        public CategoryPositionTypes PositionType
        {
            get;
            set;
        }
    }
}