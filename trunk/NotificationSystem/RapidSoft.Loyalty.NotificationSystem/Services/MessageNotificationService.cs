using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using RapidSoft.Loaylty.Logging;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;
using Rapidsoft.Loyalty.NotificationSystem.Email;
using Rapidsoft.Loyalty.NotificationSystem.DataAccess;
using Rapidsoft.Loyalty.NotificationSystem.DataAccess.Repositories;
using Vtb24.Common.Configuration;

namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    public class MessageNotificationService : IMessageNotificationService
    {
        private static readonly Regex EmailRegex = new Regex(@"([a-zA-Z0-9!#\$%&\._-]+)@([a-zA-Z0-9-]+\.)+([a-zA-Z]{2,63})", RegexOptions.Compiled);

        private readonly IEmailBlackListRepository _emailBlackListRepository;
        private readonly IThreadsRepository _threadsRepository;
        private readonly IProfile _profileService;
        private readonly IFileProvider _fileProvider;
        private readonly ISender _sender;
        private readonly ILog _log = LogManager.GetLogger(typeof(MessageNotificationService));

        public MessageNotificationService() : this(null)
        {
        }

        public MessageNotificationService(
            IEmailBlackListRepository emailBlackListRepository = null,
            IThreadsRepository threadsRepository = null,
            IProfile profileService = null,
            IFileProvider fileProvider = null,
            ISender sender = null)
        {
            _emailBlackListRepository = emailBlackListRepository ?? new EmailBlackListRepository();
            _threadsRepository = threadsRepository ?? new ThreadsRepository();
            _profileService = profileService ?? new Profile();
            _fileProvider = fileProvider ?? new WebFileProvider();
            _sender = sender ?? new Sender();
        }

        #region IMessageNotificationService Members

        public void SendMessageNotification(ThreadMessage threadMessage)
        {
            if (!FeaturesConfiguration.Instance.Gen1070EnableEmailMessageNotification)
            {
                return;
            }

            try
            {
                SendMessageNotificationInternal(threadMessage);
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
            }
        }

        public void Unsubscribe(string emailHash)
        {
            var privateKey = ConfigurationManager.AppSettings["unsubscribe_key"];
            var privateKeyBytes = Encoding.UTF8.GetBytes(privateKey);

            var buffer = HttpServerUtility.UrlTokenDecode(emailHash);
            if (buffer == null)
            {
                return;
            }

            var emailHashBytes = buffer.Take(32).ToArray();
            var emailBytes = buffer.Skip(32).ToArray();

            var email = Encoding.UTF8.GetString(emailBytes);

            using (var hmac = new HMACSHA256(privateKeyBytes))
            {
                var hashBytes = hmac.ComputeHash(emailBytes);

                if (hashBytes.SequenceEqual(emailHashBytes))
                {
                    AddToBlackList(email);
                }
            }
        }

        public void AddToBlackList(string email)
        {
            _emailBlackListRepository.Add(email);
        }

        public void RemoveFromBlackList(string email)
        {
            _emailBlackListRepository.Remove(email);
        }

        #endregion

        private static MailMessage BuildMessage(string email, IFileProvider fileProvider)
        {
            var from = ConfigurationManager.AppSettings["ClientFeedbackFrom"];
            var body = BuildMessageBody(email);

            var message = new MailMessage(from, email)
            {
                Subject = @"Новое сообщение в программе «Коллекция»",
                IsBodyHtml = true
            };

            var alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

            alternateView.LinkedResources.Add(NewLinkedResource("content_bg", @"/images/content_bg.png", "image/png", fileProvider));
            alternateView.LinkedResources.Add(NewLinkedResource("header_bg", @"/images/header_bg.png", "image/png", fileProvider));
            alternateView.LinkedResources.Add(NewLinkedResource("logo", @"/images/logo.png", "image/png", fileProvider));
            alternateView.LinkedResources.Add(NewLinkedResource("message_bg", @"/images/message_bg.png", "image/png", fileProvider));
            alternateView.LinkedResources.Add(NewLinkedResource("message_bottom", @"/images/message_bottom.png", "image/png", fileProvider));
            alternateView.LinkedResources.Add(NewLinkedResource("message_top", @"/images/message_top.png", "image/png", fileProvider));

            message.AlternateViews.Add(alternateView);

            return message;
        }

        private static LinkedResource NewLinkedResource(string contentId, string filePath, string mimeType, IFileProvider fileProvider)
        {
            var fileName = fileProvider.MapPath(filePath);

            return new LinkedResource(fileName, mimeType)
            {
                ContentId = contentId
            };
        }

        private static string BuildMessageBody(string email)
        {
            var threadUrl = ConfigurationManager.AppSettings["ClientSiteUrl"] +
                            "/mymessages/";

            var unsubscribeUrl = ConfigurationManager.AppSettings["ClientSiteUrl"] +
                            "/unsubscribe/" + BuildUnsubscribeHash(email);

            var bodyTemplate = new Templates.ClientMessageNotificationBody
            {
                ThreadUrl = threadUrl,
                UnsubscribeUrl = unsubscribeUrl
            };

            var msgBody = bodyTemplate.TransformText();
            return msgBody;
        }

        private static string BuildUnsubscribeHash(string email)
        {
            var privateKey = ConfigurationManager.AppSettings["unsubscribe_key"];
            var privateKeyBytes = Encoding.UTF8.GetBytes(privateKey);

            using (var hmac = new HMACSHA256(privateKeyBytes))
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(email));
                var temp = hashBytes.Concat(Encoding.UTF8.GetBytes(email));
                var hashString = HttpServerUtility.UrlTokenEncode(temp.ToArray());

                return hashString;
            }
        }

        private void SendMessageNotificationInternal(ThreadMessage threadMessage)
        {
            if (threadMessage.ThreadId == Guid.Empty)
            {
                throw new ArgumentException("threadMessage.ThreadId is empty");
            }

            var thread = _threadsRepository.GetThread(threadMessage.ThreadId, false);

            if (thread == null)
            {
                return;
            }

            var clientId = thread.ClientId;
            var clientEmail = _profileService.GetProfileEmail(clientId);

            if (string.IsNullOrWhiteSpace(clientEmail))
            {
                _log.InfoFormat("Client has no Email: clientId = {0}, threadMessage.Id = {1}", clientId, threadMessage.Id);
                return;
            }

            if (!IsValidEmail(clientEmail))
            {
                throw new ArgumentNullException(string.Format("Invalid email {0}, threadMessage.Id {1}", clientEmail, threadMessage.Id));
            }

            var emailBlaskListEntry = _emailBlackListRepository.GetByEmail(clientEmail);

            if (emailBlaskListEntry == null)
            {
                var message = BuildMessage(clientEmail, _fileProvider);

                _sender.SendEmail(message);
            }
            else
            {
                _log.InfoFormat("Email in blacklist {0}, threadMessage.Id {1}", clientEmail, threadMessage.Id);
            }
        }

        private bool IsValidEmail(string email)
        {
            return EmailRegex.Match(email).Success;
        }
    }
}