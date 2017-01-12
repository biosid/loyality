namespace RapidSoft.VTB24.BankConnector.API.Exceptions
{
    using System;

    public class InconsistentProfileException : Exception
    {
        public InconsistentProfileException(string message)
            : base(message)
        {
        }
    }
}
