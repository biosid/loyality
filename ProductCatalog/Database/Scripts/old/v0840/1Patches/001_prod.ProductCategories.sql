-- NOTE: Переформирование путей
;with Categories
as
(
	select distinct	
			Id,
			ParentId,
			Name,
			NamePath,
			cast('/' + Name + '/' as nvarchar(max)) AS [NewName]
    from prod.ProductCategories nolock
    WHERE ParentId IS NULL
  union all
	select	pc.Id,
			pc.ParentId,
			pc.Name,
			pc.NamePath,
			cast(c.[NewName] + pc.Name + '/' as nvarchar(max)) AS [NewName]
    from prod.ProductCategories pc (nolock)
		inner join Categories c ON (pc.ParentId = c.Id)
)
UPDATE
    prod.ProductCategories
SET
    prod.ProductCategories.NamePath = (SELECT [NewName] FROM Categories c WHERE c.Id = prod.ProductCategories.Id)