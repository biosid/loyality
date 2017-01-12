
using Vtb24.Site.Services.CardRegistration.Models.Outputs;

namespace Vtb24.Site.Models.Card
{
    public class CardRegistrationModel
    {
        public string RegistratorUrl { get; set; }

        public string ShopId { get; set; }

        public string OrderId { get; set; }

        public string CustomerId { get; set; }

        public string Subtotal { get; set; }

        public string Signature { get; set; }

        public string SuccessUrl { get; set; }

        public string ErrorUrl { get; set; }

        public static CardRegistrationModel Map(RegistrationParameters original)
        {
            return new CardRegistrationModel
            {
                RegistratorUrl = original.RegistratorUrl,
                ShopId = original.ShopId,
                OrderId = original.OrderId,
                CustomerId = original.CustomerId,
                Subtotal = original.Subtotal,
                Signature = original.Signature
            };
        }
    }
}
