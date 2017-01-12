CREATE PROCEDURE [prod].[DeleteProductPrices]
@seconds int = 0
AS
BEGIN

	DECLARE c CURSOR READ_ONLY FAST_FORWARD FOR
	SELECT price.ContextId
	FROM [prod].[ProductPrices] price		
	WHERE DATEADD(SECOND, @seconds, price.InsertedDate) < GETDATE()

	DECLARE @key nvarchar(50)
	DECLARE @cmd nvarchar(max)

	OPEN c

	FETCH NEXT FROM c INTO @key
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
	    
		SET @cmd = 'IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[prod].[ProductPrices_' + @key + ']'') AND type in (N''U''))
DROP TABLE [prod].[ProductPrices_' + @key + ']'
		exec(@cmd)
	    
	    DELETE FROM [prod].[ProductPrices]
		WHERE ContextId = @key

		FETCH NEXT FROM c INTO @key
	END

	CLOSE c
	DEALLOCATE c
END