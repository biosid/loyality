DELETE FROM [dbo].[EtlPackages]
WHERE [Id] = N'49df85e6-e3d6-47d1-aac8-7dd9d03ff845'

INSERT [dbo].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled]) 
VALUES (N'49df85e6-e3d6-47d1-aac8-7dd9d03ff845', N'Очистка корзины BasketItem', N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>49df85e6-e3d6-47d1-aac8-7dd9d03ff845</Id>
  <Name>Очистка корзины</Name>
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
      <DefaultValue />
      <Binding>Value</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
  </Variables>
  <Steps>
    <ExecuteQuery>
      <Name>Запрос очистки корзины</Name>
      <Description>Очищаются продукты созданные 24 часа назад и более</Description>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <ConnectionString>$(DB)</ConnectionString>
		<ProviderName>System.Data.SqlClient</ProviderName>
		 <Text>delete from [prod].[BasketItems] where id in (select id FROM [prod].[BasketItems] where DATEDIFF(hour, CreatedDate, GETDATE()) &gt;= 24)</Text>
		<Parameters />
      </Source>
    </ExecuteQuery>
  </Steps>
</EtlPackage>', 0, 1)
GO


