namespace RapidSoft.Loaylty.PartnersConnector.TestPartner.Controllers
{
    using System;

    public class AssertException : ApplicationException
    {
        public AssertException(string message)
            : base(message)
        {
        }
    }
}