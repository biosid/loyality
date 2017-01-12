namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.Online
{
    using System;
    using System.Text;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Services;

    public partial class NotifyOrderResult
    {
        public static NotifyOrderResult BuildFail(OperationException exception, string orderId = null)
        {
            return BuildFail(exception.ResultCode, exception.ResultDescription, orderId);
        }

        public static NotifyOrderResult BuildFail(ResultCodes code, string mess, string orderId = null)
        {
            return BuildFail((int)code, mess, orderId);
        }

        public static NotifyOrderResult BuildFail(int code, string mess, string orderId = null)
        {
            var result = new NotifyOrderResult
                             {
                                 OrderId = orderId ?? string.Empty,
                                 Status = (sbyte)code,
                                 Error = mess,
                                 UtcDateTime = DateTime.UtcNow,
                                 Signature = string.Empty
                             };
            return result;
        }

        public static NotifyOrderResult BuildSuccess(string orderId)
        {
            if (orderId.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Идентификатор не может быть null или пустой строкой");
            }

            var result = new NotifyOrderResult
                             {
                                 OrderId = orderId,
                                 Status = (int)ResultCodes.Success,
                                 Error = string.Empty,
                                 UtcDateTime = DateTime.UtcNow,
                                 Signature = string.Empty
                             };
            return result;
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