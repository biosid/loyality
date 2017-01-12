create procedure [prod].[GetOrders] @userId nvarchar(255),
 	@dateTimeStart [datetime],
 	@dateTimeEnd [datetime],
 	@countToTake [int],
 	@countToSkip [int],
 	@calcTotalCount [bit],
 	@statuses nvarchar(MAX) = null,
 	@skipStatuses nvarchar(MAX) = null,
 	@totalCount [int] out as
BEGIN
SET NOCOUNT ON

SELECT TOP (@countToTake)
	 [Id]
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
	  WHERE [InsertedUserId] = @userId
        AND [InsertedDate] BETWEEN @dateTimeStart AND @dateTimeEnd
        AND (@statuses IS NULL OR [Status] IN (SELECT value FROM dbo.ParamParserString(@statuses,',')))
        AND (@skipStatuses IS NULL OR [Status] NOT IN (SELECT value FROM dbo.ParamParserString(@skipStatuses,',')))) orders
WHERE rownum > @countToSkip

IF (@calcTotalCount != 0)
BEGIN
	SELECT @totalCount = count(1)
	FROM [prod].[Orders]
	WHERE [InsertedUserId] = @userId
	  AND [InsertedDate] BETWEEN @dateTimeStart AND @dateTimeEnd
	  AND (@statuses IS NULL OR [Status] IN (SELECT value FROM dbo.ParamParserString(@statuses,',')))
      AND (@skipStatuses IS NULL OR [Status] NOT IN (SELECT value FROM dbo.ParamParserString(@skipStatuses,',')))
END
	
END

