ALTER TABLE [prod].[ProductsHistory] ADD [IsDeliveredByEmail] [bit] NULL

ALTER TABLE [prod].[ProductsFromAllPartners] ADD [IsDeliveredByEmail] [bit] NOT NULL CONSTRAINT [DF_ProductsFromAllPartners_IsDeliveredByEmail] DEFAULT 0

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
ALTER TABLE [prod].[Products_' + @key + '] ADD [IsDeliveredByEmail] [bit] NOT NULL CONSTRAINT [DF_Products_' + @key +'_IsDeliveredByEmail] DEFAULT 0'
    EXEC(@cmd)

    FETCH NEXT FROM c INTO @key
END

CLOSE c
DEALLOCATE c

EXEC sp_refreshview '[prod].[Products]'
