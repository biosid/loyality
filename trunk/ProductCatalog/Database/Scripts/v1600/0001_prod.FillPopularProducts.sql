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

-- ���������� �������� �������������
delete from [prod].[PopularProducts] where PopularType = 0

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
    SELECT ProductId, 0, count(*)
    FROM [prod].[WishListItems]
    where CreatedDate between @pastDate and @nowDate
    group by [ProductId]
    order by count(*) desc

---- ���������� �������� ������������
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

-- ���������� �������� ���������������
delete from [prod].[PopularProducts] where PopularType = 2

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
    SELECT ProductId, 2, count(*)
    FROM [prod].[ProductViewStatistics]
    where UpdatedDate between @pastDate and @nowDate
    group by [ProductId]
    order by count(*) desc

-- ���������� �������� ��������������� � �������������� ��� 300
delete from [prod].[PopularProducts] where PopularType = 3

-- ������� ����� ������� �������� �������� �� ����������
declare @maxPopularRateByView int
select @maxPopularRateByView = max(PopularRate)
from prod.PopularProducts
where PopularType = 2

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
select ProductId, 3, PopularRate
from
    (
        -- ������ 300 ����� ���������� �� ���������� ������������ � ��������� �������
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
                where
                    t1.RowNumber <= 300
            ) t2

        union all

        -- ��������� ��������
        select t3.ProductId, t3.PopularRate
        from
            (
                select ProductId, PopularRate, row_number() over (order by PopularRate desc) as RowNumber
                from prod.PopularProducts
                where PopularType = 2
            ) t3
        where
            t3.RowNumber > 300
    ) t
order by
    PopularRate desc


-- �������� ������ ���������� �� ������� (������ � �������� � ��������� ��������������)
-- ��� ������������ ����� ������������:
-- 0 (��������) - �������
-- 1 (������������) - �������
-- 2 (���������������) - �������
-- 3 (��������������� � ��������������� ��� 300) - �� �������

-- ���������:
-- * �������������� ������ � ������, ���� � ��� �������� ������������� ��� ������ � ��������;
-- * �������������� ����� ������ � ��������� ����� (���� �� � ���� ������);
-- * � ������ ������ ���� ������������ �� ������ ���� ���������� �������������� �� ����� ������;
-- * ���� � ������ ������ ���� ������������ ��� ������������ ������ ������������ ��������� ��������������,
--   �� �������� �������������� � ��������� PopularRate.

-- ���������: ���� �������������� ������ � ������ "g1" � "g2", � ��� ���� ���� ������ ��������������
-- � ������ "g1" � ����� ������� PopularRate, �� ������ �������������� �� ����� �������������� � ����������,
-- ���� ���� ��� ������ �������������� � ������ "g2".
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

END
