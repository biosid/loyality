/****** Object:  UserDefinedFunction [prod].[GetCategoryNestingLevel]    Script Date: 26.04.2013 19:06:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [prod].[GetStringFromProductAudienceRows]
(
	@productId nvarchar(256)
)
RETURNS nvarchar(MAX)
AS
BEGIN
	DECLARE @Names VARCHAR(8000) 

	SELECT @Names = COALESCE(@Names + ',', '') + TargetAudienceId 
	FROM [prod].[ProductTargetAudiences]
	where ProductId = @productId
	
	RETURN @Names
END
GO


