using System;
using System.Collections.Generic;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.Profile.Models
{
    public class ClientProfile
    {
        public string ClientId { get; set; }
        
        public string FirstName { get; set; } // WTF: Почему необязательный?

        public string MiddleName { get; set; } // WTF: Почему необязательный?

        public string LastName { get; set; } // WTF: Почему необязательный?

        public DateTime? BirthDate { get; set; }

        public ClientProfileGender Gender { get; set; } // WTF: Почему необязательный?

        public string Email { get; set; }

        public UserLocation Location { get; set; }

        public ClientStatus Status { get; set; }

        public List<ClientProfilePhone> Phones
        {
            get { return _phones ?? (_phones = new List<ClientProfilePhone>()); }
            set { if (_phones == null) _phones = value; }
        }

        public List<ClientProfileDocument> Documents
        {
            get { return _documents ?? (_documents = new List<ClientProfileDocument>()); }
            set { if (_documents == null) _documents = value; }            
        }

        public string GetFullName()
        {
            return UseRealName()
                       ? string.Format("{0} {1} {2}", LastName, FirstName, MiddleName).Trim()
                       : "Уважаемый клиент";
        }

        public string GetPoliteName()
        {
            return UseRealName()
                       ? string.Format("{0} {1}", FirstName, MiddleName).Trim()
                       : "Уважаемый клиент";
        }

        public string GetPoliteNameWithAppeal()
        {
            if (!UseRealName())
                return "Уважаемый клиент";

            switch (Gender)
            {
                case ClientProfileGender.Male:
                    return string.Format("Уважаемый {0} {1}", FirstName, MiddleName).Trim();
                case ClientProfileGender.Female:
                    return string.Format("Уважаемая {0} {1}", FirstName, MiddleName).Trim();;
                default:
                    return string.Format("{0} {1}", FirstName, MiddleName).Trim();
            }
        }

        private List<ClientProfilePhone> _phones;

        private List<ClientProfileDocument> _documents;

        private bool UseRealName()
        {
            return Status == ClientStatus.Activated;
        }
    }
}
