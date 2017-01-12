DECLARE @PackageId NVARCHAR(50) = N'aafab588-3868-495c-8c23-930d90583097';
DECLARE @Name NVARCHAR(255) = N'NotifyIncompleteOrders'
DECLARE @xml NVARCHAR(MAX) = N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <Id>aafab588-3868-495c-8c23-930d90583097</Id>
    <Name>NotifyIncompleteOrders</Name>
    <RunIntervalSeconds>0</RunIntervalSeconds>
    <Enabled>true</Enabled>
    <Variables/>
    <Steps>
      <InvokeMethod>
        <Name>–азослать напоминан€ о незвершенных заказах</Name>
        <TimeoutMilliseconds xsi:nil="true" />
        <Source>
          <Name>assembly</Name>
          <AssemblyName>RapidSoft.VTB24.BankConnector</AssemblyName>
          <TypeName>RapidSoft.VTB24.BankConnector.EtlInvokeHelper</TypeName>
          <MethodName>NotifyIncompleteOrders</MethodName>
          <Parameters/>
        </Source>
      </InvokeMethod>
    </Steps>
</EtlPackage>
'

IF NOT EXISTS(SELECT 1 FROM [etl].[EtlPackages] WHERE Id = @PackageId)
        INSERT [etl].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled])
        VALUES (@PackageId, @Name, @xml, 0, 1)
ELSE
        UPDATE [etl].[EtlPackages] SET [Text]=@xml,[Name]=@Name
        WHERE [Id] = @PackageId
