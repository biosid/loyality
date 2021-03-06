DECLARE @packageId nvarchar(50) = N'4686b3b3-fc3f-460a-98e7-37a834527f38'

DECLARE @packegeXml nvarchar(max) = N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>4686b3b3-fc3f-460a-98e7-37a834527f38</Id>
  <Name>Отправка партнерам нотификаций по заказам</Name>
  <RunIntervalSeconds>0</RunIntervalSeconds>
  <Enabled>true</Enabled>
  <Variables>
    <Variable>
      <Name>EtlSessionId</Name>
      <Modifier>Bound</Modifier>
      <Binding>EtlSessionId</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>EtlPackageId</Name>
      <Modifier>Bound</Modifier>
      <Binding>EtlPackageId</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>DB</Name>
      <Modifier>Input</Modifier>
      <DefaultValue></DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>MaxOrdersCount</Name>
      <Modifier>Input</Modifier>
      <DefaultValue></DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
  </Variables>
  <Steps>
    <ExecuteProcedure>
      <Name>Создание нотификаций по заказам</Name>
      <TimeoutMilliseconds>200000</TimeoutMilliseconds>
      <Source>
        <Name>prod.FillOrdersNotifications</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <ProcedureName>prod.FillOrdersNotifications</ProcedureName>
        <Parameters>
          <Parameter>
            <Name>EtlSessionId</Name>
            <Value>$(EtlSessionId)</Value>
          </Parameter>
          <Parameter>
            <Name>MaxOrdersCount</Name>
            <Value>$(MaxOrdersCount)</Value>
          </Parameter>
        </Parameters>
      </Source>
    </ExecuteProcedure>
    <InvokeMethod>
      <Name>Формирование писем с нотификациями</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>assembly</Name>
        <AssemblyName>RapidSoft.Loaylty.ProductCatalog</AssemblyName>
        <TypeName>RapidSoft.Loaylty.ProductCatalog.OrdersNotifications.OrdersNotificationsSteps</TypeName>
        <MethodName>ComposeEmails</MethodName>
        <Parameters />
      </Source>
    </InvokeMethod>
    <InvokeMethod>
      <Name>Отправка писем с нотификациями</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>assembly</Name>
        <AssemblyName>RapidSoft.Loaylty.ProductCatalog</AssemblyName>
        <TypeName>RapidSoft.Loaylty.ProductCatalog.OrdersNotifications.OrdersNotificationsSteps</TypeName>
        <MethodName>SendEmails</MethodName>
        <Parameters />
      </Source>
    </InvokeMethod>
  </Steps>
</EtlPackage>'

IF EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = @packageId)
BEGIN
	UPDATE [dbo].[EtlPackages]
	SET [Text] = @packegeXml
	WHERE @packageId = [Id]
END
ELSE
BEGIN
	INSERT [dbo].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled])
	VALUES (@packageId, N'Отправка партнерам нотификаций по заказам', @packegeXml, 0, 1)
END
GO
