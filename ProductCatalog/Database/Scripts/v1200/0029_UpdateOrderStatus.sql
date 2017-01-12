/****** Object:  StoredProcedure [prod].[UpdateOrderStatus]    Script Date: 03/21/2014 15:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [prod].[UpdateOrderStatus]
    @OrderId int = null
    ,@Status int = null
    ,@UpdatedUserId nvarchar(255)
    ,@ClientId nvarchar(255) = null
    ,@OrderStatusDescription nvarchar(1000)
    ,@MissingOrderId int OUTPUT
    ,@WrongWorkflow int OUTPUT
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
    set @MissingOrderId = -1
    return
end
else
begin
    set @MissingOrderId = 0
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

if (@Status is not null and
    not exists(select top 1 f.*
               from [prod].[Orders] o
               join [prod].[FullOrderStatusWorkFlow] f
               on o.[Status] = @Status
                  or
                  (o.[Status] = f.[FromStatus] and f.[ToStatus] = @Status)
               where o.[Id] = @target))
begin
    set @WrongWorkflow = 1
    return
end

-- нельзя устанавливать статус далее чем "в обработке", пока пользователь не подтвердил заказ
-- (иными словами, нельзя пропускать статус "в обработке")
if (@Status > 10 and
    not exists(select top 1 *
               from [prod].[Orders]
               where [Id] = @target and [Status] >= 10))
begin
    set @WrongWorkflow = 2
    return
END

set @WrongWorkflow = 0

update [prod].[Orders]
set
[Status] = isnull(@Status, [Status])
,[StatusChangedDate] = getdate()
,[StatusUtcChangedDate] = GETUTCDATE()
,[UpdatedUserId] = @UpdatedUserId
,[OrderStatusDescription] = @OrderStatusDescription
where Id = @target

END
