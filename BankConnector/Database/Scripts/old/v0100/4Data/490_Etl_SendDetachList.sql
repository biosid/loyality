DELETE FROM [etl].[EtlVariables] WHERE [EtlPackageId] = N'A8FBDF89-C705-4A35-A706-A23DD5ABADB0';
DELETE FROM [etl].[EtlMessages] WHERE [EtlPackageId] = N'A8FBDF89-C705-4A35-A706-A23DD5ABADB0';
DELETE FROM [etl].[EtlSessions] WHERE [EtlPackageId] = N'A8FBDF89-C705-4A35-A706-A23DD5ABADB0';
DELETE FROM [etl].[EtlPackages] WHERE [Id]=N'A8FBDF89-C705-4A35-A706-A23DD5ABADB0';
GO

GO
/* EtlPackages */
INSERT [etl].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled]) VALUES (N'A8FBDF89-C705-4A35-A706-A23DD5ABADB0', N'SendDetachList'
,N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>a8fbdf89-c705-4a35-a706-a23dd5abadb0</Id>
  <Name>SendDetachList</Name>
  <RunIntervalSeconds>0</RunIntervalSeconds>
  <Enabled>true</Enabled>
  <Variables>
    <Variable>
      <Name>DB</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>Data Source=.;Initial Catalog=RapidSoft.VTB24.BankConnector;Integrated Security=True</DefaultValue>
      <Binding>Value</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>Temp</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>C:\Temp</DefaultValue>
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
      <Name>EtlYear</Name>
      <Modifier>Bound</Modifier>
      <Binding>EtlSessionYear4</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>EtlMonth</Name>
      <Modifier>Bound</Modifier>
      <Binding>EtlSessionMonth2</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>EtlDay</Name>
      <Modifier>Bound</Modifier>
      <Binding>EtlSessionDay2</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>SmtpHost</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
	  <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>SmtpPort</Name>
      <Modifier>Input</Modifier>      
	  <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
	  <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>SmtpUserName</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
	  <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>SmtpPassword</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
	  <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>MailToTeradataFrom</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
	  <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>MailToTeradataTo</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
	  <IsSecure>false</IsSecure>
    </Variable>
  </Variables>
  <Steps>
    <ExportCsvFile>
      <Name>������������ ����� � �������� �� ����������</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>��</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>UPDATE [dbo].[ClientForDeletion] SET [SendEtlSessionId] = N''$(SessionId)'' OUTPUT inserted.ExternalClientId WHERE [SendEtlSessionId] IS NULL AND Status = 0</Text>
        <Parameters />
      </Source>
      <Destination>
        <Name>Temp folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\VTB_$(EtlYear)$(EtlMonth)$(EtlDay)_@count.cancelPL</FilePath>
        <CodePage>1251</CodePage>
        <HasHeaders>true</HasHeaders>
        <FieldDelimiter>;</FieldDelimiter>
        <BatchSize>999999</BatchSize>
        <BatchCounterTag>@count</BatchCounterTag>
      </Destination>
      <Mappings>
        <Mapping>
          <SourceFieldName>ExternalClientId</SourceFieldName>
          <DestinationFieldName>ClientId</DestinationFieldName>
        </Mapping>
      </Mappings>
    </ExportCsvFile>
    <Encrypt>
      <Name>���������� �����</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <WorkingDirectory>$(Temp)\VTB_$(SessionId)</WorkingDirectory>
    </Encrypt>
    <EmailSend>
      <Name>EmailSend</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>����</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\</FilePath>
        <CodePage>1251</CodePage>
      </Source>
      <EmailServer>
		<Host>$(SmtpHost)</Host>
		<Port>$(SmtpPort)</Port>
		<UseSSL>false</UseSSL>
		<UserName>$(SmtpUserName)</UserName>
        <Password>$(SmtpPassword)</Password>
      </EmailServer>
      <Message>
		<From>$(MailToTeradataFrom)</From>
		<To><string>$(MailToTeradataTo)</string></To>
        <Subject>CancelPL_Request_$(EtlYear)_$(EtlMonth)_$(EtlDay)</Subject>
        <AttachmentFileMask>VTB_*.cancelPL</AttachmentFileMask>
      </Message>
    </EmailSend>
  </Steps>
</EtlPackage>', 0, 1)