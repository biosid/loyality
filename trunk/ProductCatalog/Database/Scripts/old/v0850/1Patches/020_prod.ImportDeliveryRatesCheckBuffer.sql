IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[ImportDeliveryRatesCheckBuffer]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[ImportDeliveryRatesCheckBuffer]
GO

CREATE PROCEDURE [prod].[ImportDeliveryRatesCheckBuffer]
	@EtlPackageId [nvarchar](64), 
	@EtlSessionId [nvarchar](64), 
	@PartnerId int
AS
BEGIN
	SET NOCOUNT ON;
	
	--Устанавливаем статус -5 для Населеных пунктов (LocationName) имеющих разные значение KLADR
	DECLARE @notSet [nvarchar](64) = CAST(newId() AS [nvarchar](64))

	;WITH Dublicate AS ( 
		SELECT [EtlSessionId], [LocationName]
		FROM [prod].[BUFFER_DeliveryRates]
		WHERE [EtlSessionId] = @EtlSessionId
		  AND [Status] = 0
		GROUP BY [EtlSessionId], [LocationName]
		HAVING COUNT(DISTINCT ISNULL(KLADR,@notSet)) > 1)
	UPDATE tab
	SET [Status] = -5,
		[AddInfo] = (	SELECT TOP 1 [Line] FROM [prod].[BUFFER_DeliveryRates] t 
						WHERE t.[EtlSessionId] = tab.[EtlSessionId]
						  AND t.[LocationName] = tab.[LocationName]
						  AND t.Line != tab.Line)
	FROM [prod].[BUFFER_DeliveryRates] AS tab
	INNER JOIN Dublicate dub 
	  ON	dub.[EtlSessionId] = tab.[EtlSessionId] 
			AND dub.[LocationName] = tab.[LocationName]
	
	--Устанавливаем статус -5 для KLADR имеющих разные значение Населеного пункта (LocationName)	
	;WITH Dublicate AS (
		SELECT [EtlSessionId], [KLADR]
		FROM [prod].[BUFFER_DeliveryRates]
		WHERE [EtlSessionId] = @EtlSessionId
		  AND [Status] = 0
		  AND [KLADR] IS NOT NULL
		GROUP BY [EtlSessionId], [KLADR]
		HAVING COUNT(DISTINCT [LocationName]) > 1)
	UPDATE tab
	SET [Status] = -5,
		[AddInfo] = (	SELECT TOP 1 [Line] FROM [prod].[BUFFER_DeliveryRates] t 
						WHERE t.[EtlSessionId] = tab.[EtlSessionId]
						  AND t.[KLADR] = tab.[KLADR]
						  AND t.Line != tab.Line)
	FROM [prod].[BUFFER_DeliveryRates] AS tab
	INNER JOIN Dublicate dub 
	  ON	dub.[EtlSessionId] = tab.[EtlSessionId] 
			AND dub.[KLADR] = tab.[KLADR]
	
	--Устанавливаем статус -3 если не заполнены обязательные поля
	UPDATE [prod].[BUFFER_DeliveryRates] 
	  SET [Status] = -3
	WHERE [EtlSessionId] = @EtlSessionId 
	  AND [Status] = 0
	  AND ([MinWeightGram] IS NULL
	       OR
	       [MaxWeightGram] IS NULL
	       OR
	       [PriceRur] IS NULL
	       OR
	       [LocationName] IS NULL)
	       
	--Устанавливаем статус -4 если КЛАДР не 13 символов или содержит не цифры
	UPDATE [prod].[BUFFER_DeliveryRates] 
	  SET [Status] = -4
	WHERE [EtlSessionId] = @EtlSessionId 
	  AND [Status] = 0
	  AND [KLADR] IS NOT NULL
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
		SELECT *, ROW_NUMBER() OVER (ORDER BY [LocationName],[MinWeightGram]) as RowNum 
		FROM [prod].[BUFFER_DeliveryRates] 
		WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0),
	IntersectDeliveryRates
	AS (
	SELECT DISTINCT f.*, s.[Line] AS [SecondLine]
	FROM SessionDeliveryRates f
	INNER JOIN SessionDeliveryRates s 
		ON		f.[LocationName] = s.[LocationName] 
			AND f.RowNum != s.RowNum 
			AND	((f.[MinWeightGram] BETWEEN s.[MinWeightGram] AND s.[MaxWeightGram])
				 OR
				 (f.[MaxWeightGram] BETWEEN s.[MinWeightGram] AND s.[MaxWeightGram]))
	)
	UPDATE tab
	SET [Status] = -2,
		[AddInfo] = inter.[SecondLine]
	FROM [prod].[BUFFER_DeliveryRates] AS tab
	INNER JOIN IntersectDeliveryRates inter 
		ON	inter.[EtlSessionId] = tab.[EtlSessionId] 
		AND inter.[LocationName] = tab.[LocationName]
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
					THEN 'В диапазоне (LINE ' + CAST([Line] AS nvarchar) + ') нижная граница больше верхней: ' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))
				WHEN [Status] = -2
					THEN 'Диапазон (LINE ' + CAST([Line] AS nvarchar) + ') имеет пересечение с другим диапазоном (LINE ' + [AddInfo] + '): ' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))
				WHEN [Status] = -3
					THEN 'В диапазоне (LINE ' + CAST([Line] AS nvarchar) + ') имеются не заполненые поля: ' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + ISNULL(CAST([MinWeightGram] AS nvarchar), '') + ';' + ISNULL(CAST([MaxWeightGram] AS nvarchar), '') + ';' + ISNULL(CONVERT(nvarchar, cast([PriceRur] as money)), '')
				WHEN [Status] = -4
					THEN 'В диапазоне (LINE ' + CAST([Line] AS nvarchar) + ') код КЛАДР должен иметь длинну 13 символов и содержать только цифры: ' + [KLADR]
				WHEN [Status] = -5
					THEN 'В диапазоне (LINE ' + CAST([Line] AS nvarchar) + ') нарушена уникальность один код КЛАДР - один населеный пункт (LINE ' + [AddInfo] + '): ' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))
					
			END AS [Text],
			NULL,
			NULL,
			GetDate(), 
			GetUtcDate()
	FROM [prod].[BUFFER_DeliveryRates]
	WHERE [EtlSessionId] = @EtlSessionId
	  AND [Status] < 0 

END
GO


