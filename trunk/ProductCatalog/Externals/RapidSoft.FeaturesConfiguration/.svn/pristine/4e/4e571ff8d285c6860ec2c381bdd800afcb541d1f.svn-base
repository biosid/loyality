namespace Vtb24.Common.Configuration
{
    public sealed class FeaturesConfiguration
    {
        private static readonly FeaturesConfiguration FeaturesConfigurationInstance = new FeaturesConfiguration();

        private bool site505EnableActionPrice = true;
        private bool site500EnableARMActionPrice = true;
        private bool gen25EnableSmsPasswordReset = true;
        private bool site520Formatting = false;
        private bool site122EnableRecommendedProducts = true;
        private bool gen1070EnableEmailMessageNotification = true;

        private FeaturesConfiguration()
        {
        }

        public static FeaturesConfiguration Instance
        {
            get
            {
                return FeaturesConfigurationInstance;
            }
        }

        public bool Site505EnableActionPrice
        {
            get { return this.site505EnableActionPrice; }
            set { this.site505EnableActionPrice = value; }
        }

        public bool Site500EnableARMActionPrice
        {
            get { return this.site500EnableARMActionPrice; }
            set { this.site500EnableARMActionPrice = value; }
        }

        public bool Gen25EnableSmsPasswordReset
        {
            get { return this.gen25EnableSmsPasswordReset; }
            set { this.gen25EnableSmsPasswordReset = value; }
        }

        public bool Site520Formatting
        {
            get { return this.site520Formatting; }
            set { this.site520Formatting = value; }
        }

        public bool Site122EnableRecommendedProducts
        {
            get { return this.site122EnableRecommendedProducts; }
            set { this.site122EnableRecommendedProducts = value; }
        }

        public bool Gen1070EnableEmailMessageNotification
        {
            get { return this.gen1070EnableEmailMessageNotification; }
            set { this.gen1070EnableEmailMessageNotification = value; }
        }
    }
}
