using System;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminFeedbackServiceEndpoint;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Security.Models;
using Vtb24.Arms.Security.Models.Feedback;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Infrastructure.MvcHelpers;
using IAdminFeedbackService = Vtb24.Arms.AdminServices.IAdminFeedbackService;

namespace Vtb24.Arms.Security.Controllers
{
    [Authorize]
    public class FeedbackController : BaseController
    {
        public const int INDEX_PAGE_SIZE = 50;
        public const int THREAD_PAGE_SIZE = 50;

        public FeedbackController(IAdminFeedbackService feedback, ISecurityManagement securityManager)
        {
            _feedback = feedback;
            _securityManager = securityManager;
        }

        private readonly IAdminFeedbackService _feedback;
        private readonly ISecurityManagement _securityManager;

        public ActionResult Index(FeedbackIndexFiltersModel filters)
        {
            var options = new AdminGetThreadsParameters
            {
                ClientType = null,
                ClientEmail = filters.email,
                Filter = AnsweredFilters.UnansweredFirst,
                //IncludeTopicMessage = true,
                MaxDate = filters.to.HasValue ? filters.to.Value.AddDays(1).AddMilliseconds(-1) : (DateTime?) null,
                MinDate = filters.from,
                SearchTerm = filters.keywords,
                OperatorLogin = filters.oplogin,
                FeedbackType = filters.MapThreadType(),
                UserId = User.Identity.Name,
                CountToSkip = PagingHelper.GetSkip(filters.page, INDEX_PAGE_SIZE),
                CountToTake = INDEX_PAGE_SIZE
            };

            if (!string.IsNullOrWhiteSpace(filters.phone))
            {
                var clientId = _securityManager.GetClientIdByLogin(PhoneFormatter.StripPhoneNumber(filters.phone));

                if (clientId == null)
                {
                    return View("Index", new FeedbackIndexModel
                    {
                        Filters = filters,
                        Threads = new FeedbackIndexThreadModel[0]
                    });
                }

                options.ClientId = clientId;
            }

            var result = _feedback.GetThreads(options);
            var threads = result
                .Result
                .Select(r => FeedbackIndexThreadModel.Map(r, THREAD_PAGE_SIZE))
                .ToArray();

            var model = new FeedbackIndexModel
            {
                Filters = filters,
                PagesCount = PagingHelper.GetPagesCount(result.TotalCount, INDEX_PAGE_SIZE),
                Threads = threads
            };

            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Thread(Guid id, string filter, int page = 1)
        {
            var model = CreateThreadModel(id, filter, page);
            model.Reply = new FeedbackThreadReplyModel {Id = id};
            return View("Thread", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Thread(FeedbackThreadReplyModel reply, string filter, int page = 1)
        {
            if (reply.IgnoreThread)
            {
                _feedback.ChangeAnsweredStatus(new ChangeAnsweredStatusParameters
                {
                    IsAnswered = true,
                    ThreadId = reply.Id,
                    UserId = User.Identity.Name
                });
                return RedirectToAction("Index", "Feedback", FeedbackIndexModel.DeserializeFilters(filter));
            }

            // валидация аттачей
            reply.Files.ValidateFileSizes(
                AppSettingsHelper.Int("feedback_max_single_upload_size_mb", 5),
                (msg, index, file) => ModelState.AddModelError("ReplyForm.Files[0]", msg)
            );
            reply.Files.ValidateTotalSize(
                AppSettingsHelper.Int("feedback_max_total_upload_size_mb", 5),
                (msg, size) => ModelState.AddModelError("ReplyForm.Files[0]", msg)
            );

            if (!ModelState.IsValid)
            {
                var model = CreateThreadModel(reply.Id, filter, page);
                model.Reply = reply;
                return View("Thread", model);
            }

            // сохранение аттачей
            var attachments = reply.Files
                    .SaveUploads()
                    .Select(u => new MessageAttachment { Id = u.Id, FileName = u.FileName, FileSize = u.FileSize })
                    .ToArray();

            var options = new AdminReplyParameters
            {
                ThreadId = reply.Id,
                MessageBody = reply.Text,
                Attachments = attachments,
                UserId = User.Identity.Name
            };
            var result = _feedback.Reply(options);

            var url = CreateNewMessageUrl(result.Thread, filter, result.Thread.MessagesCount - 1);
            return Redirect(url);
        }

        private string CreateNewMessageUrl(Thread thread, string filter, int messageIndex)
        {
            var page = PagingHelper.GetPagesCount(messageIndex + 1, THREAD_PAGE_SIZE);
            return Url.Action("Thread", "Feedback", new {id = thread.Id, page, filter}) + "#message-" + messageIndex;
        }

        private FeedbackThreadModel CreateThreadModel(Guid id, string filter, int page = 1)
        {
            var options = new AdminGetThreadMessagesParameters
            {
                ThreadId = id,
                UserId = User.Identity.Name,
                CountToSkip = PagingHelper.GetSkip(page, THREAD_PAGE_SIZE),
                CountToTake = THREAD_PAGE_SIZE
            };
            var result = _feedback.GetThreadMessages(options);

            var clientPhone = result.Thread.ClientType == ThreadClientTypes.Client
                ? _securityManager.GetLoginByClientId(result.Thread.ClientId)
                : null;

            var model = FeedbackThreadModel.Map(result, clientPhone);
            model.Page = page;
            model.Filter = filter;
            model.PagesCount = PagingHelper.GetPagesCount(result.TotalCount, THREAD_PAGE_SIZE);
            model.MaxFileSizeMb = AppSettingsHelper.Int("feedback_max_single_upload_size_mb", 10);
            model.MaxTotalFilesSizeMb = AppSettingsHelper.Int("feedback_max_total_upload_size_mb", 100);

            return model;
        }
    }
}
