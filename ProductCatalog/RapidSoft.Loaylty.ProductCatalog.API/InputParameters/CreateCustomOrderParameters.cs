using System;

namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    public class CreateCustomOrderParameters
    {
        public string ClientId { get; set; }

        /// <summary>
        /// Идентификатор Партнера
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Идентификатор заказа в информационной системе Партнера
        /// </summary>
        public string ExternalOrderId { get; set; }

        /// <summary>
        /// Элементы, содержащий информацию о строке заказа. 
        /// </summary>
        public CustomOrderItem[] Items { get; set; }
    }
}
