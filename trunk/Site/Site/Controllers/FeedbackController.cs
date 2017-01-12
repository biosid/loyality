using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.Feedback;
using Vtb24.Site.Security;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services;
using Vtb24.Site.Services.ClientFeedback.Models.Inputs;
using Vtb24.Site.Services.ClientMessage.Models.Exceptions;
using Vtb24.Site.Services.ClientMessageService;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Infrastructure.MvcHelpers;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Profile.Models;
using IClientMessageService = Vtb24.Site.Services.IClientMessageService;

namespace Vtb24.Site.Controllers
{
    public class FeedbackController : BaseController
    {
        public const int PAGE_SIZE = 20;

        public FeedbackController(IClientFeedbackService feedback, IClientMessageService messages, IClientService client, ClientPrincipal principal, ISecurityService security, IGiftShop gifts)
        {
            _client = client;
            _principal = principal;
            _security = security;
            _feedback = feedback;
            _messages = messages;
            _gifts = gifts;
        }

        private readonly IClientFeedbackService _feedback;
        private readonly IClientMessageService _messages;
        private readonly IClientService _client;
        private readonly ClientPrincipal _principal;
        private readonly ISecurityService _security;
        private readonly IGiftShop _gifts;

        #region  Отправка

        [HttpGet]
        public ActionResult Index(FeedbackType? type)
        {
            var model = new FeedbackModel
            {
                SelectedType = type ?? FeedbackType.Suggestion
            };

            var profile = User.Identity.IsAuthenticated ? _client.GetProfile() : null;

            if (profile != null)
            {
                model.Email = profile.Email;
                model.IsClient = true;
            }

            HydrateFeedbackModel(model, profile);
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FeedbackModel model)
        {
            var selectedType = model.SelectedType.Map();
            if (!selectedType.HasValue)
            {
                LogInfo("неподдерживаемый тип сообщения обратной связи", new { feedbackType = model.SelectedType });
                throw new ArgumentException("SelectedType");
            }

            var profile = User.Identity.IsAuthenticated ? _client.GetProfile() : null;

            if (profile != null)
            {
                // для залогиненых пользователей капча не требуется
                ModelState.Remove("Captcha");
                // для предложений и претензий залогиненых пользователей email необязательный
                if (string.IsNullOrEmpty(model.Email) && selectedType.Value != Services.ClientFeedback.Models.FeedbackType.Unsubscribe)
                {
                    ModelState.Remove("Email");
                }

                if (profile.Status == ClientStatus.Activated)
                {
                    // для активированных клиентов не нужно валидировать ФИО
                    ModelState.Remove("Fio");
                }
            }

            if (model.SelectedType != FeedbackType.OrderIncident)
            {
                // номер заказа обязателен только для обращений по оформленным заказам
                ModelState.Remove("OrderId");
            }

            if (!ModelState.IsValid)
            {
                HydrateFeedbackModel(model, profile);
                return View("Index", model);
            }

            var metadata = HttpUtility.ParseQueryString("");
            switch (model.SelectedType)
            {
                case FeedbackType.OrderIncident:
                    var order = _gifts.GetOrder(model.OrderId ?? 0);
                    metadata["OrderId"] = order.Id.ToString(CultureInfo.InvariantCulture);
                    metadata["ExternalOrderId"] = order.ExternalId;
                    break;
            }

            var options = new SendFeedbackOptions
            {
                Title = CreateFeedbackTitle(model, profile, metadata),
                Type = selectedType.Value,
                ClientFullName = model.Fio,
                ClientEmail = model.Email,
                Text = model.Text,
                MetaData = metadata.Count == 0 ? null : metadata.ToString()
            };
            _feedback.SendFeedback(options);

            if (selectedType.Value == Services.ClientFeedback.Models.FeedbackType.Unsubscribe)
            {
                // разлогиниваем текущего пользователя
                _security.Logout();
                return RedirectToAction("UnsubscribeSurvey", "Feedback");
            }

            return RedirectToAction("ThankYou", "Feedback");
        }

        [HttpGet]
        public ActionResult ThankYou()
        {
            return View("ThankYou");
        }

        [HttpGet]
        public ActionResult UnsubscribeSurvey()
        {
            return View("UnsubscribeSurvey");
        }

        #region Приватные методы

        private static readonly FeedbackType[] ClientFeedbackTypes = new[]
        {
            FeedbackType.Suggestion,
            FeedbackType.Issue,
            FeedbackType.OrderIncident,
            FeedbackType.Unsubscribe
        };

        private static readonly FeedbackType[] GuestFeedbackTypes = new[]
        {
            FeedbackType.Suggestion,
            FeedbackType.Issue
        };

        private void HydrateFeedbackModel(FeedbackModel model, ClientProfile profile)
        {
            if (profile != null)
            {
                model.ValidateCaptcha = false;
                model.IsFioReadonly = profile.Status == ClientStatus.Activated;

                if (model.IsFioReadonly)
                {
                    model.Fio = profile.GetFullName();
                }

                model.Types = ClientFeedbackTypes;

                var orders = _gifts.GetOrdersHistory(
                    new []
                    {
                        OrderStatus.Cancelled,
                        OrderStatus.Delivered,
                        OrderStatus.Delivery,
                        OrderStatus.DeliveryWaiting,
                        OrderStatus.NotDelivered,
                        OrderStatus.Processing
                    },
                    new DateTimeRange(),
                    PagingSettings.ByOffset(0, 1000)
                );

                model.Orders = orders.Select(o => new SelectListItem
                {
                    Text = string.Format(new PluralizerFormatter(), "Заказ №{0} на сумму {2:N0} {2:pluralize|бонус|бонуса|бонусов}: {3}... ({1:dd.MM.yyyy})", o.Id, o.CreateDate, o.TotalPrice, o.Items.First().Title),
                    Value = o.Id.ToString(CultureInfo.InvariantCulture)
                }).ToArray();
            }
            else
            {
                model.ValidateCaptcha = true;
                model.IsFioReadonly = false;
                model.Types = GuestFeedbackTypes;
            }
        }

        private string CreateFeedbackTitle(FeedbackModel model, ClientProfile profile, NameValueCollection metadata)
        {
            var title = "Обращение";
            switch (model.SelectedType)
            {
                case FeedbackType.Issue:
                    title = "Претензия";
                    break;
                case FeedbackType.OrderIncident:
                    title = string.Format("Вопрос по оформленному заказу (№{0}\\{1})", metadata["OrderId"], metadata["ExternalOrderId"] ?? "-");
                    break;
                case FeedbackType.Suggestion:
                    title = "Предложение";
                    break;
            }
            var isGuest = profile == null;
            var hasProfileFio = !isGuest && profile.Status == ClientStatus.Activated;

            return string.Format(
                "{0} {1} {2} ({3}) от {4:dd MMMM yyyy HH:mm}",
                title,
                isGuest ? "посетителя" : "клиента",
                hasProfileFio ? profile.GetFullName() : model.Fio,
                isGuest ? model.Email : PhoneFormatter.FormatPhoneNumber(profile.Phones.First().Number),
                DateTime.Now
            );
        }

        #endregion

        #endregion


        #region Стать партнёром

        [HttpGet]
        public ActionResult BecomeAPartner()
        {
            var model = new BecomeAPartnerModel();
            return View("BecomeAPartner", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BecomeAPartner(BecomeAPartnerModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("BecomeAPartner", model);
            }

            var request = new BecomeAPartnerRequest
            {
                Location = model.Location,
                LegalEntity = model.LegalEntity,
                Contact = model.Contact,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Branch = model.Branch,
                GroupOfGoods = model.GroupOfGoods,
                Site = model.Site,
                PosCount = model.PosCount,
                Comment = model.Comment
            };

            _feedback.SendBecomeAPartnerRequest(request);

            return RedirectToAction("ThankYouPartner", "Feedback");
        }

        [HttpGet]
        public ActionResult ThankYouPartner()
        {
            return View("ThankYouPartner");
        }

        #endregion


        #region Переписка

        [HttpGet]
        public ActionResult AnonymousThread(Guid id, int page = 1)
        {
            var model = CreateFeedbackThreadModel(id, page, menu: false, uploads: false, captcha: true);

            if (!model.IsAnonymous)
            {
                throw new HttpException(404, "Переписка не найдена");
            }

            MarkThreadAsRead(id);

            return View("Thread", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnonymousThread(FeedbackThreadModel model, int page = 1)
        {
            return Reply(model.ReplyForm, page, menu: false, uploads: false, captcha: true);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ClientThread(Guid id, int page = 1)
        {
            var model = CreateFeedbackThreadModel(id, page, menu: true, uploads: true, captcha: false);

            if (model.IsAnonymous)
            {
                throw new HttpException(404, "Переписка не найдена");
            }

            MarkThreadAsRead(id);

            return View("Thread", model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ClientThread(FeedbackThreadModel model, int page = 1)
        {
            return Reply(model.ReplyForm, page, menu: true, uploads: true, captcha: false);
        }

        #region Приватные методы

        private ActionResult Reply(FeedbackReplyModel model, int page, bool menu, bool uploads, bool captcha)
        {
            if (!captcha)
            {
                ModelState.Remove("ReplyForm.Captcha");
            }

            // валидация аттачей
            if (uploads)
            {
                model.Files.ValidateFileSizes(
                    AppSettingsHelper.Int("feedback_max_single_upload_size_mb", 5),
                    (msg, index, file) => ModelState.AddModelError("ReplyForm.Files[0]", msg)
                );

                model.Files.ValidateTotalSize(
                    AppSettingsHelper.Int("feedback_max_total_upload_size_mb", 5),
                    (msg, size) => ModelState.AddModelError("ReplyForm.Files[0]", msg)
                );
            }

            if (!ModelState.IsValid)
            {
                var threadModel = CreateFeedbackThreadModel(model.Id, page, menu, uploads, captcha);
                threadModel.ReplyForm = model;
                return View("Thread", threadModel);
            }

            var thread = _messages.GetThreadById(new ClientGetThreadByIdParameters
            {
                ClientId = _principal.ClientId,
                ThreadId = model.Id
            });

            var fullName = thread.Thread.ClientFullName;
            var email = thread.Thread.ClientEmail;

            // сохранение аттачей
            MessageAttachment[] attachments = null;
            if (uploads)
            {
                attachments = model.Files
                    .SaveUploads()
                    .Select(u => new MessageAttachment { Id = u.Id, FileName = u.FileName, FileSize = u.FileSize })
                    .ToArray();
            }

            var result = _messages.Reply(new ClientReplyParameters
            {
                ThreadId = model.Id,
                ClientId = _principal.ClientId,
                ClientEmail = email,
                ClientFullName = fullName,
                Attachments = attachments,
                MessageBody = model.Text
            });

            var newPage = (int) Math.Ceiling((decimal) result.Thread.MessagesCount/PAGE_SIZE);

            var url = Url.Action(
                ControllerContext.Controller.ValueProvider.GetValue("action").AttemptedValue,
                new { id = model.Id, page= newPage}
            );

            return Redirect(url + "#message-" + (result.Thread.MessagesCount-1));
        }

        private FeedbackThreadModel CreateFeedbackThreadModel(Guid id, int page, bool menu, bool uploads, bool captcha)
        {
            try
            {
                var threadResult = _messages.GetThreadMessages(new ClientGetThreadMessagesParameters
                {
                    ClientId = _principal.ClientId,
                    ThreadId = id,
                    CountToTake = PAGE_SIZE,
                    CountToSkip = (page -1) * PAGE_SIZE
                });

                if (threadResult == null)
                {
                    throw new HttpException(404, "Переписка не найдена");
                }

                var model = FeedbackThreadModel.Map(threadResult.Thread, threadResult.ThreadMessages);
                model.MvcAction = ControllerContext.Controller.ValueProvider.GetValue("action").AttemptedValue;
                model.ShowClientMenu = menu;
                model.AllowUploads = uploads;
                model.ShowCaptcha = captcha;
                model.TotalPages = (int) Math.Ceiling((decimal)threadResult.TotalCount/PAGE_SIZE);
                model.Page = page;
                model.ShowReplyForm = page == model.TotalPages && model.ShowReplyForm;

                return model;
            }
            catch (ThreadNotFoundException)
            {
                throw new HttpException(404, "Переписка не найдена");
            }
            catch (ThreadSecurityException)
            {
                throw new HttpException(403, "Недостаточно прав");
            }
        }

        private void MarkThreadAsRead(Guid id)
        {
            _messages.MarkThreadAsRead(new MarkThreadAsReadParameters
            {
                ClientId = _principal.ClientId,
                ThreadId = id
            });
        }

        #endregion

        #endregion

    }
}
