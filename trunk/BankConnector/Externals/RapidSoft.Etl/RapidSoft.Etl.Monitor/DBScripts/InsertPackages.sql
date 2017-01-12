DELETE FROM [sysmon].[EtlPackages] WHERE Id = 'E80DF826-E4B2-43E8-8CA3-E80C39266B15'
INSERT [sysmon].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled]) VALUES (N'E80DF826-E4B2-43E8-8CA3-E80C39266B15', N'Мониторинг планировщика', N'<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>E80DF826-E4B2-43E8-8CA3-E80C39266B15</Id>
  <Name>SystemPing</Name>
  <Variables>
    <Variable>
      <Name>etl_site_address</Name>
      <Binding>String</Binding>
      <ReadOnly>false</ReadOnly>
      <IsSecure>false</IsSecure>
      <Value>localhost:3000</Value>
      <Secure>false</Secure>
      <CanBeOverridden>true</CanBeOverridden>
    </Variable>
  </Variables>
  <Steps>
    <DownloadFile>
      <Id>3868d249-3e60-4925-a2ff-378ac51f5094</Id>
      <Name>PingPacket</Name>
      <Disabled>false</Disabled>
      <Timeout>0</Timeout>
      <TimeoutMilliseconds>0</TimeoutMilliseconds>
      <Source>
        <Name>Ping</Name>
        <Uri>http://$(etl_site_address)/Ping.svc</Uri>
        <AllowInvalidCertificates>false</AllowInvalidCertificates>
        <Method>POST</Method>
        <ContentType>text/xml;charset=UTF-8</ContentType>
        <Headers>
          <Header>
            <Name>SOAPAction</Name>
            <Value>http://tempuri.org/IPing/PingService</Value>
          </Header>
        </Headers>
        <Request>&lt;soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/"&gt;
   &lt;soapenv:Header/&gt;
   &lt;soapenv:Body&gt;
      &lt;tem:PingService/&gt;
   &lt;/soapenv:Body&gt;
&lt;/soapenv:Envelope&gt;</Request>
      </Source>
      <SourceUri>http://$(etl_site_address)/Ping.svc</SourceUri>
      <AllowInvalidCertificates>false</AllowInvalidCertificates>
    </DownloadFile>
  </Steps>
</EtlPackage>', 0, 1)  