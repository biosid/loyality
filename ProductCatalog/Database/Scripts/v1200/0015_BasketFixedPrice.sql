SET ANSI_PADDING ON
GO

UPDATE t
set 
ProductPrice.modify('
insert sql:column("t.xmlVal") into (/)[1]
')
from 
(select  
ProductPrice
,CAST('<FixedPrice>'+ CAST(ProductPrice.query(N'/ProductPrice/*') as nvarchar(max)) +'</FixedPrice>' as xml) as xmlVal
from [prod].[BasketItems]) as t
where ProductPrice is not null
GO

UPDATE [prod].[BasketItems]
set 
ProductPrice.modify('delete /ProductPrice')
where ProductPrice is not null
GO

EXECUTE sp_rename N'prod.BasketItems.ProductPrice', N'FixedPrice', 'COLUMN' 
GO