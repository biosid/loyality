CREATE procedure [prod].[UpdateOrderDeliveryStatus] 
	@OrderId int
 	,@DeliveryStatus int  	
 	,@UpdatedUserId nvarchar(255)
 	,@MissingOrderId int OUTPUT as
BEGIN

if (@OrderId is null)
begin
	RAISERROR (N'@OrderId is null', 10, 1);
end

declare @targetOrderId int
set @targetOrderId = (select top 1 Id from prod.Orders where Id = @OrderId)

if(@targetOrderId is null)
begin
	set @MissingOrderId = @OrderId
end
else
begin
	set @MissingOrderId = NULL
end

UPDATE [prod].[Orders]
set     
[DeliveryPaymentStatus] = @DeliveryStatus
,[StatusChangedDate] = getdate()
,[StatusUtcChangedDate] = GETUTCDATE()
,[UpdatedUserId] = @UpdatedUserId
where
(@OrderId is not null and Id = @OrderId)

END


GO


