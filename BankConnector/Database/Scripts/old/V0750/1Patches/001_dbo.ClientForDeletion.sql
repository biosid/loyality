ALTER TABLE [dbo].[ClientForDeletion]
ADD	[MobilePhone] [nvarchar](20) NOT NULL DEFAULT ''
GO
ALTER TABLE [dbo].[ClientForDeletion]
ALTER COLUMN [MobilePhone] [nvarchar](20) NOT NULL
GO