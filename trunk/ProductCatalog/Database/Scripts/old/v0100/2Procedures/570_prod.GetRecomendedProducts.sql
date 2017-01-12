create procedure [prod].[GetRecomendedProducts]
 @countToTake int
as
BEGIN

select top (@countToTake) * from prod.Products order by newid()
END

