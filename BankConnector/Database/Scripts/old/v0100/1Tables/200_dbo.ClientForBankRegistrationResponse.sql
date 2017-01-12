CREATE TABLE [dbo].[ClientForBankRegistrationResponse](
	[Id] int not null identity,
	[ClientId] [nvarchar](36) NOT NULL,
	[SessionId] [uniqueidentifier] NOT NULL,
	[Login] [nvarchar](36) not NULL,
	[Status] [int] not NULL,
	CONSTRAINT [FK_ClientForBankRegistrationResponse_EtlSessions] FOREIGN KEY([SessionId])
		REFERENCES [etl].[EtlSessions] ([EtlSessionId]),
 CONSTRAINT [PK_ClientForBankRegistrationResponse_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
