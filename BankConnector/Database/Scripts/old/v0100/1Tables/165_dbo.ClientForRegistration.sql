/****** Object:  Table [dbo].[ClientForRegistration]    Script Date: 05/17/2013 12:58:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClientForRegistration](
	[ClientId] [nvarchar](36) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[MobilePhone] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[RequestSessionId] [uniqueidentifier] NULL,
	[ResponseSessionId] [uniqueidentifier] NULL,
	[Status] [int] NULL,
	[Segment] [int] NULL,
	[Gender] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT 0,
	[ProfileClientId] [nvarchar](36) NULL,
	[BirthDate] [date] NOT NULL,
	CONSTRAINT [AK_ClientForRegistration_MobilePhone] UNIQUE NONCLUSTERED ([MobilePhone] ASC),
	CONSTRAINT [FK_ClientForRegistration_EtlSessionsRequest] FOREIGN KEY([RequestSessionId])
		REFERENCES [etl].[EtlSessions] ([EtlSessionId]),
	CONSTRAINT [FK_ClientForRegistration_EtlSessionsResponse] FOREIGN KEY([ResponseSessionId])
		REFERENCES [etl].[EtlSessions] ([EtlSessionId]),
 CONSTRAINT [PK_ClientForRegistration_ClientId] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


