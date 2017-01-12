IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[DeliveryBindings]') AND type in (N'U'))
	DROP TABLE [prod].[DeliveryBindings]
GO

CREATE TABLE [prod].[DeliveryBindings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartnerId] [int] NOT NULL,
	[LocationName] [nvarchar](max) NOT NULL,
	[Kladr] [nvarchar](13) NULL,
	[Status] [int] NOT NULL,
	[InsertDateTime] [datetime] NOT NULL,
	[UpdateDateTime] [datetime] NULL,
	[UpdateUserId] [nvarchar](64) NULL,
	CONSTRAINT [PK_DeliveryBindings] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE NONCLUSTERED INDEX [idx_prod_DeliveryBindings_PartnerId] 
	ON [prod].[DeliveryBindings] ([PartnerId] ASC)
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[DeliveryBindingsHistory]') AND type in (N'U'))
	DROP TABLE [prod].[DeliveryBindingsHistory]
GO

CREATE TABLE [prod].[DeliveryBindingsHistory](
	[Action] [char](1) NOT NULL,
	[TriggerDate] [datetime] NOT NULL,
	[TriggerUtcDate] [datetime] NOT NULL,
	[Id] [int] NOT NULL,
	[PartnerId] [int] NOT NULL,
	[LocationName] [nvarchar](max) NOT NULL,
	[Kladr] [nvarchar](13) NULL,
	[Status] [int] NOT NULL,
	[InsertDateTime] [datetime] NOT NULL,
	[UpdateDateTime] [datetime] NULL,
	[UpdateUserId] [nvarchar](64) NULL
)
GO

CREATE TRIGGER [prod].[DeliveryBindingsInsert] 
	ON [prod].[DeliveryBindings] FOR INSERT
AS
INSERT INTO [prod].[DeliveryBindingsHistory]
SELECT	'I',
		getdate(),
		getutcdate(),
		*
FROM INSERTED
GO

CREATE TRIGGER [prod].[DeliveryBindingsUpdate] 
	ON [prod].[DeliveryBindings] FOR UPDATE
AS
INSERT INTO [PROD].[DELIVERYBINDINGSHISTORY]           
SELECT	'U',
		getdate(),
		getutcdate(),
		*
from INSERTED
GO
