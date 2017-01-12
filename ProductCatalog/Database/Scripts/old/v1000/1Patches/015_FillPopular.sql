/****** Object:  StoredProcedure [prod].[FillPopularProducts]    Script Date: 12/31/2013 12:58:29 ******/
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

-- «аполнение наиболее откладываемых
delete from [prod].[PopularProducts] where PopularType = 0

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
	SELECT ProductId, 0, count(*)
	FROM [prod].[WishListItems]
	where CreatedDate between @pastDate and @nowDate
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
			where InsertedDate between @pastDate and @nowDate
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
	where UpdatedDate between @pastDate and @nowDate
	group by [ProductId]
	order by count(*) desc
END
