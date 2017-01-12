DECLARE @PackageId NVARCHAR(50) = N'9517516a-ee80-4963-b326-7ef56efd9691';
DECLARE @Name NVARCHAR(255) = N'SendRegistrationClients'
DECLARE @xml NVARCHAR(MAX) = N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>9517516a-ee80-4963-b326-7ef56efd9691</Id>
  <Name>SendRegistrationClients</Name>
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
      <Name>BatchSize</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
  </Variables>
  <Steps>
  
    <ExportCsvFile>
      <Name>Файл запроса</Name>
      <Description>Формирование файла для выгрузки</Description>
      <TimeoutMilliseconds xsi:nil="true" />
      <EndSessionOnEmptySource>true</EndSessionOnEmptySource>
      <Source>
        <Name>База данных</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>UPDATE [dbo].[ClientForRegistration] 
SET 
[RequestSessionId] = N''$(SessionId)''
OUTPUT INSERTED.ClientId, 
DELETED.LastName, 
DELETED.FirstName,
DELETED.MiddleName, 
SUBSTRING(INSERTED.MobilePhone, 2, LEN(INSERTED.MobilePhone)) AS MobilePhone, 
DELETED.Email, CONVERT(VARCHAR(10), DELETED.BirthDate, 120) AS BirthDate  
WHERE [RequestSessionId] IS NULL</Text>
        <Parameters />
      </Source>
      <Destination>
        <Name>Файл</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\VTB_$(EtlYear)$(EtlMonth)$(EtlDay)_@count.regPL</FilePath>
        <CodePage>1251</CodePage>
        <HasHeaders>true</HasHeaders>
        <FieldDelimiter>;</FieldDelimiter>
        <FileCounterDbStorage>
          <ConnectionString>$(DB)</ConnectionString>
          <SchemaName>etl</SchemaName>
          <BatchSize>$(BatchSize)</BatchSize>
          <BatchCounterTag>@count</BatchCounterTag>
        </FileCounterDbStorage>      </Destination>
      <Mappings>
        <Mapping>
          <SourceFieldName>ClientId</SourceFieldName>
          <DestinationFieldName>ClientId</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>LastName</SourceFieldName>
          <DestinationFieldName>LastName</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>FirstName</SourceFieldName>
          <DestinationFieldName>FirstName</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>MiddleName</SourceFieldName>
          <DestinationFieldName>MiddleName</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>BirthDate</SourceFieldName>
          <DestinationFieldName>BirthDate</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>MobilePhone</SourceFieldName>
          <DestinationFieldName>MobilePhone</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>Email</SourceFieldName>
          <DestinationFieldName>Email</DestinationFieldName>
        </Mapping>
      </Mappings>
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
      <EmailDbStorage>
        <ConnectionString>$(DB)</ConnectionString>
        <SchemaName>etl</SchemaName>
      </EmailDbStorage>
      <Message>
		<From>$(MailToTeradataFrom)</From>
		<To><string>$(MailToTeradataTo)</string></To>
		<Subject>Registration_Request_$(EtlYear)_$(EtlMonth)_$(EtlDay)</Subject>
		<AttachmentFileMask>VTB_*.regPL</AttachmentFileMask>
      </Message>
    </EmailSend>
    
    <ExecuteQuery>
      <Name>Удаление персональных данных</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>База данных</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>UPDATE [dbo].[ClientForRegistration] 
SET 
[FirstName] = N''N/A'',
[MiddleName] = N''N/A'',
[LastName] = N''N/A'',
[Email] = N''N/A'',
[BirthDate] = GETDATE(),
[Gender] = 0
WHERE [RequestSessionId] = N''$(SessionId)''</Text>
        <Parameters />
      </Source>
    </ExecuteQuery>

	</Steps>
</EtlPackage>'

IF NOT EXISTS(SELECT 1 FROM [etl].[EtlPackages] WHERE Id = @PackageId)
	INSERT [etl].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled]) 
	VALUES (@PackageId, @Name, @xml, 0, 1)
ELSE
	UPDATE [etl].[EtlPackages] SET [Text]=@xml
	WHERE [Id] = @PackageId
