/****** Object:  StoredProcedure [prod].[FillOrdersNotifications]    Script Date: 12/23/2013 12:23:33 ******/
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
        select top (@MaxOrdersCount) o.Id, @EtlSessionId
        from prod.Orders o
        left join prod.OrdersNotifications n
        on o.Id = n.OrderId
        inner join prod.Partners p
        on o.PartnerId = p.Id
        where n.OrderId is null and (
            ((p.[Type] = 0 or p.[Type] = 1) and -- онлайн или директ партнер
             (o.[Status] = 10 or                -- в обработке
              o.[Status] = 30 or                -- требует доставки
              o.[Status] = 40 or                -- доставка
              o.[Status] = 50 or                -- доставлен
              o.[Status] = 51 or                -- доставлен с задержкой
              o.[Status] = 60)) or              -- не доставлен
            (p.[Type] = 2 and                   -- оффлайн партнер
             (o.[Status] = 30 or                -- требует доставки
              o.[Status] = 40 or                -- доставка
              o.[Status] = 50 or                -- доставлен
              o.[Status] = 51 or                -- доставлен с задержкой
              o.[Status] = 60)))                -- не доставлен
        order by o.Id

    select @@ROWCOUNT
END
