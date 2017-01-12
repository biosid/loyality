IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetAvailablePartnersForRegion]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [prod].[GetAvailablePartnersForRegion]
GO

create function [prod].[GetAvailablePartnersForRegion] ( @kladr nvarchar(32) )
returns @paramtable table ( PartnerId int ) 
as begin

insert into @paramtable
select DISTINCT PartnerId
from [prod].[PartnerDeliveryRates]
where KLADR = @kladr

  return
end;

GO


