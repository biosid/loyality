DELETE FROM [etl].[EtlVariables] WHERE [EtlPackageId] = N'bacc74b8-f917-4905-b482-4ee90e2c62dd';
DELETE FROM [etl].[EtlMessages] WHERE [EtlPackageId] = N'bacc74b8-f917-4905-b482-4ee90e2c62dd';
DELETE FROM [etl].[EtlSessions] WHERE [EtlPackageId] = N'bacc74b8-f917-4905-b482-4ee90e2c62dd';
DELETE FROM [etl].[EtlPackages] WHERE [Id]=N'bacc74b8-f917-4905-b482-4ee90e2c62dd';
GO

GO
/* EtlPackages */
INSERT [etl].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled]) VALUES (N'bacc74b8-f917-4905-b482-4ee90e2c62dd', N'AppendDetachList'
,N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>bacc74b8-f917-4905-b482-4ee90e2c62dd</Id>
  <Name>AppendDetachList</Name>
  <RunIntervalSeconds>0</RunIntervalSeconds>
  <Enabled>true</Enabled>
  <Variables />
  <Steps>
    <InvokeMethod>
      <Name>InvokeMethod</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>assembly</Name>
        <AssemblyName>RapidSoft.VTB24.BankConnector</AssemblyName>
        <TypeName>RapidSoft.VTB24.BankConnector.EtlInvokeHelper</TypeName>
        <MethodName>AppendDetachList</MethodName>
        <Parameters />
      </Source>
    </InvokeMethod>
  </Steps>
</EtlPackage>', 0, 1)