IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetOrders]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[GetOrders]
GO

CREATE PROCEDURE [prod].[GetOrders] 
	@clientId nvarchar(255),
 	@dateTimeStart [datetime],
 	@dateTimeEnd [datetime],
 	@countToTake [int],
 	@countToSkip [int],
 	@calcTotalCount [bit],
 	@statuses nvarchar(MAX) = null,
 	@skipStatuses nvarchar(MAX) = null,
 	@totalCount [int] out AS
BEGIN
	SET NOCOUNT ON

	SELECT TOP (@countToTake)
		 [Id]
		,[ClientId]
		,[ExternalOrderId]
		,[Status]
		,[ExternalOrderStatusCode]
		,[ExternalOrderStatusDescription]
		,[ExternalOrderStatusDateTime]
		,[StatusChangedDate]
		,[StatusUtcChangedDate]
		,[DeliveryInfo]
		,[InsertedDate]
		,[InsertedUtcDate]
		,[InsertedUserId]
		,[UpdatedDate]
		,[UpdatedUtcDate]
		,[UpdatedUserId]
		,[Items]
		,[TotalWeight]
		,[ItemsCost]
		,[BonusItemsCost]
		,[DeliveryCost]
		,[BonusDeliveryCost]
		,[TotalCost]
		,[BonusTotalCost]
		,[PartnerId]
		,[PaymentStatus]
		,[DeliveryPaymentStatus]
		,[CarrierId]
	FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY [InsertedDate] DESC) rownum
		  FROM [prod].[Orders]
		  WHERE [ClientId] = @clientId
			AND [InsertedDate] BETWEEN @dateTimeStart AND @dateTimeEnd
			AND (@statuses IS NULL OR [Status] IN (SELECT value FROM dbo.ParamParserString(@statuses,',')))
			AND (@skipStatuses IS NULL OR [Status] NOT IN (SELECT value FROM dbo.ParamParserString(@skipStatuses,',')))) orders
	WHERE rownum > @countToSkip

	IF (@calcTotalCount != 0)
	BEGIN
		SELECT @totalCount = count(1)
		FROM [prod].[Orders]
		WHERE [ClientId] = @clientId
		  AND [InsertedDate] BETWEEN @dateTimeStart AND @dateTimeEnd
		  AND (@statuses IS NULL OR [Status] IN (SELECT value FROM dbo.ParamParserString(@statuses,',')))
		  AND (@skipStatuses IS NULL OR [Status] NOT IN (SELECT value FROM dbo.ParamParserString(@skipStatuses,',')))
	END
		
END


GO


