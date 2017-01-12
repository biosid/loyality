CREATE TABLE [dbo].[ClientForRegistrationResponse](
	[ClientId] [nvarchar](36) NOT NULL,
	[SessionId] [uniqueidentifier] NOT NULL,
	[Status] [int] NULL,
	[Segment] [int] NULL,
	CONSTRAINT [FK_ClientForRegistrationResponse_EtlSessions] FOREIGN KEY([SessionId])
		REFERENCES [etl].[EtlSessions] ([EtlSessionId]),
 CONSTRAINT [PK_ClientForRegistrationResponse_ClientId_SessionId] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC,
	[SessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
