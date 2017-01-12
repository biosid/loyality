/****** Object:  StoredProcedure [prod].[AdminSearchProducts]    Script Date: 03/19/2015 22:59:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[AdminSearchProducts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [prod].[AdminSearchProducts]
GO

CREATE PROCEDURE [prod].[AdminSearchProducts]
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
        from prod.ProductCategories (nolock)
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
    Id int primary key
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
    ProductsId nvarchar(256) primary key
)

if (@productIds is not null and len(@productIds) != 0)
begin
    insert into #productsIds
    select value from dbo.ParamParserString(@productIds,',')
end

declare @searchSql nvarchar(max);

declare @statusCriteria nvarchar(100) = N'';
if (@status is not null)
SET @statusCriteria = N'@status = p.Status';

declare @moderationStatusCriteria nvarchar(100) = '';
if (@moderationStatus is not null)
SET @moderationStatusCriteria = (case when @statusCriteria <> '' then ' AND ' else '' end) + '@moderationStatus = p.ModerationStatus';

declare @isRecommendedCriteria nvarchar(100) = '';
if (@isRecommended is not null)
SET @isRecommendedCriteria = (case when @statusCriteria <> '' or @moderationStatusCriteria <> '' then ' AND ' else '' end) + '@isRecommended = p.IsRecommended'

declare @searchTermCriteria nvarchar(100) = '';
if (@searchTerm is not null)
SET @searchTermCriteria = (case when @statusCriteria <> '' or @moderationStatusCriteria <> '' or @isRecommendedCriteria <> '' then ' AND ' else '' end) + 'LOWER(p.Name + p.ProductId + isnull(p.Description, '''')) like ''%'' + LOWER(@searchTerm) + ''%'''

declare @productIdsCriteria nvarchar(100) = '';
if (@productIds is not null)
SET @productIdsCriteria = (case when @statusCriteria <> '' or @moderationStatusCriteria <> '' or @isRecommendedCriteria <> '' or @searchTermCriteria <> '' then ' AND ' else '' end) 
+  '( exists(select 1 from #productsIds where ProductsId = p.ProductId) )'

declare @whereCriteria nvarchar(100) = (case when @statusCriteria <> '' or @moderationStatusCriteria <> '' or @isRecommendedCriteria <> '' or @searchTermCriteria <> '' or @productIdsCriteria <> '' then ' where ' else '' end);

if @calcTotalCount = 1
begin
	SET @searchSql = '
    select @totalCount = count(p.ProductId)
    from
        [prod].[ProductsFromAllPartners] p (nolock)
        join #newCatIds cat on p.CategoryId = cat.Id
        join #partners part on (p.PartnerId = part.Id)
    ' + @whereCriteria + '
		' + @statusCriteria + '
        ' + @moderationStatusCriteria + '
        ' + @isRecommendedCriteria + '
        ' + @searchTermCriteria + '
        '+ @productIdsCriteria;
	
	EXECUTE sp_executesql @searchSql 
	,N'@status int,
	@moderationStatus int,
	@isRecommended bit,
	@searchTerm nvarchar(MAX),
	@productIds nvarchar(MAX),
	@totalCount bigint out',
	@status = @status,
	@moderationStatus = @moderationStatus,
	@isRecommended = @isRecommended,
	@searchTerm = @searchTerm,
	@productIds = @productIds,
	@totalCount = @totalCount out;
end;

declare @sortOrder nvarchar(MAX) = '';

SET @sortOrder = (case 
when @sort is null then 'Name'
when @sort = 0 then 'Name'
when @sort = 1 then 'Name desc'
when @sort = 2 then 'PriceTotal, Name'
when @sort = 3 then 'PriceTotal desc, Name'
when @sort = 4 then 'InsertedDate desc, Name'
when @sort = 5 then 'PartnerProductId'
when @sort = 6 then 'newid() desc'
when @sort = 7 then 'newid()' else '' end);

SET @searchSql = '
;with calculated as
(
    select
        p.ProductId
        , p.Name
        , p.PriceRUR as PriceTotal
        , p.InsertedDate
        , p.PartnerProductId
    from
        [prod].[ProductsFromAllPartners] p (nolock)
        join #newCatIds cat on p.CategoryId = cat.Id
        join #partners part on p.PartnerId = part.Id
    ' + @whereCriteria + '
        ' + @statusCriteria + '
        ' + @moderationStatusCriteria + '
        ' + @isRecommendedCriteria + '
        ' + @searchTermCriteria + '
        '+ @productIdsCriteria + '
)
select
	p.[ProductId]
	,p.[PartnerId]
	,p.[InsertedDate]
	,p.[UpdatedDate]
	,p.[PartnerProductId]
	,p.[Type]
	,p.[Bid]
	,p.[CBid]
	,p.[Available]
	,p.[Name]
	,p.[Url]
	,p.[PriceRUR]
	,p.[CurrencyId]
	,p.[CategoryId]
	,p.[Pictures]
	,p.[TypePrefix]
	,p.[Vendor]
	,p.[Model]
	,p.[Store]
	,p.[Pickup]
	,p.[Delivery]
	,p.[Description]
	,p.[VendorCode]
	,p.[LocalDeliveryCost]
	,p.[SalesNotes]
	,p.[ManufacturerWarranty]
	,p.[CountryOfOrigin]
	,p.[Downloadable]
	,p.[Adult]
	,p.[Barcode]
	,p.[Param]
	,p.[Author]
	,p.[Publisher]
	,p.[Series]
	,p.[Year]
	,p.[ISBN]
	,p.[Volume]
	,p.[Part]
	,p.[Language]
	,p.[Binding]
	,p.[PageExtent]
	,p.[TableOfContents]
	,p.[PerformedBy]
	,p.[PerformanceType]
	,p.[Format]
	,p.[Storage]
	,p.[RecordingLength]
	,p.[Artist]
	,p.[Media]
	,p.[Starring]
	,p.[Director]
	,p.[OriginalName]
	,p.[Country]
	,p.[WorldRegion]
	,p.[Region]
	,p.[Days]
	,p.[DataTour]
	,p.[HotelStars]
	,p.[Room]
	,p.[Meal]
	,p.[Included]
	,p.[Transport]
	,p.[Place]
	,p.[HallPlan]
	,p.[Date]
	,p.[IsPremiere]
	,p.[IsKids]
	,p.[Status]
	,p.[ModerationStatus]
	,p.[Weight]
	,p.[UpdatedUserId]
	,p.[IsRecommended]
	,p.[BasePriceRUR]
	,p.[IsDeliveredByEmail]
	, p.PriceRUR as PriceBase
    , p.PriceRUR as PriceAction
    , p.PriceRUR as PriceTotal
    , null as PriceDelivery
    , null as PriceDeliveryQuantity
    , null as PriceDeliveryQuantityRur
    , null as PriceDeliveryRur
    , null as ExternalLocationId
    , null as PopularRate
    , null as PopularType
    , [prod].[GetStringFromProductAudienceRows](p.ProductId) as TargetAudienceIds
	, null as CarrierId
	,pc.Name as CategoryName
	,pc.NamePath as CategoryNamePath
	,pc.Status as CategoryStatus
	,0 as DeliveryAvailable
	,case when (not p.[BasePriceRur] is null and p.[BasePriceRUR] > p.[PriceRur]) then cast(1 as bit) else CAST(0 as bit) end as IsActionPrice
	,pfp.FixedPriceDate as BasePriceRurDate
from
    (
        select
            *,
            row_number() over (
                order by
                ' + @sortOrder + '
            ) as row_number
        from calculated
    ) as t
    join [prod].[ProductsFromAllPartners] p on (p.ProductId = t.ProductId)
    left join [prod].[ProductCategories] pc on (p.CategoryId = pc.Id)
    left join [prod].[ProductsFixedPrices] pfp on (p.ProductId = pfp.ProductId)
where row_number between (@countToSkip + 1) and (@countToSkip + @countToTake)
order by row_number';

EXECUTE sp_executesql @searchSql 
	,N'@status int,
	@moderationStatus int,
	@isRecommended bit,
	@searchTerm nvarchar(MAX),
	@productIds nvarchar(MAX),
	@sort int,
	@countToSkip bigint,
    @countToTake int',
	@status = @status,
	@moderationStatus = @moderationStatus,
	@isRecommended = @isRecommended,
	@searchTerm = @searchTerm,
	@productIds = @productIds,
	@sort = @sort,
	@countToSkip = @countToSkip,
	@countToTake = @countToTake;

END
