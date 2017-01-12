DECLARE @PackageId NVARCHAR(50);
DECLARE @xml NVARCHAR(MAX);
SET @PackageId = N'7772648e-708f-43d6-8153-c4caa3e2fb05';
SET @xml = N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>7772648e-708f-43d6-8153-c4caa3e2fb05</Id>
  <Name>Отправка данных о акциях</Name>
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
      <Name>SecurityDir</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>C:\Temp\security</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>FileDate</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>20000101</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>FileNum</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>0</DefaultValue>
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
      <Name>DB</Name>
      <Modifier>Input</Modifier>
      <Binding>None</Binding>
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
      <Name>SmtpUseSSL</Name>
      <Modifier>Input</Modifier>      
	  <Binding>None</Binding>
      <DefaultValue>notset</DefaultValue>
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
    <Variable>
      <Name>BatchSize</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    
  </Variables>
  
  <Steps>
  
    <InvokeMethod>
      <Name>Подготовка акций</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>assembly</Name>
        <AssemblyName>RapidSoft.VTB24.BankConnector</AssemblyName>
        <TypeName>RapidSoft.VTB24.BankConnector.EtlInvokeHelper</TypeName>
        <MethodName>PreparePromoAction</MethodName>
        <Parameters>
          <EtlMethodParameter>
            <Name>dateSent</Name>
            <Value>$(FileDate)</Value>
          </EtlMethodParameter>
          <EtlMethodParameter>
            <Name>indexSent</Name>
            <Value>$(FileNum)</Value>
          </EtlMethodParameter>
        </Parameters>
      </Source>
    </InvokeMethod>
     
    <ExportCsvFile>
      <Name>Выгрузка полученных акций в файл</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>Database</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>SELECT [PromoId],[Description],
		CONVERT(VARCHAR(19), [FromDate], 126) AS [FromDate],
		CONVERT(VARCHAR(19), [ToDate], 126) AS [ToDate] 
		FROM [dbo].[PromoAction] WHERE [EtlSessionId] = @sessionId</Text>
        <Parameters>
          <EtlQueryParameter>
            <Name>sessionId</Name>
            <Value>$(SessionId)</Value>
          </EtlQueryParameter>
        </Parameters>
      </Source>
      <Destination>
        <Name>Temp folder</Name>
        <!--FilePath>$(Temp)\VTB_$(SessionId)\VTB_$(FileDate)_$(FileNum).promoPL</FilePath-->
        <FilePath>$(Temp)\VTB_$(SessionId)\VTB_$(FileDate)_@count.promoPL</FilePath>
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
          <SourceFieldName>PromoId</SourceFieldName>
          <DestinationFieldName>PromoId</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>Description</SourceFieldName>
          <DestinationFieldName>Description</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>FromDate</SourceFieldName>
          <DestinationFieldName>FromDate</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>ToDate</SourceFieldName>
          <DestinationFieldName>ToDate</DestinationFieldName>
        </Mapping>
      </Mappings>
    </ExportCsvFile>
    
	<Encrypt>
      <Name>Шифрование данных</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <WorkingDirectory>$(Temp)\VTB_$(SessionId)</WorkingDirectory>
    </Encrypt>
	
    <EmailSend>
      <Name>Отправка файла</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>Файл</Name>
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
		<Subject>PromoAction_Request_$(FileDate)_$(FileNum)</Subject>
		<AttachmentFileMask>VTB_*.promoPL</AttachmentFileMask>
      </Message>
    </EmailSend>

  </Steps>
</EtlPackage>';
IF NOT EXISTS(SELECT 1 FROM [etl].[EtlPackages] WHERE Id = @PackageId)
INSERT [etl].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled]) VALUES (@PackageId, N'Отправка данных о акциях'
,@xml, 0, 1)
ELSE
UPDATE [etl].[EtlPackages] SET [Text]=@xml
WHERE [Id] = @PackageId