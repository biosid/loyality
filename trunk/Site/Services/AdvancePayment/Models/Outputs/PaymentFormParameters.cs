using System.Collections.Generic;

namespace Vtb24.Site.Services.AdvancePayment.Models.Outputs
{
    public class PaymentFormParameters
    {
        public string Url { get; set; }

        public string Method { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }
}
