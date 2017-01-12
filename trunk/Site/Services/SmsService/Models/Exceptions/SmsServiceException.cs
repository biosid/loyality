using System;

namespace Vtb24.Site.Services.SmsService.Models.Exceptions
{
    public class SmsServiceException : Exception
    {
        public SmsServiceException(string message) : base("[SMS сервис]:" + message)
        {
        }
    }
}