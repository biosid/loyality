using Vtb24.Site.Services.BankConnectorService;
using Vtb24.Site.Services.VtbBankConnector.Models.Exceptions;
using Vtb24.Site.Services.VtbBankConnector.Models.Inputs;
using CardRegistrationParameters = Vtb24.Site.Services.VtbBankConnector.Models.Outputs.CardRegistrationParameters;

namespace Vtb24.Site.Services.VtbBankConnector
{
    public class VtbBankConnector : IVtbBankConnectorService
    {
        // REVIEW: сконфигурировать в контейнере как singleton

        public bool IsCardRegistered(string clientId)
        {
            using (var service = new BankConnectorClient())
            {
                var response = service.IsCardRegistered(clientId);

                if (!response.Success || !response.Result.HasValue)
                {
                    throw new VtbBankConnectorServiceException(response.ResultCode, response.Error);
                }

                return response.Result.Value;
            }
        }

        public void RegisterCard(string clientId)
        {
            using (var service = new BankConnectorClient())
            {
                var response = service.RegisterCard(clientId);

                if (!response.Success)
                {
                    throw new VtbBankConnectorServiceException(response.ResultCode, response.Error);
                }
            }
        }

        public void RegisterClient(RegisterClientParams parameters)
        {
            using (var service = new BankConnectorClient())
            {
                var response = service.RegisterNewClient(new RegisterClientRequest
                    {
                        Email = parameters.Email,
                        FirstName = parameters.FirstName,
                        MiddleName = parameters.MiddleName,
                        LastName = parameters.LastName,
                        Gender = new MappingsToService().ToGender(parameters.Gender),
                        MobilePhone = parameters.Phone,
                        BirthDate = parameters.BirthDate
                    });

                if (!response.Success)
                {
                    if (response.ResultCode == 2)
                    {
                        throw new VtbBankConnectorClientAlreadyExistsException();
                    }

                    throw new VtbBankConnectorServiceException(response.ResultCode, response.Error);
                }
            }
        }

        public void BlockClientToDelete(string clientId)
        {
            using (var service = new BankConnectorClient())
            {
                var response = service.BlockClientToDelete(clientId);

                if (!response.Success)
                {
                    throw new VtbBankConnectorServiceException(response.ResultCode, response.Error);
                }
            }
        }

        public CardRegistrationParameters GetCardRegistrationParameters(string clientId)
        {
            using (var service = new BankConnectorClient())
            {
                var response = service.GetCardRegistrationParameters(clientId);

                if (!response.Success)
                {
                    throw new VtbBankConnectorServiceException(response.ResultCode, response.Error);
                }

                return MappingsFromService.ToCardRegistrationParameters(response.Result);
            }
        }

        public bool VerifyCardRegistration(string orderId)
        {
            using (var service = new BankConnectorClient())
            {
                var response = service.VerifyCardRegistration(orderId);

                if (!response.Success)
                {
                    throw new VtbBankConnectorServiceException(response.ResultCode, response.Error);
                }

                return response.Result;
            }
        }

        public bool IsClientOnBlocking(string clientId)
        {
            using (var service = new BankConnectorClient())
            {
                var response = service.IsClientAddedToDetachList(clientId);

                if (!response.Success)
                {
                    throw new VtbBankConnectorServiceException(response.ResultCode, response.Error);
                }

                return response.Result;
            } 
        }

        public void UpdateClientEmail(string clientId, string email)
        {
            var parameters = new UpdateClientRequest
            {
                ClientId = clientId,
                UpdateProperties = new[] { UpdateProperty.Email },
                Email = email
            };

            using (var service = new BankConnectorClient())
            {
                var response = service.UpdateClient(parameters);

                if (!response.Success)
                {
                    throw new VtbBankConnectorServiceException(response.ResultCode, response.Error);
                }
            }
        }
    }
}
