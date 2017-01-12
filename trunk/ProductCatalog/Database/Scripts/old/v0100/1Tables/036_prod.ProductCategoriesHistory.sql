
CREATE TABLE [prod].[ProductCategoriesHistory](
	[Action] [char](1) NOT NULL,
	[TriggerDate] [datetime] NOT NULL,
	[TriggerUtcDate] [datetime] NOT NULL,
	[Id] [int] NOT NULL,
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
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO