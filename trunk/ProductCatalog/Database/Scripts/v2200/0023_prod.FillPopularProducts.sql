/****** Object:  StoredProcedure [prod].[FillPopularProducts]    Script Date: 09/22/2014 00:02:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [prod].[FillPopularProducts]
    @viewsDate date
AS
BEGIN

declare @today date = getdate()
declare @twoWeeksAgo date = dateadd(day, -14, @today)


-- Заполнение наиболее откладываемых
delete from prod.PopularProducts
where PopularType = 0

insert into prod.PopularProducts (ProductId, PopularType, PopularRate)
select ProductId, 0, count(1)
from prod.WishListItems
where CreatedDate between @twoWeeksAgo and @today
group by ProductId
order by count(1) desc


---- Заполнение наиболее заказываемых
delete from prod.PopularProducts
where PopularType = 1

insert into prod.PopularProducts (ProductId, PopularType, PopularRate)
select ProductId, 1, count(ProductId)
from
(
    select ProductId, ClientId
    from
    (
        select OrderItem.value('./Product[1]/ProductId[1]', 'nvarchar(256)') as ProductId, ClientId
        from prod.Orders
        cross apply Items.nodes('/ArrayOfOrderItem/OrderItem') as OrderItems(OrderItem)
        where InsertedDate between @twoWeeksAgo and @today
    ) as t
    where ProductId is not null
    group by ProductId, ClientId
) as t2
group by ProductId
order by count(ProductId) desc

-- Заполнение наиболее просматриваемых за две недели
delete from prod.PopularProducts
where PopularType = 2

insert into prod.PopularProducts (ProductId, PopularType, PopularRate)
select ProductId, 2, sum(ViewsCount)
from prod.ProductViewsByDay
where ViewsDate between @twoWeeksAgo and @today
group by ProductId


-- Заполнение наиболее просматриваемых за две недели с перемешиванием топ 300
delete from prod.PopularProducts
where PopularType = 3

-- находим самое высокое значение рейтинга по просмотрам
declare @maxPopularRateByView int
select @maxPopularRateByView = max(PopularRate)
from prod.PopularProducts
where PopularType = 2

insert into prod.PopularProducts (ProductId, PopularType, PopularRate)
select ProductId, 3, PopularRate
from
(
    -- первые 300 самых популярных по просмотрам перемешиваем в случайном порядке
    select t2.ProductId, (t2.RowNumber + @maxPopularRateByView) as PopularRate
    from
    (
        select t1.ProductId, row_number() over (order by newid()) as RowNumber
        from
        (
            select ProductId, PopularRate, row_number() over (order by PopularRate desc) as RowNumber
            from prod.PopularProducts
            where PopularType = 2
        ) t1
        where t1.RowNumber <= 300
    ) t2

    union all

    -- остальные копируем
    select t3.ProductId, t3.PopularRate
    from
    (
        select ProductId, PopularRate, row_number() over (order by PopularRate desc) as RowNumber
        from prod.PopularProducts
        where PopularType = 2
    ) t3
    where t3.RowNumber > 300
) t
order by PopularRate desc


-- Заполнение наиболее просматриваемых
merge prod.PopularProducts dst
using
(
    select 4 as PopularType, ProductId, ViewsCount as PopularRate
    from prod.ProductViewsByDay
    where ViewsDate = @viewsDate
) src
on dst.PopularType = src.PopularType and dst.ProductId = src.ProductId
when matched then
    update set dst.PopularRate = dst.PopularRate + src.PopularRate
when not matched then
    insert (PopularType, ProductId, PopularRate)
    values (4, src.ProductId, src.PopularRate);

-- Удаление лишних дубликатов по группам (фразам в кавычках в названиях вознаграждений)
-- для определенных типов популярности:
-- 0 (желаемое) - удаляем
-- 1 (заказываемое) - удаляем
-- 2 (просматриваемое за две недели) - удаляем
-- 3 (просматриваемое за две недели с перемешиваением топ 300) - НЕ удаляем
-- 4 (просматриваемое за все время) - НЕ удаляем
-- 5 (просматриваемое за две недели - топ 300) - НЕ удаляем (копируем из типа 2, где дубликаты уже удалены)

-- Пояснения:
-- * вознаграждение входит в группу, если в его названии присутстсвует имя группы в кавычках;
-- * вознаграждение может входит в несколько групп (либо ни в одну группу);
-- * в рамках одного типа популярности не должно быть нескольких вознаграждений из одной группы;
-- * если в рамках одного типа популярности для определенной группы присутствует несколько вознаграждений,
--   то остается вознаграждение с наивысшим PopularRate.

-- Следствие: если вознаграждение входит в группы "g1" и "g2", и при этом есть другое вознаграждение
-- в группе "g1" с более высоким PopularRate, то первое вознаграждение не будет присутствовать в популярных,
-- даже если нет других вознаграждений в группе "g2".
;with groupedPopularProducts as
(
    select
        t1.PopularType,
        t1.ProductId,
        row_number() over (partition by PopularType, GroupName order by PopularRate desc) as RowNumber
    from
    (
        select
            pp.ProductId,
            pp.PopularRate,
            p.Name as ProductName,
            pp.PopularType,
            g.name as GroupName
        from
            prod.PopularProducts pp
            join prod.Products p on pp.ProductId = p.ProductId
            cross apply prod.GetProductGroupNames(p.Name) g
        where
            pp.PopularType in (0, 1, 2)
    ) t1
)
delete pp
from
    prod.PopularProducts pp
    inner join groupedPopularProducts gpp on
        gpp.RowNumber > 1 and
        pp.PopularType = gpp.PopularType and pp.ProductId = gpp.ProductId


-- Заполнение топ 300 наиболее просматриваемых
delete from prod.PopularProducts
where PopularType = 5

insert into prod.PopularProducts (ProductId, PopularType, PopularRate)
select t.ProductId, 5, t.PopularRate
from
(
    select ProductId, PopularRate, row_number() over (order by PopularRate desc) as RowNumber
    from prod.PopularProducts
    where PopularType = 2
) t
where t.RowNumber <= 300

END
