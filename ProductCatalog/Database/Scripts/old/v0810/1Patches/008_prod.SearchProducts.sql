IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[SearchProducts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [prod].[SearchProducts]
GO

CREATE procedure [prod].[SearchProducts]
	@sort int = null,
	@countToSkip bigint = null,
	@countToTake int = null,
	@status int = null,
	@moderationStatus int = null,
	@prodCategoryIds nvarchar(MAX) = null,
	@includeSubCategory bit = null,
	@searchTerm nvarchar(MAX) = null,
	@partnerIds nvarchar(MAX) = null,
	@productIds nvarchar(MAX) = null,
	@vendors nvarchar(MAX) = null,
	@returnEmptyVendorProducts bit = null,
	@calcTotalCount bit = null,
	@minPrice money = null,
	@maxPrice money = null,
	@baseSql nvarchar(MAX) = null,
	@actionSql nvarchar(MAX) = null,
	@deliveryActionSql nvarchar(MAX) = null,
	@deliveryActionQuantitySql nvarchar(MAX) = null,
	@columns nvarchar(MAX) = null,
	@calcMaxPrice bit = null,
	@locationCode nvarchar(32) = null,
	@returnNotDeliveried bit = null,
	@audienceIds nvarchar(max)= null,
	@minInsertedDate datetime = null,
	@actionPrice bit = null,
	@checkPartnerCategoryPermission bit = null,
	@partnerProductId nvarchar(max) = null,
	@productParamsXml xml = null,
	@targetAudiencesIds nvarchar(max) = null,
	@ignoreTargetAudiences bit = 0,
	@productsQuantity int = null,
	@popularType int = 2,
	@onlyPopular bit = 0,
	@totalCount bigint OUT,
	@calculatedMaxPrice money OUT
AS
BEGIN

--exec [prod].[SearchProducts] @totalCount=100, @calculatedMaxPrice=1, @locationCode='7700000000000', 
-- @productParamsXml = '
--<ArrayOfProductParam>
--  <ProductParam>
--    <Name>Тип процессора</Name>
--    <Value>Atom</Value>
--  </ProductParam>
--  <ProductParam>
--    <Name>Размер оперативной памяти</Name>
--    <Unit>Мб</Unit>
--    <Value>2048</Value>
--  </ProductParam>
--  <ProductParam>
--    <Name>Вес</Name>
--    <Unit>гр</Unit>
--    <Value>250</Value>
--  </ProductParam>
--</ArrayOfProductParam>'

SET NOCOUNT ON;

IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#deniedCategories')) 
		DROP TABLE #deniedCategories

--таблица для иденттификаторов всех новых категорий и их родителей
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#newCatIds')) 
		DROP TABLE #newCatIds

IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#partners')) 
		DROP TABLE #partners

IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#deliveryPrice')) 
		DROP TABLE #deliveryPrice

set @sort = ISNULL(@sort, 0)
set @countToSkip = ISNULL(@countToSkip, 0)
set @countToTake = ISNULL(@countToTake, 100)
set @includeSubCategory = ISNULL(@includeSubCategory, 1)
set @returnEmptyVendorProducts = ISNULL(@returnEmptyVendorProducts, 0)
set @calcTotalCount = ISNULL(@calcTotalCount, 0)
set @columns =  ISNULL(@columns, '')
set @productsQuantity =  ISNULL(@productsQuantity, 1)
set @searchTerm = RTRIM(LTRIM(@searchTerm))

create table #deniedCategories (
	CategoryId int
)

insert into #deniedCategories
select CategoryId from [prod].[GetDeniedCategories](@audienceIds)

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
		  and Id not in (select CategoryId from #deniedCategories)
		  and isnull(ParentId, -1) not in (select CategoryId from #deniedCategories)
	
		union all

		select	pc.Id,
				pc.ParentId
		from prod.ProductCategories pc (nolock)
			inner join Categories c ON (pc.ParentId = c.Id)
		where pc.Id not in (select CategoryId from #deniedCategories)
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
	and P.Status = 1 --active only
end
else 
begin
	insert into #partners
	select Id
	from prod.Partners P with (NOLOCK)
	where P.Status = 1 --active only
end

declare @allPartnersSearch bit

select @allPartnersSearch = 
case when COUNT(*) = 0 then 1
else 0
end 
from #partners

declare @allVendorsSearch bit
select @allVendorsSearch = 
case when COUNT(*) = 0 then 1
else 0
end 
from dbo.ParamParserString(@vendors,',')

declare @deliveryPriceJoin nvarchar(MAX)
declare @priceApply nvarchar(MAX)
declare @priceQuantityApply nvarchar(MAX)

IF (@locationCode IS NULL)
BEGIN
	set @deliveryPriceJoin = ' CROSS APPLY (SELECT NULL AS [Price], null as CarrierId, null as [PartnerDeliveryRateId]) delivery'
	
	set @deliveryPriceJoin = @deliveryPriceJoin + ' CROSS APPLY (SELECT NULL AS [QuantityPrice]) deliveryQuantity';
END
ELSE
BEGIN
	
	create table #deliveryPrice
	(
		PartnerId int,  
		WeightMin int,
		WeightMax int,
		Price decimal(38, 20),
		[Type] int,
		[Priority] int,
		CarrierId int,
		[PartnerDeliveryRateId] nvarchar(250)
	)

	insert into #deliveryPrice
	select * from [prod].[GetDeliveryRatesForLocation](@locationCode)
	
	set @priceApply = ' APPLY ( select top 1 Price, CarrierId, [PartnerDeliveryRateId]
						from #deliveryPrice dp
						where dp.PartnerId = p.PartnerId AND p.Weight BETWEEN dp.WeightMin AND dp.WeightMax
						order by Priority desc, Type asc ) delivery'
						
	
	IF (@returnNotDeliveried IS NOT NULL AND @returnNotDeliveried = 1) 
	BEGIN
		set @deliveryPriceJoin = ' OUTER ' + @priceApply 
	END
	ELSE
	BEGIN
		set @deliveryPriceJoin = ' CROSS ' + @priceApply
	END
	
	-- Добавляем вычисление цены доставки для заданого кол-ва товаров.
	SET @deliveryPriceJoin = @deliveryPriceJoin + ' OUTER APPLY ( 
							select top 1 Price as QuantityPrice
							from #deliveryPrice dp
							where dp.PartnerId = p.PartnerId 
							  AND p.Weight * @productsQuantity BETWEEN dp.WeightMin AND dp.WeightMax
							order by Priority desc, Type asc) deliveryQuantity';
END

--select * from #deliveryPrice

-- Поисковый запрос
declare @searchSql nvarchar(MAX)
declare @fromSql nvarchar(MAX)
declare @whereSql nvarchar(MAX)
declare @whereMinMaxPrice nvarchar(MAX)
declare @basePriceSql nvarchar(MAX)
declare @actionPriceSql nvarchar(MAX)
declare @deliveryPriceSql nvarchar(MAX)
declare @deliveryPriceQuantitySql nvarchar(MAX)
declare @totalPriceSql nvarchar(MAX)
declare @wherePopularProducts nvarchar(max) 

set @basePriceSql = ISNULL(@baseSql, 'p.PriceRUR')
set @actionPriceSql = ISNULL(@actionSql, 'p.PriceRUR')
set @deliveryPriceSql = ISNULL(@deliveryActionSql, 'delivery.Price')
set @deliveryPriceQuantitySql = ISNULL(@deliveryActionQuantitySql, 'deliveryQuantity.QuantityPrice')
set @totalPriceSql = @actionPriceSql + ' + ISNULL(' + @deliveryPriceSql + ',0)'

declare @categoryPermissionJoin varchar(256) = ''

if (@checkPartnerCategoryPermission = 1)
	set  @categoryPermissionJoin = ' join [prod].[ProductCategoriesPermissions] pcp on pcp.CategoryId = p.CategoryId and pcp.PartnerId = p.PartnerId '; 

set @fromSql = '
		FROM 
			[prod].[Products] p			
			join #newCatIds cat on p.CategoryId = cat.Id ' + @deliveryPriceJoin + @categoryPermissionJoin +
			' left join [prod].[PopularProducts] pp on pp.ProductId = p.ProductId and pp.PopularType = @popularType'

if @allPartnersSearch != 1
begin
	set @fromSql = @fromSql + ' join #partners part on (p.PartnerId = part.Id) 
	' 
end

if (@onlyPopular = 1)
begin
	set @wherePopularProducts = ' and pp.PopularRate is not null'	
end
else
begin
	set @wherePopularProducts = ''
end

set @whereSql = '
	where
		(@status is null or @status = p.Status)
		and
		(@moderationStatus is null or @moderationStatus = p.ModerationStatus)
		and
		(@searchTerm is null or LOWER(p.Name + p.ProductId + isnull(p.Description, '''')) like ''%'' + LOWER(@searchTerm) + ''%'')
		and
		(@partnerProductId is null or @partnerProductId = p.PartnerProductId)
		and 
		(@productIds is null or ( exists(select value from dbo.ParamParserString(@productIds,'','') where value= p.ProductId) ) )
		and
		(
			@allVendorsSearch = 1 
			or 
			(
				exists
				(
				select value from dbo.ParamParserString(@vendors,'','') 
				where value = p.Vendor or (@returnEmptyVendorProducts = 1 and p.Vendor is null)
				)
			)
		)
		and
		(@minInsertedDate is null or @minInsertedDate <= p.InsertedDate)
		and
		(@actionPrice is null or @actionPrice = 0 or '+@basePriceSql+'<>'+@actionPriceSql+')
' + @wherePopularProducts


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

--print @whereSql

if (@productParamsXml is not null)
begin
	
	declare @paramNames nvarchar(max)
	declare @paramUnits nvarchar(max)
	declare @paramValues nvarchar(max)

	select @paramNames = 
		COALESCE(@paramNames + ') and ([param].exist(''ArrayOfProductParam/ProductParam/Name[. = ("', '([param].exist(''ArrayOfProductParam/ProductParam/Name[. = ("') + p.value('Name[1]','nvarchar(256)') + '")]'') = 1 and ' + 
		case 
		when p.value('Unit[1]','nvarchar(256)') is null then '' else
		COALESCE(@paramUnits + ' and ', '[param].exist(''ArrayOfProductParam/ProductParam/Unit[. = ("') + p.value('Unit[1]','nvarchar(256)') + '")]'') = 1 and ' 
		end +
		COALESCE(@paramValues + ' and ', '[param].exist(''ArrayOfProductParam/ProductParam/Value[. = ("') + p.value('Value[1]','nvarchar(256)') + '")]'') = 1'
	from @productParamsXml.nodes('/ArrayOfProductParam/ProductParam') as t(p) 

	set @whereSql = @whereSql + ' and ' + @paramNames + ') ';
end

set @whereMinMaxPrice = '
		(@minPrice is null or @minPrice <= (' + @totalPriceSql + ') )
		and
		(@maxPrice is null or (' + @totalPriceSql + ') <= @maxPrice)
'

set @searchSql = '

	if @calcTotalCount = 1
	begin
		SELECT @totalCount = count(1)
		' + @fromSql + '
		' + @whereSql + '
		and ' + @whereMinMaxPrice + '
	end
	
	if @calcMaxPrice = 1
	begin
		SELECT @calculatedMaxPrice = max(' + @totalPriceSql + ')
		' + @fromSql + '
		' + @whereSql + '		
	end

	;with calculated as
	(
		SELECT
			p.ProductId	
			,p.PartnerProductId		
			,p.Name
			,p.InsertedDate
			,' + @basePriceSql + ' as PriceBase
			,' + @actionPriceSql + ' as PriceAction
			,' + @totalPriceSql + '  as PriceTotal
			,' + @deliveryPriceSql + ' as PriceDelivery
			,' + @deliveryPriceQuantitySql + ' as PriceDeliveryQuantity
			,deliveryQuantity.QuantityPrice as PriceDeliveryQuantityRur
			, p.PriceRUR
			, delivery.Price as PriceDeliveryRur
			, delivery.PartnerDeliveryRateId
			, pp.PopularRate
			, pp.PopularType
			, [prod].[GetStringFromProductAudienceRows](p.ProductId) as TargetAudienceIds
			, delivery.CarrierId
		' + @fromSql + '
		' + @whereSql + '
		and ' + @whereMinMaxPrice + '
			
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
		,PartnerDeliveryRateId
		, pc.Name as CategoryName
		, pc.NamePath as CategoryNamePath
		,case 
			when t.PriceDeliveryRur > 0 then cast(1 as bit) 
			else cast(0 as bit) 
		end as DeliveryAvailable
		,case when t.PriceBase <> t.PriceAction then cast(1 as bit) 
			else cast(0 as bit) 
		end as IsActionPrice
		,TargetAudienceIds
		,CarrierId
		,t.PopularRate
		,t.PopularType
	from
		(
			select *,
					row_number() over 
					(
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
						case when @sort = 6 then PopularRate end desc,
						case when @sort = 7 then newid() end 
					) as row_number
			from calculated
		) as t
		join [prod].[Products] p on (p.ProductId = t.ProductId)	
		left join [prod].[ProductCategories] pc on (p.CategoryId = pc.Id)
	where row_number between (@countToSkip + 1) and (@countToSkip + @countToTake)
	order by row_number 	
'
--select @searchSql
--print @searchSql

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
	@minInsertedDate = @minInsertedDate,
	@actionPrice = @actionPrice,
	@partnerProductId = @partnerProductId,
	@targetAudiencesIds = @targetAudiencesIds,
	@productsQuantity = @productsQuantity,
	@popularType = @popularType,
	@totalCount = @totalCount OUTPUT,
	@calculatedMaxPrice = @calculatedMaxPrice OUTPUT


/*
set statistics io on;

DECLARE	@totalCount bigint,
		@calculatedMaxPrice bigint

exec prod.SearchProducts2

@sort = 2,
@status=1,
@moderationStatus = 0,
@countToSkip = 0,
@countToTake = 70,
@prodCategoryIds=default,
@includeSubCategory=default,
@searchTerm=default,
@calcTotalCount=1,
@partnerIds=default,
@productIds=default,
@vendors=default,
@returnEmptyVendorProducts=default,
@minPrice=default,
@maxPrice=default,
@locationCode=null,--'7700000000000',
@audienceIds= null,
@calcMaxPrice=1,
@totalCount = @totalCount OUTPUT,
@calculatedMaxPrice = @calculatedMaxPrice OUTPUT

set statistics io off;

SELECT	@totalCount as N'@totalCount',@calculatedMaxPrice as N'@calculatedMaxPrice'
*/

END



GO


