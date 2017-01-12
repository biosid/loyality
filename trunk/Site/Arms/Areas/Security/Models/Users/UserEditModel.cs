using System;
using Vtb24.Arms.AdminServices.SecurityManagement.Models;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Profile.Models;
using System.Linq;

namespace Vtb24.Arms.Security.Models.Users
{
    public class UserEditModel
    {
        public string Login { get; set; }

        public string SiteAccess { get; set; }

        public string PasswordStatus { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string ClientStatus { get; set; }

        public string Segments { get; set; }

        public string LocationTitle { get; set; }

        public bool IsUserDisabled { get; set; }

        public bool IsUserOnBlocking { get; set; }

        public ClientProfileStatus ProfileStatus { get; set; }

        public decimal? Balance { get; set; }

        public bool ShowPersonalInfo { get; set; }

        public string FullName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public UserCustomFiledModel[] CustomFields { get; set; }

        public UserEditPermissionsModel Permissions { get; set; }

        public PhoneNumberHistoryItemModel[] PhoneChangeHistory { get; set; }

        public int TotalPhoneChangeHistoryItems { get; set; }

        public int PhoneHistoryPage { get; set; }

        public static UserEditModel Map(Client original)
        {
            var model = new UserEditModel
            {
                Login = PhoneFormatter.FormatPhoneNumber(original.Login),
                SiteAccess = original.IsUserDisabled ? "Заблокирован" : "Предоставляется",
                PasswordStatus = original.IsPasswordSet ? "Установлен" : "Временный",
                RegistrationDate = original.RegistrationDate,
                ClientStatus = ToClientStatusText(original.Status),
                Segments = original.Groups != null && original.Groups.Length > 0
                                ? string.Join(", ", original.Groups.Where(g => g.IsSegment).Select(g => g.Name))
                                : "-нет-",
                LocationTitle = original.LocationTitle,
                IsUserDisabled = original.IsUserDisabled,
                ProfileStatus = original.ProfileStatus,
                Balance = original.Balance,
                // профиль не хранит данные неактивированных пользователей, поэтому скрываем
                ShowPersonalInfo = original.Status != Vtb24.Site.Services.Profile.Models.ClientStatus.Created
            };

            if (model.ShowPersonalInfo)
            {
                model.FullName = string.Format("{0} {1} {2}", original.LastName, original.FirstName, original.MiddleName).Trim();
                model.Email = original.Email;
                model.BirthDate = original.BirthDate;
                model.Gender = ToClientGenderText(original.Gender);
                model.CustomFields = original.CustomFields
                    .Select(f => new UserCustomFiledModel {Title = f.Title, Value = f.Value})
                    .ToArray();
            }

            return model;
        }

        private static string ToClientStatusText(ClientStatus status)
        {
            switch (status)
            {
                case Vtb24.Site.Services.Profile.Models.ClientStatus.Activated:
                    return "Активирован";
                case Vtb24.Site.Services.Profile.Models.ClientStatus.Created:
                    return "Создан";
                case Vtb24.Site.Services.Profile.Models.ClientStatus.Deactivated:
                    return "Деактивирован";
                case Vtb24.Site.Services.Profile.Models.ClientStatus.Blocked:
                    return "Блокирован";
                case Vtb24.Site.Services.Profile.Models.ClientStatus.Deleted:
                    return "Удален";
                default:
                    return "Неизвестно";
            }
        }

        private static string ToClientGenderText(ClientProfileGender gender)
        {
            switch (gender)
            {
                case ClientProfileGender.Male:
                    return "Мужской";
                case ClientProfileGender.Female:
                    return "Женский";
                default:
                    return "Не указан";
            }
        }
    }
}
