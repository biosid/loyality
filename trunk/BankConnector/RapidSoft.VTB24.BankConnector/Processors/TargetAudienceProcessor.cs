namespace RapidSoft.VTB24.BankConnector.Processors
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
	using RapidSoft.Loaylty.PromoAction.WsClients.TargetAudienceService;
	using RapidSoft.VTB24.BankConnector.DataModels;
	using RapidSoft.VTB24.BankConnector.DataSource;
	using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
	using RapidSoft.VTB24.BankConnector.Service;

    public class TargetAudienceProcessor : ProcessorBase
    {
	    private readonly ClientProfileService clientProfileService;

		private readonly ITargetAudienceService targetAudienceService;

		public TargetAudienceProcessor(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            ClientProfileService clientProfileService,
            ITargetAudienceService targetAudienceService)
            : base(logger, uow)
        {
			this.clientProfileService = clientProfileService;
			this.targetAudienceService = targetAudienceService;
        }

	    public void AssignClientTargetAudiences()
	    {
		    var sessionId = Guid.Parse(Logger.EtlSessionId);

            Logger.Info("AssignClientTargetAudiences started");

		    var linksRepository = Uow.ClientAudienceRelationRepository;

	        var clientAudienceRelations =
	            linksRepository.GetAll().Where(b => b.SessionId == sessionId).OrderBy(x => new { x.ClientId, x.PromoId });
	        var totalProcessed = this.PerfomBatchWork(clientAudienceRelations, ProcessLinksBatch);

            Logger.Info("Processed objects - " + totalProcessed);
            Logger.Info("AssignClientTargetAudiences finished");
	    }

		private int ProcessLinksBatch(List<ClientAudienceRelation> links)
		{
			var existsClients = SelectExistsUsersOnly(links);

			var assignRequest = BuildAssignRequest(existsClients);

			var assignResult = this.InvokeAssignClientsService(assignRequest);

			SaveAssignResult(assignResult, existsClients);

            Logger.InfoFormat("Processed entities in current batch: {0}", links.Count);

			return links.Count;
		}

		private List<ClientAudienceRelation> SelectExistsUsersOnly(List<ClientAudienceRelation> links)
		{
            var reqClientsIdentifiers =
		        links.Select(x => x.ClientId)
		             .Distinct()
		             .Select(x => new BatchGetClientsByExternalIdRequestTypeReqClientIdentifier { ClientExternalId = x, })
		             .ToArray();

		    var clientProfileRequest = new BatchGetClientsByExternalIdRequestType
		                            {
		                                LoyaltyProgramId = ConfigHelper.LoyaltyProgramId,
		                                ReqClientsIdentifiers = reqClientsIdentifiers
		                            };

            Logger.InfoFormat(
				"Обращение к сервису ClientProfile для выбора только существующих участников: количество идентификаторов для проверки {0}",
				clientProfileRequest.ReqClientsIdentifiers.Count());

		    var batchGetClientsByExternalIdRequest = new BatchGetClientsByExternalIdRequest(clientProfileRequest);
		    var clientProfileResponse =
				clientProfileService.BatchGetClientsByExternalId(batchGetClientsByExternalIdRequest).Response;

			if (clientProfileResponse.StatusCode == 2)
			{
				throw new Exception("Не удалось проверить существование клиентов. В ClientProfile передан неверный идентификатор программы лояльности.");
			}

			if (clientProfileResponse.StatusCode == 1)
			{
				throw new Exception("Не удалось проверить существование клиентов. При обращении к ClientProfile произошла неизвестная ошибка.");
			}

            Logger.InfoFormat("Записей в ответе ClientProfile: {0}", clientProfileResponse.ResClientsIdentifiers.Count());

			var existsClientsList = new List<ClientAudienceRelation>();

			clientProfileResponse.ResClientsIdentifiers.ToList().ForEach(x => ProcessResponseItem(links, existsClientsList, x));

			return existsClientsList;
		}

	    private void ProcessResponseItem(
		    IEnumerable<ClientAudienceRelation> links,
		    List<ClientAudienceRelation> existsClientsList,
		    BatchGetClientsByExternalIdResponseTypeResClientIdentifier responseItem)
	    {
		    if (responseItem.StatusCode != 0)
		    {
                Logger.ErrorFormat(
				    "Проверка существования клиента ({0}) c ExternalId ({1}) завершилась с ошибкой ({2} - {3})",
				    responseItem.ClientId,
					responseItem.ClientExternalId,
				    responseItem.StatusCode,
				    responseItem.Error);
			    //// rev: Зачем это? То что мы установим статус AssignClientTargetAudienceStatus.ClientNotFound ни на что не повлияет, так как links - это указатель на список пришедший из вне, а там links никак не используется, только создается и передается в этот метод. И весь profit в том что в SaveAssignResult вызывается Uow.Save(), я бы не полагался на это.
				links.Where(r => r.ClientId == responseItem.ClientExternalId)
			         .ToList()
					 .ForEach(r => r.Status = (int)AssignClientTargetAudienceStatus.ClientNotFound);
		    }
		    else
		    {
				var successList = links.Where(r => r.ClientId == responseItem.ClientExternalId).ToList();
				successList.ForEach(r => r.Status = (int)AssignClientTargetAudienceStatus.Success);
			    existsClientsList.AddRange(successList);
		    }
	    }

	    private AssignClientTargetAudienceParameters BuildAssignRequest(IEnumerable<ClientAudienceRelation> links)
	    {
		    var clientAudienceRelations =
			    links.Select(b => new ClientTargetAudienceRelation { ClientId = b.ClientId, PromoActionId = b.PromoId })
			         .ToArray();

	        var assignRequest = new AssignClientTargetAudienceParameters
	                                {
	                                    ClientAudienceRelations =
	                                        clientAudienceRelations,
	                                    UserId = ConfigHelper.VtbSystemUser
	                                };

	        return assignRequest;
	    }

	    private AssignClientAudienceResult InvokeAssignClientsService(AssignClientTargetAudienceParameters assignRequest)
	    {
		    AssignClientAudienceResult assignResult;

		    try
		    {
                Logger.InfoFormat("Отправка запроса targetAudienceService.AssignClientTargetAudience, записей в запросе: {0}", assignRequest.ClientAudienceRelations.Count());
				
                assignResult = targetAudienceService.AssignClientTargetAudience(assignRequest);

                Logger.InfoFormat(
					"Выполнение запроса targetAudienceService.AssignClientTargetAudience завешено, код результата запроса: {0}, флаг успешного завершения: {1}",
				    assignResult.ResultCode,
				    assignResult.Success);
		    }
		    catch (Exception ex)
		    {
                Logger.Error("Error executing targetAudienceService.AssignClientTargetAudience ", ex);

			    const string ParameterLogPrefix = "AssignClientTargetAudience request parameter:";
				
                for (var i = 0; i < assignRequest.ClientAudienceRelations.Count(); i++)
				{
					var link = assignRequest.ClientAudienceRelations[i];
					var relationStr = ParameterLogPrefix + string.Format(
						"ClientAudienceRelation[{0}]: ClientId ({1}), PromoActionId ({2})", i, link.ClientId, link.PromoActionId);
                    Logger.Error(relationStr);
				}

			    var parameterStr = ParameterLogPrefix
			                       + string.Format("AssignClientTargetAudienceParameters: UserId ({0})", assignRequest.UserId);
                Logger.Error(parameterStr);
				throw;
		    }

		    return assignResult;
	    }

		private void SaveAssignResult(AssignClientAudienceResult assignResult, List<ClientAudienceRelation> links)
		{
			if (!assignResult.Success || assignResult.ClientTargetAudienceRelations == null)
			{
                Logger.Error(
					string.Format(
						"Error executing service method targetAudienceServiceClient.AssignClientTargetAudience. Code - {0}, Description - {1}",
						assignResult.ResultCode,
						assignResult.ResultDescription));
				return;
			}

			assignResult.ClientTargetAudienceRelations.ToList().ForEach(
				ctar =>
					{
						var targetLink = links.Single(x => x.ClientId == ctar.ClientId && x.PromoId == ctar.PromoActionId);
						targetLink.Status = ctar.AssignResultCode;
					});
			Uow.Save();
		}
    }
}
