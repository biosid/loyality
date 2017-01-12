
namespace RapidSoft.VTB24.BankConnector.API.Exceptions
{
    public enum ExceptionType
    {
        GeneralException = 1,
        ClientAlreadyExistsException = 2,
        CardRegistrationException = 3,
        ProfileCustomFieldAlreadyExists = 4,
        AccessDenied = 5,
        ClientNotFound = 6,
        SecurityUserNotFound = 13,
        SecurityPhoneAlreadyExists = 14,

        // PaymentService
        PaymentNotFound = 101
    }
}
