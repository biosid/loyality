
using System;
using Vtb24.Site.Services.BankConnectorService;

namespace Vtb24.Site.Services.VtbBankConnector
{
    internal class MappingsToService
    {
        public Gender ToGender(Models.Gender original)
        {
            switch (original)
            {
                case Models.Gender.Female:
                    return Gender.Female;
                case Models.Gender.Male:
                    return Gender.Male;
                case Models.Gender.Other:
                    return Gender.Other;
                default:
                    throw new ArgumentException("Неверное значение пола.");
            }
        }
    }
}
