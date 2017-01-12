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
		-- �������� ��� ������ ���� @kladr ��� ��������� �����
		SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],1 as [Type], [Priority], CarrierId, ExternalLocationId
		FROM [prod].[PartnerDeliveryRates]
		WHERE 
			SUBSTRING(@kladr, 9, 3) != '000' 
			AND 
			SUBSTRING(kladr, 1, 11) = SUBSTRING(@kladr, 1, 11) 
			AND 
			CAST(SUBSTRING(kladr, 12, LEN(kladr)-11) AS BIGINT) = 0 
		UNION
		-- �������� ��� ������ ���� @kladr ��� �����
		SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],2 as [Type], [Priority], CarrierId, ExternalLocationId
		FROM [prod].[PartnerDeliveryRates]
		WHERE 
			SUBSTRING(@kladr, 6, 3) != '000' 
			AND 
			SUBSTRING(kladr, 1, 8) = SUBSTRING(@kladr, 1, 8) 
			AND 
			CAST(SUBSTRING(kladr, 9, LEN(kladr)-8) AS BIGINT) = 0
			-- NOTE: ���������� ����������� ���.����� (�����, city), ���� ���� ��� "����������" ���.�����
			AND 
			SUBSTRING(@kladr, 9, 3) = '000' 
		UNION
		-- �������� ��� ������ ���� @kladr ��� �����
		SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],3 as [Type], [Priority], CarrierId, ExternalLocationId
		FROM [prod].[PartnerDeliveryRates]
		WHERE 
			SUBSTRING(@kladr, 3, 3) != '000' 
			AND 
			SUBSTRING(kladr, 1, 5) = SUBSTRING(@kladr, 1, 5) 
			AND 
			CAST(SUBSTRING(kladr, 6, LEN(kladr)-5) AS BIGINT) = 0
		UNION
		-- �������� ��� ������ ���� @kladr ��� ������
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