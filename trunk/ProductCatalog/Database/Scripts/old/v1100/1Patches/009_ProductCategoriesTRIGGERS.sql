DROP TRIGGER [prod].[LogProductCategoriesDelete]
GO

create trigger [prod].[LogProductCategoriesDelete] on [prod].[ProductCategories] for DELETE 
as
insert into [prod].[ProductCategoriesHistory]
select 
'D',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
*
from DELETED
GO


DROP TRIGGER [prod].[LogProductCategoriesInsert]
GO

CREATE trigger [prod].[LogProductCategoriesInsert] on [prod].[ProductCategories]
for insert
as
	insert into [prod].[ProductCategoriesHistory]
	select	'I',
			dbo.GetCurrentUserId(),
			getdate(),
			getutcdate(),
			*
	from INSERTED
GO


DROP TRIGGER [prod].[LogProductCategoriesUpdate]
GO

CREATE trigger [prod].[LogProductCategoriesUpdate] on [prod].[ProductCategories]
for update
as
	insert into [prod].[ProductCategoriesHistory]            
	select	'U',
			dbo.GetCurrentUserId(),
			getdate(),
			getutcdate(),
			*
	from INSERTED
GO