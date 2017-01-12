ALTER view [prod].[PartnerDeliveryRates]
AS

WITH Rates(PartnerId, CarrierId, KLADR, MinWeightGram, MaxWeightGram, PriceRur, [ExternalLocationId], [Status]) AS 
(	
	SELECT 
		P.Id AS PartnerId, 
		P.CarrierId, 
		dl.KLADR, 
		dr.MinWeightGram, 
		dr.MaxWeightGram,
		dr.PriceRur,
		dl.[ExternalLocationId],
		dl.[Status]
	FROM prod.Partners P with (NOLOCK)
	LEFT JOIN prod.DeliveryRates dr with (NOLOCK) ON P.Id = dr.PartnerId
	LEFT JOIN prod.DeliveryLocations dl with (NOLOCK) ON dr.LocationId = dl.Id
	WHERE P.[Status] = 1
),
Rates2 AS
(
	--get own price
	SELECT	r.PartnerId, 
			r.CarrierId, 
			r.KLADR, 
			r.MinWeightGram, 
			r.MaxWeightGram, 
			r.PriceRur, 
			r.[ExternalLocationId], 
			1 AS [Priority] FROM Rates r with (NOLOCK)
	WHERE r.[Status] = 1
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
	FROM Rates r1 with (NOLOCK)
	JOIN Rates r2  with (NOLOCK) ON r1.PartnerId = r2.CarrierId
	WHERE r1.[Status] = 1
)
SELECT CAST(ROW_NUMBER() OVER(ORDER BY PartnerId) AS int) AS Id, * FROM Rates2
GO
