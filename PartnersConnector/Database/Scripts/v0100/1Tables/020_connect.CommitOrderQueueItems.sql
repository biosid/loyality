IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[connect].[CommitOrderQueueItems]') AND type in (N'U'))
	DROP TABLE [connect].[CommitOrderQueueItems]
GO

/****** Object:  Table [connect].[CommitOrderQueueItems]    Script Date: 03/04/2013 ******/
CREATE TABLE [connect].[CommitOrderQueueItems]
(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PartnerId] [int] NOT NULL,
	[Order] [xml] NOT NULL,
	[ClientId] [nvarchar](255) NOT NULL,
	[InsertedDate] [datetime] DEFAULT (GETDATE()) NOT NULL,
	[ProcessStatus] [int] DEFAULT (0) NOT NULL,
	[ProcessStatusDescription] [nvarchar](max) NULL,
	CONSTRAINT [PK_mech.Rules] PRIMARY KEY CLUSTERED ([Id] ASC),
)
GO

CREATE NONCLUSTERED INDEX [IX_CommitOrderQueueItems.PartnerId] ON [connect].[CommitOrderQueueItems] ([PartnerId] ASC, [ProcessStatus] ASC)
GO
