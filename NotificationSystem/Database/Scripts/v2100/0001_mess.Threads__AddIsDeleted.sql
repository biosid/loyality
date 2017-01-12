ALTER TABLE mess.Threads ADD
      IsDeleted bit NOT NULL CONSTRAINT DF_Threads_IsDeleted DEFAULT 0
GO
