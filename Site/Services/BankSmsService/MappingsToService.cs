using System;
using Vtb24.Site.Security.Models;

namespace Vtb24.Site.Services.BankSmsService
{
    internal static class MappingsToService
    {
        public static BankConnectorService.BankSmsType ToBankSmsType(BankSmsType original)
        {
            switch (original)
            {
                case BankSmsType.LoyaltyRegistrationSucceeded:
                    return BankConnectorService.BankSmsType.LoyaltyRegistration;
                case BankSmsType.BankRegistrationSucceeded:
                    return BankConnectorService.BankSmsType.BankRegistration;
                case BankSmsType.RegistrationDeniedUnknownClient:
                    return BankConnectorService.BankSmsType.RegistrationDeniedUnknownClient;
                case BankSmsType.RegistrationDeniedNoCards:
                    return BankConnectorService.BankSmsType.RegistrationDeniedNoCards;
                case BankSmsType.RegistrationDeniedAlreadyRegistered:
                    return BankConnectorService.BankSmsType.RegistrationDeniedAlreadyRegistered;
            }

            throw new ArgumentException("неподдерживаемый тип СМС: " + original);
        }

        public static string ToPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                throw new ArgumentNullException("phone");
            }

            return "8" + phone.Substring(1);
        }
    }
}
