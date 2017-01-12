namespace RapidSoft.VTB24.BankConnector.Processors
{
    public enum BankClientRegistrationStatusCodes
    {
        /// <summary>
        /// 1 – клиент успешно зарегистрирован;
        /// </summary>
        Success = 1,
        
        /// <summary>
        /// 2 – клиент уже был зарегистрирован ранее; 
        /// </summary>
        AllreadyRegistered = 2,
        
        /// <summary>
        /// 3 - не зарегистрирован из-за ошибки в системе лояльности
        /// </summary>
        LoyaltyError = 3
    }
}
