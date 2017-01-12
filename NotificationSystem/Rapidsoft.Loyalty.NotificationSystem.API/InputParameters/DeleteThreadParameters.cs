using System;

namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    public class DeleteThreadParameters
    {
        public Guid ThreadId { get; set; }

        public string ClientId { get; set; }
    }
}
