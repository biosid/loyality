
DROP PROCEDURE [prod].[GetOrder]
GO

CREATE PROCEDURE [prod].[GetOrder]
    @Id int = null,
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
		,[ItemsAdvance]
        ,[DeliveryCost]
        ,[BonusDeliveryCost]
        ,[DeliveryAdvance]
        ,[TotalCost]
        ,[BonusTotalCost]
		,[TotalAdvance]
        ,[PartnerId]
        ,[PaymentStatus]
        ,[DeliveryPaymentStatus]
        ,[CarrierId]
        ,[DeliveryInstructions]
    from [prod].[Orders]
    where
        [Id] = @id
        and
        ((@ClientId is not null and [ClientId] = @ClientId) or @ClientId is null)
END

GO