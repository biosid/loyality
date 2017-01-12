using RapidSoft.Loaylty.ProductCatalog.API.Entities;

namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Результат операций управления корзиной пользователя.
    /// </summary>
    [DataContract]
    public class BasketManageResult : ResultBase
    {
        public static BasketManageResult BuildSuccess(Guid? basketItemId = null)
        {
            return new BasketManageResult
            {
                ResultCode = ResultCodes.SUCCESS, 
                ResultDescription = null,
                BasketItemId = basketItemId ?? Guid.Empty
            };
        }

        public Guid BasketItemId
        {
            get;
            set;
        }
    }
}