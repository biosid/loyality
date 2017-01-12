CREATE PROCEDURE [prod].[GetPriceTableName]	
	@contextKey nvarchar(max),
	@locationCode nvarchar(32) = null,
	@baseSql nvarchar(max),
	@actionSql nvarchar(max),
	@deliveryActionSql nvarchar(max),
	@deliveryActionQuantitySql nvarchar(max),
	@productsQuantity nvarchar(max) = null,
	@priceTableName nvarchar(max) OUTPUT
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
set @priceTableName = 'prod.ProductPrices_'
declare @contextKeyHash varbinary(16)
set @contextKeyHash = HASHBYTES('MD5', @contextKey)

select
	@contextKey = [ContextKey],
	@contextId = [ContextId],
	@contextKeyHash = ContextKeyHash
from
	[prod].[ProductPrices]
where 
	ContextKeyHash = @contextKeyHash

--select @contextKey, @contextId
--return 

-- Если кеш найден возвращаем его
if (@contextId is not null)
begin
	
	set @priceTableName = @priceTableName+CAST(@contextId as nvarchar(max))
	--select @priceTableName
	return

end

--select @contextKey
--return

-- Создаём новую таблицу с кешем цен
insert into [prod].[ProductPrices] ([ContextKey], ContextKeyHash) VALUES (@contextKey, @contextKeyHash)
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
)'

EXECUTE sp_executesql @createSql




---- Заполняем кеш цен

set @productsQuantity =  ISNULL(@productsQuantity, 1)



IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#deliveryPrice')) 
		DROP TABLE #deliveryPrice

declare @deliveryPriceJoin nvarchar(MAX)
declare @priceApply nvarchar(MAX)
declare @priceQuantityApply nvarchar(MAX)


create table #deliveryPrice
(
	PartnerId int,  
	WeightMin int,
	WeightMax int,
	Price decimal(38, 20),
	[Type] int,
	[Priority] int,
	CarrierId int,
	[ExternalLocationId] nvarchar(250)
)

insert into #deliveryPrice
select * from [prod].[GetDeliveryRatesForLocation](@locationCode)
	
--select * from #deliveryPrice

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
	[prod].[Products] p 
	left join 
	[prod].[ProductCategories] pc on (p.CategoryId = pc.Id)
OUTER APPLY 
( 
	select top 1 Price, CarrierId, ExternalLocationId
	from #deliveryPrice dp
	where dp.PartnerId = p.PartnerId AND p.Weight BETWEEN dp.WeightMin AND dp.WeightMax
	order by Priority desc, Type asc 
) delivery
OUTER APPLY 
( 
	select top 1 Price as QuantityPrice
	from #deliveryPrice dp
	where dp.PartnerId = p.PartnerId 
	AND p.Weight * @productsQuantity BETWEEN dp.WeightMin AND dp.WeightMax
	order by Priority desc, Type asc
) deliveryQuantity'

--print @searchSql

EXECUTE sp_executesql 
	@searchSql 
	,N'@productsQuantity int, 
	@contextId int'
	,@productsQuantity = @productsQuantity
	,@contextId = @contextId

set @priceTableName = @priceTableName+CAST(@contextId as nvarchar(max))

END
