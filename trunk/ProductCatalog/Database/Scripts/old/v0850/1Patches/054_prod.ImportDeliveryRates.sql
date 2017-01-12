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
	
	DECLARE @ImportUpdateSourceId int = 2;
			
	BEGIN TRAN
		
	MERGE [prod].[DeliveryLocations] AS target
	USING (	
			SELECT DISTINCT	dr.[LocationName],
					dr.[ExternalLocationId],
					-- NOTE: Если КЛАДРа есть в локации, то используем его
					ISNULL(dl.[Kladr], SUBSTRING(dr.[KLADR],1,13)) AS [KLADR], 
					CASE
						-- NOTE: Если КЛАДРа нет в буфере, но есть в локация, то сохраняем статус из локации
						WHEN dr.[KLADR] IS NULL AND dl.[Kladr] IS NOT NULL THEN dl.[Status]	
						-- NOTE: Если КЛАДРа нет нигде, то статус 2 - не имеет кода КЛАДР
						WHEN dr.[KLADR] IS NULL AND dl.[Kladr] IS NULL THEN 2
						-- NOTE: Если КЛАДР есть в буфере, но не помечен как ошибочный, то статус 1 - корректная
						WHEN dr.[KLADR] IS NOT NULL AND dr.[Status] = 0 THEN 1
						-- NOTE: Если КЛАДР есть в буфере, но не правильный, то статус 3 - код КЛАДР не корректен
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
			   SET	[Kladr] = source.[KLADR],
					[ExternalLocationId] = source.[ExternalLocationId],
					[Status] = source.[BindingStatus],
					-- NOTE: Помечаем что изменения выполнены импортом
					[EtlSessionId] = @EtlSessionId,
					[UpdateDateTime] = GetDate(),
					-- NOTE: Так как изменения выполнены импортом, снимаем ответсвенность с пользователя.
					[UpdateUserId] = null,
					[UpdateSource] = @ImportUpdateSourceId
	WHEN NOT MATCHED 
		THEN	
			INSERT 
			(	PartnerId, [LocationName], [ExternalLocationId], [KLADR], [Status],
				[InsertDateTime], [EtlSessionId], [UpdateSource])
			VALUES 
			(	@PartnerId,source.[LocationName],source.[ExternalLocationId],source.[KLADR],source.[BindingStatus],
				GetDate(), @EtlSessionId, @ImportUpdateSourceId);

	DELETE FROM [prod].[DeliveryRates]
	WHERE [PartnerId] = @PartnerId

	INSERT INTO [prod].[DeliveryRates]
	([PartnerId],[MinWeightGram],[MaxWeightGram],[PriceRur],[LocationId])
	(SELECT db.[PartnerId], buf.[MinWeightGram], buf.[MaxWeightGram], buf.[PriceRur], db.[Id]
	FROM [prod].[DeliveryLocations] db
	JOIN [prod].[BUFFER_DeliveryRates] buf ON buf.[LocationName] = db.[LocationName]
	WHERE buf.EtlSessionId = @EtlSessionId
	  AND db.[PartnerId] = @PartnerId
	  AND buf.[Status] IN (0, -4, -9))
	
	-- Помечаем как обработанные
	UPDATE  [prod].[BUFFER_DeliveryRates]
	SET [Status] = 1
	WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0
	
	-- Проверяем уникальность КЛАДР и ставим статус 1 - для уникальных и статус 4 - для не уникальных. NOTE: Учитываем локации имеющие тарифы
	;WITH Duplicate AS 
	(
		SELECT [Kladr]--, COUNT([LocationName])
		FROM [prod].[DeliveryLocations] dl
		WHERE [Status] in (1,4)
		  AND [PartnerId] = @PartnerId
		  AND EXISTS (SELECT TOP 1 1 FROM [prod].[DeliveryRates] dr WHERE dr.LocationId = dl.Id)
		GROUP BY [Kladr]
		HAVING COUNT([LocationName]) > 1
	)
	UPDATE tab
	   SET [Status]  = CASE 
						  WHEN dup.Kladr IS NULL THEN 1
						  ELSE 4
						END
	FROM [prod].[DeliveryLocations] tab
	LEFT JOIN Duplicate dup ON dup.Kladr = tab.Kladr
	WHERE [Status] in (1,4)
	  AND [PartnerId] = @PartnerId
	  AND EXISTS (SELECT TOP 1 1 FROM [prod].[DeliveryRates] dr WHERE dr.LocationId = tab.Id)
	
	-- Вставляем количество успешных и неуспешных в [dbo].[EtlCounters]
	;WITH DeliveryRatesErrorDesc AS (
		SELECT	[EtlSessionId],
				CASE	
					WHEN [Status] = 0 THEN 'Не обработано'
					WHEN [Status] < 0 AND [Status] NOT IN (-4, -9) THEN 'Ошибочные (не импортировано)'
					WHEN [Status] IN (-4, -9) THEN 'КЛАДР не указан или не корректен'
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
				
	-- Логируем результат импорта по данным буфера
	;WITH DeliveryRatesErrorDesc AS (
		SELECT	[EtlSessionId],
				CASE	
					WHEN [Status] = 0 THEN 'Не обработано'
					WHEN [Status] < 0 AND [Status] NOT IN (-4, -9) THEN 'Ошибочные (не импортировано)'
					WHEN [Status] IN (-4, -9) THEN 'КЛАДР не указан или не корректен'
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

	-- Считаем статусы используемых локаций локаций
	;WITH LocationStatuses AS (
		SELECT COUNT(*) AS [Count], [Status]
		FROM [prod].[DeliveryLocations] dl
		WHERE dl.PartnerId = @PartnerId
		  AND EXISTS (SELECT TOP 1 1 FROM [prod].[DeliveryRates] dr WHERE dr.LocationId = dl.Id)
		GROUP BY [Status])
	INSERT INTO [dbo].[EtlCounters]
		([EtlPackageId],[EtlSessionId],[EntityName],[CounterName],[CounterValue],[LogDateTime],[LogUtcDateTime])
	SELECT  @EtlPackageId,
			@EtlSessionId,
			'Точек доставки', 
			CASE 
				WHEN c.[Status] = 1 THEN 'Корректных'
				WHEN c.[Status] = 2 THEN 'Не имеющих КЛАДР'
				WHEN c.[Status] = 3 THEN 'Имеющих не корректный КЛАДР'
				WHEN c.[Status] = 4 THEN 'Использующих не уникальный КЛАДР'
			END,
			c.[Count],
			GETDATE(),
			GETUTCDATE()
	FROM LocationStatuses c

	-- Считаем кол-во локаций без тарифов
	INSERT INTO [dbo].[EtlCounters]
		([EtlPackageId],[EtlSessionId],[EntityName],[CounterName],[CounterValue],[LogDateTime],[LogUtcDateTime])
	SELECT  @EtlPackageId,
			@EtlSessionId,
			'Точек доставки', 
			'Не имеющих тарифы',
			COUNT(*),
			GETDATE(),
			GETUTCDATE()
	FROM [prod].[DeliveryLocations] dl
	WHERE dl.PartnerId = @PartnerId
	  AND NOT EXISTS (SELECT TOP 1 1 FROM [prod].[DeliveryRates] dr WHERE dr.LocationId = dl.Id)
	
	COMMIT TRAN
END
GO


