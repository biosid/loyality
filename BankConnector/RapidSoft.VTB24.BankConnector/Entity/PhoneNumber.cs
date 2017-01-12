namespace RapidSoft.VTB24.BankConnector.Entity
{
    public class PhoneNumber
    {
        public string CountryCode
        {
            get;
            set;
        }

        public string CityCode
        {
            get;
            set;
        }

        public string LocalNumber
        {
            get;
            set;
        }

        public string GlobalNumber
        {
            get
            {
                return string.Format("{0}{1}{2}", this.CountryCode, this.CityCode, this.LocalNumber);
            }
        }

        public static PhoneNumber FromServicePhone(RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.PhoneNumber serviceFormat)
        {
            return new PhoneNumber
                   {
                       CityCode = serviceFormat.CityCode, 
                       CountryCode = serviceFormat.CountryCode, 
                       LocalNumber = serviceFormat.LocalNumber, 
                   };
        }
    }
}