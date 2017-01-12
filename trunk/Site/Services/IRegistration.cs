using Vtb24.Site.Services.VtbBankConnector.Models.Inputs;

namespace Vtb24.Site.Services
{
    public interface IRegistration
    {
        /// <summary>
        /// Регистрирует клиента
        /// </summary>
        void Register(RegisterClientParams parameters); 
    }
}