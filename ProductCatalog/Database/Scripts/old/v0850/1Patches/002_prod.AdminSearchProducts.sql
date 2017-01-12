IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[AdminSearchProducts]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[AdminSearchProducts]
GO

CREATE procedure [prod].[AdminSearchProducts]
	@sort int = 0,
	@countToSkip bigint = null,
	@countToTake int = null,
	@status int = null,
	@moderationStatus int = null,
	@prodCategoryIds nvarchar(MAX) = null,
	@includeSubCategory bit = 1,
	@searchTerm nvarchar(MAX) = null,
	@partnerIds nvarchar(MAX) = null,
	@productIds nvarchar(MAX) = null,
	@calcTotalCount bit = 0,
	@totalCount bigint OUT
AS
BEGIN

SET NOCOUNT ON;

--таблица для иденттификаторов всех новых категорий и их родителей
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#newCatIds')) 
		DROP TABLE #newCatIds

IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#partners')) 
		DROP TABLE #partners
		
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#productsIds')) 
		DROP TABLE #productsIds

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
		select 	Id, ParentId
		from prod.ProductCategories nolock
		where (len(isnull(@prodCategoryIds,'')) = 0 or id in (select value from dbo.ParamParserString(@prodCategoryIds,',')))		  
		union all
		select	pc.Id, pc.ParentId
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

if (@productIds is null and len(@partnerIds) != 0) 
begin
	insert into #productsIds
	select value from dbo.ParamParserString(@productIds,',')
end

if @calcTotalCount = 1
	begin
		SELECT @totalCount = count(1)
		FROM 
			[prod].[Products] p		
			join #newCatIds cat on p.CategoryId = cat.Id 
			join #partners part on (p.PartnerId = part.Id) 
		where
		(@status is null or @status = p.Status)
		and
		(@moderationStatus is null or @moderationStatus = p.ModerationStatus)
		and
		(@searchTerm is null or LOWER(p.Name + p.ProductId + isnull(p.Description, '')) like '%' + LOWER(@searchTerm) + '%')
		and
		(@productIds is null or ( exists(select 1 from #productsIds where ProductsId = p.ProductId) ) )
	end

	;with calculated as
	(
		SELECT
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
		FROM 
			[prod].[Products] p		
			join #newCatIds cat on p.CategoryId = cat.Id 
			join #partners part on p.PartnerId = part.Id 
		where
		(@status is null or @status = p.Status)
		and
		(@moderationStatus is null or @moderationStatus = p.ModerationStatus)
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
		, pc.Name as CategoryName
		, pc.NamePath as CategoryNamePath
		,0 as DeliveryAvailable
		,0 as IsActionPrice
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
						case when @sort = 6 then newid() end desc,
						case when @sort = 7 then newid() end 
					) as row_number
			from calculated
		) as t
		join [prod].[Products] p on (p.ProductId = t.ProductId)	
		left join [prod].[ProductCategories] pc on (p.CategoryId = pc.Id)
	where row_number between (@countToSkip + 1) and (@countToSkip + @countToTake)
	order by row_number 	
END


GO


