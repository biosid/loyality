/****** Object:  StoredProcedure [prod].[UpdateOrderStatus]    Script Date: 03/25/2014 17:32:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [prod].[UpdateOrderStatus]
    @OrderId int = null,
    @ClientId nvarchar(255) = null,
    @Status int = null,
    @OrderStatusDescription nvarchar(1000) = null,
    @UpdatedUserId nvarchar(255),
    @OriginalStatus int OUTPUT,
    @OrderNotFound bit OUTPUT,
    @WrongWorkflow bit OUTPUT
AS
BEGIN

if (@OrderId is null)
begin
    RAISERROR (N'@OrderId is null', 17, 1);
    return
end

declare @target int
set @target = (select top 1 Id from prod.Orders where Id = @OrderId)

if(@target is null)
begin
    set @OrderNotFound = 1
    return
end

set @OrderNotFound = 0

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

set @OriginalStatus = (select top 1 [Status] from [prod].[Orders] where [Id] = @target)

if (@Status is not null
    and
    not exists(select 1
               from [prod].[Orders]
               where [Id] = @target
                     and
                     ([Status] = @Status or exists (select 1
                                                    from [prod].[OrderStatusWorkFlow]
                                                    where [FromStatus] = [Status] and [ToStatus] = @Status))))
begin
    set @WrongWorkflow = 1
    return
end

set @WrongWorkflow = 0

update [prod].[Orders]
set
     [Status] = isnull(@Status, [Status]),
     [OrderStatusDescription] = @OrderStatusDescription,
     [StatusChangedDate] = getdate(),
     [StatusUtcChangedDate] = GETUTCDATE(),
     [UpdatedUserId] = @UpdatedUserId
where Id = @target

END
