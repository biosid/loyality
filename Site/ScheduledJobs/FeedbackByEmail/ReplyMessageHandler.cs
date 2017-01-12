using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Vtb24.Arms.AdminServices.AdminFeedback.Models.Exceptions;
using Vtb24.Arms.AdminServices.AdminFeedbackServiceEndpoint;
using Vtb24.Site.Services.Infrastructure;
using IAdminFeedbackService = Vtb24.Arms.AdminServices.IAdminFeedbackService;
using ScheduledJobs.Infrastructure.Mail;

namespace ScheduledJobs.FeedbackByEmail
{
    public class ReplyMessageHandler
    {
        public ReplyMessageHandler(IAdminFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        private readonly IAdminFeedbackService _feedbackService;

        public void Execute(MailMessage message)
        {
            try
            {
                // Шаг 1: Получить информацию о ветке из темы письма
                var threadInfo = ExtractThreadInfo(message);
                // Шаг 1.1: Если заголовок сформирован некорректно, остановить обработку
                if (threadInfo == null)
                {
                    throw new HandlerException("Ошибка!\n\nВ теме сообщения не найден идентификатор переписки");
                }

                // Шаг 2: Получить текст ответа из тела письма
                var replyText = ExtractReplyText(message);
                // Шаг 2.1: Если текст не задан или превышает 1000 символов, остановить обработку
                if (string.IsNullOrEmpty(replyText))
                {
                    throw new HandlerException("Ошибка!\n\nНе указан текст ответа");
                }
                if (replyText.Length > 1000)
                {
                    throw new HandlerException(
                        "Ошибка!\n\n"
                        + "Превышена допустимая длина сообщения (1000 символов).\n\n"
                        + "Возможно, в тексте ответа присутствует разметка или не была удалена история переписки."
                    );
                }
                //  Шаг 2.2: Если в тексте содержится почтовый ящик взаимодействия, остановить обработку и предупредить
                //           о необходимости удалять цитаты из отсылакмых сообщений
                if (replyText.Contains(Settings.MailUser))
                {
                    var error = string.Format(
                        "Ошибка!\n\n"
                        + "В тексте сообщения содержится адрес почтового ящика для приёма сообщений от операторов ({0}).\n\n"
                        + "Скорее всего, из письма не была удалена история переписки.",
                        Settings.MailUser
                    );
                    throw new HandlerException(error);
                }
                // Шаг 3: Сохранить вложения на сайт
                var attachments = ExtractAttachments(message);

                // Шаг 4 (временно отключён): Получить переписку
                /*var thread = _feedbackService.GetThreadMessages(new AdminGetThreadMessagesParameters
                {
                    UserId = Settings.UserId,
                    ThreadId = threadInfo.ThreadId,
                    CountToTake = 0,
                    CountToSkip = 0
                });*/
                // Шаг 4.1 (временно отключён): Если оператор отвечает не на последнее сообщение в переписке, остановить обработку
                /*var isStale = thread.TotalCount != threadInfo.MessagesCount;
                if (isStale)
                {
                    throw new HandlerException("Ошибка!\n\nОтвет не добавлен, так как переписка содержит новые сообщения.");
                }*/
                // Шаг 5: Отправить ответ
                _feedbackService.Reply(new AdminReplyParameters
                {
                    UserId = Settings.UserId,
                    ThreadId = threadInfo.ThreadId,
                    MessageBody = replyText,
                    Attachments = attachments
                });
            }
            catch (AdminFeedbackException e)
            {
                var error = e.ResultCode == 2 // NOT_FOUND
                    ? "Ошибка!\n\n Переписка не найдена"
                    : "Ошибка!\n\n Неизвестная ошибка при добавлении ответа в переписку";
                throw new HandlerException(error, e);
            }
        }

        private static MessageAttachment[] ExtractAttachments(MailMessage message)
        {
            if (message.Attachments.Count == 0)
            {
                return null;
            }

            var uploads = message.Attachments
                .SaveAsSiteUpload()
                .Select(u=>new MessageAttachment
                {
                    Id = u.Id,
                    FileName = u.FileName,
                    FileSize = u.FileSize
                })
                .ToArray();

            return uploads;
        }

        private static string ExtractReplyText(MailMessage message)
        {
            if (!message.IsBodyHtml)
            {
                return message.Body;
            }

            return WebUtility.HtmlDecode(HtmlSanitizer.StripHtmlTags(message.Body));
        }

        private static ThreadInfo ExtractThreadInfo(MailMessage message)
        {
            var subject = message.Subject;
            if (string.IsNullOrWhiteSpace(subject))
            {
                return null;
            }

            var matches = new Regex(@"\[ID:(.{36});C:(\d+)\]", RegexOptions.IgnoreCase).Matches(subject);

            if (matches.Count != 1)
            {
                return null;
            }

            Guid id;
            var rawId = matches[0].Groups[1].Value;
            int count;
            var rawCount = matches[0].Groups[2].Value;

            if (!Guid.TryParse(rawId, out id) || !int.TryParse(rawCount, out count))
            {
                return null;
            }

            return new ThreadInfo
            {
                ThreadId = id,
                MessagesCount = count
            };

        }

        public class HandlerException : Exception
        {
            public HandlerException(string message) : base(message)
            {
            }

            public HandlerException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }

        class ThreadInfo
        {
            public Guid ThreadId { get; set; }

            public int MessagesCount { get; set; }
        }
    }
}