using System.Collections.Generic;

namespace Vtb24.Site.Models.Advance
{
    public class PaymentFormModel
    {
        public string Url { get; set; }

        public string Method { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }
}
