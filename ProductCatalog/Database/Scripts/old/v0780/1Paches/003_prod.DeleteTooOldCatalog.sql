IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[DeleteTooOldCatalog]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[DeleteTooOldCatalog]
GO

CREATE PROCEDURE [prod].[DeleteTooOldCatalog]
AS
BEGIN
	DECLARE c CURSOR READ_ONLY FAST_FORWARD FOR
	   SELECT ppc.[Key]
		  FROM [prod].[PartnerProductCatalogs] ppc
		JOIN [prod].[PartnerSettings] ps ON ppc.PartnerId = ps.PartnerId AND ps.[Key] = 'OldCatalogTimeout'
		WHERE [IsActive] = 0 
		  AND DATEADD(HOUR, CONVERT(int, ps.Value), [InsertedDate]) < GETDATE()

	DECLARE @key nvarchar(50)
	DECLARE @cmd nvarchar(max)

	OPEN c

	FETCH NEXT FROM c INTO @key
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
	    
		SET @cmd = 'IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[PartnerProductCaterories_' + @key + ']'') AND type in (N''U''))
DROP TABLE [prod].[PartnerProductCaterories_' + @key + ']'
		exec(@cmd)
	    
		SET @cmd = 'IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[Products_' + @key + ']'') AND type in (N''U''))
DROP TABLE [prod].[Products_' + @key + ']'
		exec(@cmd)
	    
		SET @cmd = 'IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[ProductsErrors_' + @key + ']'') AND type in (N''U''))
DROP TABLE [prod].[ProductsErrors_' + @key + ']'
		exec(@cmd)
	    
		DELETE FROM [prod].[PartnerProductCatalogs]
		WHERE [Key] = @key

		FETCH NEXT FROM c INTO @key
	END

	CLOSE c
	DEALLOCATE c
END
GO


