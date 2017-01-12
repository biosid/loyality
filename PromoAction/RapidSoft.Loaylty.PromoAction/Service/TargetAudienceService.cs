namespace RapidSoft.Loaylty.PromoAction.Service
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Transactions;

	using Monitoring;

	using RapidSoft.Loaylty.Logging;
	using RapidSoft.Loaylty.Logging.Wcf;
	using RapidSoft.Loaylty.PromoAction.Api;
	using RapidSoft.Loaylty.PromoAction.Api.Entities;
	using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
	using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
	using RapidSoft.Loaylty.PromoAction.Repositories;
	using RapidSoft.Loaylty.PromoAction.Wcf;
	using RapidSoft.VTB24.ArmSecurity;

	using TargetAudience = RapidSoft.Loaylty.PromoAction.Api.Entities.TargetAudience;

	/// <summary>
	/// Реализация сервиса для работы с промоакциями.
	/// </summary>
	[DbContextBehavior(false), LoggingBehavior]
    public class TargetAudienceService : SupportService, ITargetAudienceService, IServiceInfo
	{
        private readonly ILog log = LogManager.GetLogger(typeof(TargetAudienceService));

        /// <summary>
		/// Репозиторий целевых аудиторий.
		/// </summary>
		private readonly ITargetAudienceRepository targetAudienceRepository;

		private readonly IRuleRepository ruleRepository;

		private readonly ITargetAudienceClientLinkRepository targetAudienceClientLinkRepository;

		public TargetAudienceService()
		{
			this.targetAudienceRepository = new TargetAudienceRepository();
			this.ruleRepository = new RuleRepository();
			this.targetAudienceClientLinkRepository = new TargetAudienceClientLinkRepository();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TargetAudienceService"/> class.
		/// </summary>
		/// <param name="targetAudienceRepository">Репозиторий целевых аудиторий. </param>
		/// <param name="ruleRepository">Репозиторий правил</param>
		/// <param name="targetAudienceClientLinkRepository">Репозиторий связок "Целевая аудитория - Клиент"</param>
		public TargetAudienceService(
			ITargetAudienceRepository targetAudienceRepository = null, 
			IRuleRepository ruleRepository = null, 
			ITargetAudienceClientLinkRepository targetAudienceClientLinkRepository = null)
		{
			this.targetAudienceRepository = targetAudienceRepository ?? new TargetAudienceRepository();
			this.ruleRepository = ruleRepository ?? new RuleRepository();
			this.targetAudienceClientLinkRepository = targetAudienceClientLinkRepository ?? new TargetAudienceClientLinkRepository();
		}

		/// <summary>
		/// Операция возвращает список ЦА к которым принадлежит клиент согласно хранимому реестру клиентов целевых аудиторий.
		/// </summary>
		/// <param name="clientId">
		/// Идентификатор клиента.
		/// </param>
		/// <returns>
		/// Массив записей типа Целевая аудитория.
		/// </returns>
		public GetClientTargetAudiencesResult GetClientTargetAudiences(string clientId)
		{
			try
			{
				var targetAudiences = this.targetAudienceRepository.GetByClientId(clientId).Select(x => x.ToDTO()).ToList();
				return GetClientTargetAudiencesResult.BuildSuccess(targetAudiences);
			}
			catch (Exception e)
			{
				log.Error("Ошибка получения списка целевых аудиторий", e);
				return ServiceOperationResult.BuildErrorResult<GetClientTargetAudiencesResult>(e);
			}
		}

		public AssignClientAudienceResult AssignClientTargetAudience(AssignClientTargetAudienceParameters parameters)
		{
			ArmSecurityHelper.CheckPermissions(parameters.UserId, ArmPermissions.PromoCreateUpdateDelete);

			var assignResults = ProcessClientAudienceRelations(parameters.ClientAudienceRelations, parameters.UserId);

			return new AssignClientAudienceResult
					   {
						   ResultCode = ResultCodes.SUCCESS,
						   ClientTargetAudienceRelations = assignResults.ToArray(),
					   };
		}

		public ResultBase AssignClientSegment(AssignClientSegmentParameters parameters)
		{
			try
			{
				ArmSecurityHelper.CheckPermissions(parameters.UserId, ArmPermissions.PromoCreateUpdateDelete);

				using (var transaction = new TransactionScope())
				{
					foreach (var segment in parameters.Segments)
					{
						var ta = this.GetOrCreateSegment(segment.Id, parameters.UserId);

						foreach (var clientId in segment.ClientIds)
						{
							RemoveExistingTargetAudienceLink(clientId, parameters.UserId);
							this.SaveTargetAudienceClientLink(ta.Id, clientId, parameters.UserId);
						}
					}

					transaction.Complete();

					return ResultBase.BuildSuccess();
				}
			}
			catch (Exception ex)
			{
				log.Error("Ошибка привязвания клиента к сегменту", ex);
				return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
			}
		}

		public GetTargetAudiencesResult GetTargetAudiences(bool? isSegment)
		{
			try
			{
				var ta = this.targetAudienceRepository.GetBySegment(isSegment);
				var retTA = ta.Select(x => x.ToDTO()).ToList();
				return GetTargetAudiencesResult.BuildSuccess(retTA);
			}
			catch (Exception ex)
			{
				log.Error("Ошибка получения списка целевых аудиторий/сегментов", ex);
				return ServiceOperationResult.BuildErrorResult<GetTargetAudiencesResult>(ex);
			}
		}

		private IEnumerable<ClientTargetAudienceRelationResult> ProcessClientAudienceRelations(IEnumerable<ClientTargetAudienceRelation> relations, string userId)
		{
			if (relations == null)
			{
				return new List<ClientTargetAudienceRelationResult>();
			}

			var cache = new TargetAudienceExistenceCache(this.ruleRepository, this.targetAudienceRepository);

			return relations.Select(clientAudience => this.ProcessClientAudienceRelation(clientAudience, userId, cache));
		}

		private void RemoveExistingTargetAudienceLink(string clientId, string userId)
		{
			 targetAudienceClientLinkRepository.DeleteSegment(clientId, userId);
		}

		private TargetAudience GetOrCreateSegment(string segmentId, string userId)
		{
			var ta = this.targetAudienceRepository.Get(segmentId);

			if (ta != null)
			{
				if (!ta.IsSegment)
				{
					var mess = string.Format(
						"Существует целевая аудитория с идентификатором {0}, но не являющейся сегментом", segmentId);
					throw new OperationException(ResultCodes.INVALID_TARGET_AUDIENCE, mess);
				}

				return ta;
			}

			var targetAudience = TargetAudience.BuildSegment(segmentId, userId);
			this.targetAudienceRepository.Save(targetAudience);
			return targetAudience;
		}

		#region IServiceInfo Members

		/// <summary>
		/// Метод проверки связнанных ресурсов.
		/// </summary>
		public void Ping()
		{
			var errorStr = new StringBuilder();

			try
			{
				this.targetAudienceRepository.GetByClientId(string.Empty);
			}
			catch (Exception e)
			{
				log.Error("База данных компонента не доступна", e);
				errorStr.AppendLine("База данных компонента не доступна");
			}

			if (errorStr.Length > 0)
			{
				throw new Exception(errorStr.ToString());
			}
		}

		/// <summary>
		/// Метод получения версии компонента.
		/// </summary>
		/// <returns>
		/// The <see cref="Version"/>.
		/// </returns>
		public Version GetServiceVersion()
		{
			return Assembly.GetExecutingAssembly().GetName().Version;
		}
		#endregion

		private ClientTargetAudienceRelationResult ProcessClientAudienceRelation(
			ClientTargetAudienceRelation clientAudience,
			string userId,
			TargetAudienceExistenceCache cache)
		{
			var targetAudienceData = cache.GetTargetAudience(clientAudience.PromoActionId, userId);

			if (!targetAudienceData.Item1)
			{
				// NOTE: Проблемы с ЦА
				return ClientTargetAudienceRelationResult.BuildNotFound(clientAudience, targetAudienceData.Item3);
			}

			this.SaveTargetAudienceClientLink(targetAudienceData.Item2.Id, clientAudience.ClientId, userId);
			
			return ClientTargetAudienceRelationResult.BuildSuccess(clientAudience);
		}

		private void SaveTargetAudienceClientLink(string targetAudienceId, string clientId, string createUserId)
		{
			var innerDateTime = DateTime.Now;

			var exists = this.targetAudienceClientLinkRepository.Get(targetAudienceId, clientId);

			if (exists == null)
			{
				var link = new TargetAudienceClientLink
							   {
								   ClientId = clientId,
								   CreateDateTime = innerDateTime,
								   CreateDateTimeUtc = innerDateTime.ToUniversalTime(),
								   CreateUserId = createUserId,
								   TargetAudienceId = targetAudienceId
							   };

				this.targetAudienceClientLinkRepository.Insert(link);
			}
		}
	}
}