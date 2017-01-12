IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'DeletionEtlSessionId' AND Object_ID = Object_ID(N'[dbo].[ClientForActivation]'))    
BEGIN
	ALTER TABLE [dbo].[ClientForActivation]
	ADD [DeletionEtlSessionId] [uniqueidentifier] NULL
END
GO
