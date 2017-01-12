ALTER TABLE mess.Threads ADD
	FirstMessageType int NULL,
	LastMessageType int NULL
GO

UPDATE mess.Threads
SET 
FirstMessageType = (select top 1 [MessageType] from [mess].[ThreadMessages] where [ThreadId] = mess.Threads.Id order by [Index]),
LastMessageType = (select top 1 [MessageType] from [mess].[ThreadMessages] where [ThreadId] = mess.Threads.Id order by [Index] desc)
GO

delete from mess.Threads where FirstMessageType is null or LastMessageType is null
GO

ALTER TABLE mess.Threads alter column FirstMessageType int NOT NULL
ALTER TABLE mess.Threads alter column LastMessageType int NOT NULL
GO