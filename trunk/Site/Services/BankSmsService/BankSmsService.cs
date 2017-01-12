using Vtb24.Site.Security;
using Vtb24.Site.Services.BankConnectorService;
using Vtb24.Site.Services.BankSmsService.Models.Exceptions;
using BankSmsType = Vtb24.Site.Security.Models.BankSmsType;

namespace Vtb24.Site.Services.BankSmsService
{
    public class BankSmsService : IBankSmsService
    {
        public void Send(BankSmsType type, string phone, string password)
        {
            using (var service = new BankConnectorClient())
            {
                var response = service.EnqueueSms(MappingsToService.ToBankSmsType(type),
                                                  MappingsToService.ToPhone(phone),
                                                  password);

                AssertResponse(response.ResultCode, response.Error);
            }
        }

        private static void AssertResponse(int code, string message)
        {
            if (code == 0)
            {
                return;
            }

            throw new BankSmsException(code, message);
        }
    }
}
