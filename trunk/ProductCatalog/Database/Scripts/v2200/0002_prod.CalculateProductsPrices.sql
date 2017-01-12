
/****** Object:  StoredProcedure [prod].[CalculateProductsPrices]    Script Date: 03/20/2015 02:11:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[CalculateProductsPrices]') AND type in (N'P', N'PC'))
DROP PROCEDURE [prod].[CalculateProductsPrices]
GO

CREATE PROCEDURE [prod].[CalculateProductsPrices]
    @productsIds nvarchar(max),
    @baseSql nvarchar(max),
    @actionSql nvarchar(max)
AS
BEGIN

    declare @searchSql nvarchar(max)

    set @searchSql = '
select
    p.ProductId,
    p.PriceRUR,
    ' + @baseSql + ' as PriceBase,
    ' + @actionSql + ' as PriceAction
from
    prod.ProductsFromAllPartners p with (nolock)
    left join
    prod.ProductCategories pc with (nolock) on (p.CategoryId = pc.Id)
where
    p.ProductId in (' + @productsIds + ')
'

    execute sp_executesql @searchSql
END

GO