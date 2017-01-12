IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[InsertOrder]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[InsertOrder]
GO

CREATE PROCEDURE [prod].[InsertOrder] 
	 @ClientId [nvarchar](255)
	,@PartnerId int
 	,@ExternalOrderId nvarchar(36) = null
 	,@Status int
 	,@PaymentStatus int
 	,@DeliveryPaymentStatus int
 	,@ExternalOrderStatusCode nvarchar(50) = null
 	,@ExternalOrderStatusDescription nvarchar(1000) = null
 	,@ExternalOrderStatusDateTime datetime = null
 	,@DeliveryInfo xml = null

 	,@UpdatedUserId nvarchar(255)
 	,@Items xml
 	,@TotalWeight int
 	,@ItemsCost money
 	,@BonusItemsCost int
 	,@DeliveryCost money
 	,@BonusDeliveryCost int
 	,@TotalCost money
 	,@BonusTotalCost int 
	,@CarrierId int = null
	AS
BEGIN

	INSERT INTO [prod].[Orders]
			   (
				 [ClientId]
				,[PartnerId]
				,[ExternalOrderId]
				,[Status]
				,[ExternalOrderStatusCode]
				,[ExternalOrderStatusDescription]
				,ExternalOrderStatusDateTime
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
				,[PaymentStatus]
				,[DeliveryPaymentStatus]
				,[CarrierId]
			   )
		 VALUES
			   (@ClientId
			   ,@PartnerId
			   ,@ExternalOrderId
			   ,@Status
			   ,@ExternalOrderStatusCode
			   ,@ExternalOrderStatusDescription
			   ,@ExternalOrderStatusDateTime
			   ,GETDATE()
			   ,GETUTCDATE()
			   ,@DeliveryInfo
			   ,GETDATE()
			   ,GETUTCDATE()
			   ,GETDATE()
			   ,GETUTCDATE()
			   ,@UpdatedUserId
			   ,@Items
			   ,@TotalWeight
			   ,@ItemsCost
			   ,@BonusItemsCost
			   ,@DeliveryCost
			   ,@BonusDeliveryCost
			   ,@TotalCost
			   ,@BonusTotalCost
			   ,@PaymentStatus
			   ,@DeliveryPaymentStatus
			   ,@CarrierId
			   )

	SELECT @@IDENTITY
END


GO


