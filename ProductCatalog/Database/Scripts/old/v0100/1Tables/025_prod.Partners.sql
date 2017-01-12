/****** Object:  Table [prod].[Partners]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[Partners](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](256) NULL,
	[InsertedUserId] [nvarchar](64) NOT NULL,
	[UpdatedUserId] [nvarchar](64) NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Type] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ThrustLevel] [int] NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[CarrierId] [int] NULL,
	[IsCarrier] [bit] NOT NULL DEFAULT ((0)),
	[ReportRecipients] [nvarchar](max) NULL,
	[FixPriceSupported] [bit] NULL,
	[ImportDeliveryRatesEtlPackageId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Partners] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Trigger [prod].[LogPartnersInsert]    Script Date: 30.04.2013 18:34:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create trigger [prod].[LogPartnersInsert] on [prod].[Partners] for insert
as
insert into prod.[PartnersHistory]
select 
'I',
getdate(),
getutcdate(),
*
from INSERTED
GO
/****** Object:  Trigger [prod].[LogPartnersUpdate]    Script Date: 30.04.2013 18:34:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create trigger [prod].[LogPartnersUpdate] on [prod].[Partners] for update
as

insert into prod.[PartnersHistory]            
select 
'U',
getdate(),
getutcdate(),
*
from INSERTED
GO

create trigger [prod].[LogPartnersDelete] on [prod].[Partners] for DELETE 
as
insert into prod.[PartnersHistory]
select 
'D',
getdate(),
getutcdate(),
*
from DELETED
GO
