IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[FillPopularProducts]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[FillPopularProducts]
GO

CREATE PROCEDURE [prod].[FillPopularProducts]

AS
BEGIN


-- Параметры 
declare @maxWished int
declare @maxOrdered int
declare @mostViewed int

set @maxWished = 100
set @maxOrdered = 100
set @mostViewed = 100

-- Заполнение наиболее откладываемых
delete from [prod].[PopularProducts] where PopularType = 0

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
	SELECT top (@maxWished) ProductId, 0, count(*)
	FROM [prod].[WishListItems]
	group by [ProductId]
	order by count(*) desc

---- Заполнение наиболее заказываемых 
delete from [prod].[PopularProducts] where PopularType = 1

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
	select top (@maxOrdered) ProductId, 1, count(ProductId)
	from
	(
		select ProductId, InsertedUserId
		from
		(
			select Items.value('/ArrayOfOrderItem[1]/OrderItem[1]/Product[1]/ProductId[1]', 'nvarchar(256)') as ProductId, InsertedUserId
			from [prod].[Orders]
		) as t
		where ProductId is not null
		group by ProductId, InsertedUserId
	) as t2
	group by ProductId
	order by count(ProductId) desc

-- Заполнение наиболее просматриваемых
delete from [prod].[PopularProducts] where PopularType = 2

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
	SELECT top (@mostViewed) ProductId, 2, count(*)
	FROM [prod].[ProductViewStatistics]
	group by [ProductId]
	order by count(*) desc
END


GO