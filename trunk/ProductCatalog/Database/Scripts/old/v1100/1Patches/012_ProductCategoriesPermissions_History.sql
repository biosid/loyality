CREATE TABLE [prod].[ProductCategoriesPermissionsHistory](
	[Action] [char](1) NOT NULL,
	[TriggerUserId] [nvarchar](255) NULL,
	[TriggerDate] [datetime] NOT NULL,
	[TriggerUtcDate] [datetime] NOT NULL,
	[PartnerId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[InsertedUserId] [nvarchar](50) NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
) ON [PRIMARY]

GO

create trigger [prod].[LogProductCategoriesPermissionsDelete] on [prod].[ProductCategoriesPermissions] for DELETE 
as
insert into [prod].[ProductCategoriesPermissionsHistory]
select 
'D',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
*
from DELETED
GO

create trigger [prod].[LogProductCategoriesPermissionsInsert] on [prod].[ProductCategoriesPermissions] for INSERT 
as
insert into [prod].[ProductCategoriesPermissionsHistory]
select 
'I',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
*
from inserted
GO

create trigger [prod].[LogProductCategoriesPermissionsUpdate] on [prod].[ProductCategoriesPermissions] for UPDATE 
as
insert into [prod].[ProductCategoriesPermissionsHistory]
select 
'U',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
*
from inserted
GO
