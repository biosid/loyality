DECLARE c CURSOR READ_ONLY FAST_FORWARD FOR
	 SELECT ppc.[Key]
		FROM [prod].[PartnerProductCatalogs] ppc		

DECLARE @key nvarchar(50)
DECLARE @cmd nvarchar(max)

OPEN c

FETCH NEXT FROM c INTO @key
WHILE (@@FETCH_STATUS = 0)
BEGIN	
	SET @cmd = 'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[LogProducts_'+@key+'_DELETE]''))
	DROP TRIGGER [prod].[LogProducts_'+@key+'_DELETE]'
	
	exec(@cmd)

	SET @cmd = 'CREATE TRIGGER [prod].[LogProducts_'+@key+'_DELETE] ON [prod].[Products_'+@key+']
	FOR DELETE
	AS
		INSERT INTO [prod].[ProductsHistory]
		select	''D'',
				getdate(),
				getutcdate(),
				*
		from DELETED'

	exec(@cmd)
		
	FETCH NEXT FROM c INTO @key
END

CLOSE c;
DEALLOCATE c;