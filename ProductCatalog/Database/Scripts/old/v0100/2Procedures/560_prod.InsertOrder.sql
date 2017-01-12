create procedure [prod].[InsertOrder] @PartnerId int
 	,@ExternalOrderId nvarchar(36) = null
 	,@Status int
 	,@PaymentStatus int
 	,@DeliveryPaymentStatus int
 	,@ExternalOrderStatusCode nvarchar(50) = null
 	,@ExternalOrderStatusDescription nvarchar(1000) = null
 	,@ExternalOrderStatusDateTime datetime = null
 	,@DeliveryInfo xml = null
 	,@InsertedUserId nvarchar(255)	
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
	as
BEGIN

INSERT INTO [prod].[Orders]
           (
		   PartnerId
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
			,[PaymentStatus]
			,[DeliveryPaymentStatus]
			,[CarrierId]
		   )
     VALUES
           (@PartnerId
		   ,@ExternalOrderId
           ,@Status
		   ,@ExternalOrderStatusCode
		   ,@ExternalOrderStatusDescription
		   ,@ExternalOrderStatusDateTime
           ,getdate()
           ,getutcdate()
		   ,@DeliveryInfo
		   ,getdate()
           ,getutcdate()
		   ,@InsertedUserId                     
           ,getdate()
           ,getutcdate()
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

select @@IDENTITY
END

