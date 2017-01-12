namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;

    public class ClientGetThreadByIdParameters
    {
        public Guid ThreadId
        {
            get;
            set;
        }

        public string ClientId
        {
            get;
            set;
        }  
    }
}