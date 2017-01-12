namespace RapidSoft.VTB24.BankConnector.Quartz_Jobs
{
    public enum DataKeys
    {
        /// <summary>
        /// Временная папка
        /// </summary>
        TempFolderPath, 

        /// <summary>
        /// Хост СМТП
        /// </summary>
        HostNameSmtp, 

        /// <summary>
        /// Логин СМТП
        /// </summary>
        LoginSmtp, 

        /// <summary>
        /// Пароль на СМТП
        /// </summary>
        PasswordSmtp, 

        /// <summary>
        /// Адресат назначения
        /// </summary>
        MailTo, 

        /// <summary>
        /// Получено от
        /// </summary>
        MailFrom, 

        /// <summary>
        /// Хост ПОП
        /// </summary>
        HostNamePop, 

        /// <summary>
        /// Логин ПОП
        /// </summary>
        LoginPop, 

        /// <summary>
        /// Пароль ПОП
        /// </summary>
        PasswordPop
    }
}