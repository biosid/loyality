using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.MyMessages;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services.ClientMessageService;
using Vtb24.Site.Services.Models;
using IClientMessageService = Vtb24.Site.Services.IClientMessageService;

namespace Vtb24.Site.Controllers
{
    [Authorize]
    public class MyMessagesController : BaseController
    {
        private const int PAGE_SIZE = 30;

        public MyMessagesController(ClientPrincipal principal, IClientMessageService clientMessage)
        {
            _principal = principal;
            _clientMessage = clientMessage;
        }

        private readonly ClientPrincipal _principal;
        private readonly IClientMessageService _clientMessage;

        public ActionResult Index(int page = 1)
        {
            if (page <= 0)
            {
                throw new HttpException(404, "страница сообщений не найдена");
            }

            var paging = PagingSettings.ByPage(page, PAGE_SIZE);

            var threads = _clientMessage.GetThreads(new ClientGetThreadsParameters
            {
                ClientId = _principal.ClientId,
                Filter = ReadFilters.All,
                CountToSkip = paging.Skip,
                CountToTake = paging.Take
            });

            var totalPages = paging.GetTotalPages(threads.TotalCount);

            if (page > totalPages)
            {
                throw new HttpException(404, "страница сообщений не найдена");
            }

            var model = new ThreadsModel
            {
                Threads = threads.Threads.Select(ThreadModel.Map).ToArray(),
                Page = page,
                TotalPages = totalPages
            };

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetRead(Guid threadId)
        {
            _clientMessage.MarkThreadAsRead(new MarkThreadAsReadParameters
            {
                ClientId = _principal.ClientId,
                ThreadId = threadId
            });

            return new EmptyResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid threadId, int redirectToPage = 1)
        {
            try
            {
                _clientMessage.Delete(new DeleteThreadParameters
                {
                    ThreadId = threadId,
                    ClientId = _principal.ClientId
                });
            }
            catch (Exception e)
            {
                LogError(e);
            }

            return RedirectToAction("Index", "MyMessages", new { page = redirectToPage });
        }
    }
}
