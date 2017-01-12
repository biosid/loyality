ALTER TABLE [prod].[ProductsHistory] ADD IsRecommended bit NULL

DECLARE c CURSOR READ_ONLY FAST_FORWARD FOR
    SELECT [Key]
    FROM [prod].[PartnerProductCatalogs]

DECLARE @key nvarchar(50)
DECLARE @cmd nvarchar(max)

OPEN c
FETCH NEXT FROM c INTO @key

WHILE (@@FETCH_STATUS = 0)
BEGIN

    SET @cmd = 'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[Products_' + @key + ']'') AND type in (N''U''))
ALTER TABLE [prod].[Products_' + @key + '] ADD IsRecommended bit NOT NULL CONSTRAINT [DF_Products_' + @key +'_IsRecommended] DEFAULT 0'
    EXEC(@cmd)

    FETCH NEXT FROM c INTO @key
END

CLOSE c
DEALLOCATE c

EXEC sp_refreshview '[prod].[Products]'
