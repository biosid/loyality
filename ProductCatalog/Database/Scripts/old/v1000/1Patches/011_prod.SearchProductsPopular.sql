/****** Object:  StoredProcedure [prod].[SearchProducts]    Script Date: 25.12.2013 20:55:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






ALTER procedure [prod].[SearchProducts]
	@sort int = null,
	@countToSkip bigint = null,
	@countToTake int = null,
	@status int = null,
	@moderationStatus int = null,
	@prodCategoryIds nvarchar(MAX) = null,
	@includeSubCategory bit = null,
	@searchTerm nvarchar(MAX) = null,
	@partnerIds nvarchar(MAX) = null,
	@ignorePartnerStatus bit = 0,
	@productIds nvarchar(MAX) = null,
	@vendors nvarchar(MAX) = null,
	@returnEmptyVendorProducts bit = null,
	@calcTotalCount bit = null,
	@minPrice money = null,
	@maxPrice money = null,
	@baseSql nvarchar(MAX),
	@actionSql nvarchar(MAX),
	@deliveryActionSql nvarchar(MAX),
	@deliveryActionQuantitySql nvarchar(MAX),
	@calcMaxPrice bit = null,
	@locationCode nvarchar(32) = null,
	@returnNotDeliveried bit = null,
	@minInsertedDate datetime = null,
	@actionPrice bit = null,
	@checkPartnerCategoryPermission bit = null,
	@partnerProductId nvarchar(max) = null,
	@productParamsXml xml = null,
	@targetAudiencesIds nvarchar(max) = null,
	@ignoreTargetAudiences bit = 0,
	@productsQuantity int = null,
	@popularType int = null,
	@onlyPopular bit = 0,
	@contextKey nvarchar(MAX),
	@totalCount bigint OUT,
	@calculatedMaxPrice money OUT
AS
BEGIN

--declare @sort int = null
--declare @countToSkip bigint = null
--declare @countToTake int = null
--declare @status int = null
--declare @moderationStatus int = null
--declare @prodCategoryIds nvarchar(MAX) = null
--declare @includeSubCategory bit = null
--declare @searchTerm nvarchar(MAX) = null
--declare @partnerIds nvarchar(MAX) = null
--declare @ignorePartnerStatus bit = 0
--declare @productIds nvarchar(MAX) = null
--declare @vendors nvarchar(MAX) = null
--declare @returnEmptyVendorProducts bit = null
--declare @calcTotalCount bit = null
--declare @minPrice money = null
--declare @maxPrice money = null
--declare @calcMaxPrice bit = null
--declare @returnNotDeliveried bit = null
--declare @minInsertedDate datetime = null
--declare @actionPrice bit = null
--declare @checkPartnerCategoryPermission bit = null
--declare @partnerProductId nvarchar(max) = null
--declare @productParamsXml xml = null
--declare @ignoreTargetAudiences bit = 0
--declare @popularType int = 2
--declare @onlyPopular bit = 0
--declare @totalCount bigint
--declare @calculatedMaxPrice money
--declare @locationCode nvarchar(max) ='7700000000000'
--declare @targetAudiencesIds nvarchar(max) ='VIP,AUD_123'
--declare @baseSql nvarchar(max) = N'p.PriceRUR * 3.33333 + 0'
--declare @actionSql nvarchar(max) = N'CASE WHEN ((p.PriceRUR * 3.33333 + 0) * 1 + 0) > (p.PriceRUR * 10 / 100) THEN ((p.PriceRUR * 3.33333 + 0) * 1 + 0) ELSE (p.PriceRUR * 10 / 100) END'
--declare @deliveryActionSql nvarchar(max) = N'CASE WHEN ((delivery.Price * 3.33333 + 0) * 1 + 0) > (delivery.Price * 10 / 100) THEN ((delivery.Price * 3.33333 + 0) * 1 + 0) ELSE (delivery.Price * 10 / 100) END'
--declare @deliveryActionQuantitySql nvarchar(max) = N'CASE WHEN ((deliveryQuantity.QuantityPrice * 3.33333 + 0) * 1 + 0) > (deliveryQuantity.QuantityPrice * 10 / 100) THEN ((deliveryQuantity.QuantityPrice * 3.33333 + 0) * 1 + 0) ELSE (deliveryQuantity.QuantityPrice * 10 / 100) END'
--declare @productsQuantity int
--declare @contextKey nvarchar(max) = '1233425'


SET NOCOUNT ON;

if (LEN(@contextKey) < 10)
begin
	declare @message nvarchar(max) = 'Wrong @contextKey lenght ' + @contextKey
	RAISERROR (@message, 16, 1)
	return
end

--таблица для иденттификаторов всех новых категорий и их родителей
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#newCatIds')) 
		DROP TABLE #newCatIds

IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#deliveryPrice')) 
		DROP TABLE #deliveryPrice

set @sort = ISNULL(@sort, 0)
set @countToSkip = ISNULL(@countToSkip, 0)
set @countToTake = ISNULL(@countToTake, 100)
set @includeSubCategory = ISNULL(@includeSubCategory, 1)
set @returnEmptyVendorProducts = ISNULL(@returnEmptyVendorProducts, 0)
set @calcTotalCount = ISNULL(@calcTotalCount, 0)
set @productsQuantity =  ISNULL(@productsQuantity, 1)
set @searchTerm = RTRIM(LTRIM(@searchTerm))

if (@sort = 6)
	set @popularType = 2

create table #newCatIds
(
	Id int primary key
)

-- если поиск с подкатегориями добавляем все дочерние категории
if (@includeSubCategory = 1)
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
-- если поиск без подкатегорий добавляем только переданные категории
else
begin
	insert into #newCatIds
	select value from dbo.ParamParserString(@prodCategoryIds,',')
end

-- условия Join
declare @allCategoriesSearch bit
select @allCategoriesSearch = 
case when COUNT(*) = 0 then 1
else 0
end 
from #newCatIds

-- Условие по партнёрам
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#partners')) 
		DROP TABLE #partners

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
	and (@ignorePartnerStatus = 1 OR P.[Status] = 1) --active only
end
else 
begin
	insert into #partners
	select Id
	from prod.Partners P with (NOLOCK)
	where (@ignorePartnerStatus = 1 OR P.[Status] = 1) --active only
end

declare @allPartnersSearch bit

select @allPartnersSearch = 
case when COUNT(*) = 0 then 1
else 0
end 
from #partners

declare @activePartnerIds varchar(MAX)
select @activePartnerIds = coalesce(@activePartnerIds+',','') + CAST(Id as nvarchar(MAX))
from #partners

-- Условие по вендорам
declare @allVendorsSearch bit
select @allVendorsSearch = 
case when COUNT(*) = 0 then 1
else 0
end 
from dbo.ParamParserString(@vendors,',')


--select * from #deliveryPrice

-- Создаём кеш
declare @productsCacheTable nvarchar (max)
declare @categoriesCacheTable nvarchar (max)

exec prod.CreateCache
	@contextKey,
	@locationCode,
	@baseSql,
	@actionSql,
	@deliveryActionSql,
	@deliveryActionQuantitySql,
	@productsQuantity,
	@targetAudiencesIds,
	@productsCacheTable OUTPUT,
	@categoriesCacheTable OUTPUT



-- Формируем from
declare @fromSql nvarchar(MAX)
set @fromSql = '
		FROM 
			prod.Products p WITH(NOLOCK)
			inner join '+@productsCacheTable+' price WITH(NOLOCK) on p.[ProductId] = price.[ProductId] '

if (@allCategoriesSearch = 0)
	set @fromSql = @fromSql + '
			join #newCatIds cat on p.CategoryId = cat.Id '

if (@checkPartnerCategoryPermission = 1)
	set @fromSql = @fromSql + ' 
			join [prod].[ProductCategoriesPermissions] pcp on pcp.CategoryId = p.CategoryId and pcp.PartnerId = p.PartnerId '

if @popularType is not null
	set @fromSql = @fromSql + ' 
			left join [prod].[PopularProducts] pp WITH(NOLOCK) on pp.ProductId = p.ProductId and pp.PopularType = @popularType 
			'
else 
	set @fromSql = @fromSql + ' 
			outer apply (select null as PopularType, null as PopularRate) pp
			'

--if @allPartnersSearch != 1
--	set @fromSql = @fromSql + ' join #partners part on (p.PartnerId = part.Id) 
--	' 

-- Формируем where
declare @whereSql nvarchar(MAX)
set @whereSql = '
where 
(@status is null or @status = p.Status)'

if (@moderationStatus is not null)		
set @whereSql = @whereSql +										
		'
		and
		(@moderationStatus = p.ModerationStatus)'

if (@allPartnersSearch != 1)		
set @whereSql = @whereSql +										
		'
		and
		(p.PartnerId in ('+@activePartnerIds+'))'
		
if (@searchTerm is not null)		
set @whereSql = @whereSql +								
		'
		and
		(LOWER(p.Name + p.ProductId + isnull(p.Description, '''')) like ''%'' + LOWER(@searchTerm) + ''%'')'

if (@partnerProductId is not null)		
set @whereSql = @whereSql +						
		'
		and
		(@partnerProductId = p.PartnerProductId)'

if (@productIds is not null)		
set @whereSql = @whereSql +				
		'
		and 
		( exists(select value from dbo.ParamParserString(@productIds,'','') where value= p.ProductId) )'		
		
if (@allVendorsSearch <> 1)		
set @whereSql = @whereSql +				
		'
		and
		(
			exists
			(
			select value from dbo.ParamParserString(@vendors,'','') 
			where value = p.Vendor or (@returnEmptyVendorProducts = 1 and p.Vendor is null)
			)
		)'

if (@minInsertedDate is not null)		
set @whereSql = @whereSql +		
		'
		and
		(@minInsertedDate is null or @minInsertedDate <= p.InsertedDate)'

if (@actionPrice = 1)		
set @whereSql = @whereSql +
		'
		and
		(price.[PriceBase] <> price.[PriceAction]) 
		' 
if (ISNULL(@returnNotDeliveried,0) = 0)
set @whereSql = @whereSql +
'
and 
(price.PriceTotal IS NOT NULL) '

if (@onlyPopular = 1)
set @whereSql = @whereSql + ' 
and pp.PopularRate is not null '



IF (@ignoreTargetAudiences != 1 )
BEGIN
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


	SET @whereSql = @whereSql + ' AND (NOT EXISTS(SELECT 1 FROM [prod].[ProductTargetAudiences] pta WHERE pta.ProductId = p.ProductId) ' 
		+ @whereTargetAudience
		+ ') '
END

if (@productParamsXml is not null)
begin
	
	declare @paramNames nvarchar(max)
	declare @paramUnits nvarchar(max)
	declare @paramValues nvarchar(max)

	select @paramNames = 
		COALESCE(@paramNames + ') and ([param].exist(''ArrayOfProductParam/ProductParam/Name[. = ("', '([param].exist(''ArrayOfProductParam/ProductParam/Name[. = ("') + p.value('Name[1]','nvarchar(MAX)') + '")]'') = 1 and ' + 
		case 
		when p.value('Unit[1]','nvarchar(MAX)') is null then '' else
		COALESCE(@paramUnits + ' and ', '[param].exist(''ArrayOfProductParam/ProductParam/Unit[. = ("') + p.value('Unit[1]','nvarchar(MAX)') + '")]'') = 1 and ' 
		end +
		COALESCE(@paramValues + ' and ', '[param].exist(''ArrayOfProductParam/ProductParam/Value[. = ("') + p.value('Value[1]','nvarchar(MAX)') + '")]'') = 1'
	from @productParamsXml.nodes('/ArrayOfProductParam/ProductParam') as t(p) 

	set @whereSql = @whereSql + ' and ' + @paramNames + ') ';
end

--select @whereSql

declare @totalPriceSql nvarchar(MAX)
set @totalPriceSql = 'price.[PriceAction] + price.[PriceDelivery]'

declare @whereMinMaxPrice nvarchar(MAX)
set @whereMinMaxPrice = ''

if (@minPrice is not null)
set @whereMinMaxPrice = ' 
and (@minPrice <= ('+@totalPriceSql+')) '

if (@maxPrice is not null)
set @whereMinMaxPrice = ' 
and (('+@totalPriceSql+') <= @maxPrice) '



declare @orderBy nvarchar(MAX)
set @orderBy = 'case when @sort = 0 then Name end,
				case when @sort = 1 then Name end desc,
				case when @sort = 2 then PriceTotal end, 
				case when @sort = 2 then Name end,
				case when @sort = 3 then PriceTotal end desc,
				case when @sort = 3 then Name end,
				case when @sort = 4 then p.InsertedDate end desc,
				case when @sort = 4 then Name end,
				case when @sort = 5 then PartnerProductId end,'

if (@popularType is not null)				
set @orderBy = @orderBy + 				
				'
				case when @sort = 6 then PopularRate end desc,'

set @orderBy = @orderBy	+			
				'
				case when @sort = 6 then newid() end,
				case when @sort = 7 then newid() end,
				case when @sort = 8 then p.IsRecommended end desc,
				case when @sort = 8 then newid() end '


declare @searchSql nvarchar(MAX)
set @searchSql = '

	if @calcTotalCount = 1
	begin
		SELECT @totalCount = count(1)
		' + @fromSql + '
		' + @whereSql + '
		' + @whereMinMaxPrice + '
	end
	
	if @calcMaxPrice = 1
	begin
		SELECT @calculatedMaxPrice = max(' + @totalPriceSql + ')
		' + @fromSql + '
		' + @whereSql + '		
	end

	select 
		t.*
		,PriceTotal
		,PriceDelivery
		,PriceDeliveryQuantity
		,PriceDeliveryQuantityRur
		,PriceDeliveryRur
		,ExternalLocationId
		, pc.Name as CategoryName
		, pc.NamePath as CategoryNamePath
		,case 
			when t.PriceDeliveryRur > 0 then cast(1 as bit) 
			else cast(0 as bit) 
		end as DeliveryAvailable
		,case when t.PriceBase <> t.PriceAction then cast(1 as bit) 
			else cast(0 as bit) 
		end as IsActionPrice
		,CarrierId
		,null as TargetAudienceIds
	from
		(
			select 
					row_number() over 
					(
						order by 
						'+@orderBy+'
					) as row_number
					,p.*
					,price.[PriceBase] as PriceBase
					,price.[PriceAction] as PriceAction
					,price.[PriceAction] + price.[PriceDelivery] as PriceTotal
					,price.[PriceDelivery] as PriceDelivery
					,price.[PriceDeliveryQuantity] as PriceDeliveryQuantity
					,price.[PriceDeliveryQuantityRur] as PriceDeliveryQuantityRur
					,price.PriceDeliveryRur as PriceDeliveryRur
					,price.ExternalLocationId
					,price.CarrierId
					,pp.PopularRate
					,pp.PopularType
				' + @fromSql + '
				' + @whereSql + '
				' + @whereMinMaxPrice + '
		) as t		
		left join [prod].[ProductCategories] pc on (t.CategoryId = pc.Id)
	where row_number between (@countToSkip + 1) and (@countToSkip + @countToTake)
	order by row_number 	
'
--print @searchSql
--select @searchSql

EXECUTE sp_executesql 
	@searchSql, 
	N'@sort int, 
	@status int, 
	@moderationStatus int,
	@countToSkip int, 
	@countToTake int, 
	@searchTerm nvarchar(MAX), 
	@partnerIds nvarchar(MAX), 
	@returnEmptyVendorProducts bit, 
	@vendors nvarchar(MAX), 
	@allVendorsSearch bit, 
	@productIds nvarchar(MAX), 
	@minPrice money, 
	@maxPrice money,
	@allCategoriesSearch bit,
	@allPartnersSearch bit,
	@calcTotalCount bit,
	@calcMaxPrice bit,
	@locationCode nvarchar(32),
	@returnNotDeliveried bit,
	@minInsertedDate datetime,
	@actionPrice bit,
	@partnerProductId nvarchar(max),
	@targetAudiencesIds varchar(max),
	@productsQuantity int,
	@popularType int,
	@totalCount bigint OUTPUT,
	@calculatedMaxPrice money OUTPUT', 
	@sort = @sort,
	@status = @status,
	@moderationStatus = @moderationStatus,
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
	@allCategoriesSearch = @allCategoriesSearch,
	@allPartnersSearch = @allPartnersSearch,
	@calcTotalCount = @calcTotalCount,
	@calcMaxPrice = @calcMaxPrice,
	@locationCode = @locationCode,
	@returnNotDeliveried = @returnNotDeliveried,
	@minInsertedDate = @minInsertedDate,
	@actionPrice = @actionPrice,
	@partnerProductId = @partnerProductId,
	@targetAudiencesIds = @targetAudiencesIds,
	@productsQuantity = @productsQuantity,
	@popularType = @popularType,
	@totalCount = @totalCount OUTPUT,
	@calculatedMaxPrice = @calculatedMaxPrice OUTPUT


END
