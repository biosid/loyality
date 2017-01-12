IF EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'Approved' AND Object_ID = Object_ID(N'[dbo].[PromoActionResponse]'))    
BEGIN
	PRINT 'Удаляем столбец Approved в таблицу [dbo].[PromoActionResponse]'
    ALTER TABLE [dbo].[PromoActionResponse]
    DROP COLUMN [Approved]
END
GO

IF EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'ApproveDescription' AND Object_ID = Object_ID(N'[dbo].[PromoActionResponse]'))    
BEGIN
	PRINT 'Удаляем столбец ApproveDescription в таблицу [dbo].[PromoActionResponse]'
    ALTER TABLE [dbo].[PromoActionResponse]
    DROP COLUMN [ApproveDescription]
END
GO

