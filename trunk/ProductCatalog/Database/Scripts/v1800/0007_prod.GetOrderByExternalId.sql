SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [prod].[GetOrderByExternalId]
    @ExternalOrderId nvarchar(36),
    @ClientId nvarchar(255) = null
AS
BEGIN
    select
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
        ,[DeliveryAdvance]
        ,[TotalCost]
        ,[BonusTotalCost]
        ,[PartnerId]
        ,[PaymentStatus]
        ,[DeliveryPaymentStatus]
        ,[CarrierId]
        ,[DeliveryInstructions]
    from [prod].[Orders]
    where
        [ExternalOrderId] = @ExternalOrderId
        and
        ((@ClientId is not null and [ClientId] = @ClientId) OR @ClientId is null)
END
