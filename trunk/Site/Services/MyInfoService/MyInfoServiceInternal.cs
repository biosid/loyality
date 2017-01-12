using System;
using System.Linq;
using Vtb24.Site.Services.BankConnectorService;
using Vtb24.Site.Services.MyInfoService.Models;
using Vtb24.Site.Services.MyInfoService.Models.Exceptions;
using Vtb24.Site.Services.MyInfoService.Models.Inputs;

namespace Vtb24.Site.Services.MyInfoService
{
    public class MyInfoServiceInternal : IMyInfoServiceInternal, IDisposable
    {

        public MyInfo GetMyInfo(string clientId)
        {
            using (var service = new BankConnectorClient())
            {
                var result = service.GetClientProfile(clientId);

                HandleResult(result.ResultCode, result.Error);

                return MappingsFromService.ToMyInfo(result.Result);
            }
        }

        public void UpdateMyInfo(string clientId, UpdateMyInfoParams options)
        {
            using (var service = new BankConnectorClient())
            {
                var result = service.UpdateClient(new UpdateClientRequest()
                {
                    ClientId = clientId,
                    UpdateProperties = new[] { UpdateProperty.Email, UpdateProperty.CustomFields },
                    Email = options.Email,
                    CustomFields = options.CustomFields.ToDictionary(f => f.FieldId, f => f.Value)
                });

                HandleResult(result.ResultCode, result.Error);
            }
        }

        private static void HandleResult(int code, string message)
        {
            if (code != 0)
            {
                throw new MyInfoException(code, message);
            }
        }

        public void Dispose()
        {
            // Do nothing
        }
    }
}