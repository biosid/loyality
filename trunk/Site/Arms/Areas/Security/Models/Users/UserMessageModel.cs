using System;
using Vtb24.Site.Services.ClientMessageService;

namespace Vtb24.Arms.Security.Models.Users
{
    public class UserMessageModel
    {
        public DateTime CreateDateTime { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public bool IsRead { get; set; }

        public UserMessageType Type { get; set; }

        public static UserMessageModel Map(Thread original)
        {
            return new UserMessageModel
            {
                CreateDateTime = original.InsertedDate,
                Subject = original.Title, // TODO: это неверно
                IsRead = original.UnreadMessagesCount == 0,
                Type = original.Type.Map(),
                Text = original.TopicMessage != null ? original.TopicMessage.MessageBody : string.Empty
            };
        }
    }
}
