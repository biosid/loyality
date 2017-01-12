ALTER PROCEDURE [prod].[ImportDeliveryRatesCheckBuffer]
	@EtlPackageId [nvarchar](64), 
	@EtlSessionId [nvarchar](64), 
	@PartnerId int
AS
BEGIN
	SET NOCOUNT ON;
	
	--�������� ����� �� 13 ��������
	UPDATE [prod].[BUFFER_DeliveryRates]
	   SET [KLADR] = SUBSTRING([KLADR], 1, 13)
	 WHERE [KLADR] IS NOT NULL AND LEN([KLADR]) > 13
	
	--������������� ������ -5 ��� ��������� ������� (LocationName) ������� ������ �������� KLADR
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
			
	--������������� ������ -7 ��� ��������� ������� (LocationName) ������� ������ �������� ������������� ������	(ExternalLocationId)
	;WITH Dublicate AS ( 
		SELECT [EtlSessionId], [LocationName]
		FROM [prod].[BUFFER_DeliveryRates]
		WHERE [EtlSessionId] = @EtlSessionId
		  AND [Status] = 0
		GROUP BY [EtlSessionId], [LocationName]
		HAVING COUNT(DISTINCT ISNULL([ExternalLocationId],@notSet)) > 1)
	UPDATE tab
	SET [Status] = -7,
		[AddInfo] = (	SELECT TOP 1 [Line] FROM [prod].[BUFFER_DeliveryRates] t 
						WHERE t.[EtlSessionId] = tab.[EtlSessionId]
						  AND t.[LocationName] = tab.[LocationName]
						  AND t.Line != tab.Line)
	FROM [prod].[BUFFER_DeliveryRates] AS tab
	INNER JOIN Dublicate dub 
	  ON	dub.[EtlSessionId] = tab.[EtlSessionId] 
			AND dub.[LocationName] = tab.[LocationName]
			
	--������������� ������ -8 ��� KLADR ������� ������ �������� ���������� ������ (LocationName)	
	;WITH Dublicate AS (
		SELECT [EtlSessionId], [ExternalLocationId]
		FROM [prod].[BUFFER_DeliveryRates]
		WHERE [EtlSessionId] = @EtlSessionId
		  AND [Status] = 0
		  AND [ExternalLocationId] IS NOT NULL
		GROUP BY [EtlSessionId], [ExternalLocationId]
		HAVING COUNT(DISTINCT [LocationName]) > 1)
	UPDATE tab
	SET [Status] = -8,
		[AddInfo] = (	SELECT TOP 1 [Line] FROM [prod].[BUFFER_DeliveryRates] t 
						WHERE t.[EtlSessionId] = tab.[EtlSessionId]
						  AND t.[ExternalLocationId] = tab.[ExternalLocationId]
						  AND t.Line != tab.Line)
	FROM [prod].[BUFFER_DeliveryRates] AS tab
	INNER JOIN Dublicate dub 
	  ON	dub.[EtlSessionId] = tab.[EtlSessionId] 
			AND dub.[ExternalLocationId] = tab.[ExternalLocationId]
	
	--������������� ������ -3 ���� �� ��������� ������������ ����
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
	       
	--������������� ������ -4 ���� ����� �� 13 �������� ��� �������� �� �����
	UPDATE [prod].[BUFFER_DeliveryRates] 
	  SET [Status] = -4
	WHERE [EtlSessionId] = @EtlSessionId 
	  AND [Status] = 0
	  AND [KLADR] IS NOT NULL
	  AND (	LEN([KLADR]) != 13
			OR
			ISNUMERIC([KLADR]) = 0)
	       
	--������������� ������ -1 ���� ������ ������� �������� (MinWeightGram) ������ ������� (MaxWeightGram)
	UPDATE [prod].[BUFFER_DeliveryRates] 
	  SET [Status] = -1
	WHERE [EtlSessionId] = @EtlSessionId 
	  AND [Status] = 0
	  AND [MinWeightGram] > [MaxWeightGram] 
	  
	--������������� ������ -2 ��� ����������� ������ ������ �������� (MinWeightGram) ��� ������� (MaxWeightGram)
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
		
	----������������� ������ -10 ��� ������� ��� ��� ����� ��� ������������ ������ ��������� ������� (LocationName) � �������� (DeliveryLocations)
	--UPDATE dr
	--   SET dr.[Status] = -10, dr.[AddInfo] = (dl.[LocationName] + ' (id=' + CAST(dl.[Id] AS NVARCHAR(MAX)) + ')')
	--  FROM [prod].[BUFFER_DeliveryRates] dr
	--  JOIN [prod].[DeliveryLocations] dl ON dl.[Kladr] = dr.[KLADR] AND dl.[LocationName] != dr.[LocationName]
	-- WHERE dr.[Status] = 0
	--   AND dr.[EtlSessionId] = @EtlSessionId	
			
	-- �������� ��������� ������
	INSERT INTO [dbo].[EtlMessages]
	([EtlPackageId],[EtlSessionId],[EtlStepName],[MessageType],[Text],[Flags],[StackTrace],[LogDateTime],[LogUtcDateTime])
	SELECT	@EtlPackageId,
			[EtlSessionId],
			NULL,
			6, -- Error, ��. RapidSoft.Etl.Logging.EtlMessageType 
			CASE
				WHEN [Status] = -1 
					THEN '� ��������� (LINE ' + CAST([Line] AS nvarchar) + ') ������ ������� ������ �������: ' + ISNULL([ExternalLocationId], '') + ';' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))
				WHEN [Status] = -2
					THEN '�������� (LINE ' + CAST([Line] AS nvarchar) + ') ����� ����������� � ������ ���������� (LINE ' + [AddInfo] + '): ' + ISNULL([ExternalLocationId], '') + ';' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))
				WHEN [Status] = -3
					THEN '� ��������� (LINE ' + CAST([Line] AS nvarchar) + ') ������� �� ����������� ����: ' + ISNULL([ExternalLocationId], '') + ';' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + ISNULL(CAST([MinWeightGram] AS nvarchar), '') + ';' + ISNULL(CAST([MaxWeightGram] AS nvarchar), '') + ';' + ISNULL(CONVERT(nvarchar, cast([PriceRur] as money)), '')
				WHEN [Status] = -4
					THEN '� ��������� (LINE ' + CAST([Line] AS nvarchar) + ') ��� ����� ������ ����� ������ 13 �������� � ��������� ������ �����: ' + [KLADR]
				WHEN [Status] = -5
					THEN '� ��������� (LINE ' + CAST([Line] AS nvarchar) + ') �������� ������������ ���� ��������� ����� - ���� ��� ����� (LINE ' + [AddInfo] + '): ' + ISNULL([ExternalLocationId], '') + ';' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))
				--WHEN [Status] = -6
				--	THEN '� ��������� (LINE ' + CAST([Line] AS nvarchar) + ') �������� ������������ ���� ��� ����� - ���� ��������� ����� (LINE ' + [AddInfo] + '): ' + ISNULL([ExternalLocationId], '') + ';' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))
				WHEN [Status] = -7
					THEN '� ��������� (LINE ' + CAST([Line] AS nvarchar) + ') �������� ������������ ���� ��������� ����� - ���� ������������� ������ (LINE ' + [AddInfo] + '): ' + ISNULL([ExternalLocationId], '') + ';' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))	
				WHEN [Status] = -8
					THEN '� ��������� (LINE ' + CAST([Line] AS nvarchar) + ') �������� ������������ ���� ������������� ������ - ���� ��������� ����� (LINE ' + [AddInfo] + '): ' + ISNULL([ExternalLocationId], '') + ';' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))	
				--WHEN [Status] = -10
				--	THEN '� ��������� (LINE ' + CAST([Line] AS nvarchar) + ') ������ ��� ����� ��� ������������ ������ �������� "' + [AddInfo] + '": ' + ISNULL([ExternalLocationId], '') + ';' + ISNULL([KLADR], '') + ';' + ISNULL([LocationName], '') + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money))	
			END AS [Text],
			NULL,
			NULL,
			GetDate(), 
			GetUtcDate()
	FROM [prod].[BUFFER_DeliveryRates]
	WHERE [EtlSessionId] = @EtlSessionId
	  AND [Status] < 0 

END