/****** Object:  Table [dbo].[ClientUpdates]    Script Date: 07.10.2013 17:27:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClientUpdates](
	[SeqId] [int] IDENTITY(1,1) NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[ClientId] [nvarchar](36) NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[FirstName] [nvarchar](50) NULL,
	[MiddleName] [nvarchar](50) NULL,
	[Email] [nvarchar](255) NULL,
	[BirthDate] [date] NULL,
	[Segment] [int] NULL,
	[Gender] [int] NULL,
	[UpdStatus] [int] NULL,
 CONSTRAINT [PK_ClientUpdates] PRIMARY KEY CLUSTERED 
(
	[SeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ClientUpdates] ADD  CONSTRAINT [DF_UpdateClients_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO


