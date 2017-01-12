namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.Online
{
    using System;
    using System.Text;
    using Extensions;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;

    public partial class GetPaymentStatusResult
    {
        public GetPaymentStatusResult()
        {
        }

        public GetPaymentStatusResult(ResultCodes status, string orderId)
        {
            this.statusField = (sbyte)status;
            this.orderIdField = orderId;
            this.utcDateTimeField = DateTime.UtcNow;
            this.signatureField = string.Empty;
        }

        public string SerializeWithSing(ICryptoService cryptoService)
        {
            var serialized = this.Serialize(Encoding.UTF8, true);
            var sing = cryptoService.CreateSignature(serialized);
            this.Signature = sing;
            var retVal = this.Serialize(Encoding.UTF8, true);
            return retVal;
        }
    }
}