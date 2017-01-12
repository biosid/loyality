IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetOrderByExternalId]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[GetOrderByExternalId]
GO

CREATE PROCEDURE [prod].[GetOrderByExternalId] 
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
		,[ExternalOrderStatusDescription]
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

GO


