/****** Object:  Table [prod].[ProductCategories]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[ProductCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[ParentId] [int] NULL,
	[Name] [nvarchar](256) NOT NULL,
	[NamePath] [nvarchar](max) NOT NULL,
	[Status] [int] NOT NULL,
	[InsertedUserId] [nvarchar](50) NOT NULL,
	[UpdatedUserId] [nvarchar](50) NULL,
	[OnlineCategoryUrl] [nvarchar](1000) NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[CatOrder] [int] NOT NULL,
	[NotifyOrderStatusUrl] [nvarchar](1000) NULL,
	[OnlineCategoryPartnerId] [int] null
 CONSTRAINT [PK_ProductCategories_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


/****** Object:  Trigger [prod].[LogProductCategoriesInsert]    Script Date: 30.04.2013 18:34:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE trigger [prod].[LogProductCategoriesInsert] on [prod].[ProductCategories]
for insert
as
	insert into [prod].[ProductCategoriesHistory]
	select	'I',
			getdate(),
			getutcdate(),
			*
	from INSERTED
GO
/****** Object:  Trigger [prod].[LogProductCategoriesUpdate]    Script Date: 30.04.2013 18:34:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE trigger [prod].[LogProductCategoriesUpdate] on [prod].[ProductCategories]
for update
as
	insert into [prod].[ProductCategoriesHistory]            
	select	'U',
			getdate(),
			getutcdate(),
			*
	from INSERTED
GO

create trigger [prod].[LogProductCategoriesDelete] on [prod].[ProductCategories] for DELETE 
as
insert into [prod].[ProductCategoriesHistory]
select 
'D',
getdate(),
getutcdate(),
*
from DELETED
GO
