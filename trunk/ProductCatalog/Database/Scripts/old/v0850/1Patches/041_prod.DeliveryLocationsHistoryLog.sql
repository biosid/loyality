IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[prod].[DeliveryLocationsHistoryLog]'))
	DROP VIEW [prod].[DeliveryLocationsHistoryLog]
GO

CREATE view [prod].[DeliveryLocationsHistoryLog]
AS
	WITH Histories AS (
		SELECT [Action]
			  ,[TriggerDate]
			  ,[Id]
			  ,[PartnerId]
			  ,[LocationName]
			  ,[Kladr]
			  ,[Status]
			  ,[InsertDateTime]
			  ,[UpdateDateTime]
			  ,[UpdateUserId]
			  ,[EtlSessionId]
			  ,[ExternalLocationId]
			  ,[UpdateSource]
			  ,ROW_NUMBER()  OVER(PARTITION BY [Id] ORDER BY [TriggerDate]) AS RowNumber
		  FROM [prod].[DeliveryLocationsHistory]
	)
	SELECT	h1.[Action],
			h1.[TriggerDate],
			h1.[Id],
			h1.[PartnerId],
			h1.[LocationName],
			h1.[UpdateUserId],
			h1.[EtlSessionId],
			h1.[InsertDateTime],
			h1.[UpdateDateTime],
			h2.[ExternalLocationId] AS [OldExternaLocationlId],
			h1.[ExternalLocationId] AS [NewExternaLocationlId],
			h2.[Kladr] AS [OldKladr],
			h1.[Kladr] AS [NewKladr],
			h2.[Status] AS [OldStatus],
			h1.[Status] AS [NewStatus]
	  FROM Histories h1
	LEFT JOIN Histories h2 ON h1.RowNumber = h2.RowNumber + 1 AND h1.[Id] = h2.[Id]
	WHERE h1.[UpdateSource] = 1 -- см. RapidSoft.Loaylty.ProductCatalog.API.Entities.DeliveryLocation.UpdateSources.Arm
GO


