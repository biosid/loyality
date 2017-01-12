ALTER PROCEDURE [prod].[CreateCache]
    @contextKey nvarchar(max),
    @baseSql nvarchar(max),
    @actionSql nvarchar(max),
    @activePartnerIds nvarchar(max),
    @targetAudiencesIds nvarchar(max) = NULL,
    @productsCacheTable nvarchar(max) OUTPUT,
    @categoriesCacheTable nvarchar(max) OUTPUT
AS
BEGIN

declare @contextId int
set @productsCacheTable = 'prod.ProductsCache_'
set @categoriesCacheTable = 'prod.CategoriesCache_'
declare @contextKeyHash varbinary(16)
set @contextKeyHash = hashbytes('MD5', @contextKey)

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

begin transaction

execute sp_getapplock @Resource = @contextKeyHash, @LockMode = 'Exclusive'

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

    execute sp_releaseapplock @Resource = @contextKeyHash
    commit transaction

    return
end

-- Создаём новую таблицу с кешем товаров
insert into [prod].Cache ([ContextKey], ContextKeyHash) values (@contextKey, @contextKeyHash)
set @contextId = scope_identity()

declare @createSql nvarchar(max)
set @createSql =
'create table ' + @productsCacheTable + cast(@contextId as nvarchar(max)) + '
(
    [ProductId] [nvarchar](256) not null,
    [PartnerId] [int] not null,
    [PriceBase] [money] null,
    [PriceAction] [money] null
) on [primary]'

declare @createIndexSql nvarchar(max)
set @createIndexSql =
'create nonclustered index ProductsCache_' + cast(@contextId as nvarchar(max)) + '_PID
on ' + @productsCacheTable + CAST(@contextId as nvarchar(max)) + ' ([ProductId])
include ([PriceBase],[PriceAction])'

execute sp_executesql @createSql
execute sp_executesql @createIndexSql

-- WAITFOR DELAY '00:00:10'; -- Для тестирования паралельного вызова процедуры с одинаковыми параметрами

---- Заполняем кеш цен

-- Условие по деактивированным категориям
if exists (select * from tempdb..sysobjects where id = object_id('tempdb..#deactivateCatIds'))
    drop table #deactivateCatIds

create table #deactivateCatIds
(
    Id int primary key
)

begin
    ;with Categories
    (
        Id,
        ParentId
    )
    as
    (
        select Id, ParentId
        from prod.ProductCategories with (nolock)
        where Status=0

        union all

        select pc.Id, pc.ParentId
        from prod.ProductCategories pc with (nolock)
            inner join Categories c on pc.ParentId = c.Id
    )
    insert into #deactivateCatIds
    select distinct Id
    from Categories
end

-- Таблица для учёта ЦА
declare @whereTargetAudience nvarchar(max) = ''
if (@targetAudiencesIds is not null)
begin
    if exists (select * from tempdb..sysobjects where id=object_id('tempdb..#productTargetAudiences'))
        drop table #productTargetAudiences

    create table #productTargetAudiences
    (
        TargetAudienceId [nvarchar](256) primary key
    )

    insert into #productTargetAudiences
    select value from dbo.ParamParserString(@targetAudiencesIds,',')

    set @whereTargetAudience = '
    or
    exists
    (
        select 1 from [prod].[ProductTargetAudiences] pta
        where pta.ProductId = p.ProductId
        and
        pta.TargetAudienceId in (select TargetAudienceId from #productTargetAudiences)
    )'
end

set @whereTargetAudience = '
    and
    not exists
    (
        select 1 from [prod].[ProductTargetAudiences] pta
        where pta.ProductId = p.ProductId
    )' + @whereTargetAudience

-- Поисковый запрос
declare @searchSql nvarchar(max)

if @activePartnerIds is not null and @activePartnerIds<>''
begin
    set @searchSql = '
insert into ' + cast(@productsCacheTable as nvarchar(max)) + cast(@contextId as nvarchar(max)) + '
select
    p.ProductId, p.PartnerId, ' + @baseSql + ' as PriceBase, ' + @actionSql + ' as PriceAction
from
    [prod].[ProductsFromAllPartners] p with (nolock)
    left join
    [prod].[ProductCategories] pc with (nolock) on (p.CategoryId = pc.Id)
where
    p.Status = 1
    and
    p.ModerationStatus = 2
    and
    (p.CategoryId not in (select * from #deactivateCatIds))
    and
    p.PartnerId in (' + @activePartnerIds+ ')' + @whereTargetAudience

    execute sp_executesql @searchSql
end

set @productsCacheTable = @productsCacheTable + cast(@contextId as nvarchar(max))

-- Создаём кеш для категорий
-- Получаем количество товаров для каждой категории
if exists (select * from tempdb..sysobjects where id=object_id('tempdb..#countByCat'))
    drop table #countByCat

create table #countByCat
(
    CategoryId int,
    NamePath nvarchar(max),
    ProductsCount int
)

set @searchSql = '
insert into #countByCat
select
    c.Id as CategoryId,
    max(c.NamePath),
    count(1) as productsCount
from
    [prod].[ProductCategories] c
    inner join
    [prod].[ProductsFromAllPartners] p on c.Id = p.[CategoryId]
    inner join
    ' + @productsCacheTable + ' price on price.[ProductId] = p.[ProductId]
    inner join
    [prod].[ProductCategoriesPermissions] pcp on pcp.CategoryId = p.CategoryId and pcp.PartnerId = p.PartnerId
group by c.Id
'
 
execute sp_executesql @searchSql

-- Создание таблицы кеша категорий
set @createSql =
'create table ' + @categoriesCacheTable + cast(@contextId as nvarchar(max))+ '
(
    CategoryId int not null,
    SubCategoriesCount int not null,
    ProductsCount int not null,
) on [primary]'

execute sp_executesql @createSql

set @searchSql = '
insert into ' + cast(@categoriesCacheTable as nvarchar(max)) + cast(@contextId as nvarchar(max)) + '
select
    c.Id,
    isnull((select count(1) from prod.ProductCategories c2 where c2.[ParentId] = c.Id), 0) as SubCategoriesCount,
    isnull((select sum(productsCount) from #countByCat where #countByCat.NamePath like c.NamePath + ''%'' ), 0) as ProductsCount
from
    [prod].[ProductCategories] c
'

execute sp_executesql @searchSql

set @categoriesCacheTable = @categoriesCacheTable + cast(@contextId as nvarchar(max))

execute sp_releaseapplock @Resource = @contextKeyHash

commit transaction

END
