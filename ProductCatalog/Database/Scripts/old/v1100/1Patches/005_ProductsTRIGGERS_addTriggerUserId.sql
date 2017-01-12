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

	SET @cmd = 'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[LogProducts_'+@key+'_INSERT]''))
	DROP TRIGGER [prod].[LogProducts_'+@key+'_INSERT]'	
	exec(@cmd)

	SET @cmd = 'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[LogProducts_'+@key+'_UPDATE]''))
	DROP TRIGGER [prod].[LogProducts_'+@key+'_UPDATE]'	
	exec(@cmd)

	SET @cmd = 'CREATE TRIGGER [prod].[LogProducts_'+@key+'_DELETE] ON [prod].[Products_'+@key+']
	FOR DELETE
	AS
		INSERT INTO [prod].[ProductsHistory]
		select	''D'',
				dbo.GetCurrentUserId(),
				getdate(),
				getutcdate(),
				*
		from DELETED'
	exec(@cmd)
		
	SET @cmd = 'CREATE TRIGGER [prod].[LogProducts_'+@key+'_INSERT] ON [prod].[Products_'+@key+']
	FOR INSERT
	AS
		INSERT INTO [prod].[ProductsHistory]
		select	''I'',
				dbo.GetCurrentUserId(),
				getdate(),
				getutcdate(),
				*
		from INSERTED'
	exec(@cmd)
		
	SET @cmd = 'CREATE TRIGGER [prod].[LogProducts_'+@key+'_UPDATE] ON [prod].[Products_'+@key+']
	FOR UPDATE
	AS
		INSERT INTO [prod].[ProductsHistory]
		select	''U'',
				dbo.GetCurrentUserId(),
				getdate(),
				getutcdate(),
				*
		from INSERTED'
	exec(@cmd)
		
	FETCH NEXT FROM c INTO @key
END

CLOSE c;
DEALLOCATE c;