namespace RapidSoft.VTB24.BankConnector.API.Exceptions
{
    using System;

    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException(string message)
            : base(message)
        {
        }
    }
}
