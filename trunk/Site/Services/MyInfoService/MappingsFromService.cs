using System;
using System.Linq;
using Vtb24.Site.Services.BankConnectorService;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.MyInfoService.Models;
using Vtb24.Site.Services.Profile.Models;
using ClientProfile = Vtb24.Site.Services.BankConnectorService.ClientProfile;

namespace Vtb24.Site.Services.MyInfoService
{
    internal class MappingsFromService
    {
        public static MyInfo ToMyInfo(ClientProfile result)
        {
            return new MyInfo
            {
                FirstName = result.FirstName,
                MiddleName = result.MiddleName,
                LastName = result.LastName,
                BirthDate = result.BirthDate,
                Email = result.Email,
                PhoneNumber = result.PhoneNumber,
                Gender = ToGender(result.Gender),
                LocationKladr = result.LocationKladr,
                LocationTitle = result.LocationName,
                Status = ToStatus(result.Status),
                CustomFields = result.CustomFields
                    .Select(f => new CustomField {Id = f.Id, Title = f.Name, Value = f.Value})
                    .ToArray()
            };
        }

        private static ClientStatus ToStatus(ClientProfileStatus status)
        {
            switch (status)
            {
                case ClientProfileStatus.Activated:
                    return ClientStatus.Activated;
                case ClientProfileStatus.Blocked:
                    return ClientStatus.Blocked;
                case ClientProfileStatus.Created:
                    return ClientStatus.Created;
                case ClientProfileStatus.Deaactivated:
                    return ClientStatus.Deactivated;
                case ClientProfileStatus.Deleted:
                    return ClientStatus.Deleted;
                default:
                    return ClientStatus.Unknown;
            }
        }

        private static ClientProfileGender ToGender(Gender? gender)
        {
            switch (gender)
            {
                case Gender.Female:
                    return ClientProfileGender.Female;
                case Gender.Male:
                    return ClientProfileGender.Male;
                default:
                    return ClientProfileGender.Unknown;
            }
        }
    }
}