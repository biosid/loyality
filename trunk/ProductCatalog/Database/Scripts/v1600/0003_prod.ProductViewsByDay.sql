create table prod.ProductViewsByDay
(
    ViewsDate date not null,
    ProductId nvarchar(256) not null,
    ViewsCount int not null,
    constraint PK_ProductViewsByDay primary key clustered
    (
        ViewsDate asc,
        ProductId asc
    )
    with
    (
        pad_index = off,
        statistics_norecompute  = off,
        ignore_dup_key = off,
        allow_row_locks = on,
        allow_page_locks  = on
    ) on [primary]
) on [primary]

-- declare @date datetime2 = dateadd(day, -2, getutcdate())

-- insert into prod.ProductViewsByDay(ViewsDate, ProductId, ViewsCount)
-- select @date, ProductId, sum(ViewCount)
-- from prod.ProductViewStatistics
-- group by ProductId

-- insert into prod.PopularProducts(PopularType, ProductId, PopularRate)
-- select 4, ProductId, sum(ViewCount)
-- from prod.ProductViewStatistics
-- group by ProductId
