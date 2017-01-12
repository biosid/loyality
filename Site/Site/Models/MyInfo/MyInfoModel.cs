using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.MyInfoService.Models;
using Vtb24.Site.Services.Profile.Models;

namespace Vtb24.Site.Models.MyInfo
{
    public class MyInfoModel
    {
        public string Fio { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        [StringLength(250, ErrorMessage = "Превышена допустимая длина E-mail (250 символов)")]
        [SimpleEmailAddress(ErrorMessage = "Неверный формат E-mail")]
        public string Email { get; set; }

        public CustomFieldModel[] CustomFields { get; set; }

        public static MyInfoModel Map(Services.MyInfoService.Models.MyInfo info)
        {
            return new MyInfoModel
            {
                Fio = string.Format("{0} {1} {2}", info.LastName, info.FirstName, info.MiddleName),
                BirthDate = info.BirthDate,
                Gender = MapGender(info.Gender),
                PhoneNumber = PhoneFormatter.FormatPhoneNumber(info.PhoneNumber),
                Email = info.Email,
                CustomFields = info.CustomFields.Select(CustomFieldModel.Map).ToArray()
            };
        }

        public void Merge(Services.MyInfoService.Models.MyInfo info)
        {
            Fio = string.Format("{0} {1} {2}", info.LastName, info.FirstName, info.MiddleName);
            BirthDate = info.BirthDate;
            Gender = MapGender(info.Gender);
            PhoneNumber = PhoneFormatter.FormatPhoneNumber(info.PhoneNumber);

            foreach (var field in CustomFields)
            {
                field.Title = info.CustomFields.First(c => c.Id == field.Id).Title;
            }
        }

        private static string MapGender(ClientProfileGender gender)
        {
            switch (gender)
            {
                case ClientProfileGender.Female:
                    return "Женский";
                case ClientProfileGender.Male:
                    return "Мужской";
                default:
                    return "Не указан";
            }
        }
    }
}