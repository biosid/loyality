namespace RapidSoft.VTB24.BankConnector.Service
{
    using System;
    using System.Linq;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Wcf;
    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.VTB24.BankConnector.API;
    using RapidSoft.VTB24.BankConnector.API.Entities;
    using RapidSoft.VTB24.BankConnector.API.Exceptions;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Processors;

    public class AdminClientManagementService : IAdminClientManagementService
    {
        private static readonly string[] PermissionsToChangePhoneNumber = new[]
                                                                          {
                                                                              ArmPermissions.ARMSecurityLogin,
                                                                              ArmPermissions.SecurityClients,
                                                                              ArmPermissions.ClientsChangePhone
                                                                          };

        private static readonly string[] PermissionsToManageCustomFields = new[]
                                                                           {
                                                                               ArmPermissions.ARMSecurityLogin,
                                                                               ArmPermissions.SecurityCustomFields
                                                                           };

        private readonly IUnitOfWork uow;
        private readonly LoyaltyClientUpdateProcessor loyaltyClientUpdateProcessor;

        private readonly ILog logger = LogManager.GetLogger(typeof(AdminClientManagementService));

        public AdminClientManagementService(IUnitOfWork uow, LoyaltyClientUpdateProcessor loyaltyClientUpdateProcessor)
        {
            this.uow = uow;
            this.loyaltyClientUpdateProcessor = loyaltyClientUpdateProcessor;
        }

        public SimpleBankConnectorResponse UpdateClientPhoneNumber(UpdateClientPhoneNumberRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            request.Validate();

            if (!this.CheckPermissionsToChangePhoneNumber(request.UserId))
            {
                return new SimpleBankConnectorResponse((int)ExceptionType.AccessDenied, false, "недостаточно прав");
            }

            var updateStatus = this.loyaltyClientUpdateProcessor.UpdateClientPhoneNumber(request);

            switch (updateStatus)
            {
                case LoyaltyClientUpdateStatuses.Success:
                    return new SimpleBankConnectorResponse();

                case LoyaltyClientUpdateStatuses.ChangePhone_UserNotFound:
                    return new SimpleBankConnectorResponse(
                        (int)ExceptionType.SecurityUserNotFound,
                        false,
                        "пользователь не найден в подсистеме безопасности");

                case LoyaltyClientUpdateStatuses.ChangePhone_PhoneIsAlreadyUsed:
                    return new SimpleBankConnectorResponse(
                        (int)ExceptionType.SecurityPhoneAlreadyExists, false, "номер телефона уже используется");

                default:
                    return new SimpleBankConnectorResponse((int)ExceptionType.GeneralException, false, string.Format("ошибка обновления ({0})", updateStatus));
            }
        }

        public SimpleBankConnectorResponse UpdateClientEmail(UpdateClientEmailRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            request.Validate();

            if (!this.CheckPermissionsToChangePhoneNumber(request.UserId))
            {
                return new SimpleBankConnectorResponse((int)ExceptionType.AccessDenied, false, "недостаточно прав");
            }

            var updateStatus = this.loyaltyClientUpdateProcessor.UpdateClient(new UpdateClientRequest
            {
                ClientId = request.ClientId,
                Email = request.Email,
                UpdateProperties = new[] { UpdateProperty.Email }
            });

            switch (updateStatus)
            {
                case LoyaltyClientUpdateStatuses.Success:
                    return new SimpleBankConnectorResponse();

                case LoyaltyClientUpdateStatuses.ChangePhone_UserNotFound:
                    return new SimpleBankConnectorResponse(
                        (int)ExceptionType.SecurityUserNotFound,
                        false,
                        "пользователь не найден в подсистеме безопасности");

                default:
                    return new SimpleBankConnectorResponse((int)ExceptionType.GeneralException, false, string.Format("ошибка обновления ({0})", updateStatus));
            }
        }

        public GenericBankConnectorResponse<ClientProfileCustomField[]> GetAllProfileCustomFields(string userId)
        {
            if (!this.CheckPermissionsToManageCustomFields(userId))
            {
                return new GenericBankConnectorResponse<ClientProfileCustomField[]>((int)ExceptionType.AccessDenied, false, "недостаточно прав");
            }

            try
            {
                var fields =
                    this.uow.ProfileCustomFieldsRepository.GetAllFieldsInOrder()
                        .Select(f => new ClientProfileCustomField { Id = f.Id, Name = f.Name })
                        .ToArray();

                return new GenericBankConnectorResponse<ClientProfileCustomField[]>(fields);
            }
            catch (Exception ex)
            {
                return new GenericBankConnectorResponse<ClientProfileCustomField[]>(ex);
            }
        }

        public GenericBankConnectorResponse<int> AppendProfileCustomField(string name, string userId)
        {
            if (!this.CheckPermissionsToManageCustomFields(userId))
            {
                return new GenericBankConnectorResponse<int>((int)ExceptionType.AccessDenied, false, "недостаточно прав");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }

            try
            {
                if (this.IsProfileCustomFieldExists(name))
                {
                    throw new ProfileCustomFieldAlreadyExistsException();
                }

                var field = this.CreateProfileCustomField(name);

                this.uow.ProfileCustomFieldsRepository.Add(field);
                this.uow.Save();

                return new GenericBankConnectorResponse<int>(field.Id);
            }
            catch (Exception ex)
            {
                return new GenericBankConnectorResponse<int>(ex);
            }
        }

        public SimpleBankConnectorResponse RemoveProfileCustomField(int id, string userId)
        {
            if (!this.CheckPermissionsToManageCustomFields(userId))
            {
                return new SimpleBankConnectorResponse((int)ExceptionType.AccessDenied, false, "недостаточно прав");
            }

            try
            {
                var field = this.uow.ProfileCustomFieldsRepository.GetById(f => f.Id == id);

                if (field != null)
                {
                    this.uow.ProfileCustomFieldsRepository.Delete(field);
                    this.uow.Save();
                }

                return new SimpleBankConnectorResponse();
            }
            catch (Exception ex)
            {
                return new SimpleBankConnectorResponse(ex);
            }
        }

        public SimpleBankConnectorResponse RenameProfileCustomField(int id, string name, string userId)
        {
            if (!this.CheckPermissionsToManageCustomFields(userId))
            {
                return new SimpleBankConnectorResponse((int)ExceptionType.AccessDenied, false, "недостаточно прав");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }

            try
            {
                if (this.IsProfileCustomFieldExists(name))
                {
                    throw new ProfileCustomFieldAlreadyExistsException();
                }

                var field = this.uow.ProfileCustomFieldsRepository.GetById(f => f.Id == id);
                field.Name = name;
                this.uow.Save();

                return new SimpleBankConnectorResponse();
            }
            catch (Exception ex)
            {
                return new SimpleBankConnectorResponse(ex);
            }
        }

        private bool IsProfileCustomFieldExists(string name)
        {
            return this.uow.ProfileCustomFieldsRepository.GetAll().Any(f => f.Name == name);
        }

        private ProfileCustomField CreateProfileCustomField(string name)
        {
            var orderMax = this.uow.ProfileCustomFieldsRepository.GetAll().Max(f => (int?)f.Order);

            return new ProfileCustomField
                   {
                       Name = name,
                       Order = orderMax.HasValue ? orderMax.Value + 1 : 1
                   };
        }

        private bool CheckPermissionsToChangePhoneNumber(string userId)
        {
            return this.CheckPermissions(userId, PermissionsToChangePhoneNumber);
        }

        private bool CheckPermissionsToManageCustomFields(string userId)
        {
            return this.CheckPermissions(userId, PermissionsToManageCustomFields);
        }

        private bool CheckPermissions(string userId, string[] permissions)
        {
            if (ArmSecurity.CheckPermissionsByAccountName(userId, permissions))
            {
                return true;
            }

            var message = string.Format(
                "Не достаточно прав, необходимые права: {0}.", string.Join(", ", permissions));
            logger.Error(message);

            return false;
        }
    }
}
