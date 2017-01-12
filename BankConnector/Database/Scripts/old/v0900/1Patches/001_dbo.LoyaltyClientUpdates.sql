IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LoyaltyClientUpdates]') AND type in (N'U'))
	DROP TABLE [dbo].[LoyaltyClientUpdates]
GO

CREATE TABLE [dbo].[LoyaltyClientUpdates](
	[SeqId] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [nvarchar](36) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[FirstName] [nvarchar](50) NULL,
	[MiddleName] [nvarchar](50) NULL,
	[MobilePhone] [nvarchar](20) NULL,
	[MobilePhoneId] [int] NULL,
	[Email] [nvarchar](255) NULL,
	[BirthDate] [date] NULL,
	[Segment] [int] NULL,
	[Gender] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdateProperties] [int] NOT NULL,
	[UpdateStatus] [int] NULL,
	[SendEtlSessionId] [uniqueidentifier] NULL,
	CONSTRAINT [PK_LoyaltyClientUpdates] PRIMARY KEY CLUSTERED ([SeqId] ASC)
)
GO

ALTER TABLE [dbo].[LoyaltyClientUpdates]  WITH CHECK ADD  CONSTRAINT [FK_LoyaltyClientUpdates_SendEtlSessionId] FOREIGN KEY([SendEtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO

ALTER TABLE [dbo].[LoyaltyClientUpdates] CHECK CONSTRAINT [FK_LoyaltyClientUpdates_SendEtlSessionId]
GO


