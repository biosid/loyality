/****** Object:  View [prod].[DeactivatedCategories]    Script Date: 02/25/2014 13:14:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE view [prod].[DeactivatedCategories]
AS

with Categories(Id, ParentId)
as (select Id, ParentId
    from prod.ProductCategories nolock
    where Status = 0
    
    union all
    
    select pc.Id, pc.ParentId
    from prod.ProductCategories pc (nolock)
	     inner join Categories c on pc.ParentId = c.Id)
select distinct Id
from Categories

GO
