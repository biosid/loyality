using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Profile.Models;
using Vtb24.Site.Services.Profile.Models.Inputs;

namespace Vtb24.Site.Services.Profile.Stubs
{
    public class ProfileStub : IProfile
    {
        private readonly ClientProfile _profile;

        public ProfileStub()
        {
            _profile = new ClientProfile
            {
                Email = "vasya@pupkin.ru",
                FirstName = "Васисуалий",
                LastName = "Лоханкин",
                MiddleName = "Весисуальевич",
                Gender = ClientProfileGender.Male,
                Status = ClientStatus.Activated
            };
        }

        public ClientProfile GetProfile(GetProfileParameters parameters)
        {
            return _profile;
        }

        public ClientStatus GetStatus(GetProfileParameters parameters)
        {
            return _profile.Status;
        }

        public string GetEmail(GetProfileParameters parameters)
        {
            return _profile.Email;
        }

        public void SetLocation(SetLocationParameters parameters)
        {
            _profile.Location = new UserLocation
            {
                KladrCode = parameters.LocationKladr,
                Title = parameters.LocationTitle
            };
        }
    }
}
