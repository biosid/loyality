namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Service;
    using RapidSoft.VTB24.Site.SecurityWebApi;

    public class ClientLoginBankUpdatesProcessor : ProcessorBase
    {
        private ClientProfileService clientProfileServicePort;

        private ISecurityWebApi securityWebApi;

        private Guid sessionId;

        public ClientLoginBankUpdatesProcessor(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            ISecurityWebApi securityWebApi,
            ClientProfileService clientProfileServicePort)
            : base(logger, uow)
        {
            this.securityWebApi = securityWebApi;
            this.clientProfileServicePort = clientProfileServicePort;
        }

        public void Execute()
        {
            this.sessionId = Guid.Parse(this.Logger.EtlSessionId);
            var processedRecords = 0;
            var clientLoginBankUpdatesAll = this.Uow.ClientLoginBankUpdatesRepository.GetBySessionId(this.sessionId).OrderBy(u => u.SeqId);

            this.Logger.Info("Начало изменения логинов пользователей из банка");

            ClientLoginBankUpdate[] batch;
            while ((batch = clientLoginBankUpdatesAll.Skip(processedRecords).Take(ConfigHelper.BatchSize).ToArray()).Any())
            {
                foreach (var clientLoginBankUpdate in batch)
                {
                    var mobilePhone = ConfigHelper.DefaultCountryCode + clientLoginBankUpdate.MobilePhone;
                    var clientId = clientLoginBankUpdate.ClientId;
                    var isValidPhone = Regex.IsMatch(mobilePhone, ConfigHelper.ValidPhoneRegExp); // TODO: вынести в метод

                    if (!isValidPhone)
                    {
                        this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неверный формат телефона");
                        continue;
                    }

                    var login = this.GetUserPhoneByClientId(clientId);
                    var profile = login == null ? null : this.GetCurrentProfile(clientId);
                    var profilePhone = profile == null ? null : this.ExtractPhone(clientId, profile);

                    if (login != null && profile != null)
                    {
                        var isProfileUpdated = false; 
                        var isSecurityUpdated = false;

                        // обновление профиля
                        this.Logger.InfoFormat("Обновление номера телефона клиента {0} в профиле: начало", clientId);
                        var profileResponse = this.ChangeUserPhoneNumberInProfile(clientId, profilePhone.PhoneId, mobilePhone);
                        isProfileUpdated = HandleChangeUserPhoneNumberResponse(profileResponse, clientId);

                        // обновление безопасности
                        if (isProfileUpdated)
                        {
                            this.Logger.InfoFormat("Обновление номера телефона клиента {0} в безопасности: начало", clientId);
                            var securityResponse = this.ChangeUserPhoneNumberInSecurity(login, mobilePhone);
                            isSecurityUpdated = this.HandleChangeUserPhoneNumberResponse(securityResponse, clientId);
                        }

                        if (isProfileUpdated && isSecurityUpdated)
                        {
                            // если обе системы обновились
                            this.AddResponse(clientId, BankClientLoginUpdateStatus.Success);
                        }
                        else if (isProfileUpdated)
                        {
                            // откатываем изменения в профиле, если безопасность не обновилась
                            this.ChangeUserPhoneNumberInProfile(clientId, profilePhone.PhoneId, login);
                        }
                    }
                }

                this.Uow.Save();

                processedRecords += batch.Length;
            }

            this.Logger.Info("Изменение логинов пользователей завершено");
        }

        private ChangeUserPhoneNumberResult ChangeUserPhoneNumberInSecurity(string login, string mobilePhone)
        {
            var options = new ChangePhoneNumberOptions { Login = login, NewPhoneNumber = mobilePhone, ChangedBy = ConfigHelper.VtbSystemUser };
            var response = this.securityWebApi.ChangeUserPhoneNumber(options);
            return response;
        }

        private bool HandleChangeUserPhoneNumberResponse(ChangeUserPhoneNumberResult response, string clientId)
        {
            if (response == null)
            {
                this.Logger.ErrorFormat("Обновление номера телефона клиента {0} в безопасности: response == null", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return false;
            }

            if (response.Status != ChangeUserPhoneNumberStatus.Changed)
            {
                switch (response.Status)
                {
                    case ChangeUserPhoneNumberStatus.UserNotFound:
                        this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Клиент не найден");
                        this.Logger.ErrorFormat("Обновление номера телефона клиента {0} в безопасности: Клиент не найден", clientId);
                        break;
                    case ChangeUserPhoneNumberStatus.PhoneNumberIsUsedByAnotherUser:
                        this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Данный номер телефона уже используется другим клиентом");
                        this.Logger.ErrorFormat(
                            "Обновление номера телефона клиента {0} в безопасности: Данный номер телефона уже используется другим клиентом",
                            clientId);
                        break;
                    default:
                        this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");
                        this.Logger.ErrorFormat("Обновление номера телефона клиента {0} в безопасности: Неизвестная ошибка", clientId);
                        break;
                }

                return false;
            }

            this.Logger.Info("Обновление номера телефона клиента в безопасности: выполнено");

            return true;
        }

        private UpdateClientProfileResponse ChangeUserPhoneNumberInProfile(string clientId, int profilePhoneId, string mobilePhone)
        {
            var response =
                this.clientProfileServicePort.UpdateClientProfile(
                    new UpdateClientProfileRequest
                    {
                        Request =
                            new UpdateClientProfileRequestType
                            {
                                ClientProfile =
                                    new UpdateClientProfileRequestTypeClientProfile
                                    {
                                        ClientId = clientId,
                                        Phones = new[] { mobilePhone.ToPhoneUpdateField(profilePhoneId, true) }
                                    }
                            }
                    });

            return response;
        }

        private bool HandleChangeUserPhoneNumberResponse(UpdateClientProfileResponse response, string clientId)
        {
            if (response == null)
            {
                this.Logger.ErrorFormat("Обновление номера телефона клиента {0} в профиле: response == null", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return false;
            }

            if (response.Response == null)
            {
                this.Logger.ErrorFormat("Обновление номера телефона клиента {0} в профиле: response.Response == null", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return false;
            }

            if (response.Response.StatusCode == 1)
            {
                this.Logger.ErrorFormat("Обновление номера телефона клиента {0} в профиле: Код ошибки: {1}", clientId, response.Response.StatusCode);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return false;
            }

            if (response.Response.StatusCode == 2)
            {
                this.Logger.ErrorFormat("Обновление номера телефона клиента {0} в профиле: Клиент не найден", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Клиент не найден");

                return false;
            }

            if (response.Response.StatusCode == 8)
            {
                this.Logger.ErrorFormat("Обновление номера телефона клиента {0} в профиле: Неверный формат телефона", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неверный формат телефона");

                return false;
            }

            if (response.Response.StatusCode < 0 || (response.Response.StatusCode > 2 && response.Response.StatusCode != 8))
            {
                this.Logger.ErrorFormat("Обновление номера телефона клиента {0} в профиле: Неизвестный статус код {1}", clientId, response.Response.StatusCode);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return false;
            }

            this.Logger.Info("Обновление номера телефона клиента в профиле: выполнено");

            return true;
        }

        private GetClientProfileFullResponseTypeClientProfilePhone ExtractPhone(
            string clientId,
            GetClientProfileFullResponseTypeClientProfile profile)
        {
            this.Logger.InfoFormat("Получение телефона из профиля клиента {0}: начало", clientId);

            if (profile.Phones.Length > 1)
            {
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");
                this.Logger.ErrorFormat("Получение телефона из профиля клиента {0}: У клиента более одного телефона", clientId);
            }

            if (profile.Phones.Length == 0)
            {
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");
                this.Logger.ErrorFormat("Получение телефона из профиля клиента {0}: У клиента нет установленных телефонов", clientId);
            }

            this.Logger.InfoFormat("Получение телефона из профиля клиента {0}: выполнено", clientId);

            return profile.Phones.First();
        }

        private GetClientProfileFullResponseTypeClientProfile GetCurrentProfile(string clientId)
        {
            this.Logger.InfoFormat("Получение профиля для клиента {0}: начало", clientId);

            var getClientProfileFullRequestType = new GetClientProfileFullRequestType { ClientId = clientId };
            var getClientProfileFullRequest = new GetClientProfileFullRequest { Request = getClientProfileFullRequestType };
            var getProfileResponse = this.clientProfileServicePort.GetClientProfileFull(getClientProfileFullRequest);

            if (getProfileResponse == null)
            {
                this.Logger.ErrorFormat("Получение профиля для клиента {0}: getProfileResponse == null", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return null;
            }

            if (getProfileResponse.Response == null)
            {
                this.Logger.ErrorFormat("Получение профиля для клиента {0}: getProfileResponse.Response == null", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return null;
            }

            if (getProfileResponse.Response.StatusCode == 1 ||
                (getProfileResponse.Response.StatusCode < 0 || getProfileResponse.Response.StatusCode > 2))
            {
                this.Logger.ErrorFormat("Получение профиля для клиента {0}: Неизвестный статус код {1}", clientId, getProfileResponse.Response.StatusCode);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return null;
            }

            if (getProfileResponse.Response.StatusCode == 2)
            {
                this.Logger.ErrorFormat("Получение профиля для клиента {0}: Профиль не найден", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Клиент не найден");

                return null;
            }

            if (getProfileResponse.Response.ClientProfile == null)
            {
                this.Logger.ErrorFormat("Получение профиля для клиента {0}: getProfileResponse.Response.ClientProfile == null", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return null;
            }

            this.Logger.InfoFormat("Получение профиля для клиента {0}: выполнено", clientId);

            return getProfileResponse.Response.ClientProfile;
        }

        private string GetUserPhoneByClientId(string clientId)
        {
            this.Logger.InfoFormat("Получение номера телефона для клиента в Безопасности {0}: начало", clientId);

            var users = this.securityWebApi.BatchResolveUsersByClientId(new[] { clientId });
            if (users == null || !users.Any())
            {
                this.Logger.ErrorFormat("Получение номера телефона для клиента в Безопасности {0}: неизвестный clientId", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return null;
            }

            if (users.Count > 1)
            {
                this.Logger.ErrorFormat("Получение номера телефона для клиента в Безопасности {0}: сервис вернул более 1 телефона", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return null;
            }

            var user = users.Single();
            if (user.Value == null)
            {
                this.Logger.ErrorFormat("Получение номера телефона для клиента в Безопасности {0}: безопасность вернула невалидный ответ", clientId);
                this.AddResponse(clientId, BankClientLoginUpdateStatus.Error, "Неизвестная ошибка");

                return null;
            }

            this.Logger.InfoFormat("Получение номера телефона для клиента {0} в Безопасности: выполнено", clientId);

            return user.Value.PhoneNumber;
        }

        private void AddResponse(string clientId, BankClientLoginUpdateStatus status, string message = null)
        {
            this.Uow.ClientLoginBankUpdatesResponseRepository.Add(
                new ClientLoginBankUpdatesResponse
                {
                    ClientId = clientId, 
                    EtlSessionId = this.sessionId, 
                    Message = message, 
                    Status = (int)status
                });
        }
    }
}