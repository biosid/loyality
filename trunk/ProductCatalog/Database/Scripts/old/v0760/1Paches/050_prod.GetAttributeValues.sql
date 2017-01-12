IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetAttributeValues]') AND type in (N'P', N'PC'))
DROP PROCEDURE [prod].[GetAttributeValues]
GO


CREATE procedure [prod].[GetAttributeValues]
	@columnName nvarchar(32),
	@categoryId int = null,
	@moderationStatus int = null
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
  AND (@moderationStatus is null or @moderationStatus = p.ModerationStatus)
'
EXECUTE sp_executesql @sql, N'@moderationStatus int', @moderationStatus

/****** Object:  StoredProcedure [prod].[GetCategoriesMapping]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO


