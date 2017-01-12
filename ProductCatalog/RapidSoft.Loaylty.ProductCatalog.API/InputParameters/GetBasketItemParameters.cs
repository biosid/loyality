namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Collections.Generic;

    public class GetBasketItemParameters
    {
        /// <summary>
        /// Идентификатор клиента для которого берётся запись из корзины
        /// </summary>
        public string ClientId
        {
            get;
            set;
        }
        
        /// <summary>
        /// Идентификатор продукта для которого берётся запись из корзины
        /// </summary>
        public string ProductId
        {
            get;
            set;
        }

        /// <summary>
        /// Контекст клиента.
        /// </summary>
        public Dictionary<string, string> ClientContext { get; set; }
    }
}