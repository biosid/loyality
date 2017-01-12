namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Service;
    using RapidSoft.VTB24.Site.SecurityWebApi;

    using Processing = RapidSoft.Loaylty.Processing.ProcessingService;

    public class AccrualProcessor : ProcessorBase
    {
        private readonly Processing.ProcessingService processingService;
        private readonly ISecurityWebApi securityWebApi;

        public AccrualProcessor(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            Processing.ProcessingService processingService,
            ISecurityWebApi securityWebApi)
            : base(logger, uow)
        {
            this.processingService = processingService;
            this.securityWebApi = securityWebApi;
        }

        public void ReceiveAccrualsExecute()
        {
            var etlSessionId = new Guid(Logger.EtlSessionId);

            int totalCount = 0;

            Accrual[] targetAccruals;

            while ((targetAccruals = this.Uow.AccrualRepository.GetAll().Where(x => x.ReceiveEtlSessionId == etlSessionId).OrderBy(x => x.Id).Skip(totalCount).Take(ConfigHelper.BatchSize).ToArray()).Length > 0)
            {
                totalCount += targetAccruals.Length;

                this.Logger.InfoFormat("Необработанных записей к начислению взято из БД: ({0})", targetAccruals.Length);

                // Проверяем что клиенты зарегистрированы
                var validAccurals = this.CheckSecurityExistedClients(targetAccruals);

                var processingRequest = this.PrepareProcessingRequest(validAccurals);

                this.Logger.InfoFormat("Сохранение данных в БД перед отправкой запроса к Processing");

                this.Uow.Save();

                this.Logger.InfoFormat("Выполнение запроса к Processing");

                try
                {
                    var processingResponse = this.CallProcessing(processingRequest, validAccurals);
                }
                catch (Exception ex)
                {
                    this.Logger.Error("Произошла ошибка при обращении к Processing", ex);
                }

                this.Uow.Save();
            }
        }

        private Accrual[] CheckSecurityExistedClients(Accrual[] accruals)
        {
            if (accruals.Length == 0)
            {
                return new Accrual[]
                       {
                       };
            }

            try
            {
                var resolveUsersResp = securityWebApi.BatchResolveUsersByClientId(accruals.Select(x => x.ClientId).ToArray());

                if (resolveUsersResp == null)
                {
                    throw new Exception("SecurityWebApi.BatchResolveUsersByPhone response returns with null");
                }

                var existed = (from r in resolveUsersResp join c in accruals on r.Key equals c.ClientId where r.Value != null && r.Value.ClientId == c.ClientId select c).ToArray();

                var notExisted = accruals.Where(c => !existed.Contains(c)).ToArray();

                foreach (var accrual in notExisted)
                {
                    this.Logger.Warn(string.Format("Клиент не найдён в Security {0}", accrual.ClientId));
                    accrual.Status = (int)AccrualStatus.Error;
                }

                this.Uow.Save();

                return existed.ToArray();
            }
            catch (Exception ex)
            {
                this.Logger.Error("Ошибка при проверке существования клиентов в Security", ex);
                throw;
            }
        }

        private Processing.BatchDepositByClientsRequestType PrepareProcessingRequest(Accrual[] targetAccruals)
        {
            var processingRequest = new Processing.BatchDepositByClientsRequestType
                                    {
                                        EtlSessionId = Logger.EtlSessionId, 
                                        LoyaltyProgramId = ConfigHelper.LoyaltyProgramId
                                    };

            var processingTransactionRequests = new List<Processing.BatchDepositByClientsRequestTypeDepositTransaction>();

            targetAccruals.ForEach(
                x =>
                    {
                    x.ExternalId = string.Format("{0}.{1}", Guid.NewGuid().ToString(), x.Type);
                    decimal bonusSum;
                    if (!decimal.TryParse(x.BonusSum, out bonusSum))
                    {
                        x.Status = (int)AccrualStatus.IncorrectFormat;
                        x.BonusOperationDateTime = DateTime.Now;
                        return;
                    }

                    using (var hasher = MD5.Create())
                    {
                        var hash = hasher.ComputeHash(Encoding.Default.GetBytes(x.ExternalId)).Select(b => b.ToString("x2")).Aggregate((workingSeq, nextByte) => workingSeq + nextByte);

                        processingTransactionRequests.Add(
                            new Processing.BatchDepositByClientsRequestTypeDepositTransaction
                            {
                                BonusSum = bonusSum, 
                                ClientExternalId = x.ClientId, // NOTE: считаем, что ClientId == ExternalClientId
                                TransactionDescription = x.Description, 
                                TransactionExternalId = x.ExternalId, 
                                TransactionHash = hash, 
                                TransactionPOSDateTime = DateTime.Now
                            });
                    }
                });

            processingRequest.DepositTransactions = processingTransactionRequests.ToArray();

            return processingRequest;
        }

        private Processing.BatchDepositByClientsResponseType CallProcessing(Processing.BatchDepositByClientsRequestType processingRequest, Accrual[] targetAccruals)
        {
            var processingResponse = this.processingService.BatchDepositByClients(new Processing.BatchDepositByClientsRequest(processingRequest)).Response;

            if (processingResponse.StatusCode != 0)
            {
                throw new Exception(string.Format("Processing вернул ошибочный статус для запроса ({0})", processingResponse.StatusCode));
            }

            processingResponse.DepositByClientResults.ToList().ForEach(
                x =>
                {
                    var targetAccrual = targetAccruals.Single(accrual => accrual.ExternalId == x.TransactionExternalId);
                    if (x.StatusCode == 0)
                    {
                        targetAccrual.Status = (int)AccrualStatus.Success;
                    }
                    else
                    {
                        this.Logger.ErrorFormat("Запрос к процессингу для начисления ({0}) вернул ошибочный статус ({1}), причина ({2}), Id транзакции в процессинге ({3})", x.TransactionExternalId, x.StatusCode, x.Error, x.TransactionId);
                        targetAccrual.Status = (int)AccrualStatus.Error;
                    }

                    targetAccrual.BonusOperationDateTime = x.TransactionDateTime ?? DateTime.Now;
                });
            return processingResponse;
        }
    }
}