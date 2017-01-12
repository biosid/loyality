IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[CategoriesByAudiences]') AND type in (N'U'))
	DROP TABLE [prod].[CategoriesByAudiences]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetDeniedCategories]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [prod].[GetDeniedCategories]
GO
