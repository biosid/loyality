IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetCategories]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[GetCategories]
GO

CREATE PROCEDURE [prod].[GetCategories]
	@status int = null,
	@parentId int = null,
	@nestingLevel int = null,
	@countToTake int = null,
	@countToSkip int = null,
	@calcTotalCount bit = null,
	@includeParent bit = 0,
	@locationCode nvarchar(32) = null,
	@returnNotDeliveried bit = null,
	@type int = null,
	@countPublicProducts bit = 0,
	@totalCount int out,
	@childrenCount int out
	
AS
BEGIN

--EXEC [prod].[GetCategories] @includeParent=1, @totalCount = 1000, @childrenCount = 1000,@countToTake=1000, @parentId = 660, @countPublicProducts = 1, @nestingLevel = 4

set @countToTake = isnull(@countToTake, 20)
set @countToSkip = isnull(@countToSkip, 0)

IF EXISTS (
	SELECT * 
	FROM tempdb..sysobjects 
	WHERE id=OBJECT_ID('tempdb..#availablePartners')) 
		DROP TABLE #availablePartners

create table #availablePartners (
	PartnerId int
)

insert into #availablePartners
select PartnerId from [prod].[GetAvailablePartnersForRegion](@locationCode)

IF EXISTS (
	SELECT * 
	FROM tempdb..sysobjects 
	WHERE id=OBJECT_ID('tempdb..#categories')) 
		DROP TABLE #categories

create table #categories
(
	Id int primary key,
	[Type] int,
	ParentId int,
	Name nvarchar(256),
	NamePath nvarchar(max),
	[Status] int,
	NestingLevel int,
	RootNestingLevel int,
	InsertedUserId nvarchar(max),
	UpdatedUserId nvarchar(max),
	OnlineCategoryUrl nvarchar(max),
	NotifyOrderStatusUrl nvarchar(max),
	InsertedDate datetime,
	UpdatedDate datetime,
	[CatOrder] int,
	OnlineCategoryPartnerId int
)

declare @parentNamePath nvarchar(256)

if (@parentId is not null)
begin
	select @parentNamePath = NamePath
	from [prod].[ProductCategories] nolock
	where Id = @parentId
end 

declare @id int 

if (@includeParent = 1)
begin 
	set @id = @parentId
	set @parentId = null
end

;with Categories
(
	Id
	,Type
	,ParentId
	,Name
	,NamePath
	,[Status]
	,[RootNestingLevel]
	,InsertedUserId
	,UpdatedUserId
	,OnlineCategoryUrl
	,NotifyOrderStatusUrl
	,InsertedDate
	,UpdatedDate
	,[CatOrder]	
	,OnlineCategoryPartnerId
) 
as
(
	select distinct	
			Id,
			Type,
			ParentId,
			Name,
			NamePath,
			[Status],
			1 as RootNestingLevel,
			InsertedUserId,
			UpdatedUserId,
			OnlineCategoryUrl,
			NotifyOrderStatusUrl,
			InsertedDate,
			UpdatedDate,
			[CatOrder],
			OnlineCategoryPartnerId
    from prod.ProductCategories nolock
    where ((@Id is not null) and Id = @Id
	  or (@parentId is not null) and ParentId = @parentId
	  or (@Id is null and @parentId is null) and ParentId is null)
	  and (@status is null or status = @status)
	  and (@type is null or [Type] = @type)
	  	
	union all

	select	pc.Id,
			pc.Type,
			pc.ParentId,
			pc.Name,
			pc.NamePath,
			pc.[Status],
			c.RootNestingLevel + 1 as RootNestingLevel,
			pc.InsertedUserId,
			pc.UpdatedUserId,
			pc.OnlineCategoryUrl,
			pc.NotifyOrderStatusUrl,
			pc.InsertedDate,
			pc.UpdatedDate,
			pc.[CatOrder],
			pc.OnlineCategoryPartnerId
    from prod.ProductCategories pc (nolock)
		inner join Categories c ON (pc.ParentId = c.Id)
	where 
		(@status is null or pc.Status = @status)
		and (@type is null or pc.Type = @type)
)
insert into #categories 
	(Id
	,Type
	,ParentId
	,Name
	,NamePath
	,Status
	,RootNestingLevel
	,InsertedUserId
	,UpdatedUserId
	,OnlineCategoryUrl
	,NotifyOrderStatusUrl
	,InsertedDate
	,UpdatedDate
	,[CatOrder]
	,OnlineCategoryPartnerId
	,NestingLevel
	)
select *, [prod].[GetCategoryNestingLevel](id, null) as NestingLevel
from Categories
order by NamePath

if (@calcTotalCount = 1)
begin
	select @totalCount = count(1)
	from #categories c
	where  (@nestingLevel is null or c.NestingLevel <= @nestingLevel)
end

select @childrenCount = count(1)
from #categories c
where (@parentId is null and c.ParentId is null) or c.ParentId = @parentId

create table #countByCat (
	Id int,
	NamePath nvarchar(max),
	ProductsCount int
)

IF (@locationCode IS NULL) OR (@returnNotDeliveried IS NOT NULL AND @returnNotDeliveried = 1)
BEGIN
	-- NOTE: Не учитываем доставку.
	if (@countPublicProducts = 1)
	begin
		insert into #countByCat
		select c.Id, max(c.NamePath), count(1) as productsCount
		from #categories c
			join [prod].[Products] p on (p.CategoryId = c.Id)
			join [prod].[ProductCategoriesPermissions] pcp on pcp.CategoryId = p.CategoryId and pcp.PartnerId = p.PartnerId
		where 
		p.Status = 1 and p.ModerationStatus = 2
		group by c.Id
	end
	else
	begin
		insert into #countByCat
		select c.Id, max(c.NamePath), count(1) as productsCount
		from #categories c
			join [prod].[Products] p on (p.CategoryId = c.Id)
			join [prod].[ProductCategoriesPermissions] pcp on pcp.CategoryId = p.CategoryId and pcp.PartnerId = p.PartnerId
		group by c.Id	
	end
END
ELSE
BEGIN
	IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#deliveryPrice')) 
		DROP TABLE #deliveryPrice

	create table #deliveryPrice
	(
		PartnerId int,  
		WeightMin int,
		WeightMax int,
		Price decimal(38, 20),
		[Type] int,
		[Priority] int,
		CarrierId int,
		[ExternalLocationId] nvarchar(250)
	)

	insert into #deliveryPrice
	select * from [prod].[GetDeliveryRatesForLocation](@locationCode)

	if (@countPublicProducts = 1)
	begin
		insert into #countByCat
		select c.Id, max(c.NamePath), count(1) as productsCount
		from #categories c
			join [prod].[Products] p on (p.CategoryId = c.Id)
			join [prod].[ProductCategoriesPermissions] pcp on pcp.CategoryId = p.CategoryId and pcp.PartnerId = p.PartnerId
			CROSS APPLY (
				 SELECT dp.[Price]
				 FROM #deliveryPrice dp
				 WHERE dp.[Type] = 
					(SELECT min(mindp.[Type]) AS MinType 
					 FROM #deliveryPrice mindp 
					 WHERE mindp.PartnerId = p.PartnerId AND p.Weight BETWEEN mindp.WeightMin AND mindp.WeightMax)
				  AND dp.PartnerId = p.PartnerId AND p.Weight BETWEEN dp.WeightMin AND dp.WeightMax
				 ) delivery
		where p.Status = 1 and p.ModerationStatus = 2
		group by c.Id
	end
	else
	begin
		insert into #countByCat
		select c.Id, max(c.NamePath), count(1) as productsCount
		from #categories c
			join [prod].[Products] p on (p.CategoryId = c.Id)
			join [prod].[ProductCategoriesPermissions] pcp on pcp.CategoryId = p.CategoryId and pcp.PartnerId = p.PartnerId
			CROSS APPLY (
				 SELECT dp.[Price]
				 FROM #deliveryPrice dp
				 WHERE dp.[Type] = 
					(SELECT min(mindp.[Type]) AS MinType 
					 FROM #deliveryPrice mindp 
					 WHERE mindp.PartnerId = p.PartnerId AND p.Weight BETWEEN mindp.WeightMin AND mindp.WeightMax)
				  AND dp.PartnerId = p.PartnerId AND p.Weight BETWEEN dp.WeightMin AND dp.WeightMax
				 ) delivery
		group by c.Id
	end
END

declare @catOrderIncrement int
select @catOrderIncrement = min(catorder) from #categories

if (@catOrderIncrement > 0) 
	set @catOrderIncrement = 0

select top (@countToTake) * 
from (
select *, row_number() over (order by NameRoot, CatOrder)  as RowNumber 
from
(
	select	c.*,
			isnull((select sum(productsCount) from #countByCat where #countByCat.NamePath like c.NamePath + '%' ), 0) as ProductsCount,
			isnull((select count(1) from #categories c2 
					where substring(c2.NamePath, 0, len(c.NamePath) + 1) = c.NamePath and Id != c.Id and c2.NestingLevel = c.NestingLevel + 1), 0) as SubCategoriesCount
			, [dbo].[GetProdCatOrderName] (Id, @catOrderIncrement) as NameRoot
	from #categories c
	where (@nestingLevel is null or c.RootNestingLevel <= @nestingLevel)
) as t
) as tt
where tt.RowNumber > @countToSkip
order by tt.NameRoot

end

GO


