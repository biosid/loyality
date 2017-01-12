using System;
using System.Net.Http;
using System.Web;
using Vtb24.Site.Security;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.SmsService.Models.Exceptions;

namespace Vtb24.Site.Services.SmsService.Stubs
{
    public class SmsServiceStub : ISmsService
    {
        public void Send(string phone, string message)
        {
            // Ничего не делаем
        }
    }
}
