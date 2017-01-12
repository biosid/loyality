
ALTER PROCEDURE [prod].[GetOrderByExternalId] 
	@ExternalOrderId nvarchar(36),
	@ClientId nvarchar(255) = null
	AS
BEGIN
	SELECT
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
	FROM 
		[prod].[Orders]
	WHERE 
		[ExternalOrderId] = @ExternalOrderId
		AND ((@ClientId IS NOT NULL AND [ClientId] = @ClientId) OR @ClientId IS NULL)
END
