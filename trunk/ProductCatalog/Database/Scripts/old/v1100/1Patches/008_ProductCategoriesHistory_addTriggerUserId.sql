BEGIN TRANSACTION
GO
CREATE TABLE prod.Tmp_ProductCategoriesHistory
	(
	Action char(1) NOT NULL,
	TriggerUserId nvarchar(256) NULL,
	TriggerDate datetime NOT NULL,
	TriggerUtcDate datetime NOT NULL,
	Id int NOT NULL,
	Type int NOT NULL,
	ParentId int NULL,
	Name nvarchar(256) NOT NULL,
	NamePath nvarchar(MAX) NOT NULL,
	Status int NOT NULL,
	InsertedUserId nvarchar(50) NOT NULL,
	UpdatedUserId nvarchar(50) NULL,
	OnlineCategoryUrl nvarchar(1000) NULL,
	InsertedDate datetime NOT NULL,
	UpdatedDate datetime NULL,
	CatOrder int NOT NULL,
	NotifyOrderStatusUrl nvarchar(1000) NULL,
	OnlineCategoryPartnerId int NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE prod.Tmp_ProductCategoriesHistory SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM prod.ProductCategoriesHistory)
	 EXEC('INSERT INTO prod.Tmp_ProductCategoriesHistory (Action, TriggerDate, TriggerUtcDate, Id, Type, ParentId, Name, NamePath, Status, InsertedUserId, UpdatedUserId, OnlineCategoryUrl, InsertedDate, UpdatedDate, CatOrder, NotifyOrderStatusUrl, OnlineCategoryPartnerId)
		SELECT Action, TriggerDate, TriggerUtcDate, Id, Type, ParentId, Name, NamePath, Status, InsertedUserId, UpdatedUserId, OnlineCategoryUrl, InsertedDate, UpdatedDate, CatOrder, NotifyOrderStatusUrl, OnlineCategoryPartnerId FROM prod.ProductCategoriesHistory WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE prod.ProductCategoriesHistory
GO
EXECUTE sp_rename N'prod.Tmp_ProductCategoriesHistory', N'ProductCategoriesHistory', 'OBJECT' 
GO
COMMIT
