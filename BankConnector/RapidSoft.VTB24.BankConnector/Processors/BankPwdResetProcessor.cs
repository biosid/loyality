namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Service;
    using RapidSoft.VTB24.Site.SecurityWebApi;

    public class BankPwdResetProcessor : ProcessorBase
    {
        private readonly ISecurityWebApi securityWebApi;

        private Guid sessionId;
        
        public BankPwdResetProcessor(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            ISecurityWebApi securityWebApi)
            : base(logger, uow)
        {
            this.securityWebApi = securityWebApi;
        }

        public void Execute()
        {
            this.sessionId = Guid.Parse(this.Logger.EtlSessionId);

            var processedCount = 0;
            var succeededCount = 0;

            var clientsForBankPwdReset = this.Uow.ClientForBankPwdResetRepository.GetBySessionId(this.sessionId).OrderBy(u => u.SeqId);

            this.Logger.Info("Начало сброса паролей пользователей по запросам банка");

            ClientForBankPwdReset[] batch;
            while ((batch = clientsForBankPwdReset.Skip(processedCount).Take(ConfigHelper.BatchSize).ToArray()).Length > 0)
            {
                var responses = this.ProcessBatch(batch).ToArray();

                this.Uow.ClientForBankPwdResetResponseRepository.Add(responses);
                this.Uow.Save();

                processedCount += batch.Length;
                succeededCount += responses.Count(r => r.Status == (int)ClientForBankPwdResetResponseStatus.Success);

                this.Logger.InfoFormat("Обработано {0} запросов, из них {1} успешно.", processedCount, succeededCount);
            }

            this.Logger.Info("Сброс паролей по запросам банка завершен");
        }

        private IEnumerable<ClientForBankPwdResetResponse> ProcessBatch(ClientForBankPwdReset[] batch)
        {
            var loginsByClientId = this.ResolveClientLogins(batch.Select(c => c.ClientId));

            return loginsByClientId != null
                       ? batch.Select(client => this.ProcessClient(client.ClientId, loginsByClientId))
                       : batch.Select(client => this.CreateErrorResponse(client.ClientId, "ошибка обработки"));
        }

        private ClientForBankPwdResetResponse ProcessClient(string clientId, Dictionary<string, string> loginsByClientId)
        {
            string login;
            if (!loginsByClientId.TryGetValue(clientId, out login))
            {
                return this.CreateErrorResponse(clientId, "клиент не найден");
            }

            string errorReason;
            return this.ResetPassword(login, out errorReason)
                       ? this.CreateSuccessResponse(clientId)
                       : this.CreateErrorResponse(clientId, "не удалось сбросить пароль: " + errorReason);
        }

        private Dictionary<string, string> ResolveClientLogins(IEnumerable<string> clientIds)
        {
            try
            {
                var response = this.securityWebApi.BatchResolveUsersByClientId(clientIds.ToArray());

                return response.Where(x => x.Value != null)
                               .ToDictionary(x => x.Key, x => x.Value != null ? x.Value.PhoneNumber : null);
            }
            catch (Exception e)
            {
                this.Logger.Error("ошибка получения логинов клиентов", e);
                return null;
            }
        }

        private bool ResetPassword(string login, out string errorReason)
        {
            try
            {
                var response = this.securityWebApi.ResetUserPassword(new ResetUserPasswordOptions { Login = login });

                errorReason = response.Success ? string.Empty : "сервис вернул статус " + (int)response.Status;

                return response.Success;
            }
            catch (Exception e)
            {
                this.Logger.Error("ошибка вызова системы безопасности", e);
                errorReason = "при обращении к сервису произошла ошибка";
                return false;
            }
        }

        private ClientForBankPwdResetResponse CreateErrorResponse(string clientId, string message)
        {
            return new ClientForBankPwdResetResponse
                   {
                       EtlSessionId = this.sessionId,
                       ClientId = clientId,
                       Status = (int)ClientForBankPwdResetResponseStatus.Error,
                       Message = message
                   };
        }

        private ClientForBankPwdResetResponse CreateSuccessResponse(string clientId)
        {
            return new ClientForBankPwdResetResponse
                   {
                       EtlSessionId = this.sessionId,
                       ClientId = clientId,
                       Status = (int)ClientForBankPwdResetResponseStatus.Success,
                   };
        }
    }
}
