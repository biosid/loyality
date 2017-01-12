DECLARE @PackageId NVARCHAR(50) = N'd83d02df-98e0-4714-b06a-9f967930d051';
DECLARE @Name NVARCHAR(255) = N'ReceivePersonalMessages'
DECLARE @xml NVARCHAR(MAX) = N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>D83D02DF-98E0-4714-B06A-9F967930D051</Id>
  <Name>ReceivePersonalMessages</Name>
  <RunIntervalSeconds>0</RunIntervalSeconds>
  <Enabled>true</Enabled>
  <Variables>
    <Variable>
      <Name>DB</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>data source=.;initial catalog=RapidSoft.VTB24.BankConnector;integrated security=True;</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>Temp</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>C:\Temp</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	 <Variable>
      <Name>SecurityDir</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>C:\Temp\security</DefaultValue>
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
      <Name>SessionDateTime</Name>
      <Modifier>Bound</Modifier>
      <Binding>EtlSessionDateTime</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>SessionDateTimeUtc</Name>
      <Modifier>Bound</Modifier>
      <Binding>None</Binding>
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
      <Name>SmtpUseSSL</Name>
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
	  <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
	  <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>LoyaltyImapUseSSL</Name>
      <Modifier>Input</Modifier>      
	  <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
	  <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>LoyaltyImapUserName</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
	  <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>LoyaltyImapPassword</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
	  <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>MailToLoyaltyFrom</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
	  <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>MailToLoyaltyTo</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
	  <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>BatchSize</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
		<Name>ResponseFileName</Name>
		<DefaultValue>notset</DefaultValue>
		<Modifier>Output</Modifier>	  
	</Variable>
  </Variables>    
  <Steps>

   <EmailReceiveImap>
      <Name>Загрузка вложений писем</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <EndSessionOnEmptySource>true</EndSessionOnEmptySource>
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
              <SubjectContains>messagePL</SubjectContains>
            </ReceiveFilter>
          </Filters>
          <AttachmentRegExp>^VTB_\d{8}_\d{1,6}.messagePL$</AttachmentRegExp>
      </Message>
      <Destination>
        <Name>Temp Folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\Receive</FilePath>
        <CodePage>1251</CodePage>
      </Destination>
    </EmailReceiveImap>
	
	<EmailDeleteImap>
      <Name>DeleteMailImap</Name>
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

	<Decrypt>
      <Name>Decrypt</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <WorkingDirectory>$(Temp)\VTB_$(SessionId)\Receive</WorkingDirectory>
    </Decrypt>
	
	<ImportCsvFileBatch>
      <Name>ImportCsvFileBatch</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>Файл</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\Receive</FilePath>
        <CodePage>1251</CodePage>
        <HasHeaders>true</HasHeaders>
        <FieldDelimiter>;</FieldDelimiter>
      </Source>
      <Destination>
        <Name>База данных</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <TableName>dbo.ClientPersonalMessage</TableName>
      </Destination>
      <BatchSize>0</BatchSize>
      <Mappings>
        <Mapping>
          <SourceFieldName>ClientId</SourceFieldName>
          <DestinationFieldName>ClientId</DestinationFieldName>
        </Mapping>
        <Mapping>
          <DestinationFieldName>SessionId</DestinationFieldName>
          <DefaultValue>$(SessionId)</DefaultValue>
        </Mapping>
        <Mapping>
          <SourceFieldName>Message</SourceFieldName>
          <DestinationFieldName>Message</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>FromDateTime</SourceFieldName>
          <DestinationFieldName>FromDateTime</DestinationFieldName>
        </Mapping>
		<Mapping>
          <SourceFieldName>ToDateTime</SourceFieldName>
          <DestinationFieldName>ToDateTime</DestinationFieldName>
        </Mapping>
      </Mappings>
      <DataLossBehavior>Skip</DataLossBehavior>
    </ImportCsvFileBatch>	
	
	<InvokeMethod>
      <Name>Вызов оркестратора</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>assembly</Name>
        <AssemblyName>RapidSoft.VTB24.BankConnector</AssemblyName>
        <TypeName>RapidSoft.VTB24.BankConnector.EtlInvokeHelper</TypeName>
        <MethodName>RegisterMessages</MethodName>
        <Parameters />
      </Source>
    </InvokeMethod>
	
	<ExecuteQuery>
      <Name>BuildResponseFileName</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Text>
		SELECT TOP 1 [FileName] + ''.response'' as ResponseFileName 
		FROM [etl].[EtlIncomingMailAttachments]
		WHERE EtlSessionId =''$(SessionId)''
		order by [SeqId] desc
		</Text>
		<ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
      </Source>
      <OutputVariables>
        <FirstRow>
          <FieldToVariable>
            <SourceFieldName>ResponseFileName</SourceFieldName>
            <VariableName>ResponseFileName</VariableName>
            <DefaultValue>notset</DefaultValue>
          </FieldToVariable>
        </FirstRow>
      </OutputVariables>
    </ExecuteQuery>

	<ExportCsvFile>
      <Name>Файл ответа</Name>
      <Description>Формирование файла для выгрузки</Description>
      <TimeoutMilliseconds xsi:nil="true" />
      <EndSessionOnEmptySource>true</EndSessionOnEmptySource>
      <Source>
        <Name>Database</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>SELECT [ClientId],MAX(Status) as [Status] FROM [dbo].[ClientPersonalMessageResponse] WHERE [SessionId] = @SessionId group by ClientId, SessionId</Text>
        <Parameters>
          <EtlQueryParameter>
            <Name>SessionId</Name>
            <Value>$(SessionId)</Value>
          </EtlQueryParameter>
        </Parameters>
      </Source>
      <Destination>
        <Name>Temp folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\Response\$(ResponseFileName)</FilePath>
        <CodePage>1251</CodePage>
        <HasHeaders>true</HasHeaders>
        <FieldDelimiter>;</FieldDelimiter>
		<FileCounterDbStorage>
          <ConnectionString>$(DB)</ConnectionString>
          <SchemaName>etl</SchemaName>
          <BatchSize>$(BatchSize)</BatchSize>
          <BatchCounterTag>@count</BatchCounterTag>
        </FileCounterDbStorage>
      </Destination>
      <Mappings>
        <Mapping>
          <SourceFieldName>ClientId</SourceFieldName>
          <DestinationFieldName>ClientId</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>Status</SourceFieldName>
          <DestinationFieldName>Status</DestinationFieldName>
        </Mapping>        
      </Mappings>
    </ExportCsvFile>
	
	<Encrypt>
      <Name>Шифрование данных</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <WorkingDirectory>$(Temp)\VTB_$(SessionId)\Response</WorkingDirectory>
    </Encrypt>
	
	<EmailSend>
      <Name>Отправка файла</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>Файл</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\Response</FilePath>
        <CodePage>1251</CodePage>
      </Source>
      <EmailServer>
		<Host>$(SmtpHost)</Host>
		<Port>$(SmtpPort)</Port>
		<UseSSL>$(SmtpUseSSL)</UseSSL>
		<UserName>$(SmtpUserName)</UserName>
        <Password>$(SmtpPassword)</Password>
      </EmailServer>
      <EmailDbStorage>
        <ConnectionString>$(DB)</ConnectionString>
        <SchemaName>etl</SchemaName>
      </EmailDbStorage>
      <Message>
		<From>$(MailToTeradataFrom)</From>
		<To>
			<string>$(MailToTeradataTo)</string> 
		</To>
		<Subject>PersonalMessages_Response_$(EtlYear)_$(EtlMonth)_$(EtlDay)</Subject>
		<AttachmentFileMask>VTB_*.messagePL.response</AttachmentFileMask>
      </Message>
    </EmailSend>
  </Steps>
</EtlPackage>'

IF NOT EXISTS(SELECT 1 FROM [etl].[EtlPackages] WHERE Id = @PackageId)
	INSERT [etl].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled]) 
	VALUES (@PackageId, @Name, @xml, 0, 1)
ELSE
	UPDATE [etl].[EtlPackages] SET [Text]=@xml
	WHERE [Id] = @PackageId
