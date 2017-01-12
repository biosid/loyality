namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;

    public class AdminGetThreadByIdParameters
    {
        public Guid ThreadId
        {
            get;
            set;
        }

        public string UserId
        {
            get;
            set;
        }  
    }
}