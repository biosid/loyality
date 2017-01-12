
create procedure [prod].[GetOrder] 
	@Id int = null,
 	@InsertedUserId nvarchar(255) = null
	as
BEGIN
select
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
from 
	[prod].[Orders]
where 
	[Id] = @id
	and ((@InsertedUserId is not null and [InsertedUserId] = @InsertedUserId) or @InsertedUserId is null)
END

