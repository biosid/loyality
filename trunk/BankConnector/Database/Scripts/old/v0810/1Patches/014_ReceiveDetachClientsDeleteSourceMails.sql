DECLARE @PackageId NVARCHAR(50);
DECLARE @xml NVARCHAR(MAX);
SET @PackageId = N'F2C56EAB-2B60-4AF2-8712-90F533EE095E';
SET @xml = N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>f2c56eab-2b60-4af2-8712-90f533ee095e</Id>
  <Name>ReceiveDetachClients</Name>
  <RunIntervalSeconds>0</RunIntervalSeconds>
  <Enabled>true</Enabled>
  <Variables>
    <Variable>
      <Name>Temp</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>C:\Temp</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>DB</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>data source=.\sql2008;initial catalog=RapidSoft.VTB24.BankConnector;integrated security=True;</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>SessionId</Name>
      <Modifier>Bound</Modifier>
      <Binding>EtlSessionId</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>LoyaltyImapHost</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
	  <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>LoyaltyImapPort</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
	  <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>LoyaltyImapUserName</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
	  <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>LoyaltyImapPassword</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
	  <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
  </Variables>
  <Steps>

    <EmailReceiveImap>
      <Name>Загрузка вложений писем</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <EmailServer>
        <Host>$(LoyaltyImapHost)</Host>
        <Port>$(LoyaltyImapPort)</Port>
        <UseSSL>false</UseSSL>
        <UserName>$(LoyaltyImapUserName)</UserName>
        <Password>$(LoyaltyImapPassword)</Password>
      </EmailServer>
      <EmailDbStorage>
        <ConnectionString>$(DB)</ConnectionString>
        <SchemaName>etl</SchemaName>
      </EmailDbStorage>
      <Message>
        <Filters>
          <ReceiveFilter>
            <SubjectContains>cancelPL</SubjectContains>
          </ReceiveFilter>
        </Filters>
        <AttachmentRegExp>^VTB_[1-2][0-9]{3}[0-1][0-9][0-3][0-9]_[0-9]+.cancelPL.response$</AttachmentRegExp>
      </Message>
      <Destination>
        <Name>Temp Folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)</FilePath>
        <CodePage>1251</CodePage>
      </Destination>
    </EmailReceiveImap>

	<EmailDeleteImap>
      <Name>EmailDeleteImap</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <EmailServer>
        <Host>$(LoyaltyImapHost)</Host>
        <Port>$(LoyaltyImapPort)</Port>
        <UseSSL>false</UseSSL>
        <UserName>$(LoyaltyImapUserName)</UserName>
        <Password>$(LoyaltyImapPassword)</Password>
      </EmailServer>
      <EmailDbStorage>
        <ConnectionString>$(DB)</ConnectionString>
        <SchemaName>etl</SchemaName>
      </EmailDbStorage>
    </EmailDeleteImap>

    <Decrypt>
      <Name>Decrypt</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <WorkingDirectory>$(Temp)\VTB_$(SessionId)</WorkingDirectory>
    </Decrypt>

    <ImportCsvFileBatch>
      <Name>ImportCsvFileBatch</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>Файл</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)</FilePath>
        <CodePage>1251</CodePage>
        <HasHeaders>true</HasHeaders>
        <FieldDelimiter>;</FieldDelimiter>
      </Source>
      <Destination>
        <Name>База данных</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <TableName>dbo.ClientForDeletionResponse</TableName>
      </Destination>
      <BatchSize>0</BatchSize>
      <Mappings>
        <Mapping>
          <DestinationFieldName>EtlSessionId</DestinationFieldName>
          <DefaultValue>$(SessionId)</DefaultValue>
        </Mapping>
        <Mapping>
          <SourceFieldName>ClientId</SourceFieldName>
          <DestinationFieldName>ClientId</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>Status</SourceFieldName>
          <DestinationFieldName>Status</DestinationFieldName>
        </Mapping>
      </Mappings>
      <DataLossBehavior>Skip</DataLossBehavior>
    </ImportCsvFileBatch>

    <InvokeMethod>
      <Name>InvokeMethod</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>assembly</Name>
        <AssemblyName>RapidSoft.VTB24.BankConnector</AssemblyName>
        <TypeName>RapidSoft.VTB24.BankConnector.EtlInvokeHelper</TypeName>
        <MethodName>ReceiveDetachClients</MethodName>
        <Parameters />
      </Source>
    </InvokeMethod>
  </Steps>
</EtlPackage>';
IF NOT EXISTS(SELECT 1 FROM [etl].[EtlPackages] WHERE Id = @PackageId)
INSERT [etl].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled]) VALUES (@PackageId, N'ReceiveRegistrationClients',
@xml, 0, 1)
ELSE
UPDATE [etl].[EtlPackages] SET [Text]=@xml
WHERE [Id] = @PackageId