CREATE TABLE [prod].[ProductTargetAudiencesHistory](
	[Action] [char](1) NOT NULL,
	[TriggerUserId] [nvarchar](255) NULL,
	[TriggerDate] [datetime] NOT NULL,
	[TriggerUtcDate] [datetime] NOT NULL,
	Id int NOT NULL,
	[ProductId] [nvarchar](256) NOT NULL,
	[TargetAudienceId] [nvarchar](256) NOT NULL,
	[InsertedUserId] [nvarchar](50) NOT NULL,
	[InsertedDate] [datetime2](7) NOT NULL
) ON [PRIMARY]

GO

create trigger [prod].[LogProductTargetAudiencesDelete] on [prod].[ProductTargetAudiences] for DELETE 
as
insert into [prod].[ProductTargetAudiencesHistory]
select 
'D',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
*
from DELETED
GO

create trigger [prod].[LogProductTargetAudiencesInsert] on [prod].[ProductTargetAudiences] for INSERT 
as
insert into [prod].[ProductTargetAudiencesHistory]
select 
'I',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
*
from inserted
GO

create trigger [prod].[LogProductTargetAudiencesUpdate] on [prod].[ProductTargetAudiences] for UPDATE 
as
insert into [prod].[ProductTargetAudiencesHistory]
select 
'U',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
*
from inserted
GO
