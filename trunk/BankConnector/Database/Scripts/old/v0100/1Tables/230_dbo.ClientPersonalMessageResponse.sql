CREATE TABLE [dbo].[ClientPersonalMessageResponse](
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[ClientId] [nvarchar](36) NOT NULL,
	[SessionId] [uniqueidentifier] NOT NULL,
	[Status] [int] not NULL,	
	CONSTRAINT [FK_ClientPersonalMessageResponse_EtlSessions] FOREIGN KEY([SessionId])
		REFERENCES [etl].[EtlSessions] ([EtlSessionId]),
 CONSTRAINT [PK_ClientPersonalMessageResponse_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC	
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
