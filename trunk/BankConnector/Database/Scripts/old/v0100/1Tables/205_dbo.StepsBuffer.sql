CREATE TABLE [dbo].[StepsBuffer](
	[Id] [int] NOT NULL IDENTITY,
	[SessionId] [uniqueidentifier] NOT NULL,
	[nvarchar1_256] [nvarchar](256) NULL,
	[nvarchar2_256] [nvarchar](256) NULL,
	[tinyint1] [tinyint] NULL,
	[bit1] [bit] NULL,
 CONSTRAINT [PK_StepsBuffer_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[StepsBuffer]  WITH CHECK ADD  CONSTRAINT [FK_StepsBuffer_EtlSessions] FOREIGN KEY([SessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO

ALTER TABLE [dbo].[StepsBuffer] CHECK CONSTRAINT [FK_StepsBuffer_EtlSessions]
GO