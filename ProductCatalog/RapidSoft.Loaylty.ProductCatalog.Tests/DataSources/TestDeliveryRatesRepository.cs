namespace RapidSoft.Loaylty.ProductCatalog.ImportTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using RapidSoft.Loaylty.ProductCatalog.API.Entities;
	using RapidSoft.Loaylty.ProductCatalog.DataSources;
	using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
	using RapidSoft.Loaylty.ProductCatalog.Tests.DataSources;

    internal class TestDeliveryRatesRepository : BaseRepository
	{
		public TestDeliveryRatesRepository()
			: base(DataSourceConfig.ConnectionString)
		{
		}

		public IList<DeliveryRate> GetByPartnerId(int partnerId)
		{
			const string SQL = @"SELECT	dr.[Id],
		dr.[PartnerId],
		dl.[Kladr],
		dl.[Id] AS [LocationId],
		dl.[LocationName],
		dr.[MinWeightGram],
		dr.[MaxWeightGram],
		dr.[PriceRUR],
		dl.[ExternalLocationId],
		dl.[Status]
  FROM [prod].[DeliveryRates] dr
  JOIN [prod].[DeliveryLocations] dl ON dr.[LocationId] = dl.[Id]
WHERE dr.[PartnerId] = {0}";
			using (var ctx = this.DbNewContext())
			{
				var retVal = ctx.Database.SqlQuery<DeliveryRate>(SQL, partnerId).ToList();
				return retVal;
			}
		}

        public DeliveryRate CreateIfNotExists(
            int partnerId, int minWeightGram, int maxWeightGram, decimal priceRur, string kladr)
        {
            var rates = this.GetByPartnerId(partnerId);

            var rate = rates.FirstOrDefault(x => x.Kladr == kladr);
            if (rate == null)
            {
                return this.Create(partnerId, minWeightGram, maxWeightGram, priceRur, kladr);
            }

            return rate;
        }

        public DeliveryRate Create(int partnerId, int minWeightGram, int maxWeightGram, decimal priceRur, string kladr, string locationName = null, DeliveryLocationStatus status = DeliveryLocationStatus.CorrectBinded)
		{
		    var deliveryRate = new DeliveryRate
		                           {
		                               PartnerId = partnerId,
		                               MinWeightGram = minWeightGram,
		                               MaxWeightGram = maxWeightGram,
		                               PriceRUR = priceRur,
		                               Kladr = kladr,
                                       LocationName = locationName,
                                       Status = (int)status
		                           };
			return this.Create(deliveryRate);
		}

		public DeliveryRate Create(DeliveryRate rate)
		{
            var sql = @"DECLARE @LocationId int 

SELECT @LocationId = [Id] FROM [prod].[DeliveryLocations] WHERE [PartnerId] = {0} AND [LocationName] = {1}
IF (@LocationId IS NULL)
BEGIN
    INSERT INTO [prod].[DeliveryLocations]
    ([PartnerId],[LocationName],[Kladr],[Status],[InsertDateTime],[ExternalLocationId])
    VALUES
    ({0},{1},{2},{3},GetDate(),{7})

    SELECT @LocationId = SCOPE_IDENTITY()
END
ELSE
BEGIN
    UPDATE [prod].[DeliveryLocations]
    SET [Kladr] = {2}, [Status] = {3}, [ExternalLocationId] = {7}, [UpdateDateTime] = GetDate(), [UpdateUserId] = 'SystemTest'
    WHERE [PartnerId] = {0} AND [LocationName] = {1}
END

INSERT INTO [prod].[DeliveryRates]
([PartnerId],[MinWeightGram],[MaxWeightGram],[PriceRur],[LocationId])
VALUES
({0},{4},{5},{6},@LocationId)

SELECT CAST(SCOPE_IDENTITY() AS int)";
			using (var ctx = this.DbNewContext())
			{
				var id = ctx.Database.SqlQuery<int>(
					sql,
					rate.PartnerId,
					rate.LocationName,
					rate.Kladr,
					rate.Status,
					rate.MinWeightGram,
					rate.MaxWeightGram,
					rate.PriceRUR,
					rate.ExternalLocationId).Single();
				rate.Id = id;

			    rate.LocationId =
			        ctx.Database.SqlQuery<int>("SELECT [LocationId] FROM [prod].[DeliveryRates] WHERE [Id] = {0}", rate.Id)
			           .Single();

				return rate;
			}
		}

		public void DeleteByPartnerId(int partnerId)
		{
			const string SQL = @"DELETE FROM [prod].[DeliveryRates]
	  WHERE [PartnerId] = {0}

DELETE FROM [prod].[DeliveryLocations]
	  WHERE [PartnerId] = {0}";
			using (var ctx = this.DbNewContext())
			{
				ctx.Database.ExecuteSqlCommand(SQL, partnerId);
			}
		}

		public void DeleteByPartnerIdAndKladrCodeLike(int partnerId, string kladrCodeLike)
		{
			const string SQL = @"DELETE FROM [prod].[DeliveryRates]
WHERE [LocationId] IN (	SELECT [Id] 
						FROM [prod].[DeliveryLocations]
						WHERE [PartnerId] = {0} AND [Kladr] LIKE {1})
						
DELETE FROM [prod].[DeliveryLocations]
WHERE [PartnerId] = {0} AND [Kladr] LIKE {1}";
			using (var ctx = this.DbNewContext())
			{
				ctx.Database.ExecuteSqlCommand(SQL, partnerId, kladrCodeLike);
			}
		}

		public void Delete(int id)
		{
			const string SQL = @"DECLARE @LocationId int

SELECT @LocationId = [LocationId]
FROM [prod].[DeliveryRates] dr
WHERE dr.[Id] = {0}

DELETE FROM [prod].[DeliveryRates]
WHERE [Id] = {0}

IF NOT EXISTS(SELECT * FROM [prod].[DeliveryRates] WHERE [LocationId] = @LocationId)
BEGIN
	DELETE [prod].[DeliveryLocations]
	WHERE [Id] = @LocationId
END";
			using (var ctx = this.DbNewContext())
			{
				ctx.Database.ExecuteSqlCommand(SQL, id);
			}
		}
	}
}