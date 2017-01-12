namespace Rapidsoft.Loyalty.NotificationSystem.Configuration
{
    using System.Configuration;

    public static class ServiceConfiguration
    {
        public static bool UseStub
        {
            get
            {
                return ConfigurationManager.AppSettings["UseStub"] == null ||
            bool.Parse(ConfigurationManager.AppSettings["UseStub"]);
            }
        }
    }
}