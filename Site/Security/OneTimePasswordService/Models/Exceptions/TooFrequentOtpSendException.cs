using System;

namespace Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions
{
    public class TooFrequentOtpSendException : OneTimePasswordServiceException
    {

        public int MinIntervalInSeconds { get; private set; }

        public DateTime PreviousAttempt { get; private set; }

        internal TooFrequentOtpSendException(int interval, DateTime last) : base(CreateMessage(interval, last))
        {
            MinIntervalInSeconds = interval;
            PreviousAttempt = last;
        }

        private static string CreateMessage(int interval, DateTime last)
        {
            return string.Format(
                "Слишком частая отправка OTP с совпадающими параметрами. "
                + "Минимальный интервал между попытками: {0} сек. "
                + "Дата время предыдущей попытки: {1}",
                interval,
                last
            );
        }
    }
}