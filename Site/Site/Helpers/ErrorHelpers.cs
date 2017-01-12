using System;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions;

namespace Vtb24.Site.Helpers
{
    public static class ErrorHelpers
    {
        public static string Format(this TooFrequentOtpSendException ex, string what)
        {
            var prevAttempt = (int)Math.Floor((DateTime.UtcNow - ex.PreviousAttempt).TotalMinutes);
            var minIntervalInMinutes = (int)Math.Floor((ex.MinIntervalInSeconds / 60.0));

            return string.Format(
                "Интервал между попытками {0} должен быть больше {1}. Последняя попытка была {2} назад.",
                what,
                minIntervalInMinutes > 0 ?
                    minIntervalInMinutes.Pluralize("{1} минуты", "{2} минут")
                    : "менее минуты",
                prevAttempt > 0
                    ? prevAttempt.Pluralize("{1} минуту", "{2} минуты", "{5} минут")
                    : "менее минуты");
        }

        public static string Format(this TooManyOtpSendAttemptsException ex, string what)
        {
            var intervalInMinutes = (int)Math.Floor((ex.IntervalInSeconds / 60.0));

            return string.Format(
                "Превышено количество попыток {0} ({1} в течение {2}). Попробуйте позже.",
                what,
                ex.Attempts,
                intervalInMinutes.Pluralize("{1} минуты", "{2} минут"));
        }
    }
}
