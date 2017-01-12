/****** Object:  StoredProcedure [prod].[UpdateExternalOrderStatus]    Script Date: 03/21/2014 15:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [prod].[UpdateExternalOrderStatus]
    @OrderId int = null
    ,@ExternalOrderId nvarchar(36) = null
    ,@PartnerId int = null
    ,@Status int
    ,@ExternalOrderStatusCode nvarchar(50) = null
    ,@OrderStatusDescription nvarchar(1000) = null
    ,@ExternalOrderStatusDateTime datetime = null
    ,@ClientId nvarchar(255) = null
    ,@UpdatedUserId nvarchar(255)
    ,@OutOrderId int OUTPUT
    ,@MissingOrderId int OUTPUT
    ,@WrongWorkflow int OUTPUT
AS
BEGIN

if (@OrderId is null and @ExternalOrderId is null)
begin
    RAISERROR (N'@OrderId is null and @ExternalOrderId is null', 17, 1);
    return
end

declare @target int
set @target = (select top 1 Id from prod.Orders where Id = @OrderId)

if (@target is null)
begin
    set @target =
    (
        select top 1 Id
        from prod.Orders
        where ExternalOrderId = @ExternalOrderId and PartnerId = @PartnerId
    )
    if (@target is null)
    begin
        set @MissingOrderId = -1 --ExternalOrderId + partnerId is missing
        return
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

if (@ClientId is not null)
begin
    if (not exists (select 1
                    from [prod].[Orders]
                    where [Id] = @target and [ClientId] = @ClientId))
    begin
        declare @mess nvarchar(300) = N'Client ' + @ClientId + ' is not owner of order'
        RAISERROR (@mess, 17, 1);
        return
    end
end

set @OutOrderId = @target

if (not exists(select top 1 f.* from [prod].[Orders] o
               join [prod].[FullOrderStatusWorkFlow] f
               on o.[Status] = @Status or (o.[Status] = f.[FromStatus] and f.[ToStatus] = @Status)
               where [Id] = @target))
begin
    set @WrongWorkflow = 1
    return
end

-- нельзя устанавливать статус далее чем "в обработке", пока пользователь не подтвердил заказ
-- (иными словами, нельзя пропускать статус "в обработке")
if (@Status > 10 and
    not exists(select TOP 1 *
               from [prod].[Orders]
               where [Id] = @target and [Status] >= 10))
begin
    set @WrongWorkflow = 2
    return
end

set @WrongWorkflow = 0

update [prod].[Orders]
set
[ExternalOrderId] = isnull(@ExternalOrderId,ExternalOrderId)
,[Status] = @Status
,[ExternalOrderStatusCode] = @ExternalOrderStatusCode
,[OrderStatusDescription] = @OrderStatusDescription
,[ExternalOrderStatusDateTime] = @ExternalOrderStatusDateTime
,[StatusChangedDate] = getdate()
,[StatusUtcChangedDate] = GETUTCDATE()
,[UpdatedUserId] = @UpdatedUserId
where Id = @target

END
