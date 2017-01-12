BEGIN TRANSACTION
GO
CREATE TABLE prod.Tmp_ProductTargetAudiences
	(
	Id int NOT NULL IDENTITY (1, 1),
	ProductId nvarchar(256) NOT NULL,
	TargetAudienceId nvarchar(256) NOT NULL,
	InsertedUserId nvarchar(50) NOT NULL,
	InsertedDate datetime2(7) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE prod.Tmp_ProductTargetAudiences SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT prod.Tmp_ProductTargetAudiences OFF
GO
IF EXISTS(SELECT * FROM prod.ProductTargetAudiences)
	 EXEC('INSERT INTO prod.Tmp_ProductTargetAudiences (ProductId, TargetAudienceId, InsertedUserId, InsertedDate)
		SELECT ProductId, TargetAudienceId, InsertedUserId, InsertedDate FROM prod.ProductTargetAudiences WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE prod.ProductTargetAudiences
GO
EXECUTE sp_rename N'prod.Tmp_ProductTargetAudiences', N'ProductTargetAudiences', 'OBJECT' 
GO
ALTER TABLE prod.ProductTargetAudiences ADD CONSTRAINT
	PK_ProductTargetAudiences PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

COMMIT
