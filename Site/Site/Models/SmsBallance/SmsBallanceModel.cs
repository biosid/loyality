using System;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Models.SmsBallance
{
    public class SmsBallanceModel : BaseQueryModel
    {
        // ReSharper disable InconsistentNaming

        public string clientId { get; set; }

        public string message { get; set; }

        public int connectorId { get; set; }

        public int serviceId { get; set; }

        public DateTime receivedDate { get; set; }

        public int shortNumber { get; set; }

        public int mtSent { get; set; }

        /// <summary>
        /// см. описание incomingId
        /// </summary>
        public long? messageId { get; set; }

        /// <summary>
        /// По спецификации, поле называется messageId, но по факту приходит incomingId
        /// </summary>
        public long? incomingId { get; set; }

        public string clientPhone { get; set; }

        public int sum_sms { get; set; }

        public string hash { get; set; }

        // ReSharper restore InconsistentNaming
    }
}