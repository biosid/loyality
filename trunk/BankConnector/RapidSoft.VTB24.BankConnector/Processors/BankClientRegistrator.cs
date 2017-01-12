namespace RapidSoft.VTB24.BankConnector.Processors
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
    using System.Text.RegularExpressions;
    using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
	using RapidSoft.Loaylty.PromoAction.WsClients.TargetAudienceService;
	using RapidSoft.VTB24.BankConnector.DataModels;
	using RapidSoft.VTB24.BankConnector.DataSource;
	using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
	using RapidSoft.VTB24.BankConnector.Processors.MapHelpers;
	using RapidSoft.VTB24.BankConnector.Service;
	using RapidSoft.VTB24.Site.SecurityWebApi;
	using Processing = RapidSoft.Loaylty.Processing.ProcessingService;

    public class BankClientRegistrator : ProcessorBase
    {
        private readonly ClientProfileService clientProfileService;

	    private readonly ISecurityWebApi securityWebApi;

	    private readonly Processing.ProcessingService processingService;

        private readonly ITargetAudienceService targetAudienceService;

	    public BankClientRegistrator(
            EtlLogger.EtlLogger logger,
		    IUnitOfWork uow,
		    ClientProfileService clientProfileService,
		    ISecurityWebApi securityWebApi,
            Processing.ProcessingService processingService,
            ITargetAudienceService targetAudienceService)
            : base(logger, uow)
	    {
		    this.clientProfileService = clientProfileService;
		    this.securityWebApi = securityWebApi;
		    this.processingService = processingService;
            this.targetAudienceService = targetAudienceService;
	    }

	    public void RegisterBankClients()
        {
            List<ClientForBankRegistration> batch;
            var processedRecords = 0;
	        var sessionId = Guid.Parse(this.Logger.EtlSessionId);

	        while ((batch = this.Uow.ClientForBankRegistrationRepository.GetBySessionId(sessionId)
                     .OrderBy(x => x.MobilePhone)
                     .Skip(processedRecords)
                     .Take(ConfigHelper.BatchSize).ToList()).Any())
            {
                processedRecords += batch.Count;

                // NOTE: Так как теперь банк не присылает ClientId, генерируем его здесь, сохранение выполнится ниже: Uow.Save();
                batch.ForEach(x => x.ClientId = Guid.NewGuid().ToString());

                // Отсеим клиентов с неверным номером телефона
                var wrongPhoneClients = batch.Where(x => !Regex.IsMatch(ConfigHelper.DefaultCountryCode + x.MobilePhone, ConfigHelper.ValidPhoneRegExp)).ToArray();
                this.LogWarn(string.Format("Phone not match by regex {0}", ConfigHelper.ValidPhoneRegExp), wrongPhoneClients);
                SaveCreationResult(wrongPhoneClients, BankClientRegistrationStatusCodes.LoyaltyError, "Клиентов с неверным телефоном", batch.Count);

                var validClients = batch.Except(wrongPhoneClients).ToArray();

                if (!validClients.Any())
                {
                    this.Logger.Info(string.Format("No clients with phone match regex {0}", ConfigHelper.ValidPhoneRegExp));
                    continue;
                }

                // Проверка существования клиентов в security
                var securityExistedClients = this.GetSecurityExistedClients(validClients).ToArray();
                this.LogWarn("Client exist in security", securityExistedClients);
                this.SaveCreationResult(securityExistedClients, BankClientRegistrationStatusCodes.AllreadyRegistered, "Уже существуют в Security", validClients.Length);

                validClients = validClients.Except(securityExistedClients).ToArray();

                if (!validClients.Any())
                {
                    this.Logger.Info("No valid clients after GetSecurityExistedClients");
                    continue;
                }

                // Регистрация в профиле
                var clientProfileResult = this.ClientProfileBatchCreateClients(validClients).ToArray();

                var invalidClientProfileResponses = (from r in clientProfileResult
                                                    join c in batch on r.ClientExternalId equals c.ClientId
                                                    where 
                                                    r.StatusCode != (int)BatchCreateClientsStatusCodes.Success && 
                                                    r.StatusCode != (int)BatchCreateClientsStatusCodes.AllreadyRegistered
                                                    select c).ToArray();
                this.LogError("ClientProfile registration status unknown", invalidClientProfileResponses);
                this.SaveCreationResult(invalidClientProfileResponses, BankClientRegistrationStatusCodes.LoyaltyError, "Не удалось зарегистрировать в ClientProfile", validClients.Length);
                
                var alreadyRegisteredResponses = (from r in clientProfileResult
                                                 join c in batch on r.ClientExternalId equals c.ClientId
                                                 where r.StatusCode == (int)BatchCreateClientsStatusCodes.AllreadyRegistered
                                                 select c).ToArray();

                this.LogWarn("Client already exist in ClientProfile", alreadyRegisteredResponses);
                this.SaveCreationResult(alreadyRegisteredResponses, BankClientRegistrationStatusCodes.AllreadyRegistered, "Уже существуют в ClientProfile", validClients.Length);
                
                validClients = validClients.Where(x => !invalidClientProfileResponses.Select(y => y.ClientId).Contains(x.ClientId) && 
                    !alreadyRegisteredResponses.Select(y => y.ClientId).Contains(x.ClientId)).ToArray();

                if (!validClients.Any())
                {
                    this.Logger.Info("No valid clients after ClientProfileBatchCreateClients");
                    continue;
                }

                // Регистрация в процессинге
                var processingResult = this.ProcessingBatchCreateClients(validClients).ToArray();
                
                var invalidProcessingResponses = (from r in processingResult
                                                 join c in batch on r.ClientExternalId equals c.ClientId
                                                 where r.StatusCode != (int)BatchCreateClientsStatusCodes.Success
                                                 select c).ToArray();

                this.LogError("Processing registration status unknown", invalidProcessingResponses);
                this.SaveCreationResult(invalidProcessingResponses, BankClientRegistrationStatusCodes.LoyaltyError, "Не удалось зарегистрировать в Processing", validClients.Length);
                
                validClients = validClients.Where(x => !invalidProcessingResponses.Select(y => y.ClientId).Contains(x.ClientId)).ToArray();

                if (!validClients.Any())
                {
                    this.Logger.Info("No valid clients after ProcessingBatchCreateClients");
                    continue;
                }

                // Регистрация в безопасности
                List<ClientForBankRegistration> invalidSecurityResponses;
                validClients = this.SecurityCreateUser(validClients, out invalidSecurityResponses).ToArray();

                this.SaveCreationResult(invalidSecurityResponses, BankClientRegistrationStatusCodes.LoyaltyError, "Не удалось зарегистрировать в Security", validClients.Length);
                
                if (!validClients.Any())
                {
                    this.Logger.Info("No valid clients after SecurityCreateUser");
                    continue;
                }

                // Регистрация пользователей в сегментах
                var segments = validClients.GroupBy(x => x.Segment).Select(group => group.ToSegment(x => x.ClientId));
                this.targetAudienceService.CallAssignClientSegment(segments, this.Logger);

                this.SaveCreationResult(validClients, BankClientRegistrationStatusCodes.Success, "Зарегистрировано успешно", batch.Count);
            }

            this.Logger.Info("Bank Clients registration completed");
        }

        private void LogWarn(string message, ClientForBankRegistration[] clients)
        {
            foreach (var client in clients)
            {
                Logger.Warn(string.Format("{0} clientId:{1}", message, client.ClientId));
            }
        }

        private void LogError(string message, ClientForBankRegistration[] clients)
        {
            foreach (var client in clients)
            {
                Logger.Error(string.Format("{0} clientId:{1}", message, client.ClientId));
            }
        }

        private List<ClientForBankRegistration> GetSecurityExistedClients(ClientForBankRegistration[] validClients)
        {
            try
            {
                var clients = securityWebApi.BatchResolveUsersByPhone(validClients.Select(x => ConfigHelper.DefaultCountryCode + x.MobilePhone).ToArray());
                
                if (clients == null)
                {
                    throw new Exception("SecurityWebApi.BatchResolveUsersByPhone response returns with null");
                }

                var existedClients = (from c in clients
                                     join b in validClients on c.Key equals ConfigHelper.DefaultCountryCode + b.MobilePhone
                                     where
                                         c.Value != null
                                         && 
                                         c.Value.PhoneNumber == ConfigHelper.DefaultCountryCode + b.MobilePhone
                                     select new { validClient = b, client = c.Value }).ToArray();

                return existedClients.Any() ? existedClients.Select(e =>
                    {
                        e.validClient.ClientId = e.client.ClientId;
                        return e.validClient;
                    }).ToList() : new List<ClientForBankRegistration>();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при проверке существования клиентов в Security", ex);
                throw;
            }
        }

        private BatchCreateClientsRequestType BuildClientProfileRequest(IEnumerable<ClientForBankRegistration> batch)
		{
            var createClientsRequest = new BatchCreateClientsRequestType
			{
				EtlSessionId = this.Logger.EtlSessionId,
				LoyaltyProgramId = ConfigHelper.LoyaltyProgramId,
				ClientRegistrationFacts = batch.Select(c => new BatchCreateClientsRequestTypeClientRegistrationFact
				                                            {
				                                                ClientExternalId = c.ClientId, 
                                                                ClientId = c.ClientId, 
                                                                MobilePhone = ConfigHelper.DefaultCountryCode + c.MobilePhone,
                                                                FirstName = "Имя", 
                                                                LastName = "Фамилия"
				                                            }).ToArray()
			};

			return createClientsRequest;
		}

	    private Processing.BatchCreateClientsRequestType BuildProcessingRequest(
		    IEnumerable<ClientForBankRegistration> clientProfileSuccessList)
	    {
		    var clientRegistrationFacts = new List<Processing.BatchCreateClientsRequestTypeClientRegistrationFact>();
		    foreach (var c in clientProfileSuccessList)
		    {
			    var clientId = c.ClientId;
			    var item = new Processing.BatchCreateClientsRequestTypeClientRegistrationFact
				               {
					               ClientExternalId = clientId,
					               ClientId = clientId,
					               RegistrationDateTime =
						               DateTime.Now,
				               };
			    clientRegistrationFacts.Add(item);
		    }

		    var createClientsRequest = new Processing.BatchCreateClientsRequestType
			                               {
				                               EtlSessionId = this.Logger.EtlSessionId,
				                               LoyaltyProgramId =
					                               ConfigHelper.LoyaltyProgramId,
				                               ClientRegistrationFacts =
					                               clientRegistrationFacts.ToArray()
			                               };

		    return createClientsRequest;
	    }

        private IEnumerable<BatchCreateClientsResponseTypeClientRegistrationResult> ClientProfileBatchCreateClients(IEnumerable<ClientForBankRegistration> validClients)
        {
            var createClientsRequest = this.BuildClientProfileRequest(validClients.ToArray());

            try
            {
                var result = clientProfileService.BatchCreateClients(new BatchCreateClientsRequest(createClientsRequest));

                if (result == null)
                {
                    throw new Exception("ClientProfile.BatchCreateClients result is null");
                }

                if (result.Response == null)
                {
                    throw new Exception("ClientProfile.BatchCreateClients result.Response is null");
                }

                if (result.Response.ClientRegistrationResults == null)
                {
                    throw new Exception("ClientProfile.BatchCreateClients result.Response.ClientRegistrationResults is null");
                }

                if (result.Response.StatusCode != 0)
                {
                    throw new Exception("ClientProfile.BatchCreateClients result.Response.StatusCode is not zero");
                }

                foreach (var client in result.Response.ClientRegistrationResults)
                {
                    if (client.StatusCode == 0)
                    {
                        this.Logger.Info(string.Format("ClientProfile.BatchCreateClients Регистрация успешна {0}", client.ClientId));
                    }
                }

                return result.Response.ClientRegistrationResults;
            }
            catch (Exception ex)
            {
                this.Logger.Error("Ошибка при вызове ClientProfile.BatchCreateClients", ex);
                throw;
            }
        }

        private IEnumerable<Processing.BatchCreateClientsResponseTypeClientRegistrationResult> ProcessingBatchCreateClients(IEnumerable<ClientForBankRegistration> validClients)
		{
            var processingRequest = BuildProcessingRequest(validClients);

			try
			{
				var result = processingService.BatchCreateClients(new Processing.BatchCreateClientsRequest(processingRequest));

                if (result == null)
                {
                    throw new Exception("Processing.BatchCreateClients result is null");
                }

                if (result.Response == null)
                {
                    throw new Exception("Processing.BatchCreateClients result.Response is null");
                }

                if (result.Response.ClientRegistrationResults == null)
                {
                    throw new Exception("Processing.BatchCreateClients result.Response.ClientRegistrationResults is null");
                }
                
			    foreach (var client in result.Response.ClientRegistrationResults)
			    {
			        if (client.StatusCode == 0)
			        {
			            this.Logger.Info(string.Format("Регистрация участника в Processing {0} c ExternalId {1} завершена успешно", client.ClientId, client.ClientExternalId));
			        }
			    }

			    return result.Response.ClientRegistrationResults;
			}
			catch (Exception ex)
			{
				this.Logger.Error("Ошибка при вызове ProcessingService.BatchCreateClients", ex);
			    throw;
			}
		}

        private List<ClientForBankRegistration> SecurityCreateUser(IEnumerable<ClientForBankRegistration> validClients, out List<ClientForBankRegistration> securityErrorClients)
	    {
            securityErrorClients = new List<ClientForBankRegistration>();
            var validList = new List<ClientForBankRegistration>();
            foreach (var account in validClients)
		    {
		        var options = new CreateUserOptions()
		                          {
		                              ClientId = account.ClientId,
		                              PhoneNumber = ConfigHelper.DefaultCountryCode + account.MobilePhone,
                                      RegistrationType = RegistrationType.BankRegistration
		                          };

			    try
			    {
				    securityWebApi.CreateUser(options);
                    this.Logger.InfoFormat("Вызов сервиса Security для клиента ({0}) с телефоном ({1}) выполнен успешно", account.ClientId, account.MobilePhone);
			        validList.Add(account);
			    }
			    catch (Exception ex)
			    {
                    this.Logger.Warn(string.Format("Ошибка при вызове SecurityService.CreateUser для клиента ({0}) с телефоном ({1})", account.ClientId, account.MobilePhone), ex);
				    securityErrorClients.Add(account);
			    }
		    }

            return validList;
	    }

	    private void SaveCreationResult(IEnumerable<ClientForBankRegistration> clientsBatch, BankClientRegistrationStatusCodes status, string counterName, int total)
	    {
	        if (!clientsBatch.Any())
	        {
	            return;
	        }

            this.Logger.Info(string.Format("{0}:{1} из {2}", counterName, clientsBatch.Count(), total));         

            var repository = Uow.ClientForBankRegistrationResponseRepository;

	        foreach (var response in clientsBatch.Select(record => new ClientForBankRegistrationResponse
	                                                                  {
	                                                                      ClientId = record.ClientId,
	                                                                      Login = record.MobilePhone,
	                                                                      SessionId = Guid.Parse(this.Logger.EtlSessionId),
	                                                                      Status = (int)status
	                                                                  }))
	        {
	            repository.Add(response);
	        }

            Uow.Save();
	    }
    }
}
