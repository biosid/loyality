using System;
using Vtb24.Site.Security.OneTimePasswordService.Models.Outputs;

namespace Vtb24.Site.Models.Otp
{
    public class ConfirmationResultModel
    {
        public bool Confirmed { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }

        public int ExpiresInSeconds
        {
            get
            {
                var sec =(int) (ExpirationTimeUtc - DateTime.UtcNow).TotalSeconds;
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
                    case Statuses.AttemptsExceeded:
                        return "Время действия кода подтверждения истекло";
                    case Statuses.TokenExpired:
                        return "Превышено количество попыток ввода кода подтверждения";
                    case Statuses.TokenNotFound:
                        return "Неизвестный код подтверждения";
                }

                return "При проверке кода подтверждения произошла ошибка";
            }
        }

        public ConfirmationResultModel Map(ConfirmOtpResult original)
        {
            var model = new ConfirmationResultModel
            {
                Confirmed = original.Confirmed,
                ExpirationTimeUtc = original.ExpirationTimeUtc,
                AttemptsRemaining = original.AttemptsRemaining,
                Status = Statuses.Success
            };

            return model;
        }

        public enum Statuses
        {
            Success,
            AttemptsExceeded,
            TokenExpired,
            TokenNotFound
        }
    }
}