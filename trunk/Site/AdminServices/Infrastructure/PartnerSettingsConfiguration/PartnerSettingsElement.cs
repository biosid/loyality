using System.Configuration;

namespace Vtb24.Arms.AdminServices.Infrastructure.PartnerSettingsConfiguration
{
    public class PartnerSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string) base["name"]; }
        }

        [ConfigurationProperty("label", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Label
        {
            get { return (string) base["label"]; }
        }

        [ConfigurationProperty("type", DefaultValue = PartnerSettingType.String, IsKey = false, IsRequired = true)]
        public PartnerSettingType Type
        {
            get { return (PartnerSettingType) base["type"]; }
        }

        [ConfigurationProperty("regexp", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Regexp
        {
            get { return (string) base["regexp"]; }
        }

        [ConfigurationProperty("max-length", DefaultValue = 0, IsKey = false, IsRequired = true)]
        public int MaxLength
        {
            get { return (int) base["max-length"]; }
        }

        [ConfigurationProperty("radio-values", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string RadioValues
        {
            get { return (string)base["radio-values"]; }
        }

        [ConfigurationProperty("required", DefaultValue = false, IsKey = false, IsRequired = true)]
        public bool Required
        {
            get { return (bool) base["required"]; }
        }

        [ConfigurationProperty("required-if", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string RequiredIf
        {
            get { return (string) base["required-if"]; }
        }

        [ConfigurationProperty("required-if-value", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string RequiredIfValue
        {
            get { return (string)base["required-if-value"]; }
        }

        [ConfigurationProperty("validation-error", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string VlidationErrorMessage
        {
            get { return (string)base["validation-error"]; }
        }

        [ConfigurationProperty("length-error", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string LengthErrorMessage
        {
            get { return (string)base["length-error"]; }
        }

        [ConfigurationProperty("required-error", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string RequiredErrorMessage
        {
            get { return (string)base["required-error"]; }
        }

        [ConfigurationProperty("tags", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Tags
        {
            get { return (string) base["tags"]; }
        }
    }
}
