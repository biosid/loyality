SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [prod].[FillPopularProducts]
AS
BEGIN

declare @nowDate datetime
declare @pastDate datetime

set @nowDate = GETDATE()
set @pastDate = DATEADD(DAY, -15, @nowDate)

-- Заполнение наиболее откладываемых
delete from [prod].[PopularProducts] where PopularType = 0

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
    SELECT ProductId, 0, count(*)
    FROM [prod].[WishListItems]
    where CreatedDate between @pastDate and @nowDate
    group by [ProductId]
    order by count(*) desc

---- Заполнение наиболее заказываемых
delete from [prod].[PopularProducts] where PopularType = 1

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
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

-- Заполнение наиболее просматриваемых
delete from [prod].[PopularProducts] where PopularType = 2

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
    SELECT ProductId, 2, count(*)
    FROM [prod].[ProductViewStatistics]
    where UpdatedDate between @pastDate and @nowDate
    group by [ProductId]
    order by count(*) desc
;

-- Удаление лишних дубликатов по группам (фразам в кавычках в названиях вознаграждений)

-- Пояснения:
-- * вознаграждение входит в группу, если в его названии присутстсвует имя группы в кавычках;
-- * вознаграждение может входит в несколько групп (либо ни в одну группу);
-- * в рамках одного типа популярности не должно быть нескольких вознаграждений из одной группы;
-- * если в рамках одного типа популярности для определенной группы присутствует несколько вознаграждений,
--   то остается вознаграждение с наивысшим PopularRate.

-- Следствие: если вознаграждение входит в группы "g1" и "g2", и при этом есть другое вознаграждение
-- в группе "g1" с более высоким PopularRate, то первое вознаграждение не будет присутствовать в популярных,
-- даже если нет других вознаграждений в группе "g2".
with groupedPopularProducts as
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
    ) t1
)
delete pp
from
    prod.PopularProducts pp
    inner join groupedPopularProducts gpp on
        gpp.RowNumber > 1 and
        pp.PopularType = gpp.PopularType and pp.ProductId = gpp.ProductId

END
