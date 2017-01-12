IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'DetachStatus' AND Object_ID = Object_ID(N'[dbo].[ClientForDeletionResponse]'))    
BEGIN
	ALTER TABLE [dbo].[ClientForDeletionResponse]
	ADD [DetachStatus] int NULL
END
GO