DECLARE @PackageId NVARCHAR(50);
DECLARE @xml NVARCHAR(MAX);
SET @PackageId = N'64E1E608-C27F-43BB-88FA-4865E7178109';
SET @xml = N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>64e1e608-c27f-43bb-88fa-4865e7178109</Id>
  <Name>RegisterBankClients</Name>
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
      <Name>SecurityDir</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>C:\Temp\security</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>DB</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>Data Source=@DBHOST@;Initial Catalog=@DBNAME@;user id=@DBUSER@;password=@DBPASS@</DefaultValue>
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
      <Name>SessionDateTime</Name>
      <Modifier>Bound</Modifier>
      <Binding>EtlSessionDateTime</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>SessionDateTimeUtc</Name>
      <Modifier>Bound</Modifier>
      <Binding>EtlSessionUtcDateTime</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>LoyaltyProgramId</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>@LOYALTYPROGRAMID@</DefaultValue>
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
      <DefaultValue>notset</DefaultValue>
      <Binding>None</Binding>
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
            <SubjectContains>regB</SubjectContains>
          </ReceiveFilter>
        </Filters>
        <AttachmentRegExp>^VTB_\d{8}_\d{1,6}.regB$</AttachmentRegExp>
      </Message>
      <Destination>
        <Name>Temp Folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)</FilePath>
        <CodePage>1251</CodePage>
      </Destination>
      <LettersBySessionLimit>1</LettersBySessionLimit>
    </EmailReceiveImap>
    
	<EmailDeleteImap>
      <Name>DeleteMailImap</Name>
      <TimeoutMilliseconds xsi:nil="true" />
	  <LastStepInSession>true</LastStepInSession>
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
      <WorkingDirectory>$(Temp)\VTB_$(SessionId)</WorkingDirectory>
	  <ExceptionStepName>EmailMoveToBoxImap</ExceptionStepName>
    </Decrypt>
    
    <ImportCsvFileBatch>
      <Name>Чтение файлов вложений</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>Temp folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)</FilePath>
        <CodePage>1251</CodePage>
        <HasHeaders>true</HasHeaders>
        <FieldDelimiter>;</FieldDelimiter>
      </Source>
      <Destination>
        <Name>Database</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <TableName>dbo.ClientForBankRegistration</TableName>
      </Destination>
      <BatchSize>0</BatchSize>
      <Mappings>
        <Mapping>
          <SourceFieldName>MobilePhone</SourceFieldName>
          <DestinationFieldName>MobilePhone</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>Email</SourceFieldName>
          <DestinationFieldName>Email</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>Segment</SourceFieldName>
          <DestinationFieldName>Segment</DestinationFieldName>
        </Mapping>
        <Mapping>
          <DestinationFieldName>SessionId</DestinationFieldName>
          <DefaultValue>$(SessionId)</DefaultValue>
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
        <MethodName>RegisterBankClients</MethodName>
        <Parameters>
          <EtlMethodParameter>
            <Name>loyaltyProgramId</Name>
            <Value>$(LoyaltyProgramId)</Value>
          </EtlMethodParameter>
          <EtlMethodParameter>
            <Name>connectionString</Name>
            <Value>$(DB)</Value>
          </EtlMethodParameter>
        </Parameters>
      </Source>
    </InvokeMethod>
    
    <ExecuteQuery>
      <Name>Удаление персональных данных</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>База данных</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>UPDATE [dbo].[ClientForBankRegistration] 
SET [Email] = N''N/A''
WHERE [SessionId] = N''$(SessionId)''</Text>
        <Parameters />
      </Source>
    </ExecuteQuery>
    
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
      <Name>Формирование ответного файла</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>Database</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>SELECT  ClientId , [Login], [Status] FROM    [dbo].[ClientForBankRegistrationResponse] WHERE   [SessionId] = @sessionId</Text>
        <Parameters>
          <EtlQueryParameter>
            <Name>sessionId</Name>
            <Value>$(SessionId)</Value>
          </EtlQueryParameter>
        </Parameters>
      </Source>
      <Destination>
        <Name>Temp folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\$(ResponseFileName)</FilePath>
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
          <SourceFieldName>Login</SourceFieldName>
          <DestinationFieldName>Login</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>Status</SourceFieldName>
          <DestinationFieldName>Status</DestinationFieldName>
        </Mapping>
      </Mappings>
	  <EndSessionOnEmptySource>true</EndSessionOnEmptySource>
    </ExportCsvFile>
    
    <Encrypt>
      <Name>Encrypt</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <WorkingDirectory>$(Temp)\VTB_$(SessionId)</WorkingDirectory>
    </Encrypt>
    
    <EmailSend>
      <Name>Отправка письма с ответными файлами</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>Temp Folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\</FilePath>
        <CodePage>1251</CodePage>
      </Source>
	  <EmailServer>
		<Host>$(SmtpHost)</Host>
		<Port>$(SmtpPort)</Port>
		<UseSSL>$(SmtpUseSSL)</UseSSL>
		<UserName>$(SmtpUserName)</UserName>
        <Password>$(SmtpPassword)</Password>
      </EmailServer>
      <Message>
        <From>$(MailToTeradataFrom)</From>
		<To><string>$(MailToTeradataTo)</string></To>
        <Subject>BankRegistration_Response_$(EtlYear)_$(EtlMonth)_$(EtlDay)</Subject>
        <AttachmentFileMask>VTB_*.regB.response</AttachmentFileMask>
      </Message>
    </EmailSend>
	
	<ExecuteQuery>
      <Name>Отмечаем заявки на регистрацию как удаленные (обработаные)</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>База данных</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>UPDATE [dbo].[ClientForBankRegistration]
SET [IsDeleted] = 1
FROM  [dbo].[ClientForBankRegistration] cbr
INNER JOIN [dbo].[ClientForBankRegistrationResponse] cbrr
ON cbr.[SessionId] = cbrr.[SessionId] AND cbr.ClientId = cbrr.ClientId
WHERE cbr.[SessionId] = N''$(SessionId)''</Text>
        <Parameters />
      </Source>
    </ExecuteQuery>
    
  </Steps>
</EtlPackage>';

IF NOT EXISTS(SELECT 1 FROM [etl].[EtlPackages] WHERE Id = @PackageId)
INSERT [etl].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled]) VALUES (@PackageId, N'RegisterBankClients',
@xml, 0, 1)
ELSE
UPDATE [etl].[EtlPackages] SET [Text]=@xml
WHERE [Id] = @PackageId