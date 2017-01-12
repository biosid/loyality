IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ProfileCustomFields]') AND name = N'IX_ProfileCustomFields')
DROP INDEX [IX_ProfileCustomFields] ON [dbo].[ProfileCustomFields] WITH ( ONLINE = OFF )
GO

UPDATE dbo.ProfileCustomFields
SET Name = LEFT(Name, 128)
GO

ALTER TABLE dbo.ProfileCustomFields
ALTER COLUMN Name nvarchar(128) not null
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_ProfileCustomFields] ON [dbo].[ProfileCustomFields] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
