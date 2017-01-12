namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;

    using RapidSoft.Loaylty.ClientProfile.ClientProfileService;

    using Action = RapidSoft.Loaylty.ClientProfile.ClientProfileService.Action;

    internal static class LoyaltyClientUpdateHelpers
    {
        public const string MobilePhoneType = "MOBILE";

        public static ElementStringWithAttribute ToStringUpdateField(this string value)
        {
            return new ElementStringWithAttribute { action = Action.U, actionSpecified = true, Value = value };
        }

        public static ElementDateTimeWithAttribute ToDateTimeUpdateField(this DateTime value)
        {
            return new ElementDateTimeWithAttribute
                   {
                       action = Action.U,
                       actionSpecified = true,
                       Value = value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                   };
        }

        public static UpdateClientProfileRequestTypeClientProfilePhone ToPhoneUpdateField(
            this string phoneNumber, int phoneId, bool? isPrimary = null)
        {
            return new UpdateClientProfileRequestTypeClientProfilePhone
                   {
                       PhoneNumber = phoneNumber,
                       action = Action.U,
                       actionSpecified = true,
                       PhoneId = phoneId,
                       PhoneIdSpecified = true,
                       IsPrimary = isPrimary,
                       IsPrimarySpecified = isPrimary.HasValue,
                       PhoneType = MobilePhoneType
                   };
        }
    }
}
