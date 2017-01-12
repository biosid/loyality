using System;
using System.Configuration;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services.CardRegistration.Models.Outputs;
using Vtb24.Site.Services.VtbBankConnector;

namespace Vtb24.Site.Services.CardRegistration
{
    public class CardRegistrationService : ICardRegistration
    {
        public CardRegistrationService(IVtbBankConnectorService bank, ClientPrincipal principal)
        {
            _bank = bank;
            _principal = principal;
        }

        private readonly IVtbBankConnectorService _bank;
        private readonly ClientPrincipal _principal;

        private readonly string _url = ConfigurationManager.AppSettings["card_registrator_url"];

        public void RegisterCard()
        {
            AssertUser("Невозможно привязать карту.");

            _bank.RegisterCard(_principal.ClientId);
        }

        public bool IsCardRegistered()
        {
            AssertUser("Невозможно проверить привязку карты.");

            return _bank.IsCardRegistered(_principal.ClientId);
        }

        public RegistrationParameters GetRegistrationParameters()
        {
            AssertUser("Невозможно получить параметры привязки карты.");

            var clientId = _principal.ClientId;

            var parameters = _bank.GetCardRegistrationParameters(clientId);

            return new RegistrationParameters
            {
                RegistratorUrl = _url,
                ShopId = parameters.ShopId,
                OrderId = parameters.OrderId,
                Subtotal = parameters.Subtotal,
                CustomerId = parameters.CustomerId,
                Signature = parameters.Signature
            };
        }

        public bool VerifyRegistration(string orderId)
        {
            return _bank.VerifyCardRegistration(orderId);
        }

        private void AssertUser(string message)
        {
            if (!_principal.IsAuthenticated)
            {
                throw new InvalidOperationException("Пользователь не аутентифицирован. " + message);
            }
        }
    }
}
