IF EXISTS (SELECT * FROM sysobjects WHERE id=OBJECT_ID('mess.[MessagesBUF]')) 
begin
		DROP TABLE [mess].[MessagesBUF]
end

create table [mess].[MessagesBUF] (
	ThreadId uniqueidentifier NOT NULL,
	[Id] [bigint]  NOT NULL,
	[ClientId] [nvarchar](64) NOT NULL,
	[Subject] [nvarchar](100) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[IsRead] [bit] NOT NULL,
	[ChangeStatusDateTime] [datetime] NOT NULL,
	[FromDateTime] [datetime] NULL,
	[ToDateTime] [datetime] NULL, 
) 
GO

INSERT INTO [mess].[MessagesBUF]
select 
	newid() as ThreadId
	, *
from
[mess].[Messages] m
GO

declare @nowdate datetime
set @nowdate = getdate()

insert into [mess].[Threads] 
(
		[Id]
		,[Type]
		,[Title]
		,[ClientType]
		,[IsClosed]
		,[InsertedDate]
		,[ClientId]
		,[ClientFullName]
		,[ClientEmail]
		,[IsAnswered]
		,[MessagesCount]
		,[UnreadMessagesCount]
		,[FirstMessageTime]
		,[LastMessageTime]
		,[FirstMessageBy]
		,[LastMessageBy]
		,[ShowSince]
		,[ShowUntil]
		,[FirstMessageType]
		,[LastMessageType]
)
select 
	ThreadId,
	2,
	[Subject],
	1,
	1,
	[CreateDateTime],
	[ClientId],
	null,
	null,
	1,
	1,
	case when [IsRead] = 1 then 0 else 1 end,
	[CreateDateTime],
	[CreateDateTime],
	null,
	null,
	[FromDateTime],
	[ToDateTime],
	3,
	3
from 
	[mess].[MessagesBUF]
GO

INSERT INTO [mess].[ThreadMessages]
(
	[ThreadId]
	,[MessageBody]
	,[Index]
	,[IsUnread]
	,[MessageType]
	,[AuthorFullName]
	,[AuthorId]
	,[AuthorEmail]
	,[InsertedDate]
)
select 
	ThreadId,
	REPLACE ([Text] , '<br/>' , '' ),
	0,
	case when [IsRead] = 1 then 0 else 1 end,
	3,
	null,
	null,
	null,
	[CreateDateTime]
from 
	[mess].[MessagesBUF]