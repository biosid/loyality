using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.BonusPayments.Models.Exceptions
{
    public class BonusPointsException : ComponentException
    {
        public BonusPointsException(int resultCode, string codeDescription) : base("Сервис списания бонусов", resultCode, codeDescription)
        {
        }

        public BonusPointsException(string component, int resultCode, string codeDescription)
            : base(component, resultCode, codeDescription)
        {
        }
    }
}
