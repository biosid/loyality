namespace Vtb24.Site.Services.Processing.Models
{
    public enum ProcessingOperationType
    {
        Unknown = 0,

        /// <summary>
        /// Начисление
        /// </summary>
        Deposit = 1,
        
        /// <summary>
        /// Списание
        /// </summary>
        Withdraw = 2
    }
}