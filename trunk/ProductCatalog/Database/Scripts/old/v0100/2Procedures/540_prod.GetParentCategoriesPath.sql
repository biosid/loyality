
CREATE procedure [prod].[GetParentCategoriesPath] --676
	@categoryId int
as

-- execute prod.GetParentCategoriesPath 24354

create table #categories (
	Id int, 
	Type int,
	ParentId int, 
	Name nvarchar(256), 
	NamePath nvarchar(max), 
	Status int,
	InsertedUserId nvarchar(50),
	UpdatedUserId nvarchar(50),
	OnlineCategoryUrl nvarchar(1000),
	NotifyOrderStatusUrl nvarchar(1000),
	InsertedDate datetime,
	UpdatedDate datetime,
	CatOrder int,
	OnlineCategoryPartnerId int
)

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
insert into #categories
select distinct *
from cat

select *
from #categories
order by NamePath asc





GO