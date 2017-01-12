IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ClientForBankRegistration]') AND name = 'ErrorStatus')
BEGIN
	ALTER TABLE [dbo].[ClientForBankRegistration]
	ADD [ErrorStatus] [int] NULL
END
GO
