using Vtb24.Arms.AdminServices.Infrastructure.PartnerSettingsConfiguration;

namespace Vtb24.Arms.Catalog.Models.Partners
{
    public class PartnerSettingModel
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Label { get; set; }

        public PartnerSettingType Type { get; set; }

        public string Regexp { get; set; }

        public int MaxLength { get; set; }

        public PartnerSettingRadioValueModel[] RadioValues { get; set; }

        public bool IsRequired { get; set; }

        public string RequiredIf { get; set; }

        public string RequiredIfValue { get; set; }

        public string ValidationErrorMessage { get; set; }

        public string LengthErrorMessage { get; set; }

        public string RequiredErrorMessage { get; set; }
    }
}
