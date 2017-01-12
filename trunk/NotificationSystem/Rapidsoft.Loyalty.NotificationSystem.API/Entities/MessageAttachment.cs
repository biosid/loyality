namespace Rapidsoft.Loyalty.NotificationSystem.API.Entities
{
    using System;

    public class MessageAttachment
    {
        public string FileName  
        {
            get;
            set;
        }

        public int FileSize
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public int MessageId
        {
            get;
            set;
        }

        public Guid ThreadId
        {
            get;
            set;
        }
    }
}