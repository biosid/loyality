using System;

namespace Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions
{
    public class TooManyOtpSendAttemptsException : OneTimePasswordServiceException
    {
        public int Attempts { get; private set; }

        public int IntervalInSeconds { get; private set; }

        public DateTime BlockedDue { get; private set; }

        internal TooManyOtpSendAttemptsException(int attempts, int interval, DateTime due) : base(CreateMessage(attempts, interval, due))
        {
            Attempts = attempts;
            IntervalInSeconds = interval;
            BlockedDue = due;
        }

        private static string CreateMessage(int attempts, int interval, DateTime due)
        {
            return string.Format(
                "Превышено количество попыток ({0} в течение {1} сек.) отправки OTP с совпадающими параметрами. "
                + "Возможность отправки заблокирована до {2} для OTP с текущими параметрами",
                attempts,
                interval,
                due
            );
        }
    }
}