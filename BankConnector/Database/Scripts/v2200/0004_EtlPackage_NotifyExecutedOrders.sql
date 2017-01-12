DECLARE @PackageId NVARCHAR(50) = N'f11dee9c-4ac6-41df-bf3d-e2b9a5d93e0b';
DECLARE @Name NVARCHAR(255) = N'NotifyExecutedOrders'
DECLARE @xml NVARCHAR(MAX) = N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>f11dee9c-4ac6-41df-bf3d-e2b9a5d93e0b</Id>
  <Name>NotifyExecutedOrders</Name>
  <RunIntervalSeconds>0</RunIntervalSeconds>
  <Enabled>true</Enabled>
  <Variables>
    <Variable>
      <Name>FromDate</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>ToDate</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>SurveyUrl</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
  </Variables>
  <Steps>
    <InvokeMethod>
      <Name>Разослать предложения оценить сервис</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>assembly</Name>
        <AssemblyName>RapidSoft.VTB24.BankConnector</AssemblyName>
        <TypeName>RapidSoft.VTB24.BankConnector.EtlInvokeHelper</TypeName>
        <MethodName>NotifyExecutedOrders</MethodName>
        <Parameters>
          <EtlMethodParameter>
            <Name>fromDateTime</Name>
            <Value>$(FromDate)</Value>
          </EtlMethodParameter>
          <EtlMethodParameter>
            <Name>toDateTime</Name>
            <Value>$(ToDate)</Value>
          </EtlMethodParameter>
          <EtlMethodParameter>
            <Name>surveyUrl</Name>
            <Value>$(SurveyUrl)</Value>
          </EtlMethodParameter>
        </Parameters>
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
