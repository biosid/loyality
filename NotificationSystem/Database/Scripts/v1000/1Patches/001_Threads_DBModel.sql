/****** Object:  Table [mess].[Threads]    Script Date: 12.12.2013 19:56:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [mess].[Threads](
	[Id] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[ClientType] [int] NOT NULL,
	[IsClosed] [bit] NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[ClientId] [nvarchar](64) NULL,
	[ClientFullName] [nvarchar](255) NULL,
	[ClientEmail] [nvarchar](255) NULL,
	[IsAnswered] [bit] NOT NULL,
	[MessagesCount] [int] NOT NULL,
	[UnreadMessagesCount] [int] NOT NULL,
	[FirstMessageTime] [datetime] NOT NULL,
	[LastMessageTime] [datetime] NOT NULL,
	[FirstMessageBy] [nvarchar](500) NULL,
	[LastMessageBy] [nvarchar](500) NULL,
	[ShowSince] [datetime] NULL,
	[ShowUntil] [datetime] NULL,
 CONSTRAINT [PK_Threads] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [mess].[Threads] ADD  CONSTRAINT [DF_Threads_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO


/****** Object:  Table [mess].[ThreadMessages]    Script Date: 12.12.2013 19:56:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [mess].[ThreadMessages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ThreadId] [uniqueidentifier] NOT NULL,
	[MessageBody] [nvarchar](max) NOT NULL,
	[Index] [int] NOT NULL,
	[IsUnread] [bit] NOT NULL,
	[MessageType] [int] NOT NULL,
	[AuthorFullName] [nvarchar](255) NULL,
	[AuthorId] [nvarchar](255) NULL,
	[AuthorEmail] [nvarchar](255) NULL,
	[InsertedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ThreadMessages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [mess].[ThreadMessages] ADD  CONSTRAINT [DF_ThreadMessages_IsUnread]  DEFAULT ((1)) FOR [IsUnread]
GO

ALTER TABLE [mess].[ThreadMessages] ADD  CONSTRAINT [DF_ThreadMessages_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO

ALTER TABLE [mess].[ThreadMessages]  WITH CHECK ADD  CONSTRAINT [FK_ThreadMessages_Threads] FOREIGN KEY([ThreadId])
REFERENCES [mess].[Threads] ([Id])
GO

ALTER TABLE [mess].[ThreadMessages] CHECK CONSTRAINT [FK_ThreadMessages_Threads]
GO


/****** Object:  Table [mess].[Attachments]    Script Date: 12.12.2013 19:56:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [mess].[Attachments](
	[Id] [uniqueidentifier] NOT NULL,
	[ThreadId] [uniqueidentifier] NOT NULL,
	[MessageId] [int] NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[FileSize] [int] NOT NULL,
 CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [mess].[Attachments] ADD  CONSTRAINT [DF_Attachments_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [mess].[Attachments]  WITH CHECK ADD  CONSTRAINT [FK_Attachments_ThreadMessages] FOREIGN KEY([MessageId])
REFERENCES [mess].[ThreadMessages] ([Id])
GO

ALTER TABLE [mess].[Attachments] CHECK CONSTRAINT [FK_Attachments_ThreadMessages]
GO

ALTER TABLE [mess].[Attachments]  WITH CHECK ADD  CONSTRAINT [FK_Attachments_Threads] FOREIGN KEY([ThreadId])
REFERENCES [mess].[Threads] ([Id])
GO

ALTER TABLE [mess].[Attachments] CHECK CONSTRAINT [FK_Attachments_Threads]
GO


