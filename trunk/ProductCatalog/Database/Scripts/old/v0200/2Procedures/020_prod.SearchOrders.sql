IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[SearchOrders]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[SearchOrders]
GO

CREATE PROCEDURE [prod].[SearchOrders]
 	@dateTimeStart [datetime],
 	@dateTimeEnd [datetime],
 	@countToTake [int],
 	@countToSkip [int],
 	@calcTotalCount [bit],
 	@statuses nvarchar(MAX) = null,
 	@skipStatuses nvarchar(MAX) = null,
 	@paymentStatus nvarchar(MAX) = null,
 	@deliveryPaymentStatus nvarchar(MAX) = null,
 	@partnerIds nvarchar(MAX) = null,
	@orderIds nvarchar(MAX) = null,
	@carrierIds nvarchar(max) = null,
 	@totalCount [int] out 
	AS
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
		  WHERE 
			[InsertedDate] BETWEEN @dateTimeStart AND @dateTimeEnd
			AND (@statuses IS NULL OR [Status] IN (SELECT value FROM dbo.ParamParserString(@statuses,',')))
			AND (@skipStatuses IS NULL OR [Status] NOT IN (SELECT value FROM dbo.ParamParserString(@skipStatuses,',')))
			AND (@paymentStatus IS NULL OR [PaymentStatus] IN (SELECT value FROM dbo.ParamParserString(@paymentStatus,',')))
			AND (@deliveryPaymentStatus IS NULL OR [DeliveryPaymentStatus] IN (SELECT value FROM dbo.ParamParserString(@deliveryPaymentStatus,',')))
			AND (@partnerIds IS NULL OR [PartnerId] IN (SELECT value FROM dbo.ParamParserString(@partnerIds,',')))
			AND (@orderIds IS NULL OR [Id] IN (SELECT value FROM dbo.ParamParserString(@orderIds,',')))
			AND (@carrierIds IS NULL OR [CarrierId] IN (SELECT value FROM dbo.ParamParserString(@carrierIds,',')))
			) orders
	WHERE rownum > @countToSkip

	IF (@calcTotalCount != 0)
	BEGIN
		SELECT @totalCount = count(1)
		FROM [prod].[Orders]
		WHERE 
		  [InsertedDate] BETWEEN @dateTimeStart AND @dateTimeEnd
		  AND (@statuses IS NULL OR [Status] IN (SELECT value FROM dbo.ParamParserString(@statuses,',')))
		  AND (@skipStatuses IS NULL OR [Status] NOT IN (SELECT value FROM dbo.ParamParserString(@skipStatuses,',')))
		  AND (@paymentStatus IS NULL OR [PaymentStatus] IN (SELECT value FROM dbo.ParamParserString(@paymentStatus,',')))
		  AND (@deliveryPaymentStatus IS NULL OR [DeliveryPaymentStatus] IN (SELECT value FROM dbo.ParamParserString(@deliveryPaymentStatus,',')))
		  AND (@partnerIds IS NULL OR [PartnerId] IN (SELECT value FROM dbo.ParamParserString(@partnerIds,',')))
		  AND (@orderIds IS NULL OR [Id] IN (SELECT value FROM dbo.ParamParserString(@orderIds,',')))
		  AND (@carrierIds IS NULL OR [CarrierId] IN (SELECT value FROM dbo.ParamParserString(@carrierIds,',')))
	END
	
END

GO


