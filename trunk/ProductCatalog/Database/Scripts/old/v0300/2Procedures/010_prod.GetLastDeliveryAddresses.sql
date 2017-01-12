IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetLastDeliveryAddresses]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[GetLastDeliveryAddresses]
GO

CREATE PROCEDURE [prod].[GetLastDeliveryAddresses]
	@clientId [nvarchar](255),
	@countToTake [int]
AS
BEGIN

	--declare @countToTake int
	--set @countToTake = 7

	SELECT 
		ord.Id,
		ord.DeliveryInfo,
		AddrText
	FROM
	(
		SELECT TOP (@countToTake)
			 t.AddrText
			,MAX(t.Id) AS Id
		FROM
		(
			SELECT
				ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/CountryCode[1]', 'nvarchar(256)'), '') +
				ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/PostCode[1]', 'nvarchar(256)'), '') +
				ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/RegionKladrCode[1]', 'nvarchar(256)'), '') +
				ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/DistrictKladrCode[1]', 'nvarchar(256)'), '') +
				ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/CityKladrCode[1]', 'nvarchar(256)'), '') +
				ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/TownKladrCode[1]', 'nvarchar(256)'), '') +
				ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/StreetName[1]', 'nvarchar(256)'), '') +
				ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/House[1]', 'nvarchar(256)'), '') +
				ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/Flat[1]', 'nvarchar(256)'), '') AS AddrText,
				Id
			FROM [prod].Orders
			WHERE [ClientId] = @clientId and [DeliveryInfo] is not null
		) AS t
		GROUP BY t.AddrText
		ORDER BY MAX(t.Id) DESC
	) t2
	INNER JOIN [prod].Orders ord ON ord.Id = t2.Id

END
GO


