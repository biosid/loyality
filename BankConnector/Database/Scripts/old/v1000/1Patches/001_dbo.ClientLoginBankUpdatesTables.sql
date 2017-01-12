/****** Object:  Table [dbo].[ClientLoginBankUpdates]    Script Date: 17.12.2013 17:22:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClientLoginBankUpdates](
	[SeqId] [int] IDENTITY(1,1) NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[ClientId] [nvarchar](36) NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[MobilePhone] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_BankClientLoginUpdates] PRIMARY KEY CLUSTERED 
(
	[SeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ClientLoginBankUpdates] ADD  CONSTRAINT [DF_BankClientLoginUpdates_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO


/****** Object:  Table [dbo].[ClientLoginBankUpdatesResponse]    Script Date: 17.12.2013 17:22:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClientLoginBankUpdatesResponse](
	[SeqId] [int] IDENTITY(1,1) NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[ClientId] [nvarchar](50) NOT NULL,
	[Status] [int] NOT NULL,
	[Message] [nvarchar](1000) NULL,
 CONSTRAINT [PK_BankClientLoginUpdatesResponse] PRIMARY KEY CLUSTERED 
(
	[SeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO