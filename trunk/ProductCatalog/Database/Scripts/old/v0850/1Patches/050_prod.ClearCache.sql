/****** Object:  StoredProcedure [prod].[ClearCache]    Script Date: 03.10.2013 11:20:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [prod].[ClearCache]
@seconds int = 0
AS
BEGIN

	DECLARE c CURSOR READ_ONLY FAST_FORWARD FOR
	SELECT price.ContextId
	FROM [prod].Cache price		
	WHERE DATEADD(SECOND, @seconds, price.InsertedDate) < GETDATE()

	DECLARE @key nvarchar(50)
	DECLARE @cmd nvarchar(max)

	OPEN c

	FETCH NEXT FROM c INTO @key
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
	    
		SET @cmd = 'IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[ProductsCache_' + @key + ']'') AND type in (N''U''))
DROP TABLE [prod].[ProductsCache_' + @key + ']'
		exec(@cmd)

		SET @cmd = 'IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[CategoriesCache_' + @key + ']'') AND type in (N''U''))
DROP TABLE [prod].[CategoriesCache_' + @key + ']'
		exec(@cmd)	    		

	    DELETE FROM [prod].Cache WHERE ContextId = @key

		FETCH NEXT FROM c INTO @key
	END

	CLOSE c
	DEALLOCATE c
END
