IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[ImportDeliveryRates]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[ImportDeliveryRates]
GO

CREATE PROCEDURE [prod].[ImportDeliveryRates]
	@EtlPackageId [nvarchar](64), 
	@EtlSessionId [nvarchar](64), 
	@PartnerId int
AS
BEGIN
	SET NOCOUNT ON;
		
	BEGIN TRAN
		
	MERGE [prod].[DeliveryLocations] AS target
	USING (	
			SELECT DISTINCT	dr.[LocationName],
					dr.[ExternalLocationId],
					-- NOTE: ���� ������ ��� � ������, �� ���� � �������, �� ���������� �� �������
					ISNULL(SUBSTRING(dr.[KLADR],1,13),dl.[Kladr]) AS [KLADR], 
					CASE
						-- NOTE: ���� ������ ��� � ������, �� ���� � �������, �� ��������� ������ �� �������
						WHEN dr.[KLADR] IS NULL AND dl.[Kladr] IS NOT NULL THEN dl.[Status]	
						-- NOTE: ���� ������ ��� �����, �� ������ 2 - �� ����� ���� �����
						WHEN dr.[KLADR] IS NULL AND dl.[Kladr] IS NULL THEN 2
						-- NOTE: ���� ����� ���� � ������, �� �� ������� ��� ���������, �� ������ 1 - ����������
						WHEN dr.[KLADR] IS NOT NULL AND dr.[Status] = 0 THEN 1
						-- NOTE: ���� ����� ���� � ������, �� �� ����������, �� ������ 3 - ��� ����� �� ���������
						ELSE 3
					END AS [BindingStatus]
			  FROM	[prod].[BUFFER_DeliveryRates] dr
			  LEFT JOIN [prod].[DeliveryLocations] dl ON dl.[LocationName] = dr.[LocationName] AND dl.[PartnerId] = @PartnerId
			 WHERE	dr.[EtlSessionId] = @EtlSessionId 
			   AND	dr.[Status] IN (-4,-9,0)
		  ) AS source ([LocationName], [ExternalLocationId], [KLADR], [BindingStatus])
	ON (target.[PartnerId] = @PartnerId AND target.LocationName = source.LocationName)
	WHEN MATCHED AND 
			(
				(target.[KLADR] != source.[KLADR])
				OR
				(target.[ExternalLocationId] != source.[ExternalLocationId])
			)
		THEN
			UPDATE 
			   SET	[Kladr] = ISNULL(source.[KLADR], target.[KLADR]),
					[ExternalLocationId] = source.[ExternalLocationId],
					[Status] = source.[BindingStatus],
					-- NOTE: �������� ��� ��������� ��������� ��������
					[EtlSessionId] = @EtlSessionId,
					[UpdateDateTime] = GetDate(),
					-- NOTE: ��� ��� ��������� ��������� ��������, ������� �������������� � ������������.
					[UpdateUserId] = null
	WHEN NOT MATCHED 
		THEN	
			INSERT 
			(	PartnerId, [LocationName], [ExternalLocationId], [KLADR], [Status],
				[InsertDateTime], [EtlSessionId])
			VALUES 
			(	@PartnerId,source.[LocationName],source.[ExternalLocationId],[KLADR],[BindingStatus],
				GetDate(), @EtlSessionId);

	DELETE FROM [prod].[DeliveryRates]
	WHERE [PartnerId] = @PartnerId

	INSERT INTO [prod].[DeliveryRates]
	([PartnerId],[MinWeightGram],[MaxWeightGram],[PriceRur],[LocationId])
	(SELECT db.[PartnerId], buf.[MinWeightGram], buf.[MaxWeightGram], buf.[PriceRur], db.[Id]
	FROM [prod].[DeliveryLocations] db
	JOIN [prod].[BUFFER_DeliveryRates] buf ON buf.[EtlSessionId] = db.[EtlSessionId] AND buf.[LocationName] = db.[LocationName]
	WHERE db.EtlSessionId = @EtlSessionId
	  AND db.[PartnerId] = @PartnerId)
	
	-- �������� ��� ������������
	UPDATE  [prod].[BUFFER_DeliveryRates]
	SET [Status] = 1
	WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0
	
	-- ��������� ���������� �������� � ���������� � [dbo].[EtlCounters]
	;WITH DeliveryRatesErrorDesc AS (
		SELECT	[EtlSessionId],
				CASE	
					WHEN [Status] = 0 THEN '�� ����������'
					WHEN [Status] < 0 AND [Status] NOT IN (-4, -9) THEN '��������� (�� �������������)'
					WHEN [Status] IN (-4, -9) THEN '����� �� ������ ��� �� ���������'
					ELSE '������������� ��� ������'
				END AS [ErrorDesc]
		FROM [prod].[BUFFER_DeliveryRates]
		WHERE [EtlSessionId] = @EtlSessionId
	)
	INSERT INTO [dbo].[EtlCounters]
		([EtlPackageId],[EtlSessionId],[EntityName],[CounterName],[CounterValue],[LogDateTime],[LogUtcDateTime])
	SELECT	@EtlPackageId,
			[EtlSessionId],
			'',
			[ErrorDesc], 
			COUNT(*) AS Value, 
			GetDate(), 
			GetUtcDate()
	FROM DeliveryRatesErrorDesc ed
	GROUP BY [EtlSessionId],[ErrorDesc]
				
	-- �������� ��������� �������
	;WITH DeliveryRatesErrorDesc AS (
		SELECT	[EtlSessionId],
				CASE	
					WHEN [Status] = 0 THEN '�� ����������'
					WHEN [Status] < 0 AND [Status] NOT IN (-4, -9) THEN '��������� (�� �������������)'
					WHEN [Status] IN (-4, -9) THEN '����� �� ������ ��� �� ���������'
					ELSE '������������� ��� ������'
				END AS [ErrorDesc]
		FROM [prod].[BUFFER_DeliveryRates]
		WHERE [EtlSessionId] = @EtlSessionId
	), Counts AS (
		SELECT	[EtlSessionId] AS [EtlSessionId],
				[ErrorDesc] AS [Name], 
				COUNT(*) AS [Value]
		FROM DeliveryRatesErrorDesc
		GROUP BY [EtlSessionId],[ErrorDesc]
	)
	INSERT INTO [dbo].[EtlMessages]
	([EtlPackageId],[EtlSessionId],[EtlStepName],[MessageType],[Text],[Flags],[StackTrace],[LogDateTime],[LogUtcDateTime])
	SELECT	@EtlPackageId,
			c.[EtlSessionId],
			NULL, 
			8, -- Information, ��. RapidSoft.Etl.Logging.EtlMessageType 
			c.[Name] + ': ' + CAST(c.Value AS varchar) AS [Text],
			NULL,
			NULL,
			GetDate(),
			GetUtcDate() 
	FROM Counts c

	
	COMMIT TRAN
END
GO


