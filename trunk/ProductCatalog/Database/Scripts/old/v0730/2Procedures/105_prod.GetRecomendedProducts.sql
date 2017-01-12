IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetRecomendedProducts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [prod].[GetRecomendedProducts]