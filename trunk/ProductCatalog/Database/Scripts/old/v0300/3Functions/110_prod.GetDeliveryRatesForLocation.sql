IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetDeliveryRatesForLocation]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [prod].[GetDeliveryRatesForLocation]
GO

CREATE function [prod].[GetDeliveryRatesForLocation] ( @kladr nvarchar(32) )
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
-- Получаем все тарифы если @kladr это населённый пункт
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],1 as [Type], [Priority], CarrierId
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(@kladr, 9, 3) != '000' AND 
	SUBSTRING(kladr, 1, 11) = SUBSTRING(@kladr, 1, 11) AND CAST(SUBSTRING(kladr, 12, LEN(kladr)-11) AS BIGINT) = 0
UNION
-- Получаем все тарифы если @kladr это город
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],2 as [Type], [Priority], CarrierId
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(@kladr, 6, 3) != '000' AND 
	SUBSTRING(kladr, 1, 8) = SUBSTRING(@kladr, 1, 8) AND CAST(SUBSTRING(kladr, 9, LEN(kladr)-8) AS BIGINT) = 0
	-- NOTE: Пропускаем вышестоящий нас.пункт (город, city), если ищем для "маленького" нас.пункт
	AND SUBSTRING(@kladr, 9, 3) = '000' 
UNION
-- Получаем все тарифы если @kladr это район
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],3 as [Type], [Priority], CarrierId
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(@kladr, 3, 3) != '000' AND 
	SUBSTRING(kladr, 1, 5) = SUBSTRING(@kladr, 1, 5) AND CAST(SUBSTRING(kladr, 6, LEN(kladr)-5) AS BIGINT) = 0
UNION
-- Получаем все тарифы если @kladr это регион
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],4 as [Type], [Priority], CarrierId
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(kladr, 1, 2) = SUBSTRING(@kladr, 1, 2) AND CAST(SUBSTRING(kladr, 3, LEN(kladr)-2) AS BIGINT) = 0
) p
GROUP BY [PartnerId],[MinWeightGram],[MaxWeightGram],[PriceRur], [Priority], CarrierId

  return
end;


GO


