CREATE TABLE [dbo].[ClientPersonalMessage](
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[ClientId] [nvarchar](36) NOT NULL,
	[SessionId] [uniqueidentifier] NOT NULL,
	[Message] [nvarchar](1000) not NULL,
	[FromDateTime] [datetime] NULL,
	[ToDateTime] [datetime] NULL,
	CONSTRAINT [FK_ClientPersonalMessage_EtlSessions] FOREIGN KEY([SessionId])
		REFERENCES [etl].[EtlSessions] ([EtlSessionId]),
 CONSTRAINT [PK_ClientPersonalMessage_Id_SessionId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[SessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
