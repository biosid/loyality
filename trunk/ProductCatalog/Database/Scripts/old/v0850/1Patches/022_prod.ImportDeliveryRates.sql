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
			SELECT DISTINCT	[LocationName], 
					[KLADR], 
					CASE	WHEN [KLADR] IS NULL THEN 2
							WHEN [KLADR] IS NOT NULL AND [Status] = -4 THEN 3
							ELSE 1
					END AS [BindingStatus]
			  FROM	[prod].[BUFFER_DeliveryRates]
			 WHERE	[EtlSessionId] = @etlSessionId 
			   AND	[Status] IN (-4,0)
		  ) AS source ([LocationName], [KLADR], [BindingStatus])
	ON (target.[PartnerId] = @partnerId AND target.LocationName = source.LocationName)
	WHEN MATCHED AND (source.[KLADR] IS NOT NULL AND target.[KLADR] != SUBSTRING(source.[KLADR],1,13))
		THEN
			UPDATE 
			   SET	[Kladr] = SUBSTRING(source.[KLADR],1,13),
					[Status] = source.[BindingStatus],
					[EtlSessionId] = @etlSessionId
	WHEN NOT MATCHED 
		THEN	
			INSERT 
			(	PartnerId, [LocationName], [KLADR], [Status],
				[InsertDateTime], [EtlSessionId])
			VALUES 
			(	@partnerId,source.[LocationName],SUBSTRING(source.[KLADR],1,13),[BindingStatus],
				GetDate(), @etlSessionId);

	DELETE FROM [prod].[DeliveryRates]
	WHERE [PartnerId] = @partnerId

	INSERT INTO [prod].[DeliveryRates]
	([PartnerId],[MinWeightGram],[MaxWeightGram],[PriceRur],[ExternalLocationId],[LocationId])
	(SELECT db.[PartnerId], buf.[MinWeightGram], buf.[MaxWeightGram], buf.[PriceRur], buf.[ExternalLocationId], db.[Id]
	FROM [prod].[DeliveryLocations] db
	JOIN [prod].[BUFFER_DeliveryRates] buf ON buf.[EtlSessionId] = db.[EtlSessionId] AND buf.[LocationName] = db.[LocationName]
	WHERE db.EtlSessionId = @etlSessionId
	  AND db.[PartnerId] = @partnerId)
	
	-- Помечаем как обработанные
	UPDATE  [prod].[BUFFER_DeliveryRates]
	SET [Status] = 1
	WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0
	
	-- Вставляем количество успешных и неуспешных в [dbo].[EtlCounters]
	;WITH DeliveryRatesErrorDesc AS (
		SELECT	[EtlSessionId],
				CASE	
					WHEN [Status] = 0 THEN 'Не обработано'
					WHEN [Status] < 0 AND [Status] != -4 THEN 'Ошибочные (не импортировано)'
					WHEN [Status] = -4 THEN 'КЛАДР не указан или не корректен'
					ELSE 'Импортировано без ошибок'
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
				
	-- Логируем результат импорта
	;WITH DeliveryRatesErrorDesc AS (
		SELECT	[EtlSessionId],
				CASE	
					WHEN [Status] = 0 THEN 'Не обработано'
					WHEN [Status] < 0 AND [Status] != -4 THEN 'Ошибочные (не импортировано)'
					WHEN [Status] = -4 THEN 'КЛАДР не указан или не корректен'
					ELSE 'Импортировано без ошибок'
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
			8, -- Information, см. RapidSoft.Etl.Logging.EtlMessageType 
			c.[Name] + ': ' + CAST(c.Value AS varchar) AS [Text],
			NULL,
			NULL,
			GetDate(),
			GetUtcDate() 
	FROM Counts c

	
	COMMIT TRAN
END
GO


