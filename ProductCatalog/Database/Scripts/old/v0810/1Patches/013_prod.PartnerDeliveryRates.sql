IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[prod].[PartnerDeliveryRates]'))
	DROP VIEW [prod].[PartnerDeliveryRates]
GO

CREATE view [prod].[PartnerDeliveryRates]
as

with Rates(PartnerId, CarrierId, KLADR, MinWeightGram, MaxWeightGram, PriceRur, [ExternalLocationId]) as 
(	
	select 
		P.Id as PartnerId, 
		P.CarrierId, 
		DR.KLADR, 
		DR.MinWeightGram, 
		DR.MaxWeightGram,
		DR.PriceRur,
		DR.[ExternalLocationId]
	from prod.Partners P	
	left join prod.DeliveryRates DR	
	on P.Id = DR.PartnerId
	where P.Status = 1
),
Rates2 as
(
--get own price
select *, 1 as [Priority] from Rates r
where r.KLADR is not null --don't include records without own price

union all

--add carrier price
select 
	r2.PartnerId, 
	r2.CarrierId, 
	r1.KLADR, 
	r1.MinWeightGram, 
	r1.MaxWeightGram, 
	r1.PriceRur, 
	r1.[ExternalLocationId],
	CASE WHEN r1.CarrierId IS NULL THEN 0 ELSE 1 END as [Priority]
from Rates r1
join Rates r2 on r1.PartnerId = r2.CarrierId
where r1.KLADR is not null --don't include records without carrier price
)
select cast(ROW_NUMBER() over(order by PartnerId) as int) as Id, * from Rates2


GO


