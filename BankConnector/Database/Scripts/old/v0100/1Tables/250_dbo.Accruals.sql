CREATE TABLE [dbo].[Accruals](
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[ReceiveEtlSessionId] [uniqueidentifier] NOT NULL,
	[SendEtlSessionId] [uniqueidentifier] NULL,
	[ClientId] [nvarchar](36) NOT NULL,	
	[BonusSum] [nvarchar](18) NOT NULL,
	[Type] [int] NOT NULL,	
	[Description] [nvarchar](500) NOT NULL,	
	[Status] [int] NULL,
	[BonusOperationDateTime] DATETIME2 NULL,
	[ExternalId] [nvarchar](36) NULL,
 CONSTRAINT [PK_Accruals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
GO
ALTER TABLE [dbo].[Accruals]  WITH CHECK ADD  CONSTRAINT [FK_Accruals_ReceiveEtlSession] FOREIGN KEY([ReceiveEtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO
ALTER TABLE [dbo].[Accruals]  WITH CHECK ADD  CONSTRAINT [FK_Accruals_SendEtlSession] FOREIGN KEY([SendEtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO