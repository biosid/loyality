declare @etl_site_address nvarchar(MAX)



--##################################################################################

set @etl_site_address = 'localhost:3000'

--##################################################################################



declare @t table ( Id uniqueidentifier, Package xml )

insert into @t
select Id, CAST([Text] as XML) [Package] from sysmon.EtlPackages
where Id in (
'E80DF826-E4B2-43E8-8CA3-E80C39266B15')

UPDATE @t
SET [Package].modify('replace value of (/EtlPackage/Variables/Variable/Value/text())[1] with 
( if ((/EtlPackage/Variables/Variable/Name/text()) = "etl_site_address") then
		sql:variable("@etl_site_address")
  else "")')																										
				
update sysmon.EtlPackages
set [Text] = cast([Package] as nvarchar(MAX))
from @t t
where sysmon.EtlPackages.Id = t.Id
