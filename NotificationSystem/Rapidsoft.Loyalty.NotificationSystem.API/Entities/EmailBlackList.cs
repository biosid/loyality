namespace Rapidsoft.Loyalty.NotificationSystem.API.Entities
{
    using System;

    public class EmailBlackList
    {
        public int Id
        {
            get;
            set;
        }

        public string ClientEmail
        {
            get;
            set;
        }

        public DateTime InsertedDate
        {
            get;
            set;
        }
    }
}
