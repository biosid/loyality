using System.Linq;
using Vtb24.Site.Security;
using Vtb24.Site.Security.SecurityTokenService.Models.Exceptions;
using Vtb24.Site.Security.SecurityTokenService.Models.Inputs;
using Vtb24.Site.SecurityWebServices.PublicProfile;
using Vtb24.Site.Services.ClientService.Models;
using Vtb24.Site.Services.ClientTargeting;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.MechanicsService;
using Vtb24.Site.Services.Processing;
using Vtb24.Site.Services.Processing.Models.Inputs;
using Vtb24.Site.Services.Profile;
using Vtb24.Site.Services.Profile.Models;
using Vtb24.Site.Services.Profile.Models.Inputs;
using GetPublicProfileParameters = Vtb24.Site.SecurityWebServices.PublicProfile.Inputs.GetPublicProfileParameters;
using GetPublicProfileResult = Vtb24.Site.SecurityWebServices.PublicProfile.Outputs.GetPublicProfileResult;

namespace Vtb24.Site.SecurityWebServices
{
    public class PublicProfileWebApi : IPublicProfileWebApi
    {
        private const byte SUCCESS_STATUS = 0;
        private const byte ERROR_STATUS = 1;
        private const string NAME_LANG = "RU";
        private const string DOMAIN_NAME = "Расчёт цены вознаграждения онлайн партнёра";
        private const string KLADR = "7700000000000";
        private const string CITY = "Москва";

        public PublicProfileWebApi(ISecurityTokenService tokens, IProcessing processing, IProfile profile, IClientTargeting targeting)
        {
            _tokens = tokens;
            _processing = processing;
            _profile = profile;
            _targeting = targeting;
        }

        private readonly ISecurityTokenService _tokens;
        private readonly IProcessing _processing;
        private readonly IProfile _profile;
        private readonly IClientTargeting _targeting;

        #region API

        public GetPublicProfileResult Get(GetPublicProfileParameters parameters)
        {
            parameters.ValidateAndThrow();

            var validateParams = new ValidateSecurityTokenParameters
            {
                SecurityToken = parameters.SecurityToken
            };

            var validateResults = _tokens.Validate(validateParams);

            if (!validateResults.IsValid || validateResults.ExternalId != parameters.ShopId)
            {
                throw new InvalidSecurityTokenException(parameters.SecurityToken);
            }

            var result = new GetPublicProfileResult();

            var profile = _profile.GetProfile(new GetProfileParameters {ClientId = validateResults.PrincipalId});
            if (profile != null)
            {
                result.FirstName = profile.FirstName;
                result.MiddleName = profile.MiddleName;
                result.LastName = profile.LastName;

                result.Email = profile.Email;
                result.NameLang = AppSettingsHelper.String("default_client_culture", NAME_LANG);

                if (profile.Location != null)
                {
                    result.City = profile.Location.Title;
                }
                else
                {
                    result.City = AppSettingsHelper.String("default_client_location_name", CITY);
                }
            }
            else
            {
                result.Status = ERROR_STATUS;
                return result;
            }

            result.Balance = _processing.GetBalance(new GetBalanceParameters { ClientId = validateResults.PrincipalId });

            var context = GetMechanicsContext(profile);
            using (var mechanics = new MechanicsServiceClient())
            {
                var factors = mechanics.CalculateFactors(AppSettingsHelper.String("mechanics_domain", DOMAIN_NAME), context);
                result.Rate = factors.BaseMultiplicationFactor * factors.MultiplicationFactor;
                result.Delta = factors.BaseAdditionFactor * factors.MultiplicationFactor + factors.AdditionFactor;
            }

            result.Status = SUCCESS_STATUS;
            return result;
        }

        public string Echo(string message)
        {
            return string.Format("Echo: {0}", message);
        }

        #endregion

        private MechanicsContext GetMechanicsContext(ClientProfile profile)
        {
            var kladr = profile.Location != null && !string.IsNullOrWhiteSpace(profile.Location.KladrCode)
                ? profile.Location.KladrCode
                : AppSettingsHelper.String("default_client_location_kladr", KLADR);

            var clientGroups = _targeting.GetClientGroups(profile.ClientId);

            var context = new MechanicsContext
            {
                ClientGroups = clientGroups != null && clientGroups.Length > 0 ?
                                clientGroups.Select(g => g.Id).ToArray() :
                                null,
                ClientLocationKladr = kladr
            };

            return context;
        }
    }
}
