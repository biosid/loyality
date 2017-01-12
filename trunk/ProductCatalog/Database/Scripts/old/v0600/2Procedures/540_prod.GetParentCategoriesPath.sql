
alter procedure [prod].[GetParentCategoriesPath] 
	@categoryId int
as

-- execute prod.GetParentCategoriesPath 24354

;WITH cat
AS 
(
	SELECT 	*
	FROM [prod].[ProductCategories]
	Where id = @categoryId

	UNION ALL

	SELECT	parentCat.*
	FROM [prod].[ProductCategories] parentCat
		inner join 	cat
	on parentCat.Id = cat.ParentId
)
select distinct * from cat order by NamePath asc

GO