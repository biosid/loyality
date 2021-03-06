IF NOT EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'ExternalStatusId' AND Object_ID = Object_ID(N'[promo].[Rules]'))    
BEGIN
	PRINT 'Добавляем столбец ExternalStatusId  в таблицу [promo].[Rules]'
    ALTER TABLE [promo].[Rules]
    ADD 
	[ExternalStatusId] [nvarchar](50) NULL	
END
GO

IF NOT EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'Approved' AND Object_ID = Object_ID(N'[promo].[Rules]'))    
BEGIN
	PRINT 'Добавляем столбец Approved в таблицу [promo].[Rules]'
    ALTER TABLE [promo].[Rules]
    ADD [Approved] [bit] NOT NULL default(0)	
END
GO

IF NOT EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'ApproveDescription' AND Object_ID = Object_ID(N'[promo].[Rules]'))    
BEGIN
	PRINT 'Добавляем столбец ApproveDescription в таблицу [promo].[Rules]'
    ALTER TABLE [promo].[Rules]
    ADD [ApproveDescription] [nvarchar](255) NULL
END
GO

IF NOT EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'ExternalStatusId' AND Object_ID = Object_ID(N'[promo].[RuleHistories]'))    
BEGIN
	PRINT 'Добавляем столбец ExternalStatusId в таблицу [promo].[RuleHistories]'
    ALTER TABLE [promo].[RuleHistories]
    ADD 
	[ExternalStatusId] [nvarchar](50) NULL	
END
GO


IF NOT EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'Approved' AND Object_ID = Object_ID(N'[promo].[RuleHistories]'))    
BEGIN
	PRINT 'Добавляем столбец Approved в таблицу [promo].[RuleHistories]'
    ALTER TABLE [promo].[RuleHistories]
    ADD [Approved] [bit] NOT NULL default(0)	
END
GO

IF NOT EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'ApproveDescription' AND Object_ID = Object_ID(N'[promo].[RuleHistories]'))    
BEGIN
	PRINT 'Добавляем столбец ApproveDescription в таблицу [promo].[RuleHistories]'
    ALTER TABLE [promo].[RuleHistories]
    ADD [ApproveDescription] [nvarchar](255) NULL
END
