DROP PROCEDURE [prod].[DeleteProductPrices]
GO

/****** Object:  StoredProcedure [prod].[ClearCashe]    Script Date: 27.09.2013 15:48:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [prod].[ClearCache]
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
	    
		SET @cmd = 'IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[ProductCache_' + @key + ']'') AND type in (N''U''))
DROP TABLE [prod].[ProductCache_' + @key + ']'
		exec(@cmd)

		SET @cmd = 'IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[CategoriesCache_' + @key + ']'') AND type in (N''U''))
DROP TABLE [prod].[CategoriesCache_' + @key + ']'
		exec(@cmd)	    		

	    DELETE FROM [prod].Cache
		WHERE ContextId = @key

		FETCH NEXT FROM c INTO @key
	END

	CLOSE c
	DEALLOCATE c
END

GO

