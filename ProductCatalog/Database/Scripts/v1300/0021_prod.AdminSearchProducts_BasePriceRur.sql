/****** Object:  StoredProcedure [prod].[AdminSearchProducts]    Script Date: 06/10/2014 19:00:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [prod].[AdminSearchProducts]
    @sort int = 0,
    @countToSkip bigint = null,
    @countToTake int = null,
    @status int = null,
    @moderationStatus int = null,
    @isRecommended bit = null,
    @prodCategoryIds nvarchar(MAX) = null,
    @includeSubCategory bit = 1,
    @searchTerm nvarchar(MAX) = null,
    @partnerIds nvarchar(MAX) = null,
    @productIds nvarchar(MAX) = null,
    @calcTotalCount bit = 0,
    @totalCount bigint out
AS
BEGIN

set NOCOUNT ON;

--таблица для иденттификаторов всех новых категорий и их родителей
if exists (select * from tempdb..sysobjects where id=OBJECT_ID('tempdb..#newCatIds'))
    drop table #newCatIds

if exists (select * from tempdb..sysobjects where id=OBJECT_ID('tempdb..#partners'))
    drop table #partners

if exists (select * from tempdb..sysobjects where id=OBJECT_ID('tempdb..#productsIds'))
    drop table #productsIds

set @countToSkip = ISNULL(@countToSkip, 0)
set @countToTake = ISNULL(@countToTake, 100)
set @searchTerm = RTRIM(LTRIM(@searchTerm))

create table #newCatIds
(
    Id int primary key
)

-- если поиск с подкатегориями добавляем все дочерние категории
if (@includeSubCategory = 1)
begin
    ;with Categories (Id,ParentId)
    as
    (
        select Id, ParentId
        from prod.ProductCategories nolock
        where
            (len(isnull(@prodCategoryIds,'')) = 0 or
            id in (select value from dbo.ParamParserString(@prodCategoryIds,',')))
        union all
        select	pc.Id, pc.ParentId
        from
            prod.ProductCategories pc (nolock)
            inner join Categories c on (pc.ParentId = c.Id)
    )
    insert into #newCatIds
    select distinct Id
    from Categories
end
-- если поиск без подкатегорий добавляем только переданные категории
else
begin
    insert into #newCatIds
    select value from dbo.ParamParserString(@prodCategoryIds,',')
end

create table #partners
(
    Id int
)

if ( @partnerIds is not null and len(@partnerIds) != 0 )
begin
    insert into #partners
    select P.Id
    from prod.Partners P
    where P.Id in (
        select value
        from dbo.ParamParserString(@partnerIds,',')
    )
end
else
begin
    insert into #partners
    select Id
    from prod.Partners P with (NOLOCK)
end

create table #productsIds
(
    ProductsId nvarchar(256)
)

if (@productIds is not null and len(@productIds) != 0)
begin
    insert into #productsIds
    select value from dbo.ParamParserString(@productIds,',')
end

if @calcTotalCount = 1
begin
    select @totalCount = count(1)
    from
        [prod].[Products] p
        join #newCatIds cat on p.CategoryId = cat.Id
        join #partners part on (p.PartnerId = part.Id)
    where
        (@status is null or @status = p.Status)
        and
        (@moderationStatus is null or @moderationStatus = p.ModerationStatus)
        and
        (@isRecommended is null or @isRecommended = p.IsRecommended)
        and
        (@searchTerm is null or LOWER(p.Name + p.ProductId + isnull(p.Description, '')) like '%' + LOWER(@searchTerm) + '%')
        and
        (@productIds is null or ( exists(select 1 from #productsIds where ProductsId = p.ProductId) ) )
end

;with calculated as
(
    select
        p.ProductId
        ,p.PartnerProductId
        ,p.Name
        ,p.InsertedDate
        ,p.PriceRUR as PriceBase
        ,p.PriceRUR as PriceAction
        ,p.PriceRUR as PriceTotal
        ,null as PriceDelivery
        ,null as PriceDeliveryQuantity
        ,null as PriceDeliveryQuantityRur
        , p.PriceRUR
        , null as PriceDeliveryRur
        , null as ExternalLocationId
        , null as PopularRate
        , null as PopularType
        , [prod].[GetStringFromProductAudienceRows](p.ProductId) as TargetAudienceIds
        , null as CarrierId
    from
        [prod].[Products] p
        join #newCatIds cat on p.CategoryId = cat.Id
        join #partners part on p.PartnerId = part.Id
    where
        (@status is null or @status = p.Status)
        and
        (@moderationStatus is null or @moderationStatus = p.ModerationStatus)
        and
        (@isRecommended is null or @isRecommended = p.IsRecommended)
        and
        (@searchTerm is null or LOWER(p.Name + p.ProductId + isnull(p.Description, '')) like '%' + LOWER(@searchTerm) + '%')
        and
        (@productIds is null or ( exists(select 1 from #productsIds where ProductsId = p.ProductId) ) )
)
select
    p.*
    ,t.PriceBase
    ,t.PriceAction
    ,PriceTotal
    ,PriceDelivery
    ,PriceDeliveryQuantity
    ,PriceDeliveryQuantityRur
    ,PriceDeliveryRur
    ,ExternalLocationId
    ,pc.Name as CategoryName
    ,pc.NamePath as CategoryNamePath
    ,pc.Status as CategoryStatus
    ,0 as DeliveryAvailable
    ,case when (not p.[BasePriceRur] is null and p.[BasePriceRUR] > p.[PriceRur]) then cast(1 as bit) else CAST(0 as bit) end as IsActionPrice
    ,pfp.FixedPriceDate as BasePriceRurDate
    ,TargetAudienceIds
    ,CarrierId
    ,t.PopularRate
    ,t.PopularType
from
    (
        select
            *,
            row_number() over (
                order by
                case when @sort = 0 then Name end,
                case when @sort = 1 then Name end desc,
                case when @sort = 2 then PriceTotal end,
                case when @sort = 2 then Name end,
                case when @sort = 3 then PriceTotal end desc,
                case when @sort = 3 then Name end,
                case when @sort = 4 then InsertedDate end desc,
                case when @sort = 4 then Name end,
                case when @sort = 5 then PartnerProductId end,
                case when @sort = 6 then newid() end desc,
                case when @sort = 7 then newid() end
            ) as row_number
        from calculated
    ) as t
    join [prod].[Products] p on (p.ProductId = t.ProductId)
    left join [prod].[ProductCategories] pc on (p.CategoryId = pc.Id)
    left join [prod].[ProductsFixedPrices] pfp on (p.ProductId = pfp.ProductId)
where row_number between (@countToSkip + 1) and (@countToSkip + @countToTake)
order by row_number

END
