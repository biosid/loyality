/****** Object:  StoredProcedure [prod].[ClearCache]    Script Date: 10/21/2013 08:34:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [prod].[ClearCache]

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
	    	    
	    DELETE FROM [prod].Cache WHERE ContextId = @key
	    
		WAITFOR DELAY '0:0:1'

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
