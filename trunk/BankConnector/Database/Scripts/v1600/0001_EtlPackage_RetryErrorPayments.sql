DECLARE @PackageId NVARCHAR(50) = N'6e3e0808-96b1-4773-81e4-173cec318be8';
DECLARE @Name NVARCHAR(255) = N'RetryErrorPayments'
DECLARE @xml NVARCHAR(MAX) = N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <Id>6e3e0808-96b1-4773-81e4-173cec318be8</Id>
    <Name>RetryErrorPayments</Name>
    <RunIntervalSeconds>0</RunIntervalSeconds>
    <Enabled>true</Enabled>
    <Variables/>
    <Steps/>
</EtlPackage>
'

IF NOT EXISTS(SELECT 1 FROM [etl].[EtlPackages] WHERE Id = @PackageId)
        INSERT [etl].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled])
        VALUES (@PackageId, @Name, @xml, 0, 1)
ELSE
        UPDATE [etl].[EtlPackages] SET [Text]=@xml,[Name]=@Name
        WHERE [Id] = @PackageId
