using Vtb24.Site.Services.MyInfoService.Models;
using Vtb24.Site.Services.MyInfoService.Models.Inputs;

namespace Vtb24.Site.Services
{
    public interface IMyInfoService
    {
        MyInfo GetMyInfo();

        void UpdateMyInfo(UpdateMyInfoParams options);
    }
}