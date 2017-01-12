IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[prod].[PartnerDeliveryRates]'))
	DROP VIEW [prod].[PartnerDeliveryRates]
GO

CREATE view [prod].[PartnerDeliveryRates]
AS

WITH Rates(PartnerId, CarrierId, KLADR, MinWeightGram, MaxWeightGram, PriceRur, [ExternalLocationId]) AS 
(	
	SELECT 
		P.Id AS PartnerId, 
		P.CarrierId, 
		dl.KLADR, 
		DR.MinWeightGram, 
		DR.MaxWeightGram,
		DR.PriceRur,
		DR.[ExternalLocationId]
	FROM prod.Partners P	
	LEFT JOIN prod.DeliveryRates DR	ON P.Id = DR.PartnerId
	LEFT JOIN prod.DeliveryLocations dl ON DR.LocationId = dl.Id
	WHERE P.Status = 1
),
Rates2 AS
(
	--get own price
	SELECT *, 1 AS [Priority] FROM Rates r
	WHERE r.KLADR is not null --don't include records without own price
	UNION ALL
	--add carrier price
	SELECT 
		r2.PartnerId, 
		r2.CarrierId, 
		r1.KLADR, 
		r1.MinWeightGram, 
		r1.MaxWeightGram, 
		r1.PriceRur, 
		r1.[ExternalLocationId],
		CASE WHEN r1.CarrierId IS NULL THEN 0 ELSE 1 END AS [Priority]
	FROM Rates r1
	JOIN Rates r2 ON r1.PartnerId = r2.CarrierId
	WHERE r1.KLADR is not null --don't include records without carrier price
)
SELECT CAST(ROW_NUMBER() OVER(ORDER BY PartnerId) AS int) AS Id, * FROM Rates2
GO


