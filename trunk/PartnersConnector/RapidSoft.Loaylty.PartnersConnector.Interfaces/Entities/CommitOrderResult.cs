namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CommitOrderResult : ResultBase
    {
        /// <summary>
        /// Признак подтверждения заказа. 
        /// </summary>
        [DataMember]
        public ConfirmedStatuses Confirmed { get; set; }

        /// <summary>
        /// Текстовое описание причины отказа. Описание предназначено для отображения пользователю. Заполняется, если Checked = Rejected .
        /// </summary>
        [DataMember]
        public string Reason { get; set; }

        /// <summary>
        /// Код причины отказа в информационной системе поставщика. Код предназначен для логирования, сбора статистики и разбора. Не обязателен к заполнению.
        /// </summary>
        [DataMember]
        public string ReasonCode { get; set; }

        /// <summary>
        /// Идентификатор, присвоенный заказу информационной системой поставщика. 
        /// </summary>
        [DataMember]
        public string InternalOrderId { get; set; }

        public static CommitOrderResult BuildSuccess(ConfirmedStatuses confirmed, string reason = null, string reasonCode = null, string internalOrderId = null)
        {
            return new CommitOrderResult
                       {
                           Confirmed = confirmed,
                           Reason = reason,
                           ReasonCode = reasonCode,
                           ResultCode = (int)ResultCodes.Success,
                           ResultDescription = null,
                           InternalOrderId = internalOrderId
                       };
        }

        public static CommitOrderResult BuildUnknownFail(string description)
        {
            return BuildFail(ResultCodes.UnknownError, description);
        }

        public static CommitOrderResult BuildFail(ResultCodes code, string description)
        {
            return new CommitOrderResult
                       {
                           Confirmed = 0,
                           Reason = null,
                           ReasonCode = null,
                           ResultCode = (int)code,
                           ResultDescription = description
                       };
        }
    }
}