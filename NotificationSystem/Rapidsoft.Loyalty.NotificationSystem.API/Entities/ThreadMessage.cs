namespace Rapidsoft.Loyalty.NotificationSystem.API.Entities
{
    using System;

    public class ThreadMessage
    {
        public MessageTypes MessageType
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public Guid ThreadId
        {
            get;
            set;
        }

        public string MessageBody
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        public bool IsUnread
        {
            get;
            set;
        }

        public string AuthorFullName
        {
            get;
            set;
        }

        public string AuthorId
        {
            get;
            set;
        }

        public string AuthorEmail
        {
            get;
            set;
        }

        public DateTime InsertedDate
        {
            get;
            set;
        }

        public MessageAttachment[] Attachments
        {
            get;
            set;
        }
    }
}