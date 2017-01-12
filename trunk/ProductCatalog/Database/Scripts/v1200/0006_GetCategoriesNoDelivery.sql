ALTER PROCEDURE [prod].[GetCategories]
	@catIds nvarchar(MAX) = null,
	@status int = null,
	@parentId int = null,
	@nestingLevel int = null,
	@countToTake int = null,
	@countToSkip int = null,
	@calcTotalCount bit = null,
	@includeParent bit = 0,
	@locationCode nvarchar(32),
	@type int = null,
	@targetAudiencesIds nvarchar(max) = null,
	@totalCount int out,
	@childrenCount int out,
	@baseSql nvarchar(MAX),
	@actionSql nvarchar(MAX),
	@deliveryActionSql nvarchar(MAX),
	@deliveryActionQuantitySql nvarchar(MAX),
	@contextKey nvarchar(MAX)
AS
BEGIN

set @countToTake = isnull(@countToTake, 20)
set @countToSkip = isnull(@countToSkip, 0)

IF EXISTS (	SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#categories')) 
	DROP TABLE #categories

-- Категории
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
	select 
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
    where 		
	  ((@parentId is null and parentId is null) or (Id = @parentId))
	  and (@status is null or status = @status)
	  and (@type is null or [Type] = @type)	  
	  	
	union all

	select 
			pc.Id,
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
		inner join Categories c ON pc.ParentId = c.Id
	where 
		(@status is null or pc.Status = @status)
		and (@type is null or pc.Type = @type)
)
insert into #categories 
(
	Id
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
select
	*, 
	[prod].[GetCategoryNestingLevel](id, null) as NestingLevel
from Categories
where 
	((@includeParent is null or @includeParent = 1) or (@includeParent = 0 and (Id <> @parentId or @parentId is null)))
	and
	(@catIds is null or ( exists(select value from dbo.ParamParserString(@catIds,',') where value= Id) ) )
order by NamePath

--return ;

if (@calcTotalCount = 1)
begin
	select @totalCount = count(1)
	from #categories c
	where  (@nestingLevel is null or c.NestingLevel <= @nestingLevel)
end

select @childrenCount = count(1)
from #categories c
where (@parentId is null and c.ParentId is null) or c.ParentId = @parentId


-- Получение кеша товаров и категорий
declare @categoriesCacheTable nvarchar (max)
declare @productsCacheTable nvarchar (max)

exec prod.CreateCache
	@contextKey,
	@locationCode,
	@baseSql,
	@actionSql,
	@targetAudiencesIds,
	@productsCacheTable OUTPUT,
	@categoriesCacheTable OUTPUT

declare @catOrderIncrement int
select @catOrderIncrement = min(catorder) from #categories

if (@catOrderIncrement > 0) 
	set @catOrderIncrement = 0



declare @searchSql nvarchar(max) 

set @searchSql =
'select 
	top (@countToTake) *
from 
(
	select 
		*, 
		row_number() over (order by NameRoot, CatOrder)  as RowNumber 
	from
	(
		select	
			c.*
			,catCache.ProductsCount as ProductsCount
			,catCache.SubCategoriesCount as SubCategoriesCount
			,[dbo].[GetProdCatOrderName] (Id, @catOrderIncrement) as NameRoot
		from #categories c inner join '+@categoriesCacheTable+' as catCache
		on c.Id = catCache.CategoryId
		where @nestingLevel is null or c.RootNestingLevel <= @nestingLevel
	) as t
) as tt
where tt.RowNumber > @countToSkip
order by tt.NameRoot'

print @searchSql

EXECUTE sp_executesql @searchSql 
	,N'@countToTake int, 
	@countToSkip int,
	@nestingLevel int,
	@catOrderIncrement int', 
	@countToTake = @countToTake,
	@countToSkip = @countToSkip,
	@nestingLevel = @nestingLevel,
	@catOrderIncrement = @catOrderIncrement


end
