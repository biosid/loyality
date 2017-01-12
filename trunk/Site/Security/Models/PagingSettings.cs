namespace Vtb24.Site.Security.Models
{
    public class SecurityPagingSettings
    {
        public SecurityPagingSettings()
        {
            Take = 100;
            Skip = 0;
        }

        public SecurityPagingSettings(int take, int skip)
        {
            Take = take;
            Skip = skip;
        }

        public int Take { get; set; }

        public int Skip { get; set; }
    }
}