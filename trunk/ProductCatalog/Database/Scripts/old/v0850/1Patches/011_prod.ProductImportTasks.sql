IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[prod].[ProductImportTasks]') AND name = 'ParamsProcessType')
BEGIN
	ALTER TABLE [prod].[ProductImportTasks]
	ADD [ParamsProcessType] [int] NOT NULL DEFAULT(0)
END
GO