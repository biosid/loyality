/****** Object:  StoredProcedure [prod].[CreateCache]    Script Date: 08.12.2013 0:09:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




ALTER PROCEDURE [prod].[CreateCache]
	@contextKey nvarchar(max),
	@locationCode nvarchar(32) = null,
	@baseSql nvarchar(max),
	@actionSql nvarchar(max),
	@deliveryActionSql nvarchar(max),
	@deliveryActionQuantitySql nvarchar(max),
	@productsQuantity nvarchar(max) = null,
	@targetAudiencesIds nvarchar(max) = null,
	@priceTableName nvarchar(max) OUTPUT,
	@categoriesCacheTable nvarchar(max) OUTPUT
AS
BEGIN

--declare @locationCode nvarchar(max) ='7700000000000'
--declare @targetAudiencesIds nvarchar(max) ='VIP,AUD_123'
--declare @baseSql nvarchar(max) = N'p.PriceRUR * 3.33333 + 0'
--declare @actionSql nvarchar(max) = N'CASE WHEN ((p.PriceRUR * 3.33333 + 0) * 1 + 0) > (p.PriceRUR * 10 / 100) THEN ((p.PriceRUR * 3.33333 + 0) * 1 + 0) ELSE (p.PriceRUR * 10 / 100) END'
--declare @deliveryActionSql nvarchar(max) = N'CASE WHEN ((delivery.Price * 3.33333 + 0) * 1 + 0) > (delivery.Price * 10 / 100) THEN ((delivery.Price * 3.33333 + 0) * 1 + 0) ELSE (delivery.Price * 10 / 100) END'
--declare @deliveryActionQuantitySql nvarchar(max) = N'CASE WHEN ((deliveryQuantity.QuantityPrice * 3.33333 + 0) * 1 + 0) > (deliveryQuantity.QuantityPrice * 10 / 100) THEN ((deliveryQuantity.QuantityPrice * 3.33333 + 0) * 1 + 0) ELSE (deliveryQuantity.QuantityPrice * 10 / 100) END'
--declare @productsQuantity int
--declare @contextKey nvarchar(max) = '1233425'
--declare @priceTableName nvarchar(max)

declare @contextId int
set @priceTableName = 'prod.ProductsCache_'
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
	set @priceTableName = @priceTableName+CAST(@contextId as nvarchar(max))
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
	set @priceTableName = @priceTableName+CAST(@contextId as nvarchar(max))
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
'CREATE TABLE ' + @priceTableName + CAST(@contextId as nvarchar(max)) + '
(
	[ProductId] [nvarchar](256) NOT NULL,
	[PartnerId] [int] NOT NULL,
	[CarrierId] [int] NULL,
	[PriceBase] [money] NULL,
	[PriceAction] [money] NULL,
	[PriceDeliveryRur] [money] NULL,
	[PriceDelivery] [money] NULL,
	[PriceDeliveryQuantityRur] [money] NULL,
	[PriceDeliveryQuantity] [money] NULL,
	[PriceTotal] [money] NULL,
	ExternalLocationId nvarchar(250) NULL
) ON [PRIMARY]'

EXECUTE sp_executesql @createSql

-- WAITFOR DELAY '00:00:10'; -- Для тестирования паралельного вызова процедуры с одинаковыми параметрами
---- Заполняем кеш цен

set @productsQuantity =  ISNULL(@productsQuantity, 1)

IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#deliveryPrice')) 
begin
		DROP TABLE #deliveryPrice
end

declare @deliveryPriceJoin nvarchar(MAX)
declare @priceApply nvarchar(MAX)
declare @priceQuantityApply nvarchar(MAX)

create table #deliveryPrice
(
	--[Id] [int] IDENTITY(1,1) NOT NULL,
	PartnerId int,  
	WeightMin int,
	WeightMax int,
	Price decimal(38, 20),
	[Type] int,
	[Priority] int,
	CarrierId int,
	[ExternalLocationId] nvarchar(250) NULL,
)

insert into #deliveryPrice
select * from [prod].[GetDeliveryRatesForLocation](@locationCode)
	
CREATE NONCLUSTERED INDEX IX_deliveryPrice ON #deliveryPrice
(
	[Priority] desc,
	[Type] asc,
	WeightMin asc,
	WeightMax asc,
	PartnerId asc
)
INCLUDE
(
	Price,
	CarrierId,
	[ExternalLocationId]
)

-- Поисковый запрос
declare @searchSql nvarchar(MAX)
declare @whereSql nvarchar(MAX)
declare @whereMinMaxPrice nvarchar(MAX)
declare @totalPriceSql nvarchar(MAX)
declare @wherePopularProducts nvarchar(max) 

set @baseSql = ISNULL(@baseSql, 'NULL')
set @actionSql = ISNULL(@actionSql, 'NULL')
set @deliveryActionSql = ISNULL(@deliveryActionSql, 'NULL')
set @deliveryActionQuantitySql = ISNULL(@deliveryActionQuantitySql, 'NULL')
set @totalPriceSql = @actionSql + ' + ' + @deliveryActionSql

set @searchSql = '
INSERT INTO ' + CAST(@priceTableName as nvarchar(max)) + CAST(@contextId as nvarchar(max)) + 
' 
SELECT
	p.ProductId	
	,p.PartnerId
	,delivery.CarrierId
	,'+@baseSql+' as PriceBase
	,'+@actionSql+' as PriceAction	
	,delivery.Price as PriceDeliveryRur
	,'+@deliveryActionSql+' as PriceDelivery
	,deliveryQuantity.QuantityPrice as PriceDeliveryQuantityRur		
	,'+@deliveryActionQuantitySql+' as PriceDeliveryQuantity
	,'+@totalPriceSql+'  as PriceTotal
	,delivery.ExternalLocationId
FROM
	[prod].[Products] p with (NOLOCK)
	left join 
	[prod].[ProductCategories] pc with (NOLOCK) on (p.CategoryId = pc.Id)
OUTER APPLY 
( 
	select top 1 Price, CarrierId, ExternalLocationId
	from #deliveryPrice dp
	where dp.PartnerId = p.PartnerId 
	AND p.Weight BETWEEN dp.WeightMin AND dp.WeightMax
	order by Priority desc, Type asc	
) delivery '

if (@productsQuantity=1)
begin
	set @searchSql = @searchSql +
	'OUTER APPLY 
	( 
		select
			delivery.Price as Price,
			delivery.Price as QuantityPrice
	) deliveryQuantity '
end
else
begin
	set @searchSql = @searchSql +
	'OUTER APPLY 
	( 
		select top 1 Price as QuantityPrice
		from #deliveryPrice dp
		where dp.PartnerId = p.PartnerId 
		AND p.Weight * @productsQuantity BETWEEN dp.WeightMin AND dp.WeightMax
		order by Priority desc, Type asc
	) deliveryQuantity '
end

EXECUTE sp_executesql 
	@searchSql 
	,N'@productsQuantity int, 
	@contextId int'
	,@productsQuantity = @productsQuantity
	,@contextId = @contextId


set @createSql =
'CREATE NONCLUSTERED INDEX IX_ProductPrices_'+CAST(@contextId as nvarchar(max))+' ON '+@priceTableName + CAST(@contextId as nvarchar(max))+'
(
	[PriceTotal] ASC
)' 
--+'ALTER TABLE '+@priceTableName + CAST(@contextId as nvarchar(max))+' ADD CONSTRAINT
--	PK_ProductPrices_'+CAST(@contextId as nvarchar(max))+' PRIMARY KEY CLUSTERED 
--	(
--	ProductId,
--	PartnerId
--	)
--'

--EXECUTE sp_executesql @createSql

set @priceTableName = @priceTableName+CAST(@contextId as nvarchar(max))

-- Создаём кеш для категорий

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
	inner join '+@priceTableName+' price on (price.[ProductId] = p.[ProductId] and price.PriceTotal is not null)
	inner join [prod].[ProductCategoriesPermissions] pcp on pcp.CategoryId = p.CategoryId and pcp.PartnerId = p.PartnerId
where 			 
	p.Status = 1 
	and 
	p.ModerationStatus = 2
	'+@whereTargetAudience+'
group by c.Id
'

--print @searchSql

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
