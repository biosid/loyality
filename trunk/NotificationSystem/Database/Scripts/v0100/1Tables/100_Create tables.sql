/****** Create schema ******/
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name ='mess')
	EXEC dbo.sp_executesql @statement=N'CREATE SCHEMA mess';
GO

/****** Drop all tables ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[mess].[ClientProfiles]') AND type in (N'U'))
	DROP TABLE [mess].[ClientProfiles]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[mess].[Messages]') AND type in (N'U'))
	DROP TABLE [mess].[Messages]
GO


/****** Object:  Table [mess].[ClientProfiles] ******/
CREATE TABLE [mess].[ClientProfiles](
	[ClientId] [nvarchar](64) NOT NULL,
	[TotalCount] [int] NOT NULL,
	[UnreadCount] [int] NOT NULL,
 CONSTRAINT [PK_ClientProfiles] PRIMARY KEY CLUSTERED ( [ClientId] ASC )
)
GO

/****** Object:  Table [mess].[Messages] ******/
CREATE TABLE [mess].[Messages](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ClientId] [nvarchar](64) NOT NULL,
	[Subject] [nvarchar](100) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[CreateDateTime] [datetime] NOT NULL DEFAULT (getdate()),
	[IsRead] [bit] NOT NULL DEFAULT (0),
	[ChangeStatusDateTime] [datetime] NOT NULL,
	[FromDateTime] [datetime] NULL,
	[ToDateTime] [datetime] NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ( [Id] ASC )
)
GO

CREATE NONCLUSTERED INDEX [IX_Messages.ClientId] ON [mess].[Messages] ([ClientId] ASC)
GO