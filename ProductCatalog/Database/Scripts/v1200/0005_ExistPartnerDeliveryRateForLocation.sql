if exists(select * from sys.objects where name='ExistPartnerDeliveryRateForLocation')
	DROP FUNCTION [prod].[ExistPartnerDeliveryRateForLocation]
GO

create function [prod].[ExistPartnerDeliveryRateForLocation] (@partnerId int, @kladr nvarchar(32))
returns bit
as 
begin

declare @res bit

if exists(
	SELECT TOP 1 *
	FROM
	(
		-- Получаем все тарифы если @kladr это населённый пункт
		SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],1 as [Type], [Priority], CarrierId, ExternalLocationId
		FROM [prod].[PartnerDeliveryRates]
		WHERE 
			SUBSTRING(@kladr, 9, 3) != '000' 
			AND 
			SUBSTRING(kladr, 1, 11) = SUBSTRING(@kladr, 1, 11) 
			AND 
			CAST(SUBSTRING(kladr, 12, LEN(kladr)-11) AS BIGINT) = 0 
		UNION
		-- Получаем все тарифы если @kladr это город
		SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],2 as [Type], [Priority], CarrierId, ExternalLocationId
		FROM [prod].[PartnerDeliveryRates]
		WHERE 
			SUBSTRING(@kladr, 6, 3) != '000' 
			AND 
			SUBSTRING(kladr, 1, 8) = SUBSTRING(@kladr, 1, 8) 
			AND 
			CAST(SUBSTRING(kladr, 9, LEN(kladr)-8) AS BIGINT) = 0
			-- NOTE: Пропускаем вышестоящий нас.пункт (город, city), если ищем для "маленького" нас.пункт
			AND 
			SUBSTRING(@kladr, 9, 3) = '000' 
		UNION
		-- Получаем все тарифы если @kladr это район
		SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],3 as [Type], [Priority], CarrierId, ExternalLocationId
		FROM [prod].[PartnerDeliveryRates]
		WHERE 
			SUBSTRING(@kladr, 3, 3) != '000' 
			AND 
			SUBSTRING(kladr, 1, 5) = SUBSTRING(@kladr, 1, 5) 
			AND 
			CAST(SUBSTRING(kladr, 6, LEN(kladr)-5) AS BIGINT) = 0
		UNION
		-- Получаем все тарифы если @kladr это регион
		SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],4 as [Type], [Priority], CarrierId, ExternalLocationId
		FROM [prod].[PartnerDeliveryRates]
		WHERE 
			SUBSTRING(kladr, 1, 2) = SUBSTRING(@kladr, 1, 2) 
			AND 
			CAST(SUBSTRING(kladr, 3, LEN(kladr)-2) AS BIGINT) = 0
	) p
	where 
		PartnerId = @partnerId
)
begin
	return 1
end
return 0

end;