
alter function [prod].[GetDeliveryRatesForLocation] ( @kladr nvarchar(32) )
returns @paramtable table ( 
	PartnerId int,  
	WeightMin int,
	WeightMax int,
	Price decimal(38, 20),
	[Type] int,
	[Priority] int,
	CarrierId int
) 
as begin

insert into @paramtable
SELECT [PartnerId],[MinWeightGram],[MaxWeightGram],[PriceRur],MIN([Type]) AS [Type], [Priority], CarrierId
FROM
(
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],1 as [Type], [Priority], CarrierId
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(@kladr, 9, 3) != '000' AND 
	SUBSTRING(kladr, 1, 11) = SUBSTRING(@kladr, 1, 11) AND CAST(SUBSTRING(kladr, 12, LEN(kladr)-11) AS BIGINT) = 0
UNION
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],2 as [Type], [Priority], CarrierId
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(@kladr, 6, 3) != '000' AND 
	SUBSTRING(kladr, 1, 8) = SUBSTRING(@kladr, 1, 8) AND CAST(SUBSTRING(kladr, 9, LEN(kladr)-8) AS BIGINT) = 0
UNION
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],3 as [Type], [Priority], CarrierId
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(@kladr, 3, 3) != '000' AND 
	SUBSTRING(kladr, 1, 5) = SUBSTRING(@kladr, 1, 5) AND CAST(SUBSTRING(kladr, 6, LEN(kladr)-5) AS BIGINT) = 0
UNION
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],4 as [Type], [Priority], CarrierId
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(kladr, 1, 2) = SUBSTRING(@kladr, 1, 2) AND CAST(SUBSTRING(kladr, 3, LEN(kladr)-2) AS BIGINT) = 0
) p
GROUP BY [PartnerId],[MinWeightGram],[MaxWeightGram],[PriceRur], [Priority], CarrierId

  return
end;


GO


