namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;

	using API.Entities;
	using DataSources;
	using Interfaces;

	using RapidSoft.Extensions;
    using RapidSoft.Loaylty.ProductCatalog.Entities;

    internal class DeliveryRatesRepository : BaseRepository, IDeliveryRatesRepository
	{
		public DeliveryRatesRepository()
			: base(DataSourceConfig.ConnectionString)
		{
		}

        public bool HasDelivery(int partnerId, string kladr)
        {
            using (var ctx = DbNewContext())
            {
                return GetRatesByLocationQuery(ctx, kladr)
                    .Any(r => r.PartnerId == partnerId);
            }
        }

        public int[] GetDeliveringPartnerIds(string kladr)
        {
            using (var ctx = DbNewContext())
            {
                return GetRatesByLocationQuery(ctx, kladr)
                    .Select(r => r.PartnerId)
                    .Distinct()
                    .ToArray();
            }
        }
        
        public PartnerDeliveryRate GetMinPriceRate(int partnerId, string kladr, int weight)
        {
            using (var ctx = DbNewContext())
            {
                return GetRatesByLocationQuery(ctx, kladr).Where(
                    r => r.PartnerId == partnerId
                        &&
                        r.MinWeightGram <= weight
                        &&
                        weight <= r.MaxWeightGram).OrderBy(r => r.Priority).ThenBy(r => r.Type).FirstOrDefault();
            }
        }

		public Page<DeliveryLocation> GetDeliveryLocations(
			int partnerId, DeliveryLocationStatus[] statuses, int? countToSkip, int? countToTake, bool? calcTotalCount, bool? hasRates, string searchTerm)
		{
			using (var ctx = this.DbNewContext())
			{
				var query = ctx.DeliveryBindings.Where(b => b.PartnerId == partnerId);

				if (statuses != null && statuses.Length > 0)
				{
					query = query.Where(b => statuses.Contains(b.Status));
				}

				if (!string.IsNullOrWhiteSpace(searchTerm))
				{
					query = query.Where(x => x.Kladr.Contains(searchTerm) || x.LocationName.Contains(searchTerm));
				}

				if (hasRates.HasValue)
				{
					var hasRatesValue = hasRates.Value;

					if (hasRatesValue)
					{
						query =
							query.GroupJoin(
								ctx.DeliveryRates, dl => dl.Id, dr => dr.LocationId, (location, rates) => new { location, rates })
								 .Where(x => x.rates.Any())
								 .Select(x => x.location);
					}
					else
					{
						query =
							query.GroupJoin(
								ctx.DeliveryRates, dl => dl.Id, dr => dr.LocationId, (location, rates) => new { location, rates })
								 .Where(x => !x.rates.Any())
								 .Select(x => x.location);
					}
				}

				int? totalCount = null;
				if (calcTotalCount.HasValue && calcTotalCount.Value)
				{
					totalCount = query.Count();
				}

				query = query.OrderBy(x => x.LocationName);

				if (countToSkip.HasValue)
				{
					query = query.Skip(countToSkip.Value);
				}

				if (countToTake.HasValue)
				{
					query = query.Take(countToTake.Value);
				}

				var retList = query.ToList();

				return new Page<DeliveryLocation>(retList, countToSkip, countToTake, totalCount);
			}
		}

		public DeliveryLocation GetDeliveryLocation(int id)
		{
			using (var ctx = this.DbNewContext())
			{
				var retVal = ctx.DeliveryBindings.SingleOrDefault(x => x.Id == id);
				return retVal;
			}
		}

		public List<DeliveryLocation> GetDeliveryLocationsByPartnerAndKladr(int partnerId, string kladrCode, int? countToSkip, int? countToTake)
		{
			using (var ctx = this.DbNewContext())
			{
				var query = ctx.DeliveryBindings.Where(x => x.PartnerId == partnerId && x.Kladr == kladrCode);

				if (countToSkip.HasValue || countToTake.HasValue)
				{
					query = query.OrderBy(x => x.Id);
				}

				if (countToSkip.HasValue)
				{
					query = query.Skip(countToSkip.Value);
				}

				if (countToTake.HasValue)
				{
					query = query.Take(countToTake.Value);
				}

				var retVal = query.ToList();
				return retVal;
			}
		}

		public DeliveryLocation GetDeliveryLocationByPartnerAndKladr(int partnerId, string kladrCode, DeliveryLocationStatus[] statuses, int[] excludeIds, bool? hasRates)
		{
			using (var ctx = this.DbNewContext())
			{
				var query = ctx.DeliveryBindings.Where(x => x.PartnerId == partnerId && x.Kladr == kladrCode);
				if (excludeIds != null && excludeIds.Length > 0)
				{
					query = query.Where(x => !excludeIds.Contains(x.Id));
				}

				if (statuses != null && statuses.Length > 0)
				{
					query = query.Where(x => statuses.Contains(x.Status));
				}

				if (hasRates.HasValue)
				{
					var hasRatesValue = hasRates.Value;

					if (hasRatesValue)
					{
						query =
							query.GroupJoin(
								ctx.DeliveryRates, dl => dl.Id, dr => dr.LocationId, (location, rates) => new { location, rates })
								 .Where(x => x.rates.Any())
								 .Select(x => x.location);
					}
					else
					{
						query =
							query.GroupJoin(
								ctx.DeliveryRates, dl => dl.Id, dr => dr.LocationId, (location, rates) => new { location, rates })
								 .Where(x => !x.rates.Any())
								 .Select(x => x.location);
					}
				}

				var retVal = query.FirstOrDefault();
				return retVal;
			}
		}

		public DeliveryLocation SaveDeliveryLocation(DeliveryLocation deliveryLocation)
		{
			deliveryLocation.ThrowIfNull("deliveryBinding");

			using (var ctx = this.DbNewContext())
			{
				if (deliveryLocation.Id == 0)
				{
					if (deliveryLocation.InsertDateTime == default(DateTime))
					{
						deliveryLocation.InsertDateTime = DateTime.Now;
					}

					ctx.Entry(deliveryLocation).State = EntityState.Added;
				}
				else
				{
					if (!deliveryLocation.UpdateDateTime.HasValue)
					{
						deliveryLocation.UpdateDateTime = DateTime.Now;
					}

					ctx.Entry(deliveryLocation).State = EntityState.Modified;
				}

				ctx.SaveChanges();

				return deliveryLocation;
			}
		}

		public Page<DeliveryLocationHistory> GetDeliveryLocationHistory(int? locationId, int partnerId, int? countToSkip, int? countToTake, bool? calcTotalCount)
		{
			using (var ctx = this.DbNewContext())
			{
				var query = ctx.DeliveryBindingHistory.Where(b => b.PartnerId == partnerId);

				if (locationId.HasValue)
				{
					query = query.Where(x => x.Id == locationId.Value);
				}

				int? totalCount = null;
				if (calcTotalCount.HasValue && calcTotalCount.Value)
				{
					totalCount = query.Count();
				}

				query = query.OrderByDescending(x => x.TriggerDate);

				if (countToSkip.HasValue)
				{
					query = query.Skip(countToSkip.Value);
				}

				if (countToTake.HasValue)
				{
					query = query.Take(countToTake.Value);
				}

				var retList = query.ToList();

				return new Page<DeliveryLocationHistory>(retList, countToSkip, countToTake, totalCount);
			}
		}

		public Page<string> GetKladrCodesFromBuffer(string etlSessionId, int countToSkip, int countToTake)
		{
			using (var ctx = this.DbNewContext())
			{
				const string SQL = @"SELECT TOP ({1}) [KLADR]
FROM (	SELECT [KLADR], ROW_NUMBER() OVER (ORDER BY [KLADR]) as RowNum 
		FROM (	SELECT DISTINCT [KLADR]
				  FROM [prod].[BUFFER_DeliveryRates]
				WHERE [EtlSessionId] = {0}
				  AND [KLADR] IS NOT NULL
				  AND [Status] = 0) tab) page
WHERE page.RowNum > {2}";

				var list = ctx.Database.SqlQuery<string>(SQL, etlSessionId, countToTake, countToSkip);

				return new Page<string>(list);
			}
		}

		public void SetDeliveryBufferStatusByKladr(string etlSessionId, IList<string> kladrCodes, int status)
		{
			var paramIndex = Enumerable.Range(2, kladrCodes.Count).Select(x => "{" + x + "}");

			var kladres = string.Join(",", paramIndex);

			var sql = @"UPDATE [prod].[BUFFER_DeliveryRates]
   SET [Status] = {0}
 WHERE [EtlSessionId] = {1}
   AND [KLADR] IN (" + kladres + ")";

			var paramValues = status.MakeArray<object>(etlSessionId).Union(kladrCodes).ToArray();

			using (var ctx = this.DbNewContext())
			{
				ctx.Database.ExecuteSqlCommand(sql, paramValues);
			}
		}

        private static IQueryable<PartnerDeliveryRate> GetRatesByLocationQuery(LoyaltyDBEntities context, string kladr)
        {
            var rates = GetAllRatesQuery(context);

            // для регионов берем всегда
            var query = GetRatesByLocationSubquery(rates, kladr, 2, 4);

            // для районов
            if (kladr.Substring(2, 3) != "000")
            {
                query = query.Union(GetRatesByLocationSubquery(rates, kladr, 5, 3));
            }

            // для города (только если мы не ищем для нас.пункта)
            if (kladr.Substring(5, 3) != "000" && kladr.Substring(8, 3) == "000")
            {
                query = query.Union(GetRatesByLocationSubquery(rates, kladr, 8, 2));
            }

            // для нас.пункта
            if (kladr.Substring(8, 3) != "000")
            {
                query = query.Union(GetRatesByLocationSubquery(rates, kladr, 11, 1));
            }

            return query;
        }

        private static IQueryable<PartnerDeliveryRate> GetRatesByLocationSubquery(IQueryable<PartnerDeliveryRate> ratesQuery, string kladr, int pos, int type)
        {
            if (kladr == null)
            {
                throw new ArgumentNullException("kladr");
            }

            kladr = kladr.Substring(0, pos) + new string('0', 13 - pos);

            return ratesQuery.Where(r => r.Kladr == kladr)
                             .Select(r => new PartnerDeliveryRate
                             {
                                 PartnerId = r.PartnerId,
                                 CarrierId = r.CarrierId,
                                 Kladr = r.Kladr,
                                 MinWeightGram = r.MinWeightGram,
                                 MaxWeightGram = r.MaxWeightGram,
                                 PriceRur = r.PriceRur,
                                 ExternalLocationId = r.ExternalLocationId,
                                 Priority = r.Priority,
                                 Type = type
                             });
        }

        private static IQueryable<PartnerDeliveryRate> GetAllRatesQuery(LoyaltyDBEntities context)
        {
            // получаем всех активных партнеров
            var partners = context.Partners.Where(p => p.Status == PartnerStatus.Active);

            // объединям партнеров с их курьерами (должны быть указаны и активны)
            var partnersWithCarriers = partners.GroupJoin(
                partners,
                p => p.CarrierId,
                c => c.Id,
                (p, c) => new { partner = p, carrier = c.FirstOrDefault() });

            // объединями тарифы доставки с местами
            var ratesWithLocations = context.DeliveryRates.Join(
                context.DeliveryBindings.Where(l => l.Status == DeliveryLocationStatus.CorrectBinded),
                r => r.LocationId,
                l => l.Id,
                (r, l) => new { rate = r, location = l });

            // для каждого партнера находим все его собственные тарифы (указываем им Priority=1),
            // а также тарифы его курьера (Priority=2)
            var rates = partnersWithCarriers.SelectMany(
                pc => ratesWithLocations.Where(rl => pc.partner.Id == rl.rate.PartnerId ||
                                                     (pc.carrier != null && pc.carrier.Id == rl.rate.PartnerId))
                                        .Select(rl => new PartnerDeliveryRate
                                        {
                                            PartnerId = pc.partner.Id,
                                            CarrierId = pc.partner.Id == rl.rate.PartnerId ? (int?)null : rl.rate.PartnerId,
                                            Kladr = rl.location.Kladr,
                                            MinWeightGram = rl.rate.MinWeightGram,
                                            MaxWeightGram = rl.rate.MaxWeightGram,
                                            PriceRur = rl.rate.PriceRur,
                                            ExternalLocationId = rl.location.ExternalLocationId,
                                            Priority = pc.partner.Id == rl.rate.PartnerId ? 1 : 2,
                                            Type = 0
                                        }));

            return rates;
        }
	}
}