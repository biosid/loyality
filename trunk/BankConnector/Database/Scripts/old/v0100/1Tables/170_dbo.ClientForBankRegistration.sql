SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClientForBankRegistration](
	[Id] int not null identity,
	[MobilePhone] [nvarchar](20) NOT NULL,
	[ClientId] [nvarchar](36) NULL,
	[Email] [nvarchar](255) NULL,
	[Segment] [int] NULL,
	[SessionId] [uniqueidentifier] NULL,
	CONSTRAINT [FK_ClientForBankRegistration_EtlSessionsRequest] FOREIGN KEY([SessionId])
		REFERENCES [etl].[EtlSessions] ([EtlSessionId]),
	CONSTRAINT [PK_ClientForBankRegistration_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


