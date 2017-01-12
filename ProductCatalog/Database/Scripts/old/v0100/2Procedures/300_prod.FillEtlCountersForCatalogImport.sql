IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[FillEtlCountersForCatalogImport]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[FillEtlCountersForCatalogImport]
GO

CREATE PROCEDURE [prod].[FillEtlCountersForCatalogImport]
	@etlPackageId [nvarchar](64), 
	@etlSessionId [nvarchar](64),
	@key [nvarchar](50)
AS
BEGIN
	DECLARE @sql [nvarchar](max) = N'
;WITH ImportError ([Code], [Desc]) AS 
(
	SELECT 0, ''Партнерская категория не найдена''
	UNION
	SELECT 1, ''Найдено несколько маппингов для партнерской категории''
	UNION
	SELECT 2, ''Маппинг не найден''
	UNION
	SELECT 3, ''В задаче импорта указано что вес обязателен, но в YML-файле вес не указан''
)
INSERT INTO [dbo].[EtlCounters]
SELECT	@etlPackageId AS [EtlPackageId],
		@etlSessionId AS [EtlSessionId],
		''Ошибка'' AS [EntityName],
		ISNULL(ie.[Desc] + ''. Категория: '' + ISNULL(pe.[PartnerCategoryId], ''-'') + ISNULL('' ('' + ppc.[NamePath] +'')'', ''''), ''Не известная ошибка'') AS [CounterName],
		COUNT(pe.[PartnerProductId]) AS [CounterValue],
		GetDate() AS LogDateTime,
		GetUtcDate() AS LogUtcDateTime
FROM ImportError ie
RIGHT JOIN [prod].[ProductsErrors_{0}] pe
		ON pe.[Code] = ie.[Code]
LEFT JOIN [prod].[PartnerProductCaterories_{0}] ppc
		ON pe.[PartnerCategoryId] = ppc.[Id]
GROUP BY ISNULL(ie.[Desc] + ''. Категория: '' + ISNULL(pe.[PartnerCategoryId], ''-'') + ISNULL('' ('' + ppc.[NamePath] +'')'', ''''), ''Не известная ошибка'') 
'

	DECLARE @sqlMapping [nvarchar](max) = REPLACE(@sql, '{0}', @key)
	
	--SELECT @sqlMapping
	DECLARE @parmDefinition nvarchar(500)
	
	SET @ParmDefinition = N'@etlPackageId nvarchar(64), @etlSessionId nvarchar(64)'
	EXECUTE sp_executesql @sqlMapping, @ParmDefinition, @etlPackageId, @etlSessionId
END

GO


