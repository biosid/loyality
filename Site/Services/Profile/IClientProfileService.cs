using Vtb24.Site.Services.Profile.Models;
using Vtb24.Site.Services.Profile.Models.Inputs;

namespace Vtb24.Site.Services.Profile
{
    public interface IProfile
    {
        ClientProfile GetProfile(GetProfileParameters parameters);

        ClientStatus GetStatus(GetProfileParameters parameters);

        string GetEmail(GetProfileParameters parameters);

        void SetLocation(SetLocationParameters parameters);
    }
}
