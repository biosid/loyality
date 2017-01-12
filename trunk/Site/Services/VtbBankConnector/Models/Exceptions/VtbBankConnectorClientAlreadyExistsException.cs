using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.VtbBankConnector.Models.Exceptions
{
    public class VtbBankConnectorClientAlreadyExistsException : ComponentException
    {
        public VtbBankConnectorClientAlreadyExistsException()
            : base("Коннектор к банку", 2, "Такой номер уже зарегистрирован, регистрация невозможна")
        {
        }
    }
}
