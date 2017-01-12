/****** Object:  StoredProcedure [prod].[InsertOrder]    Script Date: 07/28/2014 18:13:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [prod].[InsertOrder]
    @ClientId [nvarchar](255)
   ,@PartnerId int
   ,@ExternalOrderId nvarchar(36) = null
   ,@Status int
   ,@PaymentStatus int
   ,@DeliveryPaymentStatus int
   ,@ExternalOrderStatusCode nvarchar(50) = null
   ,@OrderStatusDescription nvarchar(1000) = null
   ,@ExternalOrderStatusDateTime datetime = null
   ,@DeliveryInfo xml = null
   ,@UpdatedUserId nvarchar(255)
   ,@Items xml
   ,@TotalWeight int
   ,@ItemsCost money
   ,@BonusItemsCost int
   ,@DeliveryCost money
   ,@BonusDeliveryCost int
   ,@DeliveryAdvance money
   ,@TotalCost money
   ,@BonusTotalCost int
   ,@CarrierId int = null
AS
BEGIN
    insert into [prod].[Orders]
    (
         [ClientId]
        ,[PartnerId]
        ,[ExternalOrderId]
        ,[Status]
        ,[ExternalOrderStatusCode]
        ,[OrderStatusDescription]
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
        ,[DeliveryAdvance]
        ,[TotalCost]
        ,[BonusTotalCost]
        ,[PaymentStatus]
        ,[DeliveryPaymentStatus]
        ,[CarrierId]
    )
    values
    (
         @ClientId
        ,@PartnerId
        ,@ExternalOrderId
        ,@Status
        ,@ExternalOrderStatusCode
        ,@OrderStatusDescription
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
        ,@DeliveryAdvance
        ,@TotalCost
        ,@BonusTotalCost
        ,@PaymentStatus
        ,@DeliveryPaymentStatus
        ,@CarrierId
    )

    select @@identity

END
