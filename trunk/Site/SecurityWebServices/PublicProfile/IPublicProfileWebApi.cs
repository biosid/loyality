using System.ServiceModel;
using Vtb24.Site.SecurityWebServices.PublicProfile.Inputs;
using Vtb24.Site.SecurityWebServices.PublicProfile.Outputs;

namespace Vtb24.Site.SecurityWebServices.PublicProfile
{
    [ServiceContract]
    public interface IPublicProfileWebApi
    {
        [OperationContract]
        GetPublicProfileResult Get(GetPublicProfileParameters parameters);

        [OperationContract]
        string Echo(string message);
    }
}
