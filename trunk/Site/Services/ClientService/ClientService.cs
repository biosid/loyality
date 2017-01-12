using System;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services.BankProducts.Models;
using Vtb24.Site.Services.BankProducts.Models.Outputs;
using Vtb24.Site.Services.ClientService.Models;
using Vtb24.Site.Services.ClientTargeting;
using Vtb24.Site.Services.GeoService.Models;
using Vtb24.Site.Services.GeoService.Models.Inputs;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Processing;
using Vtb24.Site.Services.Processing.Models;
using Vtb24.Site.Services.Processing.Models.Inputs;
using Vtb24.Site.Services.Profile;
using Vtb24.Site.Services.Profile.Models;
using Vtb24.Site.Services.Profile.Models.Inputs;
using System.Linq;
using Vtb24.Site.Services.MyPointImagesService;
using Vtb24.Site.Services.VtbBankConnector;

namespace Vtb24.Site.Services.ClientService
{
    public class ClientService : IClientService, IDisposable
    {
        public ClientService(ClientPrincipal principal, CookieSessionStorage cookie, IProcessing processing,
                             IClientTargeting audience, IProfile profile, IGeoService geo,
                             IMyPointImagesService myPointImagesService, IBankProductsService bankProducts,
                             IVtbBankConnectorService bankConnector)
        {
            _principal = principal;
            _cookie = cookie;
            _processing = processing;
            _audience = audience;
            _profile = profile;
            _geo = geo;
            _myPointImagesService = myPointImagesService;
            _bankProducts = bankProducts;
            _bankConnector = bankConnector;
        }

        private readonly ClientPrincipal _principal;
        private readonly CookieSessionStorage _cookie;
        private readonly IProcessing _processing;
        private readonly IClientTargeting _audience;
        private readonly IProfile _profile;
        private readonly IGeoService _geo;
        private readonly IMyPointImagesService _myPointImagesService;
        private readonly IBankProductsService _bankProducts;
        private readonly IVtbBankConnectorService _bankConnector;

        public MechanicsContext GetMechanicsContext()
        {
            // Сегменты (ЦА)
            var clientGroups = _audience.GetClientGroups(_principal.ClientId);

            // местонахождение
            var location = GetUserLocation();
            
            return new MechanicsContext
            {
                ClientLocationKladr = location.KladrCode,
                ClientGroups = clientGroups != null && clientGroups.Length > 0 ? 
                                clientGroups.Select(g => g.Id).ToArray() : 
                                null
            };
        }

        public decimal GetBalance()
        {
            var options = new GetBalanceParameters {ClientId = _principal.ClientId};
            return _processing.GetBalance(options);
        }

        public ProcessingOperationsHistoryResult GetOperationsHistory(DateTime from, DateTime to, PagingSettings paging)
        {
            var options = new GetOperationHistoryParameters {ClientId = _principal.ClientId, From = from, To = to};
            var operationHistory = _processing.GetOperationsHistory(options, paging);

            var operations = operationHistory.ToList();

            if (operations.Count > 0)
            {
                var myPointImagesTask = _myPointImagesService.GetMyPointImages(operations.Select(h => h.Desc).ToArray());

                for (var i = 0; i < operations.Count; i++)
                {
                    var operation = operations[i];
                    operation.ImagePath = myPointImagesTask[i];
                }
            }

            return operationHistory;
        }

        public ClientProfile GetProfile()
        {
            var options = new GetProfileParameters
            {
                ClientId = _principal.ClientId
            };
            return _profile.GetProfile(options);
        }

        public ClientStatus GetStatus()
        {
            var options = new GetProfileParameters
            {
                ClientId = _principal.ClientId
            };
            return _profile.GetStatus(options);
        }

        public void SetEmail(string email)
        {
            _bankConnector.UpdateClientEmail(_principal.ClientId, email);
        }

        public string GetEmail()
        {
            var options = new GetProfileParameters
            {
                ClientId = _principal.ClientId
            };
            return _profile.GetEmail(options);
        }

        public void SetUserLocation(string kladrCode)
        {
            var city = _geo.GetLocationByKladr(kladrCode);

            if (city == null)
            {
                throw new InvalidOperationException(string.Format("Ошибка сохранения города. Город с кодом {0} не найден", kladrCode));
            }

            // шаг 1. сохраняем город в профиль
            if (_principal.IsAuthenticated)
            {
                var options = new SetLocationParameters
                {
                    ClientId = _principal.ClientId,
                    LocationKladr = city.KladrCode,
                    LocationTitle = city.Name
                };
                _profile.SetLocation(options);
            }

            // шаг2. сохраняем город cookie для текущего сеанса
            var userLocation = new UserLocation
            {
                KladrCode = kladrCode, 
                Title = city.Name
            };
            _cookie.SetLocation(userLocation);
        }

        public UserLocation GetUserLocation()
        {
            // Город присутствия
            // В порядке приоритета:
            // 1. сессионный cookie
            // 2. профиль
            // 3. GEO IP
            // 4. Город по умолчанию

            lock (_cookie)
            {
                // шаг 1. проверяем сессионный cookie
                var sessionLocation = _cookie.GetUserLocation();
                if (sessionLocation != null)
                {
                    return sessionLocation;
                }

                string kladr = null;
                string title = null;

                // шаг 2. проверяем профиль
                if (_principal.IsAuthenticated)
                {
                    var clientProfile = GetProfile();
                    var location = clientProfile.Location;
                    if (location != null)
                    {
                        kladr = clientProfile.Location.KladrCode;
                        title = clientProfile.Location.Title;
                    }
                }

                // шаг 3. проверяем по GEO IP
                if (string.IsNullOrWhiteSpace(kladr))
                {
                    var locationByIpParams = new GetLocationByIpParams
                    {
                        Ip = _principal.ClientIp,
                        Type = GeoLocationType.City
                    };
                    var city = _geo.GetLocationByUserIp(locationByIpParams);
                    if (city != null && city.IsCity())
                    {
                        kladr = city.KladrCode;
                        title = city.Name;
                    }
                }

                // шаг 4. устанавливаем город по умолчанию
                if (string.IsNullOrWhiteSpace(kladr))
                {
                    kladr = AppSettingsHelper.String("default_client_location_kladr", "7700000000000");
                    title = AppSettingsHelper.String("default_client_location_title", "Москва");
                }

                sessionLocation = new UserLocation {Title = title, KladrCode = kladr};

                return sessionLocation;
            }
        }

        public BankProductsResult GetBankProducts(PagingSettings pagingSettings)
        {
            if (!_principal.IsAuthenticated)
            {
                throw new InvalidOperationException("невозможно получить продукты банка для гостя");
            }

            var minExpirationDate = DateTime.Now.Date;

            return _bankProducts.GetProducts(_principal.ClientId, minExpirationDate, pagingSettings);
        }

        public BankProduct GetBankProduct(string id)
        {
            if (!_principal.IsAuthenticated)
            {
                throw new InvalidOperationException("невозможно получить продукт банка для гостя");
            }

            var minExpirationDate = DateTime.Now.Date;

            return _bankProducts.GetProduct(id, _principal.ClientId, minExpirationDate);
        }

        public void Dispose()
        {
            // Do nothing
        }
    }
}