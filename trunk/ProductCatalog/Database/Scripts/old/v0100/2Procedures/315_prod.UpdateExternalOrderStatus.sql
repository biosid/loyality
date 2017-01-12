alter procedure [prod].[UpdateExternalOrderStatus] @OrderId int = null
 	,@ExternalOrderId nvarchar(36) = null
 	,@PartnerId int = null
 	,@Status int
 	,@ExternalOrderStatusCode nvarchar(50) = null
 	,@ExternalOrderStatusDescription nvarchar(1000) = null
 	,@ExternalOrderStatusDateTime datetime = null	
 	,@UpdatedUserId nvarchar(255)
 	,@MissingOrderId int OUTPUT as
BEGIN

if (@OrderId is null and @ExternalOrderId is null)
begin
	RAISERROR (N'@OrderId is null and @ExternalOrderId is null', 10, 1);
end

declare @target int
set @target = (select top 1 Id from prod.Orders where Id = @OrderId)

if(@target is null)
begin
	set @target = 
	(
		select top 1 Id
		from prod.Orders 
		where ExternalOrderId = @ExternalOrderId and PartnerId = @PartnerId
	)
	if(@target is null)
		begin 
			set @MissingOrderId = -1 --ExternalOrderId + partnerId is missing
		end
	else
		begin
			set @MissingOrderId = 0 --missing order
		end	
end
else
begin			
	set @MissingOrderId = 0 --ok
end

IF (NOT EXISTS(SELECT TOP 1 f.* FROM [prod].[Orders] o
			JOIN [prod].[FullOrderStatusWorkFlow] f 
			ON o.[Status] = @Status OR (o.[Status] = f.[FromStatus] AND f.[ToStatus] = @Status)
			WHERE  [Id] = @target))
begin
	DECLARE @StatusAsStr VARCHAR(50) = CAST(@Status as nvarchar(50))
	DECLARE @targetAsStr VARCHAR(50) = CAST(@target as nvarchar(50))
	RAISERROR (N'Can''t set status %s for order id = %s' , 17, 1, @StatusAsStr, @targetAsStr);	
	RETURN
end

UPDATE [prod].[Orders]
set     
[ExternalOrderId] = isnull(@ExternalOrderId,ExternalOrderId)
,[Status] = @Status
,[ExternalOrderStatusCode] = @ExternalOrderStatusCode
,[ExternalOrderStatusDescription] = @ExternalOrderStatusDescription
,ExternalOrderStatusDateTime = @ExternalOrderStatusDateTime
,[StatusChangedDate] = getdate()
,[StatusUtcChangedDate] = GETUTCDATE()
,[UpdatedUserId] = @UpdatedUserId
where
(@OrderId is not null and Id = @OrderId)
or
(@ExternalOrderId is not null and (ExternalOrderId = @ExternalOrderId and @PartnerId = PartnerId))

END



GO


