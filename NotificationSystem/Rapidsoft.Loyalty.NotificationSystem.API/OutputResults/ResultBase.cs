namespace Rapidsoft.Loyalty.NotificationSystem.API.OutputResults
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Базовый результат операции
    /// </summary>
    [DataContract]
    public class ResultBase
    {
        /// <summary>
        /// Признак успешного выполнения операции.
        /// </summary>
        [DataMember]
        public bool Success
        {
            get { return this.ResultCode == ResultCodes.SUCCESS; }
            set { }
        }

        /// <summary>
        /// Код возврата.
        /// </summary>
        [DataMember]
        public int ResultCode { get; set; }

        /// <summary>
        /// Описание ошибки.
        /// </summary>
        [DataMember]
        public string ResultDescription { get; set; }
    }
}