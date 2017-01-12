ALTER PROCEDURE [prod].[FillPopularProducts]
AS
BEGIN

    declare @nowDate datetime = GETDATE()
    declare @pastDate datetime = DATEADD(DAY, -15, @nowDate)

    -- «аполнение наиболее откладываемых
    delete from prod.PopularProducts where PopularType = 0

    insert into prod.PopularProducts (ProductId,PopularType,PopularRate)
        select ProductId, 0, count(*)
        from prod.WishListItems
        where CreatedDate between @pastDate and @nowDate
        group by ProductId
        order by count(*) desc

    -- «аполнение наиболее заказываемых
    delete from prod.PopularProducts where PopularType = 1

    insert into prod.PopularProducts (ProductId,PopularType,PopularRate)
        select ProductId, 1, count(ProductId)
        from
        (
            select ProductId, ClientId
            from
            (
                select OrderItem.value('./Product[1]/ProductId[1]', 'nvarchar(256)') as ProductId, ClientId
                from prod.Orders
                cross apply Items.nodes('/ArrayOfOrderItem/OrderItem') as OrderItems(OrderItem)
                where InsertedDate between @pastDate and @nowDate
            ) as t
            where ProductId is not null
            group by ProductId, ClientId
        ) as t2
        group by ProductId
        order by count(ProductId) desc

    -- «аполнение наиболее просматриваемых
    delete from prod.PopularProducts where PopularType = 2

    insert into prod.PopularProducts (ProductId,PopularType,PopularRate)
        select ProductId, 2, count(*)
        from prod.ProductViewStatistics
        where UpdatedDate between @pastDate and @nowDate
        group by ProductId
        order by count(*) desc

END
