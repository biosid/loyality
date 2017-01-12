IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'ClientId' AND Object_ID = Object_ID(N'[prod].[WishListItems]'))    
BEGIN
	PRINT 'Добавляем столбец ClientId таблицы [prod].[WishListItems]'
	ALTER TABLE [prod].[WishListItems]
	ADD [ClientId] [nvarchar](64) NULL
END
GO

IF EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'UserId' AND Object_ID = Object_ID(N'[prod].[WishListItems]')) 
BEGIN
	UPDATE [prod].[WishListItems]
	SET [ClientId] = [UserId]
	WHERE [ClientId] IS NULL
	
	IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[WishListItems]') AND name = N'PK_WishListItems')
	BEGIN
		ALTER TABLE [prod].[WishListItems] DROP CONSTRAINT [PK_WishListItems]
	END
	
	ALTER TABLE [prod].[WishListItems]
	DROP COLUMN [UserId]
END 
GO

ALTER TABLE [prod].[WishListItems]
ALTER COLUMN [ClientId] [nvarchar](64) NOT NULL
GO

IF  NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[WishListItems]') AND name = N'PK_WishListItems')
BEGIN
	ALTER TABLE [prod].[WishListItems] ADD  CONSTRAINT [PK_WishListItems] PRIMARY KEY CLUSTERED 
	(
		[ClientId] ASC,
		[ProductId] ASC
	)
END
GO