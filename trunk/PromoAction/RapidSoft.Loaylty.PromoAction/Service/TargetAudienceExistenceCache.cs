namespace RapidSoft.Loaylty.PromoAction.Service
{
	using System;
	using System.Collections.Generic;

	using RapidSoft.Loaylty.Logging;
	using RapidSoft.Loaylty.PromoAction.Api.Entities;
	using RapidSoft.Loaylty.PromoAction.Mechanics;
	using RapidSoft.Loaylty.PromoAction.Repositories;
	using RapidSoft.Loaylty.PromoAction.Settings;

	internal class TargetAudienceExistenceCache
	{
        private readonly ILog log = LogManager.GetLogger(typeof(TargetAudienceExistenceCache));

        private readonly IRuleRepository ruleRepository;
		private readonly ITargetAudienceRepository targetAudienceRepository;
		private readonly Dictionary<string, CacheItem> cache = new Dictionary<string, CacheItem>();

		public TargetAudienceExistenceCache(IRuleRepository ruleRepository, ITargetAudienceRepository targetAudienceRepository)
		{
			this.targetAudienceRepository = targetAudienceRepository ?? new TargetAudienceRepository();
			this.ruleRepository = ruleRepository ?? new RuleRepository();
		}

		public Tuple<bool, TargetAudience, string> GetTargetAudience(string promoActionId, string userId)
		{
			// NOTE: Возможно ранее загружали ЦА
			if (this.cache.ContainsKey(promoActionId))
			{
				var value = this.cache[promoActionId];

				return value.IsSuccess
						   ? new Tuple<bool, TargetAudience, string>(true, value.TargetAudience, null)
						   : new Tuple<bool, TargetAudience, string>(false, null, value.Description);
			}

			var targetAudienceId = string.Format("{0}{1}", ApiSettings.TargetAudienceLiteralPrefix, promoActionId);
			
			// NOTE: Возможно есть в БД
			var exists = this.targetAudienceRepository.Get(targetAudienceId);

			if (exists != null)
			{
				this.cache.Add(promoActionId, CacheItem.BuildSuccess(exists));
				return new Tuple<bool, TargetAudience, string>(true, exists, null);
			}

			// NOTE: Значит надо создать, если промоакция - есть промоакция
			var checkResult = this.IsPromoActionExists(promoActionId);

			if (!checkResult.Item1)
			{
				// NOTE: Нельзя создать ЦА так как правило не промоакция!
				this.cache.Add(promoActionId, CacheItem.BuildFail(checkResult.Item2));
				return new Tuple<bool, TargetAudience, string>(false, null, checkResult.Item2);
			}

			var ta = TargetAudience.BuildTargetAudience(targetAudienceId, promoActionId, userId);
			this.targetAudienceRepository.Save(ta);

			this.cache.Add(promoActionId, CacheItem.BuildSuccess(ta));
			return new Tuple<bool, TargetAudience, string>(true, ta, null);
		}

		private Tuple<bool, string> IsPromoActionExists(string promoActionId)
		{
			long promoActionIdAsLong;

			if (!long.TryParse(promoActionId, out promoActionIdAsLong))
			{
				var description = string.Format("PromoActionId has incorrect format ({0})", promoActionId);
				log.Error(description);
				return new Tuple<bool, string>(false, description);
			}

			var targetRule = this.ruleRepository.Get(promoActionIdAsLong);

			if (targetRule == null)
			{
				var description = string.Format("Target rule is not founded, rule id: ({0})", promoActionId);
				log.Error(description);
				return new Tuple<bool, string>(false, description);
			}

			try
			{
				if (!targetRule.IsPromoAction(false))
				{
					var description = string.Format("Target rule is not promo action: rule id ({0})", targetRule.Id);
					log.Error(description);
					return new Tuple<bool, string>(false, description);
				}
			}
			catch (Exception ex)
			{
				var description = string.Format("Error on check rule predicate: rule id ({0}), predicate ({1})", targetRule.Id, targetRule.Predicate);
				log.Error(description, ex);
				return new Tuple<bool, string>(false, description);
			}

			return new Tuple<bool, string>(true, null);
		}

		internal class CacheItem
		{
			public bool IsSuccess
			{
				get { return string.IsNullOrWhiteSpace(this.Description); }
			}

			public string Description { get; set; }

			public TargetAudience TargetAudience { get; set; }

			public static CacheItem BuildSuccess(TargetAudience targetAudience)
			{
				return new CacheItem { TargetAudience = targetAudience, Description = null };
			}

			public static CacheItem BuildFail(string description)
			{
				return new CacheItem { TargetAudience = null, Description = description };
			}
		}
	}
}