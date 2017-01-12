using Vtb24.Site.Services.ClientProfileService;
using System.Linq;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Profile.Models;

namespace Vtb24.Site.Services.Profile
{
    internal static class MappingsFromService
    {
        public static ClientProfileDocumentType ToClientProfileDocumentType(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return ClientProfileDocumentType.Unknown;
            }

            switch (original)
            {
                case "PASSPORT":
                    return ClientProfileDocumentType.Passport;
                default:
                    return ClientProfileDocumentType.Unknown;
            }
        }

        public static ClientProfileGender ToClientProfileGender(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return ClientProfileGender.Unknown;
            }

            switch (original)
            {
                case "1":
                    return ClientProfileGender.Male;
                case "2":
                    return ClientProfileGender.Female;
                default:
                    return ClientProfileGender.Unknown;
            }
        }

        public static ClientProfilePhoneType ToClientProfilePhoneType(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return ClientProfilePhoneType.Unknown;
            }

            switch (original)
            {
                case "MOBILE":
                    return ClientProfilePhoneType.Mobile;
                case "HOME":
                    return ClientProfilePhoneType.Home;
                case "OTHER":
                    return ClientProfilePhoneType.Other;
                default:
                    return ClientProfilePhoneType.Unknown;                
            }
        }

        public static ClientProfilePhone ToClientProfilePhone(GetClientProfileFullResponseTypeClientProfilePhone original)
        {
            if (original == null)
            {
                return null;
            }

            return new ClientProfilePhone
            {
                Number = original.PhoneNumber,
                Type = ToClientProfilePhoneType(original.PhoneType)
            };
        }

        public static ClientProfileDocument ToClientProfileDocument(GetClientProfileFullResponseTypeClientProfileDocument original)
        {
            if (original == null)
            {
                return null;
            }

            return new ClientProfileDocument
            {
                IsPrimary = original.IsPrimary,
                Series = original.DocumentSeries,
                Number = original.DocumentNumber,
                Type = ToClientProfileDocumentType(original.DocumentTypeCode),
                IssueDate = original.IssueDate,
                IssuerCode = original.IssuerCode,
                IssuerName = original.IssuerName
            };
        }

        public static ClientStatus ToClientStatus(int? original)
        {
            if (!original.HasValue)
                return ClientStatus.Unknown;

            switch (original.Value)
            {
                case 1:
                    return ClientStatus.Activated;
                case 20:
                    return ClientStatus.Created;
                case 25:
                    return ClientStatus.Deactivated;
                case 30:
                    return ClientStatus.Blocked;
                case 40:
                    return ClientStatus.Deleted;
                default:
                    return ClientStatus.Unknown;
            }
        }

        public static ClientProfile ToClientProfile(GetClientProfileFullResponseTypeClientProfile original)
        {
            if (original == null)
            {
                return null;
            }

            return new ClientProfile
            {
                BirthDate = original.BirthDate,
                ClientId = original.ClientId,
                /*Documents = original.Documents
                                    .Select(ToClientProfileDocument)
                                    .ToList(),*/
                Email = original.Email,
                Gender = ToClientProfileGender(original.Gender),
                FirstName = original.FirstName,
                LastName = original.LastName,
                MiddleName = original.MiddleName,
                Location = new UserLocation
                {
                    KladrCode = original.ClientLocationKladr,
                    Title = original.ClientLocationName
                },
                Phones = original.Phones
                                 .Select(ToClientProfilePhone)
                                 .ToList(),
                Status = ToClientStatus(original.ClientStatus)
            };
        }
    }
}
