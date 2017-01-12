namespace Vtb24.Site.Models.Account
{
    public static class Constants
    {
        public const string PASSWORD_REGEX = @"^(?=.*\d)(?=.*[A-Za-z])[0-9A-Za-z]{6,20}$";

        public const string PASSWORD_ERROR_MESSAGE = "Новый пароль должен содержать 6-20 латинских букв и цифр. В пароле обязательно должны присутствовать буквы и цифры";

        public const string PHONE_REGEX = @"^\+7 \([34589]\d{2}\) \d{3}-\d{4}$(?<!(\+7 \(940\) \d{3}-\d{4})|(\+7 \(929\) 8((0[3-9])|1[01])-\d{4}))";

        public const string PASSWORD_RESET_VIA_SMS_HINT = "Подтвердите смену пароля цифровым кодом, который был выслан на Ваш мобильный телефон по SMS";

        public const string PASSWORD_RESET_VIA_EMAIL_HINT = "Подтвердите смену пароля цифровым кодом, который был выслан на Ваш адрес электронной почты";
    }
}