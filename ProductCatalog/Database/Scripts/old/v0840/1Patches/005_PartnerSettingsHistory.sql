CREATE TABLE [prod].[PartnerSettingsHistory](
	[Action] [char](1) NOT NULL,
	[TriggerDate] [datetime] NOT NULL,
	[TriggerUtcDate] [datetime] NOT NULL,
	[Id] [int] NULL,
	[PartnerId] [int] NULL,
	[Key] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](1000) NOT NULL,
) ON [PRIMARY]

GO

CREATE trigger [prod].[LogPartnerSettingsInsert] on [prod].PartnerSettings for insert
as
insert into prod.[PartnerSettingsHistory]
select 
'I',
getdate(),
getutcdate(),
*
from INSERTED
GO

CREATE trigger [prod].[LogPartnerSettingsUpdate] on [prod].PartnerSettings for update
as

insert into prod.[PartnerSettingsHistory]            
select 
'U',
getdate(),
getutcdate(),
*
from INSERTED
GO