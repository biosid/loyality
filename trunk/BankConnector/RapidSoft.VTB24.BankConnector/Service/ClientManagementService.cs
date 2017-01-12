namespace RapidSoft.VTB24.BankConnector.Service
{
	using System;
	using System.Linq;

	using RapidSoft.Etl.Logging;
	using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
	using RapidSoft.VTB24.BankConnector.Acquiring;
	using RapidSoft.VTB24.BankConnector.API;
	using RapidSoft.VTB24.BankConnector.API.Entities;
	using RapidSoft.VTB24.BankConnector.API.Exceptions;
	using RapidSoft.VTB24.BankConnector.DataModels;
	using RapidSoft.VTB24.BankConnector.DataSource;
	using RapidSoft.VTB24.BankConnector.EtlLogger;
	using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
	using RapidSoft.VTB24.BankConnector.Processors;
	using RapidSoft.VTB24.Site.SecurityWebApi;

	public class ClientManagementService : IClientManagementService
	{
		private readonly IUnitOfWork uow;
		private readonly ClientDetachProcessor clientDetachProcessor;
        private readonly ClientProfileService clientProfileService;
        private readonly ISecurityWebApi security;

		private readonly LoyaltyClientUpdateProcessor loyaltyClientUpdateProcessor;

		public ClientManagementService(IUnitOfWork uow, ClientDetachProcessor clientDetachProcessor, ClientProfileService clientProfileService, LoyaltyClientUpdateProcessor loyaltyClientUpdateProcessor, ISecurityWebApi securityService)
		{
			this.uow = uow;
			this.clientDetachProcessor = clientDetachProcessor;
		    this.clientProfileService = clientProfileService;
			this.loyaltyClientUpdateProcessor = loyaltyClientUpdateProcessor;
			this.security = securityService;
		}

		public SimpleBankConnectorResponse RegisterNewClient(RegisterClientRequest request)
		{
			if (request == null)
			{
				throw new ArgumentException("Request parameter can not be null.", "request");
			}

			request.Validate();

			var client = new ClientForRegistration
						 {
							 ClientId = Guid.NewGuid().ToString(),
							 Email = request.Email,
							 FirstName = request.FirstName,
							 LastName = request.LastName,
							 MiddleName = request.MiddleName,
							 MobilePhone = request.MobilePhone,
							 Gender = (int)request.Gender,
							 BirthDate = request.BirthDate
						 };

			// Если пользователь есть в системе, то не регистрируем
			var existsAnswer = CheckClientExists(request.MobilePhone);
			if (existsAnswer != null)
			{
				return existsAnswer;
			}

			this.uow.ClientForRegistrationRepository.Add(client);
			this.uow.Save();

			return new SimpleBankConnectorResponse();
		}

		public SimpleBankConnectorResponse BlockClientToDelete(string clientId)
		{
			clientDetachProcessor.AddDetachRequest(clientId);
			return new SimpleBankConnectorResponse();
		}

		public GenericBankConnectorResponse<bool> IsClientAddedToDetachList(string clientId)
		{
			var clientForDetach = this.uow.ClientForDeletionRepository.GetAll().FirstOrDefault(x => x.ExternalClientId == clientId);
			return new GenericBankConnectorResponse<bool>(clientForDetach != null);
		}

		public GenericBankConnectorResponse<CardRegistrationParameters> GetCardRegistrationParameters(string clientId)
		{
			var client = new UnitellerAuthInfo
							 {
								 ShopId = ConfigHelper.UnitellerRegisterShopId,
								 CustomerId = clientId,
								 OrderId = Guid.NewGuid().ToString(),
								 Password = ConfigHelper.UnitellerRegisterPassword,
								 Subtotal = 1,
								 ReturnUrl = ConfigHelper.UnitellerPaymentReturnUrl
							 };

			var result = new CardRegistrationParameters
							 {
								 CustomerId = clientId,
								 OrderId = client.OrderId,
								 ShopId = client.ShopId,
								 Sum = client.SubtotalString,
								 Signature = new UnitellerSignatureCreator().GetSignatureHash(client)
							 };

			return new GenericBankConnectorResponse<CardRegistrationParameters>(result);
		}

		public GenericBankConnectorResponse<bool> VerifyCardRegistration(string orderId)
		{
			var result = false;

			var response = new UnitellerProvider().GetRegistrationStatus(orderId).ResponseCode;

			if (!string.IsNullOrEmpty(response) && response == OperationStatusResponseCode.AS000)
			{
				result = true;
			}

			return new GenericBankConnectorResponse<bool>(result);
		}

		public SimpleBankConnectorResponse UpdateClient(UpdateClientRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}

			request.Validate();

			var updateStatus = this.loyaltyClientUpdateProcessor.UpdateClient(request);

            if (updateStatus != LoyaltyClientUpdateStatuses.Success)
            {
                return new SimpleBankConnectorResponse(
                    (int)ExceptionType.GeneralException, false, string.Format("ошибка обновления ({0})", updateStatus));
            }

		    return new SimpleBankConnectorResponse();
		}

        public GenericBankConnectorResponse<ClientProfile> GetClientProfile(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("clientId");
            }

            try
            {
                var profile = this.GetClientCurrentProfile(clientId);

                var clientCustomFields = GetClientCustomFieldsValues(clientId);

                var phoneNumber = profile.Phones != null && profile.Phones.Length == 1
                                      ? profile.Phones[0].PhoneNumber
                                      : null;

                var clientProfile = new ClientProfile
                                    {
                                        Status = ToClientProfileStatus(profile.ClientStatus),
                                        FirstName = profile.FirstName,
                                        LastName = profile.LastName,
                                        MiddleName = profile.MiddleName,
                                        Email = profile.Email,
                                        Gender = ToGender(profile.Gender),
                                        BirthDate = profile.BirthDate,
                                        PhoneNumber = phoneNumber,
                                        LocationKladr = profile.ClientLocationKladr,
                                        LocationName = profile.ClientLocationName,
                                        CustomFields = clientCustomFields
                                    };

                return new GenericBankConnectorResponse<ClientProfile>(clientProfile);
            }
            catch (Exception ex)
            {
                return new GenericBankConnectorResponse<ClientProfile>(ex);
            }
        }

	    public SimpleBankConnectorResponse SaveOrderAttempt(string clientId)
	    {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("clientId");
            }

            try
	        {
                uow.OrderAttemptsRepository.Save(clientId);
                uow.Save();

                return new SimpleBankConnectorResponse();
	        }
	        catch (Exception ex)
	        {
	            return new SimpleBankConnectorResponse(ex);
	        }
        }

	    public SimpleBankConnectorResponse ClearOrderAttempt(string clientId)
	    {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("clientId");
            }

            try
            {
                uow.OrderAttemptsRepository.Clear(clientId);
                uow.Save();

                return new SimpleBankConnectorResponse();
            }
            catch (Exception ex)
            {
                return new SimpleBankConnectorResponse(ex);
            }
        }

	    private static ClientProfileStatus ToClientProfileStatus(int? status)
        {
            if (!status.HasValue)
            {
                return ClientProfileStatus.Unknown;
            }

            switch ((ClientProfileClientStatusCodes)status.Value)
            {
                case ClientProfileClientStatusCodes.Activated:
                    return ClientProfileStatus.Activated;
                case ClientProfileClientStatusCodes.Created:
                    return ClientProfileStatus.Created;
                case ClientProfileClientStatusCodes.Deactivated:
                    return ClientProfileStatus.Deaactivated;
                case ClientProfileClientStatusCodes.Blocked:
                    return ClientProfileStatus.Blocked;
                case ClientProfileClientStatusCodes.Deleted:
                    return ClientProfileStatus.Deleted;
                default:
                    return ClientProfileStatus.Unknown;
            }
        }

        private static Gender? ToGender(string strGender)
        {
            int intGender;
            if (!int.TryParse(strGender, out intGender))
            {
                return null;
            }

            var gender = (Gender)intGender;

            return Enum.IsDefined(typeof(Gender), gender) ? gender : (Gender?)null;
        }

		private SimpleBankConnectorResponse CheckClientExists(string mobilePhone)
		{
			var clients = security.BatchResolveUsersByPhone(new[] { mobilePhone });
			var existsResponse = new SimpleBankConnectorResponse
									 {
										 Success = false,
										 ResultCode = (int)ExceptionType.ClientAlreadyExistsException,
									 };

			if (clients != null && clients.ContainsKey(mobilePhone) && clients[mobilePhone] != null)
			{
				existsResponse.Error = "Клиент c таким телефоном уже есть в системе";
				return existsResponse;
			}

			// NOTE: банк всегда работает с телефонами без кода страны, на регистрацию же приходит телефон с кодом страны
			var phoneWithoutCountryCode = mobilePhone.Substring(1);
			var clientRegistrationFromBank =
				this.uow.ClientForBankRegistrationRepository.GetAll()
					.FirstOrDefault(x => x.MobilePhone == phoneWithoutCountryCode && !x.IsDeleted);
			if (clientRegistrationFromBank != null)
			{
				existsResponse.Error = "Заявка на регистрацию клиента с таким телефоном была прислана со стороны банка";
				return existsResponse;
			}

			var previousRegistration =
				this.uow.ClientForRegistrationRepository.GetAll()
					.FirstOrDefault(x => x.MobilePhone == mobilePhone && !x.IsDeleted);

			if (previousRegistration != null)
			{
				existsResponse.Error = "Выполнение регистрации невозможно. Данный номер уже зарегистрирован или ожидает подтверждения регистрации со стороны банка.";
				return existsResponse;
			}

			var detachPhone =
				this.uow.ClientForDeletionRepository.GetAll()
					.FirstOrDefault(x => x.MobilePhone == mobilePhone && x.SendEtlSessionId == null);

			if (detachPhone != null)
			{
				existsResponse.Error = "Выполнение регистрации невозможно. Данный номер содержится в реестре на удаление.";
				return existsResponse;
			}

			return null;
		}

        private GetClientProfileFullResponseTypeClientProfile GetClientCurrentProfile(string clientId)
        {
            var request = new GetClientProfileFullRequest
                            {
                                Request =
                                    new GetClientProfileFullRequestType
                                    {
                                        ClientId =
                                            clientId
                                    }
                            };
            var response = this.clientProfileService.GetClientProfileFull(request);

            return response.Response.ClientProfile;
        }

        private ClientProfileCustomFieldValue[] GetClientCustomFieldsValues(string clientId)
        {
            var customFields = this.uow.ProfileCustomFieldsRepository.GetAllFieldsInOrder();

            var customFieldsValues = this.uow.ProfileCustomFieldsValuesRepository.GetByClientId(clientId);

            return
                customFields.Select(
                    f =>
                    new ClientProfileCustomFieldValue
                    {
                        Id = f.Id,
                        Name = f.Name,
                        Value =
                            customFieldsValues.ContainsKey(f.Id)
                                ? customFieldsValues[f.Id]
                                : null
                    }).ToArray();
        }
	}
}