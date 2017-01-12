if not exists(select * from sys.columns where Name = N'GroupId' and Object_ID = Object_ID(N'prod.PopularProducts'))
begin
    alter table [prod].[PopularProducts]
    add [GroupId] [nvarchar](256) NULL
end
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [prod].[GetProductGroupName]
(
	@productName nvarchar(255)
)
RETURNS NVARCHAR(255)
WITH SCHEMABINDING
AS 
BEGIN

DECLARE @startIndex int, @endIndex int

SET @startIndex = Charindex(N'"', @productName, 0)

IF (@startIndex <> 1)
	RETURN NULL

SET @endIndex = Charindex(N'"', @productName, @startIndex + 1)

IF (@endIndex = 0)
	RETURN NULL

RETURN Substring(@productName, @startIndex + 1, @endIndex - @startIndex - 1)

END
GO


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
			select Items.value('/ArrayOfOrderItem[1]/OrderItem[1]/Product[1]/ProductId[1]', 'nvarchar(256)') as ProductId, ClientId
			from [prod].[Orders]
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
	
--Заполнение GroupId
update ps
	set GroupId = prod.GetProductGroupName(p.Name)
	from
	prod.PopularProducts ps
	left join prod.Products p on ps.ProductId = p.ProductId	
;
	
--Удаление лишних дубликатов по GroupId
with calculatedProductsWishList as
(
select *,
ROW_NUMBER() over (PARTITION BY groupid order by PopularRate desc) as row_number
from prod.PopularProducts
where PopularType = 0 and not GroupId is null
)
delete from calculatedProductsWishlist
where row_number > 1;

with calculatedProductsOrders as
(
select *,
ROW_NUMBER() over (PARTITION BY groupid order by PopularRate desc) as row_number
from prod.PopularProducts
where PopularType = 1 and not GroupId is null
)
delete from calculatedProductsOrders
where row_number > 1;

with calculatedProductsViewCount as
(
select *,
ROW_NUMBER() over (PARTITION BY groupid order by PopularRate desc) as row_number
from prod.PopularProducts
where PopularType = 2 and not GroupId is null
)
delete from calculatedProductsViewCount
where row_number > 1
	
	
END