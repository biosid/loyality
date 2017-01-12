using System.Configuration;

namespace Vtb24.Site.Services.ClientFeedback.Models.Inputs
{
    public class FeedbackTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string) base["name"]); }
        }

        [ConfigurationProperty("displayName", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string DisplayName
        {
            get { return ((string)base["displayName"]); }
        }

        [ConfigurationProperty("email", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Email
        {
            get { return ((string)base["email"]); }
            set { base["email"] = value; }
        }

        [ConfigurationProperty("template", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Template
        {
            get { return ((string)base["template"]); }
            set { base["template"] = value; }
        }
    }
}