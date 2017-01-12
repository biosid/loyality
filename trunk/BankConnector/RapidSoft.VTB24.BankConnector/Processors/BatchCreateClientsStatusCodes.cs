namespace RapidSoft.VTB24.BankConnector.Processors
{
    public enum BatchCreateClientsStatusCodes
    {
        /// <summary>
        /// 0 – Клиент успешно зарегистрирован в Системе лояльности.
        /// </summary>
        Success = 0,
        
        /// <summary>
        /// 1 – Произошла неизвестная ошибка.
        /// </summary>
        UnknownError = 1,
        
        /// <summary>
        /// 2 – Передан неуникальный внешний идентификатор клиента.
        /// </summary>
        NotUniqueExternalId = 2,
        
        /// <summary>
        /// 3 – Передан неуникальный внутренний идентификатор клиента.
        /// </summary>
        NotUniqueId = 3,

        /// <summary>
        /// 4 – Клиент уже был зарегистрирован ранее.
        /// </summary>
        AllreadyRegistered = 4,
        
        /// <summary>
        /// 5 – Неверные значения параметров.
        /// </summary>
        InvalidParameters = 5
    }
}
