namespace RapidSoft.VTB24.BankConnector.Processors
{
    using RapidSoft.VTB24.BankConnector.DataModels;

    public class ClientRegistrationJoin
    {
        public ClientForRegistration RegRequest
        {
            get;
            set;
        }

        public ClientForRegistrationResponse RegResponse
        {
            get;
            set;
        }
    }
}