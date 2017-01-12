/****** Object:  UserDefinedFunction [prod].[GetCategoryNestingLevel]    Script Date: 26.04.2013 19:06:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [prod].[GetCategoryNestingLevel]
(
	@productId int,
	@level int 
)
RETURNS int
AS
BEGIN
	
	if (@productId is null) return -1;

	set  @level =  ISNULL(@level, 0) + 1;

	declare @id int = null
	select @id = parentId from prod.ProductCategories where id = @productId
	
	if (@id is not null)
	return [prod].[GetCategoryNestingLevel](@id, @level)

	RETURN @level
END
GO


