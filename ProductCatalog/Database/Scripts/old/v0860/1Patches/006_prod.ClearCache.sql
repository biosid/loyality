IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[ClearCache]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[ClearCache]
GO

--exec [prod].[ClearCache]
CREATE PROCEDURE [prod].[ClearCache]
	@insertedTimeout int = 0,
	@disableTimeout int = 10
AS
BEGIN
	-- NOTE: Помечаем как отключенный все те которые созданы более @insertedTimeout секунд назад
	UPDATE [prod].Cache
	   SET [DisableDate] = GETDATE()
	WHERE DATEADD(SECOND, @insertedTimeout, [InsertedDate]) < GETDATE()

	-- NOTE: Удаляем все те которые отключены более @disableTimeout секунд назад
	DECLARE c CURSOR READ_ONLY FAST_FORWARD FOR
	SELECT ContextId
	FROM [prod].Cache		
	WHERE DATEADD(SECOND, @disableTimeout, [DisableDate]) < GETDATE()

	DECLARE @key nvarchar(50)
	DECLARE @cmd nvarchar(max)

	OPEN c

	FETCH NEXT FROM c INTO @key
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
	    	    
	    DELETE FROM [prod].Cache WHERE ContextId = @key
	    
		SET @cmd = 'IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[ProductsCache_' + @key + ']'') AND type in (N''U''))
DROP TABLE [prod].[ProductsCache_' + @key + ']'
		exec(@cmd)

		SET @cmd = 'IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[CategoriesCache_' + @key + ']'') AND type in (N''U''))
DROP TABLE [prod].[CategoriesCache_' + @key + ']'
		exec(@cmd)	    		
		
		FETCH NEXT FROM c INTO @key
	END

	CLOSE c
	DEALLOCATE c
END

GO


