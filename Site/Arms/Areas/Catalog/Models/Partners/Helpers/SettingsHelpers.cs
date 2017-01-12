using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices.Infrastructure.PartnerSettingsConfiguration;

namespace Vtb24.Arms.Catalog.Models.Partners.Helpers
{
    public static class SettingsHelpers
    {
        public const string URL_REGEXP_PATTERN = "^https?://[^/]+/.*";

        public static bool IsSettingsRequired(this SupplierEditModel model)
        {
            var tags = GetTags(model);

            return GetSettingsConfig(tags).Any();
        }

        public static bool IsSettingsRequired(this CarrierEditModel model)
        {
            return GetSettingsConfig(CarrierTags).Any();
        }

        public static Dictionary<string, string> MergeSettings(this PartnerSettingModel[] modelSettings, Dictionary<string, string> settings)
        {
            var mergedSettings = new Dictionary<string, string>(settings);

            foreach (var setting in modelSettings)
            {
                if (!string.IsNullOrEmpty(setting.Value))
                    mergedSettings[setting.Name] = setting.Value;
                else
                    mergedSettings.Remove(setting.Name);
            }

            return mergedSettings;
        }

        public static void MapSettings(this SupplierEditModel model, Dictionary<string, string> settings)
        {
            var tags = GetTags(model);

            string value;
            model.Settings = MapSettings(tags, name => settings.TryGetValue(name, out value) ? value : string.Empty).ToArray();
            model.OtherSettings = GetOtherSettings(tags, settings);
        }

        public static void MapSettings(this CarrierEditModel model, Dictionary<string, string> settings)
        {
            string value;
            model.Settings = MapSettings(CarrierTags, name => settings.TryGetValue(name, out value) ? value : string.Empty).ToArray();
            model.OtherSettings = GetOtherSettings(CarrierTags, settings);
        }

        public static void ValidateSettings(this SupplierEditModel model, Dictionary<string, string> settings, ModelStateDictionary modelState)
        {
            var tags = GetTags(model);

            model.Settings = ValidateSettings(tags, model.Settings, modelState);
            model.OtherSettings = GetOtherSettings(tags, settings);
        }

        public static void ValidateSettings(this CarrierEditModel model, Dictionary<string, string> settings, ModelStateDictionary modelState)
        {
            model.Settings = ValidateSettings(CarrierTags, model.Settings, modelState);
            model.OtherSettings = GetOtherSettings(CarrierTags, settings);
        }

        private static PartnerSettingModel[] ValidateSettings(IEnumerable<string> tags, IEnumerable<PartnerSettingModel> modelSettings, ModelStateDictionary modelState)
        {
            var settings = MapSettings(tags, name =>
                {
                    var setting = modelSettings.FirstOrDefault(s => s.Name == name);
                    return setting != null ? setting.Value : null;
                }).ToArray();

            foreach (var index in Enumerable.Range(0, settings.Length))
            {
                var setting = settings[index];

                var isRequired = IsSettingRequired(setting, settings);

                if (isRequired && string.IsNullOrWhiteSpace(setting.Value))
                {
                    modelState.AddModelError(
                        "Settings[" + index.ToString("d") + "].Value",
                        setting.RequiredErrorMessage);
                }
                else
                {
                    switch (setting.Type)
                    {
                        case PartnerSettingType.Boolean:
                            if (setting.Value != "true" && setting.Value != "false")
                                setting.Value = "false";
                            break;

                        case PartnerSettingType.Integer:
                            int result;
                            if (setting.Value != null && !int.TryParse(setting.Value, out result))
                            {
                                AddValidationError(modelState, index, setting);
                            }
                            break;

                        case PartnerSettingType.Url:
                            if (setting.Value != null && !Regex.Match(setting.Value, URL_REGEXP_PATTERN).Success)
                            {
                                AddValidationError(modelState, index, setting);
                            }
                            break;

                        case PartnerSettingType.Regexp:
                            if (setting.Value != null && !Regex.Match(setting.Value, setting.Regexp).Success)
                            {
                                AddValidationError(modelState, index, setting);
                            }
                            break;

                        case PartnerSettingType.Radio:
                            if (setting.RadioValues.FirstOrDefault(v => v.Value == setting.Value) == null)
                            {
                                AddValidationError(modelState, index, setting);
                            }
                            break;
                    }
                }
            }

            return settings;
        }

        private static void AddValidationError(ModelStateDictionary modelState, int index, PartnerSettingModel setting)
        {
            modelState.AddModelError(
                "Settings[" + index.ToString("d") + "].Value",
                setting.ValidationErrorMessage);
        }

        private static bool IsSettingRequired(PartnerSettingModel setting, IEnumerable<PartnerSettingModel> settings)
        {
            // безусловно обязательная настройка
            if (setting.IsRequired)
            {
                return true;
            }

            // безусловно необязательная настройка
            if (string.IsNullOrWhiteSpace(setting.RequiredIf))
            {
                return false;
            }

            // условно обязательная настройка
            var requiredIfSetting = settings.FirstOrDefault(s => s.Name == setting.RequiredIf);

            // условной настройки нет, значит данная настройка необязательна
            if (requiredIfSetting == null)
            {
                return false;
            }

            // получаем необходимое значение условной настройки
            var matchingValues = string.IsNullOrEmpty(setting.RequiredIfValue)
                                    ? new[] { "true"}
                                    : setting.RequiredIfValue.Split(new[] {',' }, StringSplitOptions.RemoveEmptyEntries);

            return matchingValues.Contains(requiredIfSetting.Value);
        }

        private static Dictionary<string, string> GetOtherSettings(IEnumerable<string> tags, Dictionary<string, string> settings)
        {
            var settingsNames = GetSettingsConfig(tags).Select(s => s.Name).ToArray();

            var result = new Dictionary<string, string>(settings);

            foreach (var name in settingsNames)
            {
                result.Remove(name);
            }

            return result;
        }

        private static IEnumerable<PartnerSettingModel> MapSettings(IEnumerable<string> tags, Func<string, string> getValue)
        {
            var settingsConfig = GetSettingsConfig(tags);

            return settingsConfig.Select(s => new PartnerSettingModel
            {
                Name = s.Name,
                Value = getValue(s.Name),
                Label = s.Label,
                Type = s.Type,
                Regexp = s.Regexp,
                MaxLength = s.MaxLength,
                RadioValues = s.RadioValues
                               .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(rv => rv.Split(','))
                               .Where(rv => rv.Length == 2)
                               .Select(rv => new PartnerSettingRadioValueModel
                               {
                                   Value = rv[0],
                                   Label = rv[1]
                               })
                               .ToArray(),
                IsRequired = s.Required,
                RequiredIf = s.RequiredIf,
                RequiredIfValue = s.RequiredIfValue,
                ValidationErrorMessage = s.VlidationErrorMessage,
                LengthErrorMessage = s.LengthErrorMessage,
                RequiredErrorMessage = s.RequiredErrorMessage
            });
        }

        private static IEnumerable<PartnerSettingsElement> GetSettingsConfig(IEnumerable<string> tags)
        {
            var section = (PartnerSettingsConfigSection)ConfigurationManager.GetSection("partner_settings");
            return section.SettingsCollection
                          .Cast<PartnerSettingsElement>()
                          .Where(s => s.Tags.Split(new[] { ',' }).Any(tags.Contains));
        }

        private static readonly string[] OfflineSupplierTags = new[] { "offline" };
        private static readonly string[] OnlineSupplierTags = new[] { "online" };
        private static readonly string[] DirectSupplierTags = new[] { "direct" };
        private static readonly string[] CarrierTags = new[] { "carrier" };

        private static string[] GetTags(SupplierEditModel model)
        {
            switch (model.Type)
            {
                case Types.Offline:
                    return OfflineSupplierTags;
                case Types.Online:
                    return OnlineSupplierTags;
                case Types.Direct:
                    return DirectSupplierTags;
            }
            return new string[0];
        }
    }
}
