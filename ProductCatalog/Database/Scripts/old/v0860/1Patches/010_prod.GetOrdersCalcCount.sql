/****** Object:  StoredProcedure [prod].[GetOrders]    Script Date: 12.11.2013 15:53:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [prod].[GetOrders] 
	@clientId nvarchar(255),
 	@dateTimeStart [datetime] = null,
 	@dateTimeEnd [datetime] = null,
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
		,[OrderStatusDescription]
		,[ExternalOrderStatusDateTime]
		,[StatusChangedDate]
		,[StatusUtcChangedDate]
		,[DeliveryInfo]
		,[InsertedDate]
		,[InsertedUtcDate]
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
			AND (@dateTimeStart is null or @dateTimeStart <= [InsertedDate])
			AND (@dateTimeEnd is null or [InsertedDate] <= @dateTimeEnd)			
			AND (@statuses IS NULL OR [Status] IN (SELECT value FROM dbo.ParamParserString(@statuses,',')))
			AND (@skipStatuses IS NULL OR [Status] NOT IN (SELECT value FROM dbo.ParamParserString(@skipStatuses,',')))) orders
	WHERE rownum > @countToSkip

	IF (@calcTotalCount != 0)
	BEGIN
		SELECT @totalCount = count(1)
		FROM [prod].[Orders]
		WHERE [ClientId] = @clientId
			AND (@dateTimeStart is null or @dateTimeStart <= [InsertedDate])
			AND (@dateTimeEnd is null or [InsertedDate] <= @dateTimeEnd)
		  AND (@statuses IS NULL OR [Status] IN (SELECT value FROM dbo.ParamParserString(@statuses,',')))
		  AND (@skipStatuses IS NULL OR [Status] NOT IN (SELECT value FROM dbo.ParamParserString(@skipStatuses,',')))
	END
		
END

