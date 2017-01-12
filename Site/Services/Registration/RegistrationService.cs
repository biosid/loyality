using System;
using Vtb24.Site.Services.VtbBankConnector;
using Vtb24.Site.Services.VtbBankConnector.Models.Inputs;

namespace Vtb24.Site.Services.Registration
{
    public class RegistrationService : IRegistration, IDisposable
    {
        public RegistrationService(IVtbBankConnectorService bank)
        {
            _bank = bank;
        }

        private readonly IVtbBankConnectorService _bank;

        public void Register(RegisterClientParams parameters)
        {
            _bank.RegisterClient(parameters);
        }

        public void Dispose()
        {
            // Do nothing
        }
    }
}