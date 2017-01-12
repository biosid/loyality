using System.Web.Mvc;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Services;

namespace Vtb24.Site.Controllers
{
    public class UnsubscribeController : BaseController
    {
        private readonly IClientMessageService _clientMessageService;

        public UnsubscribeController(IClientMessageService clientMessageService)
        {
            this._clientMessageService = clientMessageService;
        }

        [HttpGet]
        public ActionResult Index(string emailHash)
        {
            this._clientMessageService.Unsubscribe(emailHash);

            return View("Unsubscribe");
        }

    }
}
