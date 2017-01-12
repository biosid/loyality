ALTER procedure [prod].[UpdateOrderStatus] 
	@OrderId int = null
 	,@Status int = null
 	,@UpdatedUserId nvarchar(255)
 	,@ClientId nvarchar(255) = null
	,@OrderStatusDescription nvarchar(1000)
 	,@MissingOrderId int OUTPUT as
BEGIN

if (@OrderId is null)
begin
	RAISERROR (N'@OrderId is null', 17, 1);
	RETURN
end

if (@ClientId is not null)
begin
	if (NOT EXISTS (SELECT 1 FROM [prod].[Orders] o 
	                WHERE @ClientId = o.ClientId
					  AND (@OrderId is not null and Id = @OrderId)))
	begin
		DECLARE @mess nvarchar(300) = N'Client ' + @ClientId + ' is not owner of order'
		RAISERROR (@mess, 17, 1);
		RETURN
	end
end

declare @targetOrderId int
set @targetOrderId = (select top 1 Id from prod.Orders where Id = @OrderId)

if(@targetOrderId is null)
begin
	set @MissingOrderId = @OrderId
end
else
begin
	set @MissingOrderId = 0
end

IF (@Status IS NOT NULL AND NOT EXISTS(SELECT TOP 1 f.* FROM [prod].[Orders] o
									JOIN [prod].[FullOrderStatusWorkFlow] f 
									ON o.[Status] = @Status OR (o.[Status] = f.[FromStatus] AND f.[ToStatus] = @Status)
									WHERE  [Id] = @OrderId))
begin
	DECLARE @StatusAsStr VARCHAR(50) = CAST(@Status as nvarchar(50))
	DECLARE @OrderIdAsStr VARCHAR(50) = CAST(@OrderId as nvarchar(50))
	RAISERROR (N'Can''t set status %s for order id = %s' , 17, 1, @StatusAsStr, @OrderIdAsStr);	
	RETURN
end

UPDATE [prod].[Orders]
set     
[Status] = isnull(@Status, [Status])
,[StatusChangedDate] = getdate()
,[StatusUtcChangedDate] = GETUTCDATE()
,[UpdatedUserId] = @UpdatedUserId
,[OrderStatusDescription] = @OrderStatusDescription
where
(@OrderId is not null and Id = @OrderId)

END