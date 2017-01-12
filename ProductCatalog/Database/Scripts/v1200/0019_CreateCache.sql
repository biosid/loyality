/****** Object:  StoredProcedure [prod].[CreateCache]    Script Date: 02/25/2014 13:22:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [prod].[CreateCache]
	@contextKey nvarchar(max),
	@baseSql nvarchar(max),
	@actionSql nvarchar(max),
	@activePartnerIds nvarchar(max),
	@targetAudiencesIds nvarchar(max) = null,
	@productsCacheTable nvarchar(max) OUTPUT,
	@categoriesCacheTable nvarchar(max) OUTPUT
AS
BEGIN

declare @contextId int
set @productsCacheTable = 'prod.ProductsCache_'
set @categoriesCacheTable = 'prod.CategoriesCache_'
declare @contextKeyHash varbinary(16)
set @contextKeyHash = HASHBYTES('MD5', @contextKey)

select
	@contextKey = [ContextKey],
	@contextId = [ContextId],
	@contextKeyHash = ContextKeyHash
from
	[prod].Cache
where 
	ContextKeyHash = @contextKeyHash
  and
	DisableDate is null

-- Если кеш найден возвращаем его
if (@contextId is not null)
begin
	set @productsCacheTable = @productsCacheTable+CAST(@contextId as nvarchar(max))
	set @categoriesCacheTable = @categoriesCacheTable+CAST(@contextId as nvarchar(max))
	return
end

BEGIN TRANSACTION

EXECUTE sp_getapplock @Resource = @contextKeyHash, @LockMode    = 'Exclusive'

select
	@contextKey = [ContextKey],
	@contextId = [ContextId],
	@contextKeyHash = ContextKeyHash
from
	[prod].Cache
where 
	ContextKeyHash = @contextKeyHash
  and
	DisableDate is null

-- Если кеш найден после получения блокировки, то снимаем блокировку и возвращаем кэш
if (@contextId is not null)
begin
	set @productsCacheTable = @productsCacheTable+CAST(@contextId as nvarchar(max))
	set @categoriesCacheTable = @categoriesCacheTable+CAST(@contextId as nvarchar(max))
	
	EXECUTE sp_releaseapplock @Resource = @contextKeyHash
	COMMIT TRANSACTION
	
	return
end       

-- Создаём новую таблицу с кешем товаров
insert into [prod].Cache ([ContextKey], ContextKeyHash) VALUES (@contextKey, @contextKeyHash)
set @contextId = SCOPE_IDENTITY()

declare @createSql nvarchar(max) 
set @createSql = 
'CREATE TABLE ' + @productsCacheTable + CAST(@contextId as nvarchar(max)) + '
(
	[ProductId] [nvarchar](256) NOT NULL,
	[PartnerId] [int] NOT NULL,
	[PriceBase] [money] NULL,
	[PriceAction] [money] NULL,
) ON [PRIMARY]'

EXECUTE sp_executesql @createSql

-- WAITFOR DELAY '00:00:10'; -- Для тестирования паралельного вызова процедуры с одинаковыми параметрами

---- Заполняем кеш цен

-- Условие по деактивированным категориям
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#deactivateCatIds')) 
		DROP TABLE #deactivateCatIds

create table #deactivateCatIds
(
	Id int primary key
)

begin
	;with Categories
	(
		Id
		,ParentId
	) 
	as
	(
		select 	Id, ParentId
		from prod.ProductCategories nolock
		where Status=0
	
		union all

		select	pc.Id,
				pc.ParentId
		from prod.ProductCategories pc (nolock)
			inner join Categories c ON (pc.ParentId = c.Id)
	)
	insert into #deactivateCatIds
	select distinct Id
	from Categories
end

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

-- Поисковый запрос
declare @searchSql nvarchar(MAX)

if @activePartnerIds is not null and @activePartnerIds<>''
begin
        set @searchSql = '
INSERT INTO ' + CAST(@productsCacheTable as nvarchar(max)) + CAST(@contextId as nvarchar(max)) + 
' 
SELECT
	p.ProductId	
	,p.PartnerId
	,'+@baseSql+' as PriceBase
	,'+@actionSql+' as PriceAction
FROM
	[prod].[Products] p with (NOLOCK)
	left join 
	[prod].[ProductCategories] pc with (NOLOCK) on (p.CategoryId = pc.Id) 
where 			 
	p.Status = 1 
	and 
	p.ModerationStatus = 2
	and 
	(p.CategoryId not in (select * from #deactivateCatIds))
	and
	p.PartnerId in (' + @activePartnerIds+ ')
	
	'+@whereTargetAudience

        EXECUTE sp_executesql @searchSql
end

set @productsCacheTable = @productsCacheTable+CAST(@contextId as nvarchar(max))

-- Создаём кеш для категорий
-- Получаем количество товаров для каждой категории
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#countByCat')) 
	DROP TABLE #countByCat

create table #countByCat (
	CategoryId int,
	NamePath nvarchar(max),
	ProductsCount int
)

set @searchSql = 
N'
insert into #countByCat
select 
	c.Id as CategoryId
	,MAX(c.NamePath)
	,count(1) as productsCount
from 
	[prod].[ProductCategories] c
	inner join [prod].[Products] p on c.Id = p.[CategoryId]
	inner join '+@productsCacheTable+' price on (price.[ProductId] = p.[ProductId])
	inner join [prod].[ProductCategoriesPermissions] pcp on pcp.CategoryId = p.CategoryId and pcp.PartnerId = p.PartnerId
group by c.Id
'

EXECUTE sp_executesql @searchSql

--select * from #countByCat 

-- Создание таблицы кеша категорий
set @createSql = 
'CREATE TABLE ' + @categoriesCacheTable + CAST(@contextId as nvarchar(max)) + '
(
	CategoryId int NOT NULL,
	SubCategoriesCount int NOT NULL,
	ProductsCount int NOT NULL,	
) ON [PRIMARY]'

EXECUTE sp_executesql @createSql

set @searchSql = '
INSERT INTO ' + CAST(@categoriesCacheTable as nvarchar(max)) + CAST(@contextId as nvarchar(max)) + 
' 
select 
	c.Id
	,isnull((	select count(1) 
				from prod.ProductCategories c2 
				where c2.[ParentId]=c.Id), 0) as SubCategoriesCount
	,isnull((	select sum(productsCount) 
				from #countByCat 
				where #countByCat.NamePath like c.NamePath + ''%'' ), 0) as ProductsCount
from
	[prod].[ProductCategories] c 
'

--print @searchSql

EXECUTE sp_executesql @searchSql 


set @categoriesCacheTable = @categoriesCacheTable+CAST(@contextId as nvarchar(max))

EXECUTE sp_releaseapplock @Resource = @contextKeyHash

COMMIT TRANSACTION

END
