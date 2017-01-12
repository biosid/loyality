

ALTER PROCEDURE [prod].[GetCategoriesMapping]
	@partnerId int,
	@key nvarchar(50)
AS
BEGIN
    DECLARE @sql [nvarchar](max) = N'
;WITH mapping 
(ProductCategoryId, Permission, PartnerCategoryId, PartnerCategoryName, PartnerCategoryNamePath, MappingNamePath)
AS
(
SELECT mapping.ProductCategoryId,
	   CAST(CASE WHEN pcp.CategoryId IS NULL THEN 0 ELSE 1 END AS [bit]),
	   partnerCat.[Id],
	   partnerCat.[Name],
	   partnerCat.[NamePath],
	   mapping.[NamePath]
FROM [prod].[PartnerProductCaterories_{0}] partnerCat --Новая таблица со всеми категориями партнера.
LEFT JOIN [prod].[PartnerProductCategoryLinks] mapping 
	ON (mapping.IncludeSubcategories = 1 AND partnerCat.[NamePath] LIKE mapping.NamePath + ''%'' AND mapping.[PartnerId] = @partnerId)
		OR
	   (mapping.IncludeSubcategories = 0 AND mapping.NamePath = partnerCat.[NamePath] AND mapping.[PartnerId] = @partnerId)
LEFT JOIN [prod].[ProductCategoriesPermissions] pcp ON pcp.[CategoryId] = mapping.ProductCategoryId AND pcp.[PartnerId] = @partnerId
WHERE partnerCat.[PartnerId] = @partnerId
)

SELECT mapping.ProductCategoryId, mapping.Permission, mapping.PartnerCategoryId, mapping.PartnerCategoryName, mapping.PartnerCategoryNamePath
FROM mapping
JOIN	(	SELECT	Permission, 
				PartnerCategoryId, 
				PartnerCategoryName,
				PartnerCategoryNamePath,
				MAX(ISNULL(LEN(MappingNamePath),0)) AS maxlen -- Возьмем длинну самого детального пути
			FROM mapping 
			GROUP BY Permission, PartnerCategoryId, PartnerCategoryName, PartnerCategoryNamePath
		) maxdetail -- Подзапрос ищет самый длинный путь маппинга в группе чтобы учесть может быть так что замаплена родительская с признаком IncludeSubcategories = 1 и сама категория.
   ON	maxdetail.Permission = mapping.Permission AND
		maxdetail.PartnerCategoryId = mapping.PartnerCategoryId AND 
		maxdetail.PartnerCategoryName = mapping.PartnerCategoryName AND
		maxdetail.PartnerCategoryNamePath = mapping.PartnerCategoryNamePath AND
		maxdetail.maxlen = ISNULL(LEN(MappingNamePath),0)

WHERE mapping.Permission = 1
'
	
	DECLARE @sqlMapping [nvarchar](max) = REPLACE(@sql, '{0}', @key)
	
	--SELECT @sqlMapping
	DECLARE @ParmDefinition nvarchar(500);
	
	SET @ParmDefinition = N'@partnerId int';
	EXECUTE sp_executesql @sqlMapping, @ParmDefinition, @partnerId
END




