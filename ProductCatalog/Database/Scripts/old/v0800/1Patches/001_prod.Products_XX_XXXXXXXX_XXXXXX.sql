DECLARE c CURSOR READ_ONLY FAST_FORWARD FOR
  SELECT 'IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N''[prod].[FK_Products_' + [Key] + '_ProductCategories]'') AND parent_object_id = OBJECT_ID(N''[prod].[Products_' + [Key] + ']''))
				ALTER TABLE [prod].[Products_' + [Key] + '] DROP CONSTRAINT [FK_Products_' + [Key] + '_ProductCategories]' AS query
	  FROM [prod].[PartnerProductCatalogs]
	WHERE [IsActive] = 0

DECLARE @cmd nvarchar(max)

OPEN c

FETCH NEXT FROM c INTO @cmd
WHILE (@@FETCH_STATUS = 0)
BEGIN 
	EXEC(@cmd)
	FETCH NEXT FROM c INTO @cmd
END

CLOSE c
DEALLOCATE c