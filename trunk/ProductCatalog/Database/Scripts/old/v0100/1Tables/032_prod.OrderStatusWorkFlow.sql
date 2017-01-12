CREATE TABLE [prod].[OrderStatusWorkFlow](
	[FromStatus] [int] NOT NULL,
	[ToStatus] [int] NOT NULL,
	CONSTRAINT [PK_OrderStatusWorkFlow] PRIMARY KEY CLUSTERED 
	(
		[FromStatus] ASC,
		[ToStatus] ASC
	)
)
GO

CREATE VIEW [prod].[FullOrderStatusWorkFlow]
AS 
	WITH FullFlow ([FromStatus], [ToStatus]) 
		AS (
			SELECT	[FromStatus], 
					[ToStatus]
			FROM [prod].[OrderStatusWorkFlow]
            UNION ALL 
            SELECT	FullFlow.[FromStatus],
					e.[ToStatus] 
			FROM FullFlow 
			INNER JOIN [prod].[OrderStatusWorkFlow] AS e ON e.[FromStatus] = FullFlow.[ToStatus])
	SELECT DISTINCT [FromStatus], [ToStatus]
    FROM    FullFlow
GO
    