ALTER PROCEDURE [prod].[FillPopularProducts]
AS
BEGIN

-- «аполнение наиболее откладываемых
delete from [prod].[PopularProducts] where PopularType = 0

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
	SELECT ProductId, 0, count(*)
	FROM [prod].[WishListItems]
	group by [ProductId]
	order by count(*) desc

---- «аполнение наиболее заказываемых 
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
		) as t
		where ProductId is not null
		group by ProductId, ClientId
	) as t2
	group by ProductId
	order by count(ProductId) desc

-- «аполнение наиболее просматриваемых
delete from [prod].[PopularProducts] where PopularType = 2

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
	SELECT ProductId, 2, count(*)
	FROM [prod].[ProductViewStatistics]
	group by [ProductId]
	order by count(*) desc
END