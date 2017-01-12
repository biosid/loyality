using System;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services.MyInfoService.Models;
using Vtb24.Site.Services.MyInfoService.Models.Inputs;

namespace Vtb24.Site.Services.MyInfoService
{
    public class MyInfoService : IMyInfoService, IDisposable
    {
        public MyInfoService(ClientPrincipal principal, IMyInfoServiceInternal impl)
        {
            _principal = principal;
            _impl = impl;
        }

        private readonly ClientPrincipal _principal;
        private readonly IMyInfoServiceInternal _impl;

        public MyInfo GetMyInfo()
        {
            var clientId = GetClientId();
            return _impl.GetMyInfo(clientId);
        }

        public void UpdateMyInfo(UpdateMyInfoParams options)
        {
            var clientId = GetClientId();
            _impl.UpdateMyInfo(clientId, options);
        }

        private string GetClientId()
        {
            if (!_principal.IsAuthenticated)
            {
                throw new InvalidOperationException("Получение личных сообщений доступно только аутентифицированным пользователям");
            }
            return _principal.ClientId;
        }

        public void Dispose()
        {
            // Do nothing
        }
    }
}