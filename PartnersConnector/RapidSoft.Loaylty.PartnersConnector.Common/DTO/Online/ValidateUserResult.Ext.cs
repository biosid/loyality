namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.Online
{
    using System;
    using System.Text;

    using RapidSoft.Loaylty.PartnersConnector.Services;
    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;

    public partial class ValidateUserResult
    {
        private bool? IsShouldSerializeRate { get; set; }
        private bool? IsShouldSerializeDelta { get; set; }

        public static ValidateUserResult BuildFail(OperationException exception)
        {
            return BuildFail(exception.ResultCode);
        }

        public static ValidateUserResult BuildFail(int code)
        {
            var retVal = new ValidateUserResult
                             {
                                 Status = (sbyte)code,
                                 UtcDateTime = DateTime.UtcNow,
                                 Signature = string.Empty,
                                 IsShouldSerializeRate = false,
                                 IsShouldSerializeDelta = false
                             };

            return retVal;
        }

        public static ValidateUserResult BuildFail(ResultCodes code)
        {
            return BuildFail((int)code);
        }

        public string SerializeWithSing(ICryptoService cryptoService)
        {
            var serialized = this.Serialize(Encoding.UTF8, true);
            var sing = cryptoService.CreateSignature(serialized);
            this.Signature = sing;
            var retVal = this.Serialize(Encoding.UTF8, true);
            return retVal;
        }

        public bool ShouldSerializeRate()
        {
            return !this.IsShouldSerializeRate.HasValue || this.IsShouldSerializeRate.Value;
        }

        public bool ShouldSerializeDelta()
        {
            return !this.IsShouldSerializeDelta.HasValue || this.IsShouldSerializeDelta.Value;
        }
    }
}