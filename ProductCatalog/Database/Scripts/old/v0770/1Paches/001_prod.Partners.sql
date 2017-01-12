-- NOTE: Удаляем столбцы FixPriceSupported, CheckOrderSupported, ReportRecipients - теперь это доп.настройки
IF EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'FixPriceSupported' AND Object_ID = Object_ID(N'[prod].[Partners]'))    
BEGIN
	ALTER TABLE [prod].[Partners]
	DROP COLUMN [FixPriceSupported]
	ALTER TABLE [prod].[PartnersHistory]
	DROP COLUMN [FixPriceSupported]
END
GO

IF EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'CheckOrderSupported' AND Object_ID = Object_ID(N'[prod].[Partners]'))    
BEGIN
	ALTER TABLE [prod].[Partners]
	DROP COLUMN [CheckOrderSupported]
	ALTER TABLE [prod].[PartnersHistory]
	DROP COLUMN [CheckOrderSupported]
END
GO

IF EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'ReportRecipients' AND Object_ID = Object_ID(N'[prod].[Partners]'))    
BEGIN
	ALTER TABLE [prod].[Partners]
	DROP COLUMN [ReportRecipients]
	ALTER TABLE [prod].[PartnersHistory]
	DROP COLUMN [ReportRecipients]
END
GO