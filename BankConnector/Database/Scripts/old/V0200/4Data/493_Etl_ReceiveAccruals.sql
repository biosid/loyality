DELETE FROM [etl].[EtlVariables] WHERE [EtlPackageId] = N'c137a6c9-059e-4323-bbd5-000fc754457a';
DELETE FROM [etl].[EtlMessages] WHERE [EtlPackageId] = N'c137a6c9-059e-4323-bbd5-000fc754457a';
DELETE FROM [etl].[EtlSessions] WHERE [EtlPackageId] = N'c137a6c9-059e-4323-bbd5-000fc754457a';
DELETE FROM [etl].[EtlPackages] WHERE [Id]=N'c137a6c9-059e-4323-bbd5-000fc754457a';
GO

GO
/* EtlPackages */
INSERT [etl].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled]) VALUES (N'c137a6c9-059e-4323-bbd5-000fc754457a', N'ReceiveAccruals',
N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>c137a6c9-059e-4323-bbd5-000fc754457a</Id>
  <Name>ReceiveAccruals</Name>
  <RunIntervalSeconds>0</RunIntervalSeconds>
  <Enabled>true</Enabled>
  <Variables>
    <Variable>
      <Name>Temp</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>C:\Temp</DefaultValue>
      <Binding>Value</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>DB</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>data source=.\sql2008;initial catalog=RapidSoft.VTB24.BankConnector;integrated security=True;</DefaultValue>
      <Binding>Value</Binding>
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
      <Name>LoyaltyImapUseSSL</Name>
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
        <UseSSL>$(LoyaltyImapUseSSL)</UseSSL>
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
            <SubjectStartsWith>NachislPL_Request</SubjectStartsWith>
          </ReceiveFilter>
        </Filters>
        <AttachmentRegExp>^VTB_[1-2][0-9]{3}[0-1][0-9][0-3][0-9]_[0-9]+.nachislPL$</AttachmentRegExp>
      </Message>
      <Destination>
        <Name>Temp Folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)</FilePath>
        <CodePage>1251</CodePage>
      </Destination>
    </EmailReceiveImap>
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
        <TableName>dbo.Accruals</TableName>
      </Destination>
      <BatchSize>0</BatchSize>
      <Mappings>
        <Mapping>
          <SourceFieldName>ClientId</SourceFieldName>
          <DestinationFieldName>ClientId</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>BonusSum</SourceFieldName>
          <DestinationFieldName>BonusSum</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>Type</SourceFieldName>
          <DestinationFieldName>Type</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>Description</SourceFieldName>
          <DestinationFieldName>Description</DestinationFieldName>
        </Mapping>
        <Mapping>
          <DestinationFieldName>ReceiveEtlSessionId</DestinationFieldName>
          <DefaultValue>$(SessionId)</DefaultValue>
        </Mapping>
      </Mappings>
      <DataLossBehavior>Fail</DataLossBehavior>
    </ImportCsvFileBatch>
    <InvokeMethod>
      <Name>ReceiveAccrualsExecute</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>ReceiveAccrualsExecute</Name>
        <AssemblyName>RapidSoft.VTB24.BankConnector</AssemblyName>
        <TypeName>RapidSoft.VTB24.BankConnector.EtlInvokeHelper</TypeName>
        <MethodName>ReceiveAccrualsExecute</MethodName>
        <Parameters />
      </Source>
    </InvokeMethod>
    <EmailDeleteImap>
      <Name>EmailDeleteImap</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <EmailServer>
        <Host>$(LoyaltyImapHost)</Host>
        <Port>$(LoyaltyImapPort)</Port>
        <UseSSL>$(LoyaltyImapUseSSL)</UseSSL>
        <UserName>$(LoyaltyImapUserName)</UserName>
        <Password>$(LoyaltyImapPassword)</Password>
      </EmailServer>
      <EmailDbStorage>
        <ConnectionString>$(DB)</ConnectionString>
        <SchemaName>etl</SchemaName>
      </EmailDbStorage>
    </EmailDeleteImap>
  </Steps>
</EtlPackage>', 0, 1)