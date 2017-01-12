/****** Object:  StoredProcedure [prod].[FillOrdersNotifications]    Script Date: 12/20/2013 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [prod].[FillOrdersNotifications]
    @EtlSessionId uniqueidentifier,
    @MaxOrdersCount int
AS
BEGIN
    SET NOCOUNT ON;

    insert into prod.OrdersNotifications(OrderId, EtlSessionId)
        select top (@MaxOrdersCount) Id, @EtlSessionId
        from prod.Orders o
        left join prod.OrdersNotifications n
        on o.Id = n.OrderId
        where n.OrderId is null
        order by o.Id

    select @@ROWCOUNT
END
