DECLARE @PackageId NVARCHAR(50);
DECLARE @xml NVARCHAR(MAX);
SET @PackageId = N'77388AA7-9C66-4771-87B9-1D3C2B96A8D5';
SET @xml = N'<?xml version="1.0" encoding="utf-16"?>  
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
	<Id>77388AA7-9C66-4771-87B9-1D3C2B96A8D5</Id>    
	<Name>Импорт каталога</Name>
	<RunIntervalSeconds>0</RunIntervalSeconds>
	<Enabled>true</Enabled>
	<Variables/>
	<Steps/>
</EtlPackage>'

IF NOT EXISTS(SELECT 1 FROM [dbo].[EtlPackages] WHERE Id = @PackageId)
	INSERT [dbo].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled]) 
	VALUES (@PackageId, N'Импорт каталога',	@xml, 0, 1)
ELSE
UPDATE [dbo].[EtlPackages] SET [Text]=@xml
WHERE [Id] = @PackageId