IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ParamParserString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ParamParserString]
GO

CREATE FUNCTION [dbo].[ParamParserString](@delimString nvarchar(max), @delim nchar(1))
RETURNS @paramtable 
TABLE ([value] nvarchar(1024)) 
AS BEGIN

	DECLARE @len int,
			@index int,
			@nextindex int

	SET @len = DATALENGTH(@delimString)
	SET @index = 0
	SET @nextindex = 0

	WHILE (@len > @index )
	BEGIN

		SET @nextindex = CHARINDEX(@delim, @delimString, @index)
		
		IF (@nextindex = 0) 
			SET @nextindex = @len + 2
		
		WHILE (SUBSTRING(@delimString, @nextindex - 1, 1) = '\')
		BEGIN
			SET @nextindex = CHARINDEX(@delim, @delimString, @nextindex + 1)
		END
		
		IF (@nextindex = 0) 
			SET @nextindex = @len + 2
		
		INSERT @paramtable
		SELECT REPLACE(SUBSTRING(@delimString, @index, @nextindex - @index), '\' + @delim, @delim)
		
		SET @index = @nextindex + 1

	END
	RETURN
END