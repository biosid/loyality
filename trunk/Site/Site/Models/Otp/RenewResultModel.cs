using System;
using Vtb24.Site.Security.OneTimePasswordService.Models.Outputs;

namespace Vtb24.Site.Models.Otp
{
    public class RenewResultModel
    {
        public string Token { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }

        public int ExpiresInSeconds
        {
            get
            {
                var sec = (int) (ExpirationTimeUtc - DateTime.UtcNow).TotalSeconds;
                return sec > 0 ? sec : 0;
            }
        }

        public int AttemptsRemaining { get; set; }

        public Statuses Status { get; set; }

        public string Error
        {
            get
            {
                switch (Status)
                {
                    case Statuses.Success:
                        return null;
                    case Statuses.TokenNotFound:
                        return "Неизвестный код подтверждения";
                }
                return "При пересылке кода подтверждения произошла ошибка";
            }
        }

        public static RenewResultModel Map(RenewOtpResult original)
        {
            var model = new RenewResultModel
            {
                Token = original.OtpToken,
                ExpirationTimeUtc = original.ExpirationTimeUtc,
                AttemptsRemaining = original.AttemptsRemaining,
                Status = Statuses.Success
            };

            return model;
        }

        public enum Statuses
        {
            Success,
            TokenNotFound
        }
    }
}