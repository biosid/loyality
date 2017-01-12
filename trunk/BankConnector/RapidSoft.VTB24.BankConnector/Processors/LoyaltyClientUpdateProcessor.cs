namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.VTB24.BankConnector.API.Entities;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Service;
    using RapidSoft.VTB24.Site.SecurityWebApi;

    public class LoyaltyClientUpdateProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoyaltyClientUpdateProcessor));

        private readonly IUnitOfWork uow;
        private readonly ClientProfileService clientProfileServicePort;
        private readonly ISecurityWebApi securityWebApi;

        public LoyaltyClientUpdateProcessor(IUnitOfWork uow, ISecurityWebApi securityWebApi, ClientProfileService clientProfileServicePort)
        {
            this.uow = uow;
            this.securityWebApi = securityWebApi;
            this.clientProfileServicePort = clientProfileServicePort;
        }

        public LoyaltyClientUpdateStatuses UpdateClientPhoneNumber(UpdateClientPhoneNumberRequest request)
        {
            var update = new LoyaltyClientUpdate { ClientId = request.ClientId, MobilePhone = request.PhoneNumber, ChangedBy = request.UserId };

            var updateProperties = new[] { UpdateProperty.MobilePhone };

            return UpdateClient(update, null, updateProperties, request.UserId);
        }

        public LoyaltyClientUpdateStatuses UpdateClient(UpdateClientRequest request)
        {
            var update = new LoyaltyClientUpdate { ClientId = request.ClientId, Email = request.Email };

            return UpdateClient(update, request.CustomFields, request.UpdateProperties, null);
        }

        private static GetClientProfileFullResponseTypeClientProfilePhone ExtractPhone(string clientId, GetClientProfileFullResponseTypeClientProfile profile)
        {
            if (profile.Phones.Length > 1)
            {
                var message = string.Format("clientId={0}: клиент имеет более одного телефона", clientId);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.Profile_MoreThenOnePhoneFound, message);
            }

            if (profile.Phones.Length == 0)
            {
                var message = string.Format("clientId={0}: клиент должен иметь один телефон", clientId);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(LoyaltyClientUpdateStatuses.Profile_PhoneNotFound, message);
            }

            return profile.Phones.First();
        }

        private static void PopulateLoyaltyClientUpdate(
            LoyaltyClientUpdate update,
            UpdateProperty[] updateProperties,
            GetClientProfileFullResponseTypeClientProfile profile,
            GetClientProfileFullResponseTypeClientProfilePhone phone)
        {
            update.FirstName = profile.FirstName;
            update.LastName = profile.LastName;
            update.MiddleName = profile.MiddleName;
            update.Gender = (Gender?)Convert.ToInt32(profile.Gender);
            update.BirthDate = profile.BirthDate;

            if (!updateProperties.Contains(UpdateProperty.MobilePhone))
            {
                update.MobilePhone = phone.PhoneNumber;
            }
            else
            {
                // NOTE: Так как надо будет обновить телефон, то надо запомнить его ид.
                update.MobilePhoneId = phone.PhoneId;
            }

            if (!updateProperties.Contains(UpdateProperty.Email))
            {
                update.Email = profile.Email;
            }
        }

        private static UpdateClientProfileRequestTypeClientProfile BuildClientProfileUpdateRequest(LoyaltyClientUpdate update, IEnumerable<UpdateProperty> updateProperties)
        {
            var request = new UpdateClientProfileRequestTypeClientProfile
            {
                ClientId = update.ClientId
            };

            foreach (var updateProperty in updateProperties)
            {
                switch (updateProperty)
                {
                    case UpdateProperty.MobilePhone:
                        request.Phones = new[]
                                         {
                                             update.MobilePhone.ToPhoneUpdateField(update.MobilePhoneId.Value, true)
                                         };
                        break;

                    case UpdateProperty.Email:
                        request.Email = update.Email.ToStringUpdateField();
                        break;
                }
            }

            return request;
        }

        private LoyaltyClientUpdateStatuses UpdateClient(LoyaltyClientUpdate update, Dictionary<int, string> customFieldsValues, UpdateProperty[] updateProperties, string userId)
        {
            if (updateProperties == null)
            {
                updateProperties = new UpdateProperty[0];
            }

            Log.Info(string.Format("Обновление информации о клиенте ({0}): начало", string.Join(", ", updateProperties)));

            LoyaltyClientUpdateStatuses updateStatus;

            try
            {
                this.uow.LoyaltyClientUpdateRepository.Add(update);
                this.uow.Save();

                var profile = this.GetCurrentProfile(update.ClientId);

                var phone = ExtractPhone(update.ClientId, profile);

                PopulateLoyaltyClientUpdate(update, updateProperties, profile, phone);
                this.uow.Save();

                if (updateProperties.Contains(UpdateProperty.MobilePhone))
                {
                    // NOTE: Теперь обновляем данные в серкурити
                    this.UpdateClientSecurity(phone.PhoneNumber, update, userId);
                }

                if (updateProperties.Contains(UpdateProperty.MobilePhone) || updateProperties.Contains(UpdateProperty.Email))
                {
                    // NOTE: Теперь обновим даные в профиле
                    this.UpdateClientProfile(update, updateProperties);
                }

                if (updateProperties.Contains(UpdateProperty.CustomFields))
                {
                    // NOTE: Теперь обновим данный в дополнительных полях
                    this.UpdateClientProfileCustomFieldsValues(update.ClientId, customFieldsValues);
                }

                updateStatus = LoyaltyClientUpdateStatuses.Success;
            }
            catch (LoyaltyClientUpdateProcessorException ex)
            {
                updateStatus = (LoyaltyClientUpdateStatuses)ex.ResultCode;
                Log.Info(string.Format("Обновление информации о клиенте: ошибка ({0})", updateStatus));
            }
            catch (Exception ex)
            {
                updateStatus = LoyaltyClientUpdateStatuses.UnknowError;
                Log.Info(string.Format("Обновление информации о клиенте: неизвестная ошибка ({0})", ex.GetType()));
            }

            try
            {
                update.UpdateStatus = updateStatus;
                this.uow.Save();

                return updateStatus;
            }
            catch (Exception ex)
            {
                updateStatus = LoyaltyClientUpdateStatuses.UnknowError;
                Log.Info(string.Format("Обновление информации о клиенте: ошибка при сохранении статуса ({0})", ex.GetType()));
            }

            return updateStatus;
        }

        private GetClientProfileFullResponseTypeClientProfile GetCurrentProfile(string clientId)
        {
            var getClientProfileFullRequestType = new GetClientProfileFullRequestType { ClientId = clientId };
            var getClientProfileFullRequest = new GetClientProfileFullRequest { Request = getClientProfileFullRequestType };
            var getProfileResponse = this.clientProfileServicePort.GetClientProfileFull(getClientProfileFullRequest);

            if (getProfileResponse == null)
            {
                var message = string.Format(
                    "clientId={0}: clientProfile.GetClientProfileFull getProfileResponse is null", clientId);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.GetProfile_InvalidResponse, message);
            }

            if (getProfileResponse.Response == null)
            {
                var message = string.Format(
                    "clientId={0}: clientProfile.GetClientProfileFull getProfileResponse.Response is null", clientId);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.GetProfile_InvalidResponse, message);
            }

            if (getProfileResponse.Response.StatusCode == 1 || (getProfileResponse.Response.StatusCode < 0 || getProfileResponse.Response.StatusCode > 2))
            {
                var message = string.Format("clientId={0}: clientProfile.GetClientProfileFull unknown StatusCode: {1}", clientId, getProfileResponse.Response.StatusCode);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.GetProfile_ErrorResponse, message);
            }

            if (getProfileResponse.Response.StatusCode == 2)
            {
                var message = string.Format(
                    "clientId={0}:  clientProfile.GetClientProfileFull StatusCode: {1}",
                    clientId,
                    getProfileResponse.Response.StatusCode);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.GetProfile_ProfileNotFound, message);
            }

            if (getProfileResponse.Response.ClientProfile == null)
            {
                var message =
                    string.Format(
                        "clientId={0}: clientProfile.GetClientProfileFull getProfileResponse.Response.ClientProfile is null",
                        clientId);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.GetProfile_InvalidResponse, message);
            }

            return getProfileResponse.Response.ClientProfile;
        }

        private void UpdateClientSecurity(string phoneNumber, LoyaltyClientUpdate update, string userId)
        {
            Log.Info("Обновление номера телефона клиента в безопасности: начало");

            var requestOptions = new ChangePhoneNumberOptions
                                 {
                                     Login = phoneNumber,
                                     NewPhoneNumber = update.MobilePhone,
                                     ChangedBy = userId
                                 };
            var response = this.securityWebApi.ChangeUserPhoneNumber(requestOptions);

            if (response == null)
            {
                var message = string.Format(
                    "clientId={0}: security.ChangeUserPhoneNumber response is null", update.ClientId);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.ChangePhone_InvalidResponse, message);
            }

            if (response.Status != ChangeUserPhoneNumberStatus.Changed)
            {
                var message = string.Format(
                    "clientId={0}: security.ChangeUserPhoneNumber error StatusCode: {1}", update.ClientId, response.Status);
                Log.Error(message);

                LoyaltyClientUpdateStatuses resultCode;

                switch (response.Status)
                {
                    case ChangeUserPhoneNumberStatus.UserNotFound:
                        resultCode = LoyaltyClientUpdateStatuses.ChangePhone_UserNotFound;
                        break;
                    case ChangeUserPhoneNumberStatus.PhoneNumberIsUsedByAnotherUser:
                        resultCode = LoyaltyClientUpdateStatuses.ChangePhone_PhoneIsAlreadyUsed;
                        break;
                    default:
                        resultCode = LoyaltyClientUpdateStatuses.ChangePhone_ErrorResponse;
                        break;
                }

                throw new LoyaltyClientUpdateProcessorException(resultCode, message);
            }

            Log.Info("Обновление номера телефона клиента в безопасности: выполнено");
        }

        private void UpdateClientProfile(LoyaltyClientUpdate update, IEnumerable<UpdateProperty> updateProperties)
        {
            Log.Info("Обновление профиля клиента: начало");

            var request = BuildClientProfileUpdateRequest(update, updateProperties);

            var updateClientProfileRequestType = new UpdateClientProfileRequestType { ClientProfile = request };
            var updateClientProfileRequest = new UpdateClientProfileRequest(updateClientProfileRequestType);
            var updateResponse = this.clientProfileServicePort.UpdateClientProfile(updateClientProfileRequest);

            if (updateResponse == null)
            {
                var message = string.Format(
                    "clientId={0}: clientProfile.UpdateClientProfile updateResponse is null", update.ClientId);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.UpdateProfile_InvalidResponse, message);
            }

            if (updateResponse.Response == null)
            {
                var message =
                    string.Format(
                        "clientId={0}: clientProfile.UpdateClientProfile updateResponse.Response is null", update.ClientId);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.UpdateProfile_InvalidResponse, message);
            }

            if (updateResponse.Response.StatusCode == 1)
            {
                var message = string.Format(
                    "clientId={0}: clientProfile.UpdateClientProfile error StatusCode: {1}",
                    update.ClientId,
                    updateResponse.Response.StatusCode);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.UpdateProfile_ErrorResponse, message);
            }

            if (updateResponse.Response.StatusCode == 2)
            {
                var message = string.Format(
                    "clientId={0}: clientProfile.UpdateClientProfile ClientNotFound StatusCode: {1}",
                    update.ClientId,
                    updateResponse.Response.StatusCode);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.UpdateProfile_ClientNotFound, message);
            }

            if (updateResponse.Response.StatusCode < 0 || updateResponse.Response.StatusCode > 2)
            {
                var message = string.Format(
                    "clientId={0}: clientProfile.UpdateClientProfile wrong StatusCode:{1}",
                    update.ClientId,
                    updateResponse.Response.StatusCode);
                Log.Error(message);
                throw new LoyaltyClientUpdateProcessorException(
                    LoyaltyClientUpdateStatuses.UpdateProfile_UnknownStatus, message);
            }

            Log.Info("Обновление профиля клиента: выполнено");
        }

        private void UpdateClientProfileCustomFieldsValues(string clientId, Dictionary<int, string> customFieldsValues)
        {
            Log.Info("Обновление доп. данных клиента: начало");

            foreach (var value in customFieldsValues)
            {
                var customFieldExists = this.uow.ProfileCustomFieldsRepository.GetAll().Any(f => f.Id == value.Key);

                if (!customFieldExists)
                {
                    var message = string.Format("дополнительное поле с id={0} не найдено", value.Key);
                    throw new LoyaltyClientUpdateProcessorException(
                        LoyaltyClientUpdateStatuses.UpdateCustomFields_FieldNotFound, message);
                }

                var customFieldValue =
                    this.uow.ProfileCustomFieldsValuesRepository.GetAll()
                        .SingleOrDefault(f => f.ClientId == clientId && f.FieldId == value.Key);

                if (customFieldValue == null)
                {
                    customFieldValue = new ProfileCustomFieldsValue
                                            {
                                                ClientId = clientId,
                                                FieldId = value.Key,
                                                Value = value.Value
                                            };
                    this.uow.ProfileCustomFieldsValuesRepository.Add(customFieldValue);
                }
                else
                {
                    customFieldValue.Value = value.Value;
                }
            }

            this.uow.Save();

            Log.Info("Обновление доп. данных клиента: выполнено");
        }

        public class LoyaltyClientUpdateProcessorException : Exception
        {
            public LoyaltyClientUpdateProcessorException(LoyaltyClientUpdateStatuses updateStatus, string description)
                : base(description)
            {
                this.ResultCode = (int)updateStatus;
            }

            public LoyaltyClientUpdateProcessorException(LoyaltyClientUpdateStatuses updateStatus, string descriptionFormat, params object[] args)
                : base(string.Format(descriptionFormat, args))
            {
                this.ResultCode = (int)updateStatus;
            }

            public LoyaltyClientUpdateProcessorException(int resultCode, string description)
                : base(description)
            {
                this.ResultCode = resultCode;
            }

            public int ResultCode { get; set; }
        }
    }
}
