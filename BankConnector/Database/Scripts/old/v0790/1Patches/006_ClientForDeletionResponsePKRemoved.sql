ALTER TABLE dbo.ClientForDeletionResponse DROP CONSTRAINT PK_ClientForDeletionResponse
GO

CREATE NONCLUSTERED INDEX IX_ClientForDeletionResponse ON dbo.ClientForDeletionResponse
	(
	ClientId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO