namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.Online
{
    using System;
    using System.Text;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Services;

    /// <summary>
    /// The cancel payment result.
    /// </summary>
    public partial class CancelPaymentResult
    {
        public static CancelPaymentResult BuildFail(OperationException exception, string orderId = null)
        {
            return BuildFail(exception.ResultCode, exception.ResultDescription, orderId);
        }

        public static CancelPaymentResult BuildFail(ResultCodes code, string error, string orderId = null)
        {
            return BuildFail((sbyte)code, error, orderId);
        }

        public static CancelPaymentResult BuildFail(int code, string error, string orderId = null)
        {
            var retVal = new CancelPaymentResult
                             {
                                 Status = (sbyte)code,
                                 UtcDateTime = DateTime.UtcNow,
                                 Signature = string.Empty,
                                 OrderId = orderId,
                                 Error = error
                             };

            return retVal;
        }

        public static CancelPaymentResult BuildSuccess(string orderId, DateTime utcDateTime)
        {
            var retVal = new CancelPaymentResult
                             {
                                 Status = (sbyte)ResultCodes.Success,
                                 UtcDateTime = utcDateTime.ToUniversalTime(),
                                 Signature = string.Empty,
                                 OrderId = orderId,
                                 Error = string.Empty
                             };

            return retVal;
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