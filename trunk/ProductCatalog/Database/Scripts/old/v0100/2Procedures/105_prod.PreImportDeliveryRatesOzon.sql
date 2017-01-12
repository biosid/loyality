IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[PreImportDeliveryRatesOzon]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[PreImportDeliveryRatesOzon]
GO

CREATE  PROCEDURE [prod].[PreImportDeliveryRatesOzon]
	@EtlPackageId [nvarchar](64),  -- NOTE: ����� �������������� ��� �����������
	@EtlSessionId [nvarchar](64)
AS
BEGIN
	SET NOCOUNT ON;
		
	BEGIN TRAN
	
	--not resolved kladr set status: -3
	;WITH couldNotResolve AS (
		select M.KLADR, B.* FROM [prod].[BUFFER_DeliveryRates_Ozon] B
		left join prod.KladrOzonMapping M
		on UPPER(B.City) = M.City and UPPER(B.Region) = M.Region 
		and B.[EtlSessionId] = @EtlSessionId 
		where KLADR is null	
	)

	UPDATE [prod].[BUFFER_DeliveryRates_Ozon]
	SET [Status] = -3 -- not resolved
	WHERE Id in (select Id from couldNotResolve )
	and [EtlSessionId] = @EtlSessionId 
	
	--resolved kladr set status 1
	;WITH couldNotResolve AS (
		select M.KLADR, B.* FROM [prod].[BUFFER_DeliveryRates_Ozon] B
		left join prod.KladrOzonMapping M
		on UPPER(B.City) = M.City and UPPER(B.Region) = M.Region 
		and B.[EtlSessionId] = @EtlSessionId 
		where KLADR is null	
	)
	UPDATE [prod].[BUFFER_DeliveryRates_Ozon]
	SET [Status] = 1 -- resolved
	WHERE Id not in (select Id from couldNotResolve )
	and [EtlSessionId] = @EtlSessionId 
		
	-- ��������� ���������� �������� � ���������� � [dbo].[EtlCounters]
	INSERT INTO [dbo].[EtlCounters]
		([EtlPackageId],[EtlSessionId],[EntityName],[CounterName],[CounterValue],[LogDateTime],[LogUtcDateTime])
	SELECT	@EtlPackageId,
			[EtlSessionId],
			'����',
			CASE	
				WHEN [Status] = 0 THEN '�� ����������'
				WHEN [Status] < 0 THEN '���������'
				ELSE '�������������'
			END AS Name, 
			COUNT(*) AS Value, 
			GetDate(), 
			GetUtcDate()
	FROM [prod].[BUFFER_DeliveryRates_Ozon]
	WHERE [EtlSessionId] = @EtlSessionId
	GROUP BY	[EtlSessionId], 
				CASE 
					WHEN [Status] = 0 THEN '�� ����������'
					WHEN [Status] < 0 THEN '���������'
					ELSE '�������������'
				END
	
	--insert only resolved
	INSERT INTO [prod].[BUFFER_DeliveryRates]
		([EtlSessionId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],[DeliveryPeriod],[Status])
	SELECT B.[EtlSessionId]		  
		  ,M.KLADR
		  ,B.[MinWeightGram]
		  ,B.[MaxWeightGram]
		  ,B.[PriceRur]
		  ,NULL		-- � ����� ��� DeliveryPeriod
		  ,0		-- ������� ������ ������ ����� ������ ����������, 
	  FROM [prod].[BUFFER_DeliveryRates_Ozon] B
	  join prod.KladrOzonMapping M
	  on UPPER(B.City) = M.City and UPPER(B.Region) = M.Region
	WHERE [EtlSessionId] = @EtlSessionId
	  AND [Status] = 1 -- inner join = kladr resolved!.
	
	COMMIT TRAN
END
GO


