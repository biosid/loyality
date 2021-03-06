/****** Object:  StoredProcedure [prod].[DeleteCategory]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [prod].[DeleteCategory]
	@id int
as
begin

if not exists(select * from prod.ProductCategories where id =  @id)
return -1;

;with Categories
(
	Id
	,ParentId
	,Name
) 
as
(
	select distinct	
			Id,
			ParentId,
			Name
    from prod.ProductCategories (nolock) where id = @id
	union all
	select	pc.Id,
			pc.ParentId,
			pc.Name
    from prod.ProductCategories pc (nolock)
		inner join Categories c ON (pc.ParentId = c.Id)
),
CategoriesWithProducts
as (
	select c.Id, c.ParentId, c.Name, COUNT(p.Name) as prodNum
	from Categories c
	left join prod.Products p on c.Id = p.CategoryId
	group by c.Id, c.ParentId, c.Name 
)
delete from prod.ProductCategories where 
	(select count(id) from CategoriesWithProducts where prodNum = 0) = (select count(id) from CategoriesWithProducts)
	and id in (select id from CategoriesWithProducts)

return @@ROWCOUNT

end


--exec [prod].[DeleteCategory] 23022323
GO
/****** Object:  StoredProcedure [prod].[FillPopularProducts]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [prod].[FillPopularProducts]

AS
BEGIN


-- Параметры 
declare @maxWished int
declare @maxOrdered int
declare @mostViewed int

set @maxWished = 100
set @maxOrdered = 100
set @mostViewed = 100

-- Заполнение наиболее откладываемых
delete from [prod].[PopularProducts] where PopularType = 0

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
	SELECT top (@maxWished) ProductId, 0, count(*)
	FROM [prod].[WishListItems]
	group by [ProductId]
	order by count(*) desc

---- Заполнение наиболее заказываемых 
delete from [prod].[PopularProducts] where PopularType = 1

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
	select top (@maxOrdered) ProductId, 1, count(ProductId)
	from
	(
		select ProductId, InsertedUserId
		from
		(
			select Items.value('/ArrayOfOrderItem[1]/OrderItem[1]/Product[1]/ProductId[1]', 'nvarchar(256)') as ProductId, InsertedUserId
			from [prod].[Orders]
		) as t
		group by ProductId, InsertedUserId
	) as t2
	group by ProductId
	order by count(ProductId) desc

-- Заполнение наиболее просматриваемых
delete from [prod].[PopularProducts] where PopularType = 2

insert into [prod].[PopularProducts] ([ProductId],[PopularType],PopularRate)
	SELECT top (@mostViewed) ProductId, 2, count(*)
	FROM [prod].[ProductViewStatistics]
	group by [ProductId]
	order by count(*) desc
END


GO
/****** Object:  StoredProcedure [prod].[GetAttributeValues]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [prod].[GetAttributeValues]
	@columnName nvarchar(32),
	@categoryId int = null
as

IF EXISTS (
	SELECT * 
	FROM tempdb..sysobjects 
	WHERE id=OBJECT_ID('tempdb..#newCatIds')) 
		DROP TABLE #newCatIds

create table #newCatIds
(
	Id nvarchar(50)
)

;WITH cat
(
	Id
	,ParentId
	,NamePath
	,status
) 
AS 
(
	SELECT 	Id,
			ParentId,
			NamePath,
			status
	FROM [prod].[ProductCategories]
	Where ((@categoryId is null and ParentId is null) or id = @categoryId)

	UNION ALL

	SELECT	childCat.Id,
			childCat.ParentId,
			childCat.NamePath,
			childCat.status
	FROM [prod].[ProductCategories] childCat
		inner join 	cat
	on childCat.ParentId=cat.Id
)
insert into #newCatIds
select distinct Id
from cat

--select * from #newCatIds

declare @sql nvarchar(MAX)

set @sql =  N'
select distinct isnull(' + @columnName + ', '''') as ' + @columnName + '
from prod.Products p 
where exists(select Id from #newCatIds where Id = p.CategoryId)
'
EXECUTE sp_executesql @sql

/****** Object:  StoredProcedure [prod].[GetCategoriesMapping]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [prod].[GetCategoriesMapping]
	@partnerId int,
	@key nvarchar(50)
AS
BEGIN
    DECLARE @sql [nvarchar](max) = N'
;WITH mapping 
(ProductCategoryId, Permission, PartnerCategoryId, PartnerCategoryName, PartnerCategoryNamePath, MappingNamePath)
AS
(
SELECT mapping.ProductCategoryId,
	   CAST(CASE WHEN pcp.CategoryId IS NULL THEN 0 ELSE 1 END AS [bit]),
	   partnerCat.[Id],
	   partnerCat.[Name],
	   partnerCat.[NamePath],
	   mapping.[NamePath]
FROM [prod].[PartnerProductCaterories_{0}] partnerCat --Новая таблица со всеми категориями партнера.
LEFT JOIN [prod].[PartnerProductCategoryLinks] mapping 
	ON (mapping.IncludeSubcategories = 1 AND partnerCat.[NamePath] LIKE mapping.NamePath + ''%'' AND mapping.[PartnerId] = @partnerId)
		OR
	   (mapping.IncludeSubcategories = 0 AND mapping.NamePath = partnerCat.[NamePath] AND mapping.[PartnerId] = @partnerId)
LEFT JOIN [prod].[ProductCategoriesPermissions] pcp ON pcp.[CategoryId] = mapping.ProductCategoryId AND pcp.[PartnerId] = @partnerId
WHERE partnerCat.[PartnerId] = @partnerId
)

SELECT mapping.ProductCategoryId, mapping.Permission, mapping.PartnerCategoryId, mapping.PartnerCategoryName, mapping.PartnerCategoryNamePath
FROM mapping
JOIN	(	SELECT	Permission, 
				PartnerCategoryId, 
				PartnerCategoryName,
				PartnerCategoryNamePath,
				MAX(ISNULL(LEN(MappingNamePath),0)) AS maxlen -- Возьмем длинну самого детального пути
			FROM mapping 
			GROUP BY Permission, PartnerCategoryId, PartnerCategoryName, PartnerCategoryNamePath
		) maxdetail -- Подзапрос ищет самый длинный путь маппинга в группе чтобы учесть может быть так что замаплена родительская с признаком IncludeSubcategories = 1 и сама категория.
   ON	maxdetail.Permission = mapping.Permission AND
		maxdetail.PartnerCategoryId = mapping.PartnerCategoryId AND 
		maxdetail.PartnerCategoryName = mapping.PartnerCategoryName AND
		maxdetail.PartnerCategoryNamePath = mapping.PartnerCategoryNamePath AND
		maxdetail.maxlen = ISNULL(LEN(MappingNamePath),0)
'
	
	DECLARE @sqlMapping [nvarchar](max) = REPLACE(@sql, '{0}', @key)
	
	--SELECT @sqlMapping
	DECLARE @ParmDefinition nvarchar(500);
	
	SET @ParmDefinition = N'@partnerId int';
	EXECUTE sp_executesql @sqlMapping, @ParmDefinition, @partnerId
END





GO
/****** Object:  StoredProcedure [prod].[GetLastDeliveryAddresses]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [prod].[GetLastDeliveryAddresses]
	@userId [nvarchar](255),
	@countToTake [int]
AS
BEGIN

--declare @countToTake int
--set @countToTake = 7


select 
	ord.Id,
	ord.DeliveryInfo,
	AddrText
from
(
	select TOP (@countToTake)
		t.AddrText
		,max(t.Id) as Id
	from
	(
		SELECT
			ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/CountryCode[1]', 'nvarchar(256)'), '') +
			ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/PostCode[1]', 'nvarchar(256)'), '') +
			ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/RegionKladrCode[1]', 'nvarchar(256)'), '') +
			ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/DistrictKladrCode[1]', 'nvarchar(256)'), '') +
			ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/CityKladrCode[1]', 'nvarchar(256)'), '') +
			ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/TownKladrCode[1]', 'nvarchar(256)'), '') +
			ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/StreetName[1]', 'nvarchar(256)'), '') +
			ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/House[1]', 'nvarchar(256)'), '') +
			ISNULL([DeliveryInfo].value('/DeliveryInfo[1]/Flat[1]', 'nvarchar(256)'), '') as AddrText,
			Id
		FROM [prod].Orders
		where InsertedUserId = @userId
	) as t
	group by 
	t.AddrText
	order by 
	max(t.Id) desc
) t2
inner join [prod].Orders ord on ord.Id = t2.Id

END
GO


/****** Object:  StoredProcedure [prod].[GetPopularProducts]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [prod].[GetPopularProducts]
	@popularType int,
	@countToTake int
AS
BEGIN

SELECT TOP (@countToTake)
	[PopularType]
	,[ProductId]
	,[PopularRate]
FROM [prod].[PopularProducts]
where 
	PopularType = @popularType
order by [PopularRate] desc

END

GO
/****** Object:  StoredProcedure [prod].[ImportDeliveryRates]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [prod].[ImportDeliveryRates]
	@EtlSessionId [nvarchar](64), 
	@PartnerId int
AS
BEGIN
	SET NOCOUNT ON;
	
	--Устанавливаем статус -1 если нижная граница диаазона (MinWeightGram) больше верхней (MaxWeightGram)
	UPDATE [prod].[BUFFER_DeliveryRates] 
	  SET [Status] = -1
	WHERE [EtlSessionId] = @EtlSessionId 
	  AND [Status] = 0
	  AND [MinWeightGram] > [MaxWeightGram] 
	  
	--Устанавливаем статус -2 для пересечений нижних границ диаазона (MinWeightGram) или верхних (MaxWeightGram)
	;WITH SessionDeliveryRates
	AS (
		SELECT *, ROW_NUMBER() OVER (ORDER BY [KLADR],[MinWeightGram]) as RowNum 
		FROM [prod].[BUFFER_DeliveryRates] 
		WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0),
	IntersectDeliveryRates
	AS (
	SELECT DISTINCT f.*
	FROM SessionDeliveryRates f
	INNER JOIN SessionDeliveryRates s 
		ON		f.KLADR = s.KLADR 
			AND f.RowNum != s.RowNum 
			AND	((f.[MinWeightGram] BETWEEN s.[MinWeightGram] AND s.[MaxWeightGram])
				 OR
				 (f.[MaxWeightGram] BETWEEN s.[MinWeightGram] AND s.[MaxWeightGram])
				 OR
				 (s.[MinWeightGram] BETWEEN f.[MinWeightGram] AND f.[MaxWeightGram])
				 OR
				 (s.[MaxWeightGram] BETWEEN f.[MinWeightGram] AND f.[MaxWeightGram]))
	)
	update tab
	SET [Status] = -2
	FROM [prod].[BUFFER_DeliveryRates] AS tab
	INNER JOIN IntersectDeliveryRates inter 
		ON	inter.[EtlSessionId] = tab.[EtlSessionId] 
		AND inter.[KLADR] = tab.[KLADR]
		AND inter.[MinWeightGram] = tab.[MinWeightGram]
		AND inter.[MaxWeightGram] = tab.[MaxWeightGram]

	BEGIN TRAN
	
	-- Удаляем старые
	DELETE FROM [prod].[DeliveryRates]
		WHERE [PartnerId] = @PartnerId
		
	-- Вставляем новые
	INSERT INTO [prod].[DeliveryRates]
    ([PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur])
    SELECT @PartnerId,[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur]
	FROM [prod].[BUFFER_DeliveryRates]
	WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0
	
	-- Помечаем как обработанные
	UPDATE  [prod].[BUFFER_DeliveryRates]
	SET [Status] = 1
	WHERE [EtlSessionId] = @EtlSessionId AND [Status] = 0
	
	COMMIT TRAN
END

GO

/****** Object:  StoredProcedure [prod].[UpdateExternalOrderStatus]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create procedure [prod].[UpdateExternalOrderStatus] @OrderId int = null
 	,@ExternalOrderId nvarchar(36) = null
 	,@PartnerId int = null
 	,@Status int
 	,@ExternalOrderStatusCode nvarchar(50) = null
 	,@ExternalOrderStatusDescription nvarchar(1000) = null
 	,@ExternalOrderStatusDateTime datetime = null	
 	,@UpdatedUserId nvarchar(255)
 	,@MissingOrderId int OUTPUT as
BEGIN

if (@OrderId is null and @ExternalOrderId is null)
begin
	RAISERROR (N'@OrderId is null and @ExternalOrderId is null', 10, 1);
end

declare @target int
set @target = (select top 1 Id from prod.Orders where Id = @OrderId)

if(@target is null)
begin
	set @target = (select top 1 Id from prod.Orders where Id = @OrderId)
	if(@target is null)
		begin 
			set @MissingOrderId = -1 --ExternalOrderId + partnerId is missing
		end
	else
		begin
			set @MissingOrderId = @OrderId --missing order
		end	
end
else
begin			
	set @MissingOrderId = 0 --ok
end

UPDATE [prod].[Orders]
set     
[ExternalOrderId] = isnull(@ExternalOrderId,ExternalOrderId)
,[Status] = @Status
,[ExternalOrderStatusCode] = @ExternalOrderStatusCode
,[ExternalOrderStatusDescription] = @ExternalOrderStatusDescription
,ExternalOrderStatusDateTime = @ExternalOrderStatusDateTime
,[StatusChangedDate] = getdate()
,[StatusUtcChangedDate] = GETUTCDATE()
,[UpdatedUserId] = @UpdatedUserId
where
(@OrderId is not null and Id = @OrderId)
or
(@ExternalOrderId is not null and (ExternalOrderId = @ExternalOrderId and @PartnerId = PartnerId))

END


GO

/****** Object:  UserDefinedFunction [dbo].[GetProdCatOrderName]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE FUNCTION [dbo].[GetProdCatOrderName]
 (
  @Id int,
  @Increment int
 )
 RETURNS nvarchar(MAX)
 AS
 BEGIN
  
 declare @parentId int
 declare @catOrder int
 declare @delim nchar(1)
 declare @ordering nchar(10)
 declare @endDelim varchar(12)

 SELECT  
  @parentId = [ParentId]
  ,@catOrder = CatOrder
 FROM prod.ProductCategories
 where Id = @Id

 set @Increment = abs(@Increment)
 -- для корректной сортировки по числам, добавляем лидирующие нули к catOrder
 set @ordering = right('0000000000' + cast((@Increment + @catOrder) as nvarchar(10)), 10)
 set @delim = '/'
 -- страхуемся от неуникальных @catOrder, добавив Id категории в строку сортировки
 set @endDelim = '_' + CAST(@id AS VARCHAR(10)) + @delim

 if (@parentId is not null)
 begin 
  RETURN dbo.GetProdCatOrderName(@parentId, @Increment) + @ordering + @endDelim
 end

 RETURN @delim + @ordering + @endDelim

 END


 --select [dbo].[GetProdCatOrderName] ('27577', 0)
GO
/****** Object:  UserDefinedFunction [dbo].[GetProdName]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetProdName]
(
	@Id nvarchar(50)
)
RETURNS nvarchar(MAX)
AS
BEGIN
	
declare @name nvarchar(max)
declare @parentId nvarchar(50)
declare @delim nchar(1)
set @delim = '/'

SELECT 	
	@name = [Name]
	,@parentId = [ParentId]
FROM prod.ProductCategories
where Id = @Id

if (@parentId is not null)
begin
	--	RETURN @name
	RETURN dbo.GetProdName(@parentId) + @name + @delim
end

RETURN @delim + @name + @delim

END





GO
/****** Object:  UserDefinedFunction [dbo].[GetProdNameForTable]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetProdNameForTable]
(
	@Id nvarchar(50),
	@TableName nvarchar(256)
)
RETURNS nvarchar(MAX)
AS
BEGIN
	
--declare @Id nvarchar(50)
--declare @TableName nvarchar(256)

--set @Id = '1154031'
--set @TableName = '[prod].[PartnerProductCaterories_111_20121113_154703]'
	
declare @name nvarchar(max)
declare @parentId nvarchar(50)
declare @sqlCommand nvarchar (MAX)

set @sqlCommand = 
'SELECT 
@name = [Name], 
@parentId = [ParentId] 
FROM '+@TableName+' 
where Id = '''+@Id+''''

EXECUTE sp_executesql @sqlCommand, N'@name nvarchar(max) OUTPUT, @parentId nvarchar(50) OUTPUT', @name = @name OUTPUT, @parentId = @parentId OUTPUT

--select @name

if (@parentId is not null)
begin
	--	RETURN @name
	RETURN dbo.GetProdName(@parentId)+@name
end

	RETURN @name

END



GO
/****** Object:  UserDefinedFunction [dbo].[ParamParserString]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[RuntimeUtilsSplitString](@String AS VARCHAR(8000), @sep AS CHAR(1))
  RETURNS TABLE
AS
	RETURN
		WITH Pieces(pn, START, STOP)
		AS (
			SELECT 1, 1, CHARINDEX(@sep, @String)
			UNION ALL
			SELECT pn + 1 AS pn, STOP + 1 AS START, CHARINDEX(@sep, @String, STOP + 1) AS STOP
			FROM   Pieces p
			WHERE  p.STOP > 0
		)
		SELECT Substr FROM
		(
		SELECT CAST(SUBSTRING(@String, START, CASE WHEN STOP > 0 THEN STOP -START ELSE 1000 END) AS NVARCHAR(100)) AS Substr
			FROM Pieces) AS t
		WHERE LEN(t.Substr) > 0
GO

CREATE FUNCTION [dbo].[ParamParserString](@delimString nvarchar(max), @delim nchar(1))
RETURNS @paramtable 
TABLE ( [value] nvarchar(1024) ) 
AS BEGIN

DECLARE @len int,
        @index int,
        @nextindex int

SET @len = DATALENGTH(@delimString)
SET @index = 0
SET @nextindex = 0


WHILE (@len > @index )
BEGIN

SET @nextindex = CHARINDEX(@delim, @delimString, @index)

if (@nextindex = 0 ) SET @nextindex = @len + 2

 INSERT @paramtable
 SELECT SUBSTRING( @delimString, @index, @nextindex - @index )


SET @index = @nextindex + 1

END
 RETURN
END



GO
/****** Object:  UserDefinedFunction [prod].[GetAvailablePartnersForRegion]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create function [prod].[GetAvailablePartnersForRegion] ( @kladr nvarchar(32) )
returns @paramtable table ( PartnerId int ) 
as begin

insert into @paramtable
select PartnerId
from [prod].[PartnerDeliveryRates]
where KLADR = @kladr

  return
end;



GO
/****** Object:  UserDefinedFunction [prod].[GetDeliveryPrice]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create function [prod].[GetDeliveryPrice]
(
	@partnerId int = null,
	@locationCode nvarchar(32) = null,
	@weight int = null
)
returns decimal(38, 20) 
as
begin

declare @deliveryPrice decimal (38, 20)
set @deliveryPrice = 0

if @partnerId is null or @partnerId = 0
	return @deliveryPrice

if @locationCode is null or len(@locationCode) = 0
	return @deliveryPrice

if @weight is null or @weight = 0
	return @deliveryPrice

set @deliveryPrice = (select pdr.PriceRur from [prod].[PartnerDeliveryRates] pdr where pdr.KLADR = @locationCode and @weight between pdr.MinWeightGram and pdr.MaxWeightGram)

return isnull (@deliveryPrice, 0)

end

GO
/****** Object:  UserDefinedFunction [prod].[GetDeliveryRatesForLocation]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE function [prod].[GetDeliveryRatesForLocation] ( @kladr nvarchar(32) )
returns @paramtable table ( 
	PartnerId int,  
	WeightMin int,
	WeightMax int,
	Price decimal(38, 20),
	[Type] int,
	[Priority] int
) 
as begin

insert into @paramtable
SELECT [PartnerId],[MinWeightGram],[MaxWeightGram],[PriceRur],MIN([Type]) AS [Type], [Priority]
FROM
(
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],1 as [Type], [Priority]
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(@kladr, 9, 3) != '000' AND 
	SUBSTRING(kladr, 1, 11) = SUBSTRING(@kladr, 1, 11) AND CAST(SUBSTRING(kladr, 12, LEN(kladr)-11) AS BIGINT) = 0
UNION
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],2 as [Type], [Priority]
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(@kladr, 6, 3) != '000' AND 
	SUBSTRING(kladr, 1, 8) = SUBSTRING(@kladr, 1, 8) AND CAST(SUBSTRING(kladr, 9, LEN(kladr)-8) AS BIGINT) = 0
UNION
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],3 as [Type], [Priority]
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(@kladr, 3, 3) != '000' AND 
	SUBSTRING(kladr, 1, 5) = SUBSTRING(@kladr, 1, 5) AND CAST(SUBSTRING(kladr, 6, LEN(kladr)-5) AS BIGINT) = 0
UNION
SELECT [PartnerId],[KLADR],[MinWeightGram],[MaxWeightGram],[PriceRur],4 as [Type], [Priority]
FROM [prod].[PartnerDeliveryRates]
WHERE SUBSTRING(kladr, 1, 2) = SUBSTRING(@kladr, 1, 2) AND CAST(SUBSTRING(kladr, 3, LEN(kladr)-2) AS BIGINT) = 0
) p
GROUP BY [PartnerId],[MinWeightGram],[MaxWeightGram],[PriceRur], [Priority]

--select distinct PartnerId, MinWeightGram, MaxWeightGram, PriceRur
--from [prod].[PartnerDeliveryRates]
--where KLADR = @kladr

  return
end;






GO
/****** Object:  UserDefinedFunction [prod].[GetDeniedCategories]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [prod].[GetDeniedCategories] ( @targetAudiences nvarchar(max) )
returns @paramtable table ( CategoryId int ) 
as begin

insert into @paramtable
select CategoryId
from prod.CategoriesByAudiences 
where AudienceId not in (select [value] from [dbo].[ParamParserString](@targetAudiences, ','))

  return
end;



GO
/****** Object:  Table [dbo].[EtlCounters]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EtlCounters](
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[Category] [nvarchar](255) NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Value] [bigint] NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[LogUtcDateTime] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EtlMessages]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EtlMessages](
	[SequentialId] [bigint] IDENTITY(1,1) NOT NULL,
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[EtlStepName] [nvarchar](255) NULL,
	[MessageType] [int] NOT NULL,
	[Text] [nvarchar](1000) NULL,
	[Flags] [bigint] NULL,
	[StackTrace] [nvarchar](1000) NULL,
	[LogDateTime] [datetime] NOT NULL,
	[LogUtcDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SequentialId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EtlPackages]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EtlPackages](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Text] [nvarchar](max) NULL,
	[RunIntervalSeconds] [int] NOT NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_EtlPackages] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EtlSessions]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EtlSessions](
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlPackageName] [nvarchar](255) NULL,
	[StartDateTime] [datetime] NOT NULL,
	[StartUtcDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NULL,
	[EndUtcDateTime] [datetime] NULL,
	[Status] [int] NOT NULL,
	[ParentEtlSessionId] [uniqueidentifier] NULL,
	[UserName] [nvarchar](50) NULL,
 CONSTRAINT [PK_EtlSessions] PRIMARY KEY NONCLUSTERED 
(
	[EtlPackageId] ASC,
	[EtlSessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EtlVariables]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EtlVariables](
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Modifier] [int] NOT NULL,
	[Value] [nvarchar](1000) NULL,
	[IsSecure] [bit] NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[LogUtcDateTime] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QRTZ_BLOB_TRIGGERS]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_BLOB_TRIGGERS](
	[SCHED_NAME] [nvarchar](100) NOT NULL,
	[TRIGGER_NAME] [nvarchar](150) NOT NULL,
	[TRIGGER_GROUP] [nvarchar](150) NOT NULL,
	[BLOB_DATA] [image] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QRTZ_CALENDARS]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_CALENDARS](
	[SCHED_NAME] [nvarchar](100) NOT NULL,
	[CALENDAR_NAME] [nvarchar](200) NOT NULL,
	[CALENDAR] [image] NOT NULL,
 CONSTRAINT [PK_QRTZ_CALENDARS] PRIMARY KEY CLUSTERED 
(
	[SCHED_NAME] ASC,
	[CALENDAR_NAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QRTZ_CRON_TRIGGERS]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_CRON_TRIGGERS](
	[SCHED_NAME] [nvarchar](100) NOT NULL,
	[TRIGGER_NAME] [nvarchar](150) NOT NULL,
	[TRIGGER_GROUP] [nvarchar](150) NOT NULL,
	[CRON_EXPRESSION] [nvarchar](120) NOT NULL,
	[TIME_ZONE_ID] [nvarchar](80) NULL,
 CONSTRAINT [PK_QRTZ_CRON_TRIGGERS] PRIMARY KEY CLUSTERED 
(
	[SCHED_NAME] ASC,
	[TRIGGER_NAME] ASC,
	[TRIGGER_GROUP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QRTZ_FIRED_TRIGGERS]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_FIRED_TRIGGERS](
	[SCHED_NAME] [nvarchar](100) NOT NULL,
	[ENTRY_ID] [nvarchar](95) NOT NULL,
	[TRIGGER_NAME] [nvarchar](150) NOT NULL,
	[TRIGGER_GROUP] [nvarchar](150) NOT NULL,
	[INSTANCE_NAME] [nvarchar](200) NOT NULL,
	[FIRED_TIME] [bigint] NOT NULL,
	[PRIORITY] [int] NOT NULL,
	[STATE] [nvarchar](16) NOT NULL,
	[JOB_NAME] [nvarchar](150) NULL,
	[JOB_GROUP] [nvarchar](150) NULL,
	[IS_NONCONCURRENT] [bit] NULL,
	[REQUESTS_RECOVERY] [bit] NULL,
 CONSTRAINT [PK_QRTZ_FIRED_TRIGGERS] PRIMARY KEY CLUSTERED 
(
	[SCHED_NAME] ASC,
	[ENTRY_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QRTZ_JOB_DETAILS]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_JOB_DETAILS](
	[SCHED_NAME] [nvarchar](100) NOT NULL,
	[JOB_NAME] [nvarchar](150) NOT NULL,
	[JOB_GROUP] [nvarchar](150) NOT NULL,
	[DESCRIPTION] [nvarchar](250) NULL,
	[JOB_CLASS_NAME] [nvarchar](250) NOT NULL,
	[IS_DURABLE] [bit] NOT NULL,
	[IS_NONCONCURRENT] [bit] NOT NULL,
	[IS_UPDATE_DATA] [bit] NOT NULL,
	[REQUESTS_RECOVERY] [bit] NOT NULL,
	[JOB_DATA] [image] NULL,
 CONSTRAINT [PK_QRTZ_JOB_DETAILS] PRIMARY KEY CLUSTERED 
(
	[SCHED_NAME] ASC,
	[JOB_NAME] ASC,
	[JOB_GROUP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QRTZ_LOCKS]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_LOCKS](
	[SCHED_NAME] [nvarchar](100) NOT NULL,
	[LOCK_NAME] [nvarchar](40) NOT NULL,
 CONSTRAINT [PK_QRTZ_LOCKS] PRIMARY KEY CLUSTERED 
(
	[SCHED_NAME] ASC,
	[LOCK_NAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QRTZ_PAUSED_TRIGGER_GRPS]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_PAUSED_TRIGGER_GRPS](
	[SCHED_NAME] [nvarchar](100) NOT NULL,
	[TRIGGER_GROUP] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_QRTZ_PAUSED_TRIGGER_GRPS] PRIMARY KEY CLUSTERED 
(
	[SCHED_NAME] ASC,
	[TRIGGER_GROUP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QRTZ_SCHEDULER_STATE]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_SCHEDULER_STATE](
	[SCHED_NAME] [nvarchar](100) NOT NULL,
	[INSTANCE_NAME] [nvarchar](200) NOT NULL,
	[LAST_CHECKIN_TIME] [bigint] NOT NULL,
	[CHECKIN_INTERVAL] [bigint] NOT NULL,
 CONSTRAINT [PK_QRTZ_SCHEDULER_STATE] PRIMARY KEY CLUSTERED 
(
	[SCHED_NAME] ASC,
	[INSTANCE_NAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QRTZ_SIMPLE_TRIGGERS]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_SIMPLE_TRIGGERS](
	[SCHED_NAME] [nvarchar](100) NOT NULL,
	[TRIGGER_NAME] [nvarchar](150) NOT NULL,
	[TRIGGER_GROUP] [nvarchar](150) NOT NULL,
	[REPEAT_COUNT] [int] NOT NULL,
	[REPEAT_INTERVAL] [bigint] NOT NULL,
	[TIMES_TRIGGERED] [int] NOT NULL,
 CONSTRAINT [PK_QRTZ_SIMPLE_TRIGGERS] PRIMARY KEY CLUSTERED 
(
	[SCHED_NAME] ASC,
	[TRIGGER_NAME] ASC,
	[TRIGGER_GROUP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QRTZ_SIMPROP_TRIGGERS]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_SIMPROP_TRIGGERS](
	[SCHED_NAME] [nvarchar](100) NOT NULL,
	[TRIGGER_NAME] [nvarchar](150) NOT NULL,
	[TRIGGER_GROUP] [nvarchar](150) NOT NULL,
	[STR_PROP_1] [nvarchar](512) NULL,
	[STR_PROP_2] [nvarchar](512) NULL,
	[STR_PROP_3] [nvarchar](512) NULL,
	[INT_PROP_1] [int] NULL,
	[INT_PROP_2] [int] NULL,
	[LONG_PROP_1] [bigint] NULL,
	[LONG_PROP_2] [bigint] NULL,
	[DEC_PROP_1] [numeric](13, 4) NULL,
	[DEC_PROP_2] [numeric](13, 4) NULL,
	[BOOL_PROP_1] [bit] NULL,
	[BOOL_PROP_2] [bit] NULL,
 CONSTRAINT [PK_QRTZ_SIMPROP_TRIGGERS] PRIMARY KEY CLUSTERED 
(
	[SCHED_NAME] ASC,
	[TRIGGER_NAME] ASC,
	[TRIGGER_GROUP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QRTZ_TRIGGERS]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_TRIGGERS](
	[SCHED_NAME] [nvarchar](100) NOT NULL,
	[TRIGGER_NAME] [nvarchar](150) NOT NULL,
	[TRIGGER_GROUP] [nvarchar](150) NOT NULL,
	[JOB_NAME] [nvarchar](150) NOT NULL,
	[JOB_GROUP] [nvarchar](150) NOT NULL,
	[DESCRIPTION] [nvarchar](250) NULL,
	[NEXT_FIRE_TIME] [bigint] NULL,
	[PREV_FIRE_TIME] [bigint] NULL,
	[PRIORITY] [int] NULL,
	[TRIGGER_STATE] [nvarchar](16) NOT NULL,
	[TRIGGER_TYPE] [nvarchar](8) NOT NULL,
	[START_TIME] [bigint] NOT NULL,
	[END_TIME] [bigint] NULL,
	[CALENDAR_NAME] [nvarchar](200) NULL,
	[MISFIRE_INSTR] [int] NULL,
	[JOB_DATA] [image] NULL,
 CONSTRAINT [PK_QRTZ_TRIGGERS] PRIMARY KEY CLUSTERED 
(
	[SCHED_NAME] ASC,
	[TRIGGER_NAME] ASC,
	[TRIGGER_GROUP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [prod].[CategoriesByAudiences]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[CategoriesByAudiences](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AudienceId] [nvarchar](50) NOT NULL,
	[CategoryId] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CategoriesByAudiences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [prod].[DeliveryCountries]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[DeliveryCountries](
	[PartnerId] [int] NOT NULL,
	[CountryCode] [nvarchar](2) NOT NULL,
 CONSTRAINT [PK_DeliveryCountries] PRIMARY KEY CLUSTERED 
(
	[PartnerId] ASC,
	[CountryCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [prod].[DeliveryRates]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[DeliveryRates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartnerId] [int] NOT NULL,
	[KLADR] [nvarchar](32) NOT NULL,
	[MinWeightGram] [int] NOT NULL,
	[MaxWeightGram] [int] NOT NULL,
	[PriceRur] [decimal](38, 20) NOT NULL,
 CONSTRAINT [PK_DeliveryRates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [prod].[PartnerDeliveryCountryItems]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[PartnerDeliveryCountryItems](
	[PartnerId] [int] NOT NULL,
	[CountryCode] [nvarchar](2) NOT NULL,
 CONSTRAINT [PK_PartnerDeliveryCountryItems] PRIMARY KEY CLUSTERED 
(
	[PartnerId] ASC,
	[CountryCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [prod].[PartnerProductCatalogs]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[PartnerProductCatalogs](
	[PartnerId] [int] NOT NULL,
	[Key] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [prod].[PartnerProductCategoryLinks]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[PartnerProductCategoryLinks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductCategoryId] [int] NOT NULL,
	[PartnerId] [int] NOT NULL,
	[NamePath] [nvarchar](max) NOT NULL,
	[IncludeSubcategories] [bit] NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[InsertedUserId] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_PartnerProductCategoryLinks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


/****** Object:  Table [prod].[PopularProducts]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[PopularProducts](
	[PopularType] [int] NOT NULL,
	[ProductId] [nvarchar](256) NOT NULL,
	[PopularRate] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [prod].[ProductCatalogLoadTasks]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[ProductCatalogLoadTasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[PartnerId] [int] NOT NULL,
	[UserSource] [nvarchar](max) NULL,
	[Source] [nvarchar](max) NOT NULL,
	[Status] [int] NOT NULL,
	[LoadedCount] [int] NOT NULL,
	[NotLoadedCount] [int] NOT NULL,
	[CreateUserId] [nvarchar](50) NOT NULL,
	[CreateDateTime] [datetime] NULL,
 CONSTRAINT [PK_prod.ProductCatalogLoadTasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


/****** Object:  Table [prod].[ProductCategoriesPermissions]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[ProductCategoriesPermissions](
	[PartnerId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[InsertedUserId] [nvarchar](50) NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductCategoriesPermissions] PRIMARY KEY CLUSTERED 
(
	[PartnerId] ASC,
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [prod].[ProductImportTasks]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[ProductImportTasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartnerId] [int] NOT NULL,
	[FileUrl] [nvarchar](256) NOT NULL,
	[Status] [int] NOT NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[InsertedUserId] [nvarchar](64) NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[CountSuccess] [int] NOT NULL,
	[CountFail] [int] NOT NULL,
	[WeightProcessType] [int] NOT NULL,
 CONSTRAINT [PK_ProductImportTasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [prod].[ProductsHistory]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [prod].[ProductsHistory](
	[Action] [char](1) NOT NULL,
	[TriggerDate] [datetime] NOT NULL,
	[TriggerUtcDate] [datetime] NOT NULL,
	[ProductId] [nvarchar](256) NULL,
	[PartnerId] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[PartnerProductId] [nvarchar](256) NULL,
	[Type] [nvarchar](20) NULL,
	[Bid] [int] NULL,
	[CBid] [int] NULL,
	[Available] [bit] NULL,
	[Name] [nvarchar](256) NULL,
	[Url] [nvarchar](256) NULL,
	[PriceRUR] [money] NOT NULL,
	[CurrencyId] [nchar](3) NULL,
	[CategoryId] [int] NULL,
	[Pictures] [xml] NULL,
	[TypePrefix] [nvarchar](50) NULL,
	[Vendor] [nvarchar](256) NULL,
	[Model] [nvarchar](256) NULL,
	[Store] [bit] NULL,
	[Pickup] [bit] NULL,
	[Delivery] [bit] NULL,
	[Description] [nvarchar](512) NULL,
	[VendorCode] [nvarchar](256) NULL,
	[LocalDeliveryCost] [money] NULL,
	[SalesNotes] [nvarchar](50) NULL,
	[ManufacturerWarranty] [bit] NULL,
	[CountryOfOrigin] [nvarchar](256) NULL,
	[Downloadable] [bit] NULL,
	[Adult] [nchar](10) NULL,
	[Barcode] [xml] NULL,
	[Param] [xml] NULL,
	[Author] [nvarchar](256) NULL,
	[Publisher] [nvarchar](256) NULL,
	[Series] [nvarchar](256) NULL,
	[Year] [int] NULL,
	[ISBN] [nvarchar](256) NULL,
	[Volume] [int] NULL,
	[Part] [int] NULL,
	[Language] [nvarchar](50) NULL,
	[Binding] [nvarchar](50) NULL,
	[PageExtent] [int] NULL,
	[TableOfContents] [nvarchar](512) NULL,
	[PerformedBy] [nvarchar](50) NULL,
	[PerformanceType] [nvarchar](50) NULL,
	[Format] [nvarchar](50) NULL,
	[Storage] [nvarchar](50) NULL,
	[RecordingLength] [nvarchar](50) NULL,
	[Artist] [nvarchar](256) NULL,
	[Media] [nvarchar](50) NULL,
	[Starring] [nvarchar](256) NULL,
	[Director] [nvarchar](50) NULL,
	[OriginalName] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[WorldRegion] [nvarchar](50) NULL,
	[Region] [nvarchar](50) NULL,
	[Days] [int] NULL,
	[DataTour] [nvarchar](50) NULL,
	[HotelStars] [nvarchar](50) NULL,
	[Room] [nchar](10) NULL,
	[Meal] [nchar](10) NULL,
	[Included] [nvarchar](256) NULL,
	[Transport] [nvarchar](256) NULL,
	[Place] [nvarchar](256) NULL,
	[HallPlan] [nvarchar](256) NULL,
	[Date] [datetime] NULL,
	[IsPremiere] [bit] NULL,
	[IsKids] [bit] NULL,
	[Status] [int] NULL,
	[ModerationStatus] [int] NULL,
	[Weight] [int] NULL,
	[UpdatedUserId] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [prod].[ProductViewStatistics]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[ProductViewStatistics](
	[ProductId] [nvarchar](256) NOT NULL,
	[UserId] [nvarchar](256) NOT NULL,
	[ViewCount] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [prod].[WishListItems]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[WishListItems](
	[UserId] [nvarchar](64) NOT NULL,
	[ProductId] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ProductsQuantity] [int] NOT NULL,
 CONSTRAINT [PK_WishListItems] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [prod].[PartnerDeliveryRates]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE view [prod].[PartnerDeliveryRates]
as

with Rates(PartnerId, CarrierId, KLADR, MinWeightGram, MaxWeightGram, PriceRur) as 
(	
	select P.Id as PartnerId, P.CarrierId, DR.KLADR, DR.MinWeightGram, DR.MaxWeightGram,
	DR.PriceRur
	from prod.Partners P	
	left join prod.DeliveryRates DR	
	on P.Id = DR.PartnerId
	where P.Status = 1
),
Rates2 as
(
--get own price
select *, 1 as [Priority] from Rates r
where r.KLADR is not null --don't include records without own price

union all

--add carrier price
select r2.PartnerId, r2.CarrierId, r1.KLADR, r1.MinWeightGram, r1.MaxWeightGram, r1.PriceRur,
CASE WHEN r1.CarrierId IS NULL THEN 0 ELSE 1 END as [Priority]
from Rates r1
join Rates r2
on r1.PartnerId = r2.CarrierId
where r1.KLADR is not null --don't include records without carrier price
)
select cast(ROW_NUMBER() over(order by PartnerId) as int) as Id, * from Rates2


GO
/****** Object:  View [prod].[Products]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [prod].[Products]
AS                        
SELECT 1 as Dummy
GO
ALTER TABLE [dbo].[EtlPackages] ADD  DEFAULT ((0)) FOR [RunIntervalSeconds]
GO
ALTER TABLE [prod].[Orders] ADD  CONSTRAINT [DF_Orders_StatusChangedDate]  DEFAULT (getdate()) FOR [StatusChangedDate]
GO
ALTER TABLE [prod].[Orders] ADD  CONSTRAINT [DF_Orders_StatusUtcChangedDate]  DEFAULT (getutcdate()) FOR [StatusUtcChangedDate]
GO
ALTER TABLE [prod].[Orders] ADD  CONSTRAINT [DF_Orders_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO
ALTER TABLE [prod].[Orders] ADD  CONSTRAINT [DF_Orders_InsertedUtcDate]  DEFAULT (getutcdate()) FOR [InsertedUtcDate]
GO
ALTER TABLE [prod].[Orders] ADD  CONSTRAINT [DF_Orders_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [prod].[Orders] ADD  CONSTRAINT [DF_Orders_UpdatedUtcDate]  DEFAULT (getutcdate()) FOR [UpdatedUtcDate]
GO
ALTER TABLE [prod].[Orders] ADD  DEFAULT ((0)) FOR [PaymentStatus]
GO
ALTER TABLE [prod].[Orders] ADD  DEFAULT ((0)) FOR [DeliveryPaymentStatus]
GO
ALTER TABLE [prod].[OrdersHistory] ADD  DEFAULT ((0)) FOR [PaymentStatus]
GO
ALTER TABLE [prod].[OrdersHistory] ADD  DEFAULT ((0)) FOR [DeliveryPaymentStatus]
GO
ALTER TABLE [prod].[PartnerProductCatalogs] ADD  CONSTRAINT [DF_PartnerProductCatalogs_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [prod].[PartnerProductCatalogs] ADD  CONSTRAINT [DF_PartnerProductCatalogs_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO
ALTER TABLE [prod].[PartnerProductCategoryLinks] ADD  CONSTRAINT [DF_PartnerProductCategoryLinks_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO
ALTER TABLE [prod].[Partners] ADD  CONSTRAINT [DF_Partners_Type]  DEFAULT ((0)) FOR [Type]
GO
ALTER TABLE [prod].[Partners] ADD  CONSTRAINT [DF_Partners_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [prod].[Partners] ADD  CONSTRAINT [DF_Partners_ThrustLevel]  DEFAULT ((0)) FOR [ThrustLevel]
GO
ALTER TABLE [prod].[Partners] ADD  CONSTRAINT [DF_Partner_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO
ALTER TABLE [prod].[ProductCategories] ADD  CONSTRAINT [DF_ProductCategories_Type]  DEFAULT ((0)) FOR [Type]
GO
ALTER TABLE [prod].[ProductCategories] ADD  CONSTRAINT [DF_ProductCategories_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO
ALTER TABLE [prod].[ProductCategories] ADD  CONSTRAINT [DF_ProductCategories_CatOrder]  DEFAULT ((0)) FOR [CatOrder]
GO
ALTER TABLE [prod].[ProductCategoriesPermissions] ADD  CONSTRAINT [DF_ProductCategoriesPermissions_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO
ALTER TABLE [prod].[ProductImportTasks] ADD  CONSTRAINT [DF_ProductImportTasks_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO
ALTER TABLE [prod].[ProductImportTasks] ADD  DEFAULT ((0)) FOR [CountSuccess]
GO
ALTER TABLE [prod].[ProductImportTasks] ADD  DEFAULT ((0)) FOR [CountFail]
GO
ALTER TABLE [prod].[ProductViewStatistics] ADD  CONSTRAINT [DF_ProductViewStatistics_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [prod].[WishListItems] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [prod].[WishListItems] ADD  CONSTRAINT [DF_WishListItems_ProductsQuantity]  DEFAULT ((1)) FOR [ProductsQuantity]
GO
ALTER TABLE [dbo].[QRTZ_CRON_TRIGGERS]  WITH CHECK ADD  CONSTRAINT [FK_QRTZ_CRON_TRIGGERS_QRTZ_TRIGGERS] FOREIGN KEY([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
REFERENCES [dbo].[QRTZ_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[QRTZ_CRON_TRIGGERS] CHECK CONSTRAINT [FK_QRTZ_CRON_TRIGGERS_QRTZ_TRIGGERS]
GO
ALTER TABLE [dbo].[QRTZ_SIMPLE_TRIGGERS]  WITH CHECK ADD  CONSTRAINT [FK_QRTZ_SIMPLE_TRIGGERS_QRTZ_TRIGGERS] FOREIGN KEY([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
REFERENCES [dbo].[QRTZ_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[QRTZ_SIMPLE_TRIGGERS] CHECK CONSTRAINT [FK_QRTZ_SIMPLE_TRIGGERS_QRTZ_TRIGGERS]
GO
ALTER TABLE [dbo].[QRTZ_SIMPROP_TRIGGERS]  WITH CHECK ADD  CONSTRAINT [FK_QRTZ_SIMPROP_TRIGGERS_QRTZ_TRIGGERS] FOREIGN KEY([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
REFERENCES [dbo].[QRTZ_TRIGGERS] ([SCHED_NAME], [TRIGGER_NAME], [TRIGGER_GROUP])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[QRTZ_SIMPROP_TRIGGERS] CHECK CONSTRAINT [FK_QRTZ_SIMPROP_TRIGGERS_QRTZ_TRIGGERS]
GO
ALTER TABLE [dbo].[QRTZ_TRIGGERS]  WITH CHECK ADD  CONSTRAINT [FK_QRTZ_TRIGGERS_QRTZ_JOB_DETAILS] FOREIGN KEY([SCHED_NAME], [JOB_NAME], [JOB_GROUP])
REFERENCES [dbo].[QRTZ_JOB_DETAILS] ([SCHED_NAME], [JOB_NAME], [JOB_GROUP])
GO
ALTER TABLE [dbo].[QRTZ_TRIGGERS] CHECK CONSTRAINT [FK_QRTZ_TRIGGERS_QRTZ_JOB_DETAILS]
GO
ALTER TABLE [prod].[DeliveryCountries]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryCountries_Partners] FOREIGN KEY([PartnerId])
REFERENCES [prod].[Partners] ([Id])
GO
ALTER TABLE [prod].[DeliveryCountries] CHECK CONSTRAINT [FK_DeliveryCountries_Partners]
GO
ALTER TABLE [prod].[DeliveryRates]  WITH CHECK ADD  CONSTRAINT [FK_DELIVERY_FK_DELIVE_PARTNERS] FOREIGN KEY([PartnerId])
REFERENCES [prod].[Partners] ([Id])
GO
ALTER TABLE [prod].[DeliveryRates] CHECK CONSTRAINT [FK_DELIVERY_FK_DELIVE_PARTNERS]
GO
ALTER TABLE [prod].[PartnerProductCatalogs]  WITH CHECK ADD  CONSTRAINT [FK_PartnerProductCatalogs_Partners] FOREIGN KEY([PartnerId])
REFERENCES [prod].[Partners] ([Id])
GO
ALTER TABLE [prod].[PartnerProductCatalogs] CHECK CONSTRAINT [FK_PartnerProductCatalogs_Partners]
GO
ALTER TABLE [prod].[Partners]  WITH CHECK ADD  CONSTRAINT [FK_PARTNERS_REFERENCE_PARTNERS] FOREIGN KEY([CarrierId])
REFERENCES [prod].[Partners] ([Id])
GO
ALTER TABLE [prod].[Partners] CHECK CONSTRAINT [FK_PARTNERS_REFERENCE_PARTNERS]
GO
ALTER TABLE [prod].[ProductCatalogLoadTasks]  WITH CHECK ADD  CONSTRAINT [FK_prod.ProductCatalogLoadTasks.PartnerId_prod.Partners] FOREIGN KEY([PartnerId])
REFERENCES [prod].[Partners] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [prod].[ProductCatalogLoadTasks] CHECK CONSTRAINT [FK_prod.ProductCatalogLoadTasks.PartnerId_prod.Partners]
GO
ALTER TABLE [prod].[ProductCategoriesPermissions]  WITH CHECK ADD  CONSTRAINT [FK_ProductCategoriesPermissions_Partners] FOREIGN KEY([PartnerId])
REFERENCES [prod].[Partners] ([Id])
GO
ALTER TABLE [prod].[ProductCategoriesPermissions] CHECK CONSTRAINT [FK_ProductCategoriesPermissions_Partners]
GO
ALTER TABLE [prod].[ProductCategoriesPermissions]  WITH CHECK ADD  CONSTRAINT [FK_ProductCategoriesPermissions_ProductCategories] FOREIGN KEY([CategoryId])
REFERENCES [prod].[ProductCategories] ([Id])
GO
ALTER TABLE [prod].[ProductCategoriesPermissions] CHECK CONSTRAINT [FK_ProductCategoriesPermissions_ProductCategories]
GO
ALTER TABLE [prod].[ProductImportTasks]  WITH CHECK ADD  CONSTRAINT [FK_ProductImportTasks_Partners] FOREIGN KEY([PartnerId])
REFERENCES [prod].[Partners] ([Id])
GO
ALTER TABLE [prod].[ProductImportTasks] CHECK CONSTRAINT [FK_ProductImportTasks_Partners]
GO
