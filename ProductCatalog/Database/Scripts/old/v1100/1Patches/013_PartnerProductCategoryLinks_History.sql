CREATE TABLE [prod].[PartnerProductCategoryLinksHistory](
	[Action] [char](1) NOT NULL,
	[TriggerUserId] [nvarchar](255) NULL,
	[TriggerDate] [datetime] NOT NULL,
	[TriggerUtcDate] [datetime] NOT NULL,	
	[Id] [int] NOT NULL,
	[ProductCategoryId] [int] NOT NULL,
	[PartnerId] [int] NOT NULL,
	[NamePath] [nvarchar](max) NOT NULL,
	[IncludeSubcategories] [bit] NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[InsertedUserId] [nvarchar](256) NOT NULL,
) ON [PRIMARY]

GO

create trigger [prod].[LogPartnerProductCategoryLinksDelete] on [prod].[PartnerProductCategoryLinks] for DELETE 
as
insert into [prod].[PartnerProductCategoryLinksHistory]
select 
'D',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
*
from DELETED
GO

create trigger [prod].[LogPartnerProductCategoryLinksInsert] on [prod].[PartnerProductCategoryLinks] for INSERT 
as
insert into [prod].[PartnerProductCategoryLinksHistory]
select 
'I',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
*
from inserted
GO

create trigger [prod].[LogPartnerProductCategoryLinksUpdate] on [prod].[PartnerProductCategoryLinks] for UPDATE 
as
insert into [prod].[PartnerProductCategoryLinksHistory]
select 
'U',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
*
from inserted
GO
