namespace Vtb24.Site.Security.Models
{
    public enum BankSmsType
    {
        LoyaltyRegistrationSucceeded,
        BankRegistrationSucceeded,
        RegistrationDeniedUnknownClient,
        RegistrationDeniedNoCards,
        RegistrationDeniedAlreadyRegistered
    }
}
