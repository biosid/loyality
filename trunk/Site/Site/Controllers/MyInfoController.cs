using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.MyInfo;
using Vtb24.Site.Services;
using Vtb24.Site.Services.MyInfoService.Models.Inputs;
using Vtb24.Site.Services.Profile.Models;

namespace Vtb24.Site.Controllers
{
    public class MyInfoController : BaseController
    {
        public MyInfoController(IClientService client, IMyInfoService info)
        {
            _client = client;
            _info = info;
        }

        private readonly IClientService _client;
        private readonly IMyInfoService _info;

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            if (_client.GetStatus() != ClientStatus.Activated)
            {
                throw new HttpException(404, "страница не найдена");
            }

            var info = _info.GetMyInfo();
            var model = MyInfoModel.Map(info);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MyInfoModel model)
        {
            if (_client.GetStatus() != ClientStatus.Activated)
            {
                throw new HttpException(404, "страница не найдена");
            }

            if (!ModelState.IsValid)
            {
                var info = _info.GetMyInfo();
                model.Merge(info);
                return View(model);
            }

            var options = new UpdateMyInfoParams
            {
                Email = model.Email,
                CustomFields = model.CustomFields != null
                                   ? model.CustomFields
                                          .Select(
                                              f =>
                                              new UpdateMyInfoParams.UpdateFieldOptions
                                              {
                                                  FieldId = f.Id,
                                                  Value = f.Value
                                              })
                                          .ToArray()
                                   : new UpdateMyInfoParams.UpdateFieldOptions[0]
            };
            _info.UpdateMyInfo(options);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetUserLocation(string locationKladr, string backUrl)
        {
            _client.SetUserLocation(locationKladr);

            return RedirectToLocal(backUrl);
        }

    }
}
