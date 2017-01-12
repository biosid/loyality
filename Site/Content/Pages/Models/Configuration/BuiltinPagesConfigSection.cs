using System.Configuration;

namespace Vtb24.Site.Content.Pages.Models.Configuration
{
    public class BuiltinPagesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("pages")]
        public BuiltinPagesCollection PagesCollection
        {
            get { return (BuiltinPagesCollection) base["pages"]; }
        }
    }
}
