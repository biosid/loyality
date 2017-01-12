IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ClientForBankRegistrationResponse]') AND name = 'ClientId')
BEGIN
	ALTER TABLE [dbo].[ClientForBankRegistrationResponse]
	ADD [ClientId] [nvarchar](36) NULL
END
ELSE
BEGIN
	ALTER TABLE [dbo].[ClientForBankRegistrationResponse]
	ALTER COLUMN [ClientId] [nvarchar](36) NULL
END
GO
