using Vtb24.Site.Services.MyInfoService.Models;
using Vtb24.Site.Services.MyInfoService.Models.Inputs;

namespace Vtb24.Site.Services.MyInfoService
{
    public interface IMyInfoServiceInternal
    {
        MyInfo GetMyInfo(string clientId);
        void UpdateMyInfo(string clientId, UpdateMyInfoParams options);
    }
}