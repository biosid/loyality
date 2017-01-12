using System.Collections.Generic;

namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    public class PaymentFormParameters
    {
        public string Url { get; set; }

        public string Method { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }
}
