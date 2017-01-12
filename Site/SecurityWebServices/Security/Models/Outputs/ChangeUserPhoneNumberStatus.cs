namespace Vtb24.Site.SecurityWebServices.Security.Models.Outputs
{
    public enum ChangeUserPhoneNumberStatus
    {
        Changed = 0,
        UserNotFound = 1,
        PhoneNumberIsUsedByAnotherUser = 2,
        FailedToSendNotification = 3
    }
}