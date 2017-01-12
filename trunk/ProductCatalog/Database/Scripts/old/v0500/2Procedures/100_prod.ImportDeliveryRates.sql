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
	
	--������������� ������ -3 ���� �� ��������� ������������ ����
	UPDATE [prod].[BUFFER_DeliveryRates] 
	  SET [Status] = -3
	WHERE [EtlSessionId] = @EtlSessionId 
	  AND [Status] = 0
	  AND ([KLADR] IS NULL
	       OR
	       [MinWeightGram] IS NULL
	       OR
	       [MaxWeightGram] IS NULL
	       OR
	       [PriceRur] IS NULL)
		   
	--������������� ������ -4 ���� ����� �� 13 �������� ��� �������� �� �����
	UPDATE [prod].[BUFFER_DeliveryRates] 
	  SET [Status] = -4
	WHERE [EtlSessionId] = @EtlSessionId 
	  AND [Status] = 0
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
		SELECT *, ROW_NUMBER() OVER (ORDER BY [KLADR],[MinWeightGram]) as RowNum 
		FROM [prod].[BUFFER_DeliveryRates] 
		WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0),
	IntersectDeliveryRates
	AS (
	SELECT DISTINCT f.*
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
	SET [Status] = -2
	FROM [prod].[BUFFER_DeliveryRates] AS tab
	INNER JOIN IntersectDeliveryRates inter 
		ON	inter.[EtlSessionId] = tab.[EtlSessionId] 
		AND inter.[KLADR] = tab.[KLADR]
		AND inter.[MinWeightGram] = tab.[MinWeightGram]
		AND inter.[MaxWeightGram] = tab.[MaxWeightGram]
	
	-- �������� ��������� ������
	INSERT INTO [dbo].[EtlMessages]
	([EtlPackageId],[EtlSessionId],[EtlStepName],[MessageType],[Text],[Flags],[StackTrace],[LogDateTime],[LogUtcDateTime])
	SELECT	@EtlPackageId,
			[EtlSessionId],
			NULL,
			8, -- Information, ��. RapidSoft.Etl.Logging.EtlMessageType 
			CASE
				WHEN [Status] = -1 
					THEN '� ��������� ������ ������� ������ �������: ' + [KLADR] + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money)) + ';' + CAST([Status] AS nvarchar) + ';'
				WHEN [Status] = -2
					THEN '�������� ����� ����������� � ������ ����������: ' + [KLADR] + ';' + CAST([MinWeightGram] AS nvarchar) + ';' + CAST([MaxWeightGram] AS nvarchar) + ';' + CONVERT(nvarchar, cast([PriceRur] as money)) + ';' + CAST([Status] AS nvarchar) + ';'
				WHEN [Status] = -3
					THEN '������� �� ���������� ����: ' + ISNULL([KLADR], '') + ';' + ISNULL(CAST([MinWeightGram] AS nvarchar), '') + ';' + ISNULL(CAST([MaxWeightGram] AS nvarchar), '') + ';' + ISNULL(CONVERT(nvarchar, cast([PriceRur] as money)), '') + ';' + ISNULL(CAST([Status] AS nvarchar), '') + ';'
				WHEN [Status] = -4
					THEN '��� ����� ������ ����� ������ 13 �������� � ��������� ������ �����: ' + [KLADR]
			END AS [Text],
			NULL,
			NULL,
			GetDate(), 
			GetUtcDate()
	FROM [prod].[BUFFER_DeliveryRates]
	WHERE [EtlSessionId] = @EtlSessionId
	  AND [Status] < 0 
	
	-- ������� ������
	DELETE FROM [prod].[DeliveryRates]
		WHERE [PartnerId] = @PartnerId
		
	-- ��������� �����
	INSERT INTO [prod].[DeliveryRates]
    ([PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur])
    SELECT @PartnerId,[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur]
	FROM [prod].[BUFFER_DeliveryRates]
	WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0
	
	-- �������� ��� ������������
	UPDATE  [prod].[BUFFER_DeliveryRates]
	SET [Status] = 1
	WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0
	
	-- ��������� ���������� �������� � ���������� � [dbo].[EtlCounters]
	INSERT INTO [dbo].[EtlCounters]
		([EtlPackageId],[EtlSessionId],[EntityName],[CounterName],[CounterValue],[LogDateTime],[LogUtcDateTime])
	SELECT	@EtlPackageId,
			[EtlSessionId],
			'',
			CASE	
				WHEN [Status] = 0 THEN '�� ����������'
				WHEN [Status] < 0 THEN '���������'
				ELSE '�������������'
			END AS Name, 
			COUNT(*) AS Value, 
			GetDate(), 
			GetUtcDate()
	FROM [prod].[BUFFER_DeliveryRates]
	WHERE [EtlSessionId] = @EtlSessionId
	GROUP BY	[EtlSessionId], 
				CASE 
					WHEN [Status] = 0 THEN '�� ����������'
					WHEN [Status] < 0 THEN '���������'
					ELSE '�������������'
				END
				
	-- �������� ��������� �������
	;WITH Counts AS (
		SELECT	[EtlSessionId] AS [EtlSessionId],
				CASE	
					WHEN [Status] = 0 THEN '�� ����������'
					WHEN [Status] < 0 THEN '���������'
					ELSE '�������������'
				END AS [Name], 
				COUNT(*) AS [Value]
		FROM [prod].[BUFFER_DeliveryRates]
		WHERE [EtlSessionId] = @EtlSessionId
		GROUP BY	[EtlSessionId],
					CASE	
						WHEN [Status] = 0 THEN '�� ����������'
						WHEN [Status] < 0 THEN '���������'
						ELSE '�������������'
					END
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



