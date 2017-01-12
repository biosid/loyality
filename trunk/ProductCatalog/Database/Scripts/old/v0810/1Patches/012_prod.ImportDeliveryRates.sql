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
	
	--Устанавливаем статус -3 если не заполнены обязательные поля
	UPDATE [prod].[BUFFER_DeliveryRates] 
	  SET [Status] = -3
	WHERE [EtlSessionId] = @EtlSessionId 
	  AND [Status] = 0
	  AND ([KLADR] IS NULL
	       OR
	       [KLADR] = 'notset'
	       OR
	       [MinWeightGram] IS NULL
	       OR
	       [MaxWeightGram] IS NULL
	       OR
	       [PriceRur] IS NULL)
		   
	--Устанавливаем статус -4 если КЛАДР не 13 символов или содержит не цифры
	UPDATE [prod].[BUFFER_DeliveryRates] 
	  SET [Status] = -4
	WHERE [EtlSessionId] = @EtlSessionId 
	  AND [Status] = 0
	  AND (	LEN([KLADR]) != 13
			OR
			ISNUMERIC([KLADR]) = 0)
	
	--Устанавливаем статус -1 если нижная граница диаазона (MinWeightGram) больше верхней (MaxWeightGram)
	UPDATE [prod].[BUFFER_DeliveryRates] 
	  SET [Status] = -1
	WHERE [EtlSessionId] = @EtlSessionId 
	  AND [Status] = 0
	  AND [MinWeightGram] > [MaxWeightGram] 
	  
	--Устанавливаем статус -2 для пересечений нижних границ диаазона (MinWeightGram) или верхних (MaxWeightGram)
	;WITH SessionDeliveryRates
	AS (
		SELECT *, ROW_NUMBER() OVER (ORDER BY [KLADR],[MinWeightGram]) as RowNum 
		FROM [prod].[BUFFER_DeliveryRates] 
		WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0),
	IntersectDeliveryRates
	AS (
	SELECT DISTINCT f.*, s.[Line] AS [SecondLine]
	FROM SessionDeliveryRates f
	INNER JOIN SessionDeliveryRates s 
		ON		f.KLADR = s.KLADR 
			AND f.RowNum != s.RowNum 
			AND	((f.[MinWeightGram] BETWEEN s.[MinWeightGram] AND s.[MaxWeightGram])
				 OR
				 (f.[MaxWeightGram] BETWEEN s.[MinWeightGram] AND s.[MaxWeightGram])
				 OR
				 (s.[MinWeightGram] BETWEEN f.[MinWeightGram] AND f.[MaxWeightGram])
				 OR
				 (s.[MaxWeightGram] BETWEEN f.[MinWeightGram] AND f.[MaxWeightGram]))
	)
	UPDATE tab
	SET [Status] = -2,
		[AddInfo] = inter.[SecondLine]
	FROM [prod].[BUFFER_DeliveryRates] AS tab
	INNER JOIN IntersectDeliveryRates inter 
		ON	inter.[EtlSessionId] = tab.[EtlSessionId] 
		AND inter.[KLADR] = tab.[KLADR]
		AND inter.[MinWeightGram] = tab.[MinWeightGram]
		AND inter.[MaxWeightGram] = tab.[MaxWeightGram]
	
	-- Логируем найденные ошибки
	INSERT INTO [dbo].[EtlMessages]
	([EtlPackageId],[EtlSessionId],[EtlStepName],[MessageType],[Text],[Flags],[StackTrace],[LogDateTime],[LogUtcDateTime])
	SELECT	@EtlPackageId,
			[EtlSessionId],
			NULL,
			5, -- Error, см. RapidSoft.Etl.Logging.EtlMessageType 
			CASE
				WHEN [Status] = -1 
					THEN 'В диапазоне (LINE ' + CAST([Line] AS nvarchar) + ') нижная граница больше верхней: ' + [KLADR] + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))
				WHEN [Status] = -2
					THEN 'Диапазон (LINE ' + CAST([Line] AS nvarchar) + ') имеет пересечение с другим диапазоном (LINE ' + [AddInfo] + '): ' + [KLADR] + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))
				WHEN [Status] = -3
					THEN 'В диапазоне (LINE ' + CAST([Line] AS nvarchar) + ') имеются не заполненые поля: ' + ISNULL([KLADR], '') + ';' + ISNULL(CAST([MinWeightGram] AS nvarchar), '') + ';' + ISNULL(CAST([MaxWeightGram] AS nvarchar), '') + ';' + ISNULL(CONVERT(nvarchar, cast([PriceRur] as money)), '')
				WHEN [Status] = -4
					THEN 'В диапазоне (LINE ' + CAST([Line] AS nvarchar) + ') код КЛАДР должен иметь длинну 13 символов и содержать только цифры: ' + [KLADR]
			END AS [Text],
			NULL,
			NULL,
			GetDate(), 
			GetUtcDate()
	FROM [prod].[BUFFER_DeliveryRates]
	WHERE [EtlSessionId] = @EtlSessionId
	  AND [Status] < 0 
	
	-- Удаляем старые
	DELETE FROM [prod].[DeliveryRates]
		WHERE [PartnerId] = @PartnerId
		
	-- Вставляем новые
	INSERT INTO [prod].[DeliveryRates]
    ([PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],[ExternalLocationId])
    SELECT @PartnerId,[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],[ExternalLocationId]
	FROM [prod].[BUFFER_DeliveryRates]
	WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0
	
	-- Помечаем как обработанные
	UPDATE  [prod].[BUFFER_DeliveryRates]
	SET [Status] = 1
	WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0
	
	-- Вставляем количество успешных и неуспешных в [dbo].[EtlCounters]
	INSERT INTO [dbo].[EtlCounters]
		([EtlPackageId],[EtlSessionId],[EntityName],[CounterName],[CounterValue],[LogDateTime],[LogUtcDateTime])
	SELECT	@EtlPackageId,
			[EtlSessionId],
			'',
			CASE	
				WHEN [Status] = 0 THEN 'Не обработано'
				WHEN [Status] < 0 THEN 'Ошибочные'
				ELSE 'Импортировано'
			END AS Name, 
			COUNT(*) AS Value, 
			GetDate(), 
			GetUtcDate()
	FROM [prod].[BUFFER_DeliveryRates]
	WHERE [EtlSessionId] = @EtlSessionId
	GROUP BY	[EtlSessionId], 
				CASE 
					WHEN [Status] = 0 THEN 'Не обработано'
					WHEN [Status] < 0 THEN 'Ошибочные'
					ELSE 'Импортировано'
				END
				
	-- Логируем результат импорта
	;WITH Counts AS (
		SELECT	[EtlSessionId] AS [EtlSessionId],
				CASE	
					WHEN [Status] = 0 THEN 'Не обработано'
					WHEN [Status] < 0 THEN 'Ошибочные'
					ELSE 'Импортировано'
				END AS [Name], 
				COUNT(*) AS [Value]
		FROM [prod].[BUFFER_DeliveryRates]
		WHERE [EtlSessionId] = @EtlSessionId
		GROUP BY	[EtlSessionId],
					CASE	
						WHEN [Status] = 0 THEN 'Не обработано'
						WHEN [Status] < 0 THEN 'Ошибочные'
						ELSE 'Импортировано'
					END
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


