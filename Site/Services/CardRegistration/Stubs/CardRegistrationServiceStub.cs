using Vtb24.Site.Services.CardRegistration.Models.Outputs;

namespace Vtb24.Site.Services.CardRegistration.Stubs
{
    public class CardRegistrationServiceStub :  ICardRegistration
    {
        public void RegisterCard()
        {
        }

        public bool IsCardRegistered()
        {
            return true;
        }

        public RegistrationParameters GetRegistrationParameters()
        {
            return new RegistrationParameters
            {
                RegistratorUrl = "/Content/DummyCardRegistrationForm.html",
                ShopId = "1",
                OrderId = "1",
                CustomerId = "1",
                Subtotal = "1",
                Signature = "1"
            };
        }

        public bool VerifyRegistration(string orderId)
        {
            return true;
        }
    }
}
