using System;
using Vtb24.Site.Services.ClientTargeting.Models;
using Vtb24.Site.Services.MyInfoService.Models;
using Vtb24.Site.Services.Profile.Models;

namespace Vtb24.Arms.AdminServices.SecurityManagement.Models
{
    public class Client
    {
        #region Информация от сервиса безопасности клиентского сайта

        public int SecurityUserId { get; set; }

        public string Login { get; set; }

        public string ClientId { get; set; }

        public bool IsUserDisabled { get; set; }

        public bool IsPasswordSet { get; set; }

        public DateTime RegistrationDate { get; set; }

        #endregion

        #region Информация о группах

        public ClientGroup[] Groups { get; set; }

        #endregion

        #region Информация из профиля

        public ClientProfileStatus ProfileStatus { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public ClientStatus Status { get; set; }

        public DateTime? BirthDate { get; set; }

        public ClientProfileGender Gender { get; set; }

        public string Email { get; set; }

        public string LocationTitle { get; set; }

        #endregion

        #region Информация из процессинга

        public decimal? Balance { get; set; }

        #endregion

        #region Дополнительные поля

        public CustomField[] CustomFields { get; set; }

        #endregion
    }
}
