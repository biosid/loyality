namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Linq;

    using DataModels;
    using DataSource;
    using Service;
    using Site.SecurityWebApi;

    public class BankOffersProcessor : ProcessorBase
    {
        private const int BatchSize = 500;

        private readonly ISecurityWebApi securityWebApi;

        public BankOffersProcessor(EtlLogger.EtlLogger logger, IUnitOfWork uow, ISecurityWebApi securityWebApi)
            : base(logger, uow)
        {
            this.securityWebApi = securityWebApi;
        }

        public void ProcessBankOffers()
        {
            var sessionId = Guid.Parse(Logger.EtlSessionId);

            RegisterBankOffer[] batch;
			Logger.Info("Начинаем обработку.");
            while ((batch =
                    Uow.RegisterBankOffersRepository
                       .GetUnprocessedBySessionId(sessionId, Uow.RegisterBankOffersResponseRepository)
                       .OrderBy(x => x.Id)
                       .Take(BatchSize)
                       .ToArray()).Length > 0)
            {
				Logger.Info("Начало обработки пачки.");

				// Проверка существования клиентов в security
                try
                {
                    RegisterBankOffer[] unknown;
                    batch = CheckInSecurity(batch, out unknown);
					Logger.Info("Проверка существования клиентов в security выполнена.");
                    SaveCreationResult(unknown, RegisterBankOfferResult.ClientNotFound, sessionId);
					Logger.Info("Результат проверки в security сохранён.");
				}
                catch (Exception e)
                {
                    Logger.Error("Ошибка при проверке существования клиентов", e);
                    SaveCreationResult(batch, RegisterBankOfferResult.InternalError, sessionId);
					Logger.Info("Результат проверки в security сохранён.");
                    continue;
                }

                try
                {
                    var offers = batch.Select(o => new BankOffer
                    {
                        Id = o.PartnerOrderNum,
                        BonusCost = o.OrderBonusCost,
                        Description = o.ArticleName,
                        CardLast4Digits = o.CardLast4Digits,
                        ClientId = o.ClientId,
                        ExpirationDate = o.ExpirationDate,
                        OfferId = o.OfferId,
                        ProductId = o.ProductId,
                        Status = BankOfferStatus.Active
                    });

                    Uow.BankOffersRepository.BulkAdd(offers);
					Logger.Info("Сохранили добавленные предложения в БД.");
                }
                catch (Exception e)
                {
                    Logger.Error("Ошибка при добавлении предложений в базу", e);

                    SaveCreationResult(batch, RegisterBankOfferResult.InternalError, sessionId);
                    continue;
                }

                SaveCreationResult(batch, RegisterBankOfferResult.Success, sessionId);
				Logger.Info("Обработка пачки завершена.");
            }

			Logger.Info("Обработка завершена.");
        }

        private RegisterBankOffer[] CheckInSecurity(RegisterBankOffer[] offers, out RegisterBankOffer[] offersForUnknown)
        {
            var clients = securityWebApi.BatchResolveUsersByClientId(offers.Select(x => x.ClientId).ToArray());

            if (clients == null)
            {
                throw new Exception("SecurityWebApi.BatchResolveUsersByClientId returns null");
            }

            offersForUnknown = offers.Where(o => !clients.ContainsKey(o.ClientId) ||
                                                 clients[o.ClientId] == null ||
                                                 clients[o.ClientId].ClientId != o.ClientId)
                                     .ToArray();

            if (offersForUnknown.Length == 0)
            {
                return offers;
            }

            var offerIdsForUnknown = offersForUnknown.Select(o => o.PartnerOrderNum).ToArray();

            Logger.Warn("Не найдены клиенты для предложений с PartnerOrderNum: " + string.Join(", ", offerIdsForUnknown));

            var offersForKnown = offers.Where(o => !offerIdsForUnknown.Contains(o.PartnerOrderNum))
                                       .ToArray();

            return offersForKnown;
        }

        private void SaveCreationResult(RegisterBankOffer[] offers, RegisterBankOfferResult status, Guid sessionId)
        {
            if (offers.Length == 0)
            {
                return;
            }

            var responses = offers.Select(o => new RegisterBankOffersResponse
            {
                PartnerOrderNum = o.PartnerOrderNum,
                ClientId = o.ClientId,
                OrderActionResult = status,
                EtlSessionId = sessionId
            });

			Uow.RegisterBankOffersResponseRepository.BulkAdd(responses);
        }
    }
}