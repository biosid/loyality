SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

update prod.Orders
set
    Status = 0,
    OrderStatusDescription = 'миграция заказов (данный заказ был ошибочно аннулирован в результате неуспешного вызова CheckOrder)',
    StatusChangedDate = getdate(),
    StatusUtcChangedDate = GETUTCDATE(),
    UpdatedUserId = 'vtbSystemUser'
where Status = 20 and ExternalOrderId is null
