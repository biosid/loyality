

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[SearchProducts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [prod].[SearchProducts]
GO


CREATE PROCEDURE [prod].[SearchProducts]
	@sort int = null,
	@countToSkip bigint = null,
	@countToTake int = null,
	@prodCategoryIds nvarchar(MAX) = null,
	@includeSubCategory bit = null,
	@searchTerm nvarchar(500) = null,
	@partnerIds nvarchar(MAX) = null,
	@productIds nvarchar(MAX) = null,
	@vendors nvarchar(MAX) = null,
	@returnEmptyVendorProducts bit = null,
	@calcTotalCount bit = null,
	@minPrice money = null,
	@maxPrice money = null,
	@baseSql nvarchar(MAX),
	@actionSql nvarchar(MAX),
	@calcMaxPrice bit = null,
	@minInsertedDate datetime = null,
	@actionPrice bit = null,
	@partnerProductId nvarchar(max) = null,
	@productParamsXml xml = null,
	@activePartnerIds nvarchar(max),
	@targetAudiencesIds nvarchar(max) = null,
	@popularType int = null,
	@contextKey nvarchar(MAX),
    @minRecommendedPrice money = null,
    @maxRecommendedPrice money = null,
    @categoryIdToRecommendFor int = null,
	@totalCount bigint OUT,
	@calculatedMaxPrice money OUT
AS
BEGIN

SET NOCOUNT ON;

if (LEN(@contextKey) < 10)
begin
	declare @message nvarchar(max) = 'Wrong @contextKey lenght ' + @contextKey
	RAISERROR (@message, 16, 1)
	return
end

set @sort = ISNULL(@sort, 0)
set @countToSkip = ISNULL(@countToSkip, 0)
set @countToTake = ISNULL(@countToTake, 100)
set @includeSubCategory = ISNULL(@includeSubCategory, 1)
set @returnEmptyVendorProducts = ISNULL(@returnEmptyVendorProducts, 0)
set @calcTotalCount = ISNULL(@calcTotalCount, 0)
set @searchTerm = RTRIM(LTRIM(@searchTerm))

-- тип популярности по умолчанию
if ((@sort = 6 or @sort = 9 or @sort = 11) and @popularType is null)
   set @popularType = 3

-- если ищем рекомендации для категории, но категория не указана, то возвращаем случайные товары,
-- иначе создаем и заполняем временную таблицу со всеми подкатегориями указанной категории
if (@sort = 10 and @categoryIdToRecommendFor is null)
    set @sort = 7

-- Условие по переданным категориям
if exists (select * from tempdb..sysobjects where id=object_id('tempdb..#newCatIds'))
    drop table #newCatIds

create table #newCatIds(Id int primary key)

if exists (select * from tempdb..sysobjects where id=object_id('tempdb..#blackListCatIds'))
    drop table #blackListCatIds

create table #blackListCatIds(CategoryId int primary key)

if exists (select 1 from tempdb..sysobjects where id=object_id('tempdb..#recommendCatIds'))
    drop table #recommendCatIds

create table #recommendCatIds (Id int primary key)

if (@includeSubCategory = 1 and @prodCategoryIds is not null)
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
		where (len(isnull(@prodCategoryIds,'')) = 0 or id in (select value from dbo.ParamParserString(@prodCategoryIds,',')))		  
	
		union all

		select	pc.Id,
				pc.ParentId
		from prod.ProductCategories pc (nolock)
			inner join Categories c ON (pc.ParentId = c.Id)
	)
	insert into #newCatIds
	select distinct Id
	from Categories
end
else
begin
	insert into #newCatIds
	select value from dbo.ParamParserString(@prodCategoryIds,',')
end

if (@sort = 10)
begin
    ;with Categories(Id)
    as
    (
        select @categoryIdToRecommendFor
        union all
        select pc.Id
        from prod.ProductCategories pc (nolock)
        inner join Categories c on pc.ParentId = c.Id
    )
    insert into #recommendCatIds
    select distinct Id
    from Categories
end

-- Условие по вендорам
declare @allVendorsSearch bit
select @allVendorsSearch = 
case when COUNT(*) = 0 then 1
else 0
end 
from dbo.ParamParserString(@vendors,',')

-- Создаём кеш
declare @productsCacheTable nvarchar (max)
declare @categoriesCacheTable nvarchar (max)

exec prod.CreateCache
	@contextKey,
	@baseSql,
	@actionSql,
	@activePartnerIds,
	@targetAudiencesIds,
	@productsCacheTable OUTPUT,
	@categoriesCacheTable OUTPUT

-- Формируем from
declare @fromSql nvarchar(MAX)
set @fromSql = '
		FROM 
			prod.ProductsFromAllPartners p WITH(NOLOCK)
			inner join '+@productsCacheTable+' price WITH(NOLOCK) on p.[ProductId] = price.[ProductId] 
			inner join [prod].[ProductCategories] pc on (p.CategoryId = pc.Id)'	

if (exists(select TOP 1 * from #newCatIds))
	set @fromSql = @fromSql + '
			join #newCatIds cat on p.CategoryId = cat.Id '

set @fromSql = @fromSql + '
		join [prod].[ProductCategoriesPermissions] pcp on pcp.CategoryId = p.CategoryId and pcp.PartnerId = p.PartnerId '

declare @popularColumns nvarchar(MAX) = ', NULL as PopularRate
										, NULL as PopularType'
if (@popularType IS NOT NULL)
BEGIN
SET @popularColumns = ',pp.PopularRate
					,pp.PopularType';
set @fromSql = @fromSql + '
				left join [prod].[PopularProducts] pp WITH(NOLOCK) on pp.ProductId = p.ProductId and pp.PopularType = @popularType'
END;

if (@sort = 10)
set @fromSql = @fromSql + '
		left join #recommendCatIds rc on p.CategoryId = rc.Id
		'

-- Формируем where
declare @whereSql nvarchar(MAX) = ''

if (@partnerIds is not null)
set @whereSql = @whereSql + (case when @whereSql <> '' then ' AND ' else '' end) + ' (p.PartnerId in ('+@partnerIds+'))'

if (@searchTerm is not null AND @searchTerm <> '')
BEGIN
set @whereSql = @whereSql + (case when @whereSql <> '' then ' AND ' else '' end) + ' CONTAINS(p.AgrDescription, @searchTerm)'
set @searchTerm = '"' + @searchTerm + '"'
END

--' (LOWER(p.Name + p.ProductId + isnull(p.Description, '''')) like ''%'' + LOWER(@searchTerm) + ''%'')'

if (@partnerProductId is not null)		
set @whereSql = @whereSql + (case when @whereSql <> '' then ' AND ' else '' end) + ' (@partnerProductId = p.PartnerProductId)'

if (@productIds is not null)		
set @whereSql = @whereSql + (case when @whereSql <> '' then ' AND ' else '' end) + ' ( exists(select value from dbo.ParamParserString(@productIds,'','') where value= p.ProductId) )'
		
-- оптим.
if (@allVendorsSearch <> 1)		
set @whereSql = @whereSql + (case when @whereSql <> '' then ' AND ' else '' end) + ' (
			exists
			(select value from dbo.ParamParserString(@vendors,'','') 
			where value = p.Vendor or (@returnEmptyVendorProducts = 1 and p.Vendor is null)
			)
		)'

if (@minInsertedDate is not null)		
set @whereSql = @whereSql + (case when @whereSql <> '' then ' AND ' else '' end) + ' (@minInsertedDate <= p.InsertedDate)'

if (@actionPrice = 1)		
set @whereSql = @whereSql + (case when @whereSql <> '' then ' AND ' else '' end) + ' (price.[PriceBase] - price.[PriceAction] >= 1)' 

if (@sort = 9)
begin
        set @minRecommendedPrice = isnull(@minRecommendedPrice, 0)
        set @maxRecommendedPrice = isnull(@maxRecommendedPrice, 999999999)

	;with BlackListCategories
	(
		Id
		,ParentId
	) 
	as
	(
		select 	Id, ParentId
		from prod.ProductCategories nolock
		where id in (select CategoryId from prod.ProductCategoriesBlackList)		  
	
		union all

		select	pc.Id, pc.ParentId
		from prod.ProductCategories pc (nolock)
			inner join BlackListCategories c ON (pc.ParentId = c.Id)
	)
	insert into #blackListCatIds
	select distinct Id
	from BlackListCategories
	
set @whereSql = @whereSql + (case when @whereSql <> '' then ' AND ' else '' end) + 				
		' ( not exists(select CategoryId from #blackListCatIds where CategoryId = p.CategoryId) )'
end;

if (@whereSql <> '')
set @whereSql ='where ' + @whereSql

if (@productParamsXml is not null)
begin
	
	declare @paramNames nvarchar(max)
	declare @paramUnits nvarchar(max)
	declare @paramValues nvarchar(max)

	
	-- '/Event/Indicator/Name[text() = "GDP"]') = 1
	-- ([param].exist('ArrayOfProductParam/ProductParam/Name[. = ("Пол")]') = 1 and [param].exist('ArrayOfProductParam/ProductParam/Value[. = ("Для девочек")]') = 1) and ([param].exist('ArrayOfProductParam/ProductParam/Name[. = ("Возраст")]') = 1 and [param].exist('ArrayOfProductParam/ProductParam/Value[. = ("Детская")]') = 1) 
    -- [param].exist('ArrayOfProductParam/ProductParam/Name[text() = "Пол"]') = 1
	select @paramNames = 
		COALESCE(@paramNames + ') and ([param].exist(''ArrayOfProductParam/ProductParam/Name[. = ("', '([param].exist(''ArrayOfProductParam/ProductParam/Name[. = ("') + p.value('Name[1]','nvarchar(MAX)') + '")]'') = 1 and ' + 
		case 
		when p.value('Unit[1]','nvarchar(MAX)') is null then '' else
		COALESCE(@paramUnits + ' and ', '[param].exist(''ArrayOfProductParam/ProductParam/Unit[. = ("') + p.value('Unit[1]','nvarchar(MAX)') + '")]'') = 1 and ' 
		end +
		COALESCE(@paramValues + ' and ', '[param].exist(''ArrayOfProductParam/ProductParam/Value[. = ("') + p.value('Value[1]','nvarchar(MAX)') + '")]'') = 1'
	from @productParamsXml.nodes('/ArrayOfProductParam/ProductParam') as t(p) 
	
	--print @paramNames
	--print @paramUnits
	--print @paramValues

	set @whereSql = @whereSql + ' and ' + @paramNames + ') ';
end


declare @totalPriceSql nvarchar(MAX)
set @totalPriceSql = 'price.[PriceAction]'

declare @whereMinMaxPrice nvarchar(MAX);
set @whereMinMaxPrice = '';

if (@minPrice is not null)
set @whereMinMaxPrice = @whereMinMaxPrice + ' and (@minPrice <= ('+@totalPriceSql+')) '

if (@maxPrice is not null)
set @whereMinMaxPrice = @whereMinMaxPrice + ' and (('+@totalPriceSql+') <= @maxPrice) '

declare @sortCriteria nvarchar(MAX);
set @sortCriteria = (case 
	when @sort = 0 then 'p.Name'
	when @sort = 1 then 'p.Name desc'
	when @sort = 2 then 'price.PriceAction, p.Name'
	when @sort = 3 then 'price.PriceAction desc, p.Name'
	when @sort = 4 then 'p.InsertedDate desc, p.Name'
	when @sort = 5 then 'p.PartnerProductId'
	when @sort = 6 then 'PopularRate desc, newid()'
	when @sort = 7 then 'newid()'
	when @sort = 8 then 'p.IsRecommended desc, newid()'
	when @sort = 9 then 'case when @minRecommendedPrice <= price.PriceAction and price.PriceAction <= @maxRecommendedPrice then 1 else 0 end desc, case when PopularRate is not null then 1 else 0 end desc, newid()'
	when @sort = 10 then 'case when rc.Id is not null then 1 else 0 end desc, newid()'
	when @sort = 11 then 'case when price.PriceAction < price.PriceBase then price.PriceBase / price.PriceAction else 0 end desc, PopularRate desc, newid()'
	end)

declare @searchSql nvarchar(MAX)
set @searchSql = '
	if @calcTotalCount = 1
	begin
		SELECT @totalCount = count(p.ProductId)
		' + @fromSql + '
		' + @whereSql + '
		' + @whereMinMaxPrice + '
	end;
	
	if @calcMaxPrice = 1
	begin
		SELECT @calculatedMaxPrice = max(' + @totalPriceSql + ')
		' + @fromSql + '
		' + @whereSql + '		
	end;

	select 
		t.row_number
		,p1.[ProductId]
      ,[PartnerId]
      ,[InsertedDate]
      ,[UpdatedDate]
      ,[PartnerProductId]
      ,[Type]
      ,[Bid]
      ,[CBid]
      ,[Available]
      ,[Name]
      ,[Url]
      ,[PriceRUR]
      ,[CurrencyId]
      ,[CategoryId]
      ,[Pictures]
      ,[TypePrefix]
      ,[Vendor]
      ,[Model]
      ,[Store]
      ,[Pickup]
      ,[Delivery]
      ,[Description]
      ,[VendorCode]
      ,[LocalDeliveryCost]
      ,[SalesNotes]
      ,[ManufacturerWarranty]
      ,[CountryOfOrigin]
      ,[Downloadable]
      ,[Adult]
      ,[Barcode]
      ,[Param]
      ,[Author]
      ,[Publisher]
      ,[Series]
      ,[Year]
      ,[ISBN]
      ,[Volume]
      ,[Part]
      ,[Language]
      ,[Binding]
      ,[PageExtent]
      ,[TableOfContents]
      ,[PerformedBy]
      ,[PerformanceType]
      ,[Format]
      ,[Storage]
      ,[RecordingLength]
      ,[Artist]
      ,[Media]
      ,[Starring]
      ,[Director]
      ,[OriginalName]
      ,[Country]
      ,[WorldRegion]
      ,[Region]
      ,[Days]
      ,[DataTour]
      ,[HotelStars]
      ,[Room]
      ,[Meal]
      ,[Included]
      ,[Transport]
      ,[Place]
      ,[HallPlan]
      ,[Date]
      ,[IsPremiere]
      ,[IsKids]
      ,[Status]
      ,[ModerationStatus]
      ,[Weight]
      ,[UpdatedUserId]
      ,[IsRecommended]
      ,[BasePriceRUR]
      ,[IsDeliveredByEmail]
	,t.PriceBase
	,t.PriceAction
	,t.PopularRate
	,t.PopularType
	,CategoryName
	,case when t.PriceBase - t.PriceAction >= 1 then cast(1 as bit) else cast(0 as bit) end as IsActionPrice
	from
		(
			select 
					row_number() over 
					(
						order by 						
						' + @sortCriteria + '
					) as row_number
					,p.ProductId
					,price.[PriceBase] as PriceBase
					,price.[PriceAction] as PriceAction
					' + @popularColumns + '
					,pc.Name as CategoryName
				' + @fromSql + '
				' + @whereSql + '
				' + @whereMinMaxPrice + '
		) as t
		JOIN prod.ProductsFromAllPartners p1 WITH(NOLOCK) ON p1.ProductId = t.ProductId
	where row_number between (@countToSkip + 1) and (@countToSkip + @countToTake)
	order by row_number
'

-- print @whereSql;
-- print @searchSql;
--select @searchSql

EXECUTE sp_executesql 
	@searchSql, 
	N'@sort int, 
	@countToSkip int, 
	@countToTake int, 
	@searchTerm nvarchar(500), 
	@partnerIds nvarchar(MAX), 
	@returnEmptyVendorProducts bit, 
	@vendors nvarchar(MAX), 
	@allVendorsSearch bit, 
	@productIds nvarchar(MAX), 
	@minPrice money, 
	@maxPrice money,
	@calcTotalCount bit,
	@calcMaxPrice bit,
	@minInsertedDate datetime,
	@actionPrice bit,
	@partnerProductId nvarchar(max),
	@targetAudiencesIds varchar(max),
	@popularType int,
        @minRecommendedPrice money,
        @maxRecommendedPrice money,
	@totalCount bigint OUTPUT,
	@calculatedMaxPrice money OUTPUT', 
	@sort = @sort,
	@countToSkip = @countToSkip,
	@countToTake = @countToTake,
	@searchTerm = @searchTerm,
	@partnerIds = @partnerIds,
	@returnEmptyVendorProducts = @returnEmptyVendorProducts,
	@vendors = @vendors,
	@allVendorsSearch = @allVendorsSearch,
	@productIds = @productIds,
	@minPrice = @minPrice,
	@maxPrice = @maxPrice,
	@calcTotalCount = @calcTotalCount,
	@calcMaxPrice = @calcMaxPrice,
	@minInsertedDate = @minInsertedDate,
	@actionPrice = @actionPrice,
	@partnerProductId = @partnerProductId,
	@targetAudiencesIds = @targetAudiencesIds,
	@popularType = @popularType,
	@minRecommendedPrice = @minRecommendedPrice,
	@maxRecommendedPrice = @maxRecommendedPrice,
	@totalCount = @totalCount OUTPUT,
	@calculatedMaxPrice = @calculatedMaxPrice OUTPUT


END
