IF EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'Approved' AND Object_ID = Object_ID(N'[dbo].[PromoAction]'))    
BEGIN
	PRINT 'Удаляем столбец Approved в таблицу [dbo].[PromoAction]'
    ALTER TABLE [dbo].[PromoAction]
    DROP COLUMN [Approved]
END
GO

IF EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'ApproveDescription' AND Object_ID = Object_ID(N'[dbo].[PromoAction]'))    
BEGIN
	PRINT 'Удаляем столбец ApproveDescription в таблицу [dbo].[PromoAction]'
    ALTER TABLE [dbo].[PromoAction]
    DROP COLUMN [ApproveDescription]
END
GO