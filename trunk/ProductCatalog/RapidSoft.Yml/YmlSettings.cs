namespace RapidSoft.YML
{
    using System.Configuration;

    public static class YmlSettings
    {
        private const string DefaultYmlParamWeightName = "Вес";
        private const string DefaultYmlParamBasePriceName = "baseprice";
        private const string DefaultYmlParamIsDeliveredByEmailName = "Доставка по e-mail";

        public static string YmlParamWeightName
        {
            get
            {
                string ymlParamWeightName = ConfigurationManager.AppSettings["YmlParamWeightName"];

                return ymlParamWeightName ?? DefaultYmlParamWeightName;
            }
        }

        public static string YmlParamBasePriceName
        {
            get
            {
                string ymlParamBasePriceName = ConfigurationManager.AppSettings["YmlParamBasePriceName"];

                return ymlParamBasePriceName ?? DefaultYmlParamBasePriceName;
            }
        }

        public static string YmlParamIsDeliveredByEmailName
        {
            get
            {
                string ymlParamIsDeliveredByEmailName = ConfigurationManager.AppSettings["YmlParamIsDeliveredByEmailName"];

                return ymlParamIsDeliveredByEmailName ?? DefaultYmlParamIsDeliveredByEmailName;
            }
        }
    }
}
