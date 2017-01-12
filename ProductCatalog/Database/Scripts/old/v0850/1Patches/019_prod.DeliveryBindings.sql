IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[DeliveryBindings]') AND type in (N'U'))
	DROP TABLE [prod].[DeliveryBindings]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[DeliveryLocations]') AND type in (N'U'))
	DROP TABLE [prod].[DeliveryLocations]
GO

CREATE TABLE [prod].[DeliveryLocations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartnerId] [int] NOT NULL,
	[LocationName] [nvarchar](max) NOT NULL,
	[Kladr] [nvarchar](13) NULL,
	[Status] [int] NOT NULL,
	[InsertDateTime] [datetime] NOT NULL,
	[UpdateDateTime] [datetime] NULL,
	[UpdateUserId] [nvarchar](64) NULL,
	[EtlSessionId] [nvarchar](64) NULL,
	CONSTRAINT [PK_DeliveryLocations] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[DeliveryBindingsHistory]') AND type in (N'U'))
	DROP TABLE [prod].[DeliveryBindingsHistory]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[DeliveryLocationsHistory]') AND type in (N'U'))
	DROP TABLE [prod].[DeliveryLocationsHistory]
GO


CREATE TABLE [prod].[DeliveryLocationsHistory](
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
	[UpdateUserId] [nvarchar](64) NULL,
	[EtlSessionId] [nvarchar](64) NULL,
	CONSTRAINT [PK_DeliveryLocationsHistory] PRIMARY KEY CLUSTERED ([TriggerDate] ASC,[Id] ASC)
)
GO

CREATE TRIGGER [prod].[DeliveryLocationsInsert] 
	ON [prod].[DeliveryLocations] FOR INSERT
AS
INSERT INTO [prod].[DeliveryLocationsHistory]
SELECT	'I',
		getdate(),
		getutcdate(),
		*
FROM INSERTED
GO

CREATE TRIGGER [prod].[DeliveryLocationsUpdate] 
	ON [prod].[DeliveryLocations] FOR UPDATE
AS
INSERT INTO [prod].[DeliveryLocationsHistory]         
SELECT	'U',
		getdate(),
		getutcdate(),
		*
from INSERTED
