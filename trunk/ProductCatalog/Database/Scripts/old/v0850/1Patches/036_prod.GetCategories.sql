/****** Object:  StoredProcedure [prod].[GetCategories]    Script Date: 27.09.2013 15:47:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [prod].[GetCategories]
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

--EXEC [prod].[GetCategories] @includeParent=1, @totalCount = 1000, @childrenCount = 1000,@countToTake=1000, @parentId = 660, @nestingLevel = 4

set @countToTake = isnull(@countToTake, 20)
set @countToSkip = isnull(@countToSkip, 0)

-- Доступные партнёры
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

-- Определяем NamePath родительской категории
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
select 
	*, 
	[prod].[GetCategoryNestingLevel](id, null) as NestingLevel
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


-- Таблица для учёта ЦА
DECLARE @whereTargetAudience nvarchar(max) = ''
IF (@targetAudiencesIds IS NOT NULL)
BEGIN
	IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#productTargetAudiences')) 
		DROP TABLE #productTargetAudiences
		
	CREATE TABLE #productTargetAudiences
	(
		TargetAudienceId [nvarchar](256) PRIMARY KEY
	)
		
	INSERT INTO #productTargetAudiences
	SELECT value FROM dbo.ParamParserString(@targetAudiencesIds,',')
		
	SET @whereTargetAudience = ' OR EXISTS (SELECT 1 FROM [prod].[ProductTargetAudiences] pta 
				WHERE pta.ProductId = p.ProductId	
					AND pta.TargetAudienceId IN(SELECT TargetAudienceId FROM #productTargetAudiences)) '
END


SET @whereTargetAudience = ' AND (NOT EXISTS(SELECT 1 FROM [prod].[ProductTargetAudiences] pta WHERE pta.ProductId = p.ProductId) ' 
	+ @whereTargetAudience
	+ ') '


-- Получение таблицы с ценами на товары
declare @categoriesCacheTable nvarchar (max)
declare @productsCacheTable nvarchar (max)

exec prod.CreateCache
	@contextKey,
	@locationCode,
	@baseSql,
	@actionSql,
	@deliveryActionSql,
	@deliveryActionQuantitySql,
	null,
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

--print @searchSql

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