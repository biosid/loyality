/****** Object:  StoredProcedure [prod].[CreateCache]    Script Date: 03/07/2014 12:23:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
    prod.Products p with (nolock)
    left join
    prod.ProductCategories pc with (nolock) on (p.CategoryId = pc.Id)
where
    p.ProductId in (' + @productsIds + ')
'

    execute sp_executesql @searchSql
END
