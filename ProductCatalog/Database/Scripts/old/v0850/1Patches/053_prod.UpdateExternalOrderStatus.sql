IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[UpdateExternalOrderStatus]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[UpdateExternalOrderStatus]
GO

CREATE procedure [prod].[UpdateExternalOrderStatus] 
     @OrderId int = null
 	,@ExternalOrderId nvarchar(36) = null
 	,@PartnerId int = null
 	,@Status int
 	,@ExternalOrderStatusCode nvarchar(50) = null
 	,@OrderStatusDescription nvarchar(1000) = null
 	,@ExternalOrderStatusDateTime datetime = null	
 	,@ClientId nvarchar(255) = null
 	,@UpdatedUserId nvarchar(255)
 	,@MissingOrderId int OUTPUT 
 	,@WrongWorkflow bit OUTPUT
AS
BEGIN

if (@OrderId is null and @ExternalOrderId is null)
begin
	RAISERROR (N'@OrderId is null and @ExternalOrderId is null', 17, 1);
	RETURN
end

if (@ClientId is not null)
begin
	if (NOT EXISTS (SELECT 1 FROM [prod].[Orders] o 
	                WHERE @ClientId = o.ClientId
					  AND (	(@OrderId is not null and Id = @OrderId)
							or
							(@ExternalOrderId is not null and (ExternalOrderId = @ExternalOrderId and @PartnerId = PartnerId))
						  )))
	begin
		DECLARE @mess nvarchar(300) = N'Client ' + @ClientId + ' is not owner of order'
		RAISERROR (@mess, 17, 1);
		RETURN
	end
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
BEGIN
	SET @WrongWorkflow = 1
	RETURN
END
ELSE
BEGIN
	SET @WrongWorkflow = 0
END

UPDATE [prod].[Orders]
set     
[ExternalOrderId] = isnull(@ExternalOrderId,ExternalOrderId)
,[Status] = @Status
,[ExternalOrderStatusCode] = @ExternalOrderStatusCode
,[OrderStatusDescription] = @OrderStatusDescription
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


