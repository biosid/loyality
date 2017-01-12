namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using RapidSoft.VTB24.BankConnector.Processors;

    public class ClientProfileServiceStub : StubBase, ClientProfileService
    {
        public static List<string> lockedIds = new List<string>();

        #region ClientProfileService Members

        public BatchActivateClientsResponse BatchActivateClients(BatchActivateClientsRequest request)
        {
            var response = new BatchActivateClientsResponseType
                           {
                               StatusCode = 0, 
                           };

            response.ClientActivationResults =
                request.Request.ClientActivationFacts.Select(
                    this.ReturnActivationRes).ToArray();

            return new BatchActivateClientsResponse(response);
        }

        private BatchActivateClientsResponseTypeClientActivationResult ReturnActivationRes(BatchActivateClientsRequestTypeClientActivationFact x)
        {
            if (x.ClientExternalId == "canNotBeActivated")
            {
                return new BatchActivateClientsResponseTypeClientActivationResult
                       {
                           StatusCode = 1
                       };
            }

            return new BatchActivateClientsResponseTypeClientActivationResult
                   {
                       ClientExternalId = x.ClientExternalId, 
                       ClientId = x.ClientExternalId, 
                       Error = this.GetStubDescription(), 
                       StatusCode = 0
                   };
        }

        public Task<BatchActivateClientsResponse> BatchActivateClientsAsync(BatchActivateClientsRequest request)
        {
            throw new NotImplementedException();
        }

        public BatchCreateClientsResponse BatchCreateClients(BatchCreateClientsRequest request)
        {
            var responseClientProfile = new BatchCreateClientsResponseType
                                        {
                                            StatusCode = 0
                                        };
            var clientProfileStubList =
                request.Request.ClientRegistrationFacts.Select(
                    x => new BatchCreateClientsResponseTypeClientRegistrationResult
                         {
                             ClientExternalId = x.ClientExternalId, 
                             ClientId = x.ClientId, 
                             Error = this.GetStubDescription(), 
                             StatusCode = 0
                         });
            responseClientProfile.ClientRegistrationResults = clientProfileStubList.ToArray();
            return new BatchCreateClientsResponse(responseClientProfile);
        }

        public Task<BatchCreateClientsResponse> BatchCreateClientsAsync(BatchCreateClientsRequest request)
        {
            throw new NotImplementedException();
        }

        public BatchDeactivateClientsResponse BatchDeactivateClients(BatchDeactivateClientsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BatchDeactivateClientsResponse> BatchDeactivateClientsAsync(BatchDeactivateClientsRequest request)
        {
            throw new NotImplementedException();
        }

        public BatchLockClientsResponse BatchLockClients(BatchLockClientsRequest request)
        {
            var response = new BatchLockClientsResponseType();
            response.StatusCode = 0;
            response.ClientLockResults =
                request.Request.LockFacts.Select(
                    x => new BatchLockClientsResponseTypeClientLockResult
                         {
                             ClientId = x.ClientExternalId, 
                             ClientExternalId = x.ClientExternalId, 
                             Error = this.GetStubDescription(), 
                             StatusCode = lockedIds.Contains(x.ClientExternalId) ? 1 : 0, 
                         }).ToArray();
            lockedIds.AddRange(request.Request.LockFacts.Select(x => x.ClientExternalId));
            lockedIds = lockedIds.Distinct().ToList();
            return new BatchLockClientsResponse(response);
        }

        public Task<BatchLockClientsResponse> BatchLockClientsAsync(BatchLockClientsRequest request)
        {
            throw new NotImplementedException();
        }

        public BatchDeleteClientsResponse BatchDeleteClients(BatchDeleteClientsRequest request)
        {
            var clientProfileResponse = new BatchDeleteClientsResponseType();
            clientProfileResponse.StatusCode = 0;
            clientProfileResponse.ClientDeletionResults =
                request.Request.DeletionFacts.Select(
                    x => new BatchDeleteClientsResponseTypeClientDeletionResult
                         {
                             ClientExternalId = x.ClientExternalId, 
                             ClientId = x.ClientExternalId, 
                             StatusCode = 0, 
                             Error = this.GetStubDescription(), 
                         }).ToArray();
            return new BatchDeleteClientsResponse(clientProfileResponse);
        }

        public Task<BatchDeleteClientsResponse> BatchDeleteClientsAsync(BatchDeleteClientsRequest request)
        {
            throw new NotImplementedException();
        }

        public GetClientProfileFullResponse GetClientProfileFull(GetClientProfileFullRequest request)
        {
            if (request.Request.ClientId == "createdClient")
            {
                return new GetClientProfileFullResponse
                       {
                           Response = new GetClientProfileFullResponseType
                                      {
                                          StatusCode = 0, 
                                          ClientProfile = new GetClientProfileFullResponseTypeClientProfile
                                                          {
                                                              ClientStatus = (int)ClientProfileClientStatusCodes.Created
                                                          }
                                      }
                       };
            }

            if (request.Request.ClientId == "profileNotExists")
            {
                return new GetClientProfileFullResponse
                       {
                           Response = new GetClientProfileFullResponseType
                                      {
                                          StatusCode = 2
                                      }
                       };
            }

            if (request.Request.ClientId == "profileFail")
            {
                return new GetClientProfileFullResponse
                       {
                           Response = new GetClientProfileFullResponseType
                                      {
                                          StatusCode = 1
                                      }
                       };
            }

            const string DetachedClientsPrefix = "detached_client_id";
            const string NotExistsClientsPrefix = "wrong_client_id";
            const string NonActivatedClientsPrefix = "not_activated";
            var response = new GetClientProfileFullResponseType();
            response.Error = this.GetStubDescription();

            if (!request.Request.ClientId.StartsWith(NotExistsClientsPrefix))
            {
                response.StatusCode = 0;
                response.ClientProfile = new GetClientProfileFullResponseTypeClientProfile
                                         {
                                             BirthDate = DateTime.Now.AddYears(-45).AddDays(-105), 
                                             ClientId = request.Request.ClientId, 
                                             ClientLocationKladr = "77", 
                                             ClientLocationName = "77", 
                                             Documents = new GetClientProfileFullResponseTypeClientProfileDocument[0], 
                                             Addresses = new GetClientProfileFullResponseTypeClientProfileAddress[0], 
                                             Email = "test@test.test", 
                                             FirstName = "First", 
                                             LastName = "LastName", 
                                             MiddleName = "MiddleName", 
                                         };
                response.ClientProfile.Phones = new[]
                                                {
                                                    new GetClientProfileFullResponseTypeClientProfilePhone
                                                    {
                                                        IsPrimary = true, 
                                                        PhoneId = 1, 
                                                        PhoneNumber = "79271234567", 
                                                        PhoneType = "1"
                                                    }
                                                };
                if (request.Request.ClientId.StartsWith(DetachedClientsPrefix))
                {
                    response.ClientProfile.ClientStatus = (int)ClientProfileClientStatusCodes.Blocked;
                }
                else
                {
                    if (request.Request.ClientId.StartsWith(NonActivatedClientsPrefix))
                    {
                        response.ClientProfile.ClientStatus = (int)ClientProfileClientStatusCodes.Created;
                    }
                    else
                    {
                        response.ClientProfile.ClientStatus = (int)ClientProfileClientStatusCodes.Activated;
                    }
                }
            }
            else
            {
                response.StatusCode = 2;
            }

            return new GetClientProfileFullResponse(response);
        }

        public Task<GetClientProfileFullResponse> GetClientProfileFullAsync(GetClientProfileFullRequest request)
        {
            throw new NotImplementedException();
        }

        public UpdateClientProfileResponse UpdateClientProfile(UpdateClientProfileRequest request)
        {
            const string NotExistsClientsPrefix = "wrong_client_id";
            const string errorUpdateId = "error_update_id";
            var response = new UpdateClientProfileResponseType();
            response.Error = this.GetStubDescription();
            var newValues = request.Request.ClientProfile;

            if (newValues.ClientId == "Success")
            {
                response.StatusCode = 0;
            }
            else if (newValues.ClientId.StartsWith(errorUpdateId))
            {
                response.StatusCode = 1;
            }
            else if (newValues.ClientId.StartsWith(NotExistsClientsPrefix) ||
                     newValues.ClientId == "profileNotExists")
            {
                response.StatusCode = 2;
            }
            else
            {
                response.StatusCode = 0;
            }

            return new UpdateClientProfileResponse(response);
        }

        public Task<UpdateClientProfileResponse> UpdateClientProfileAsync(UpdateClientProfileRequest request)
        {
            throw new NotImplementedException();
        }

        public GetClientDeliveryAddressesResponse GetClientDeliveryAddresses(GetClientDeliveryAddressesRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<GetClientDeliveryAddressesResponse> GetClientDeliveryAddressesAsync(
            GetClientDeliveryAddressesRequest request)
        {
            throw new NotImplementedException();
        }

        public AddClientDeliveryAddressResponse AddClientDeliveryAddress(AddClientDeliveryAddressRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AddClientDeliveryAddressResponse> AddClientDeliveryAddressAsync(
            AddClientDeliveryAddressRequest request)
        {
            throw new NotImplementedException();
        }

        public SetClientLocationResponse SetClientLocation(SetClientLocationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SetClientLocationResponse> SetClientLocationAsync(SetClientLocationRequest request)
        {
            throw new NotImplementedException();
        }

        public BatchGetClientsByExternalIdResponse BatchGetClientsByExternalId(
            BatchGetClientsByExternalIdRequest request)
        {
            var result = new BatchGetClientsByExternalIdResponseType
                         {
                             StatusCode = 0, 
                         };
            result.ResClientsIdentifiers =
                request.Request.ReqClientsIdentifiers.Select(
                    x => new BatchGetClientsByExternalIdResponseTypeResClientIdentifier
                         {
                             ClientExternalId = x.ClientExternalId, 
                             ClientId = x.ClientExternalId, 
                             Error = this.GetStubDescription(), 
                             StatusCode = 0, 
                         }).ToArray();
            return new BatchGetClientsByExternalIdResponse(result);
        }

        public Task<BatchGetClientsByExternalIdResponse> BatchGetClientsByExternalIdAsync(
            BatchGetClientsByExternalIdRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        public static void ResetStub()
        {
            lockedIds = new List<string>();
        }
    }
}