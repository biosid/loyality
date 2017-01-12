namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public enum ResultCodes
    {
        General = 100,
        ActiveDirectoryError = 101,
        SetPasswordError = 102,

        Denied = 200,

        GroupAlreadyExists = 301,
        GroupNotFound = 302,
        UserAlreadyExists = 303,
        UserNotFound = 304,
        SelfEditIsInvalid = 305,
        SelfDeleteIsAttempted = 306
    }
}
