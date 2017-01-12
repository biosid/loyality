DECLARE @PackageId NVARCHAR(50) = N'777BE283-09F1-4528-BD59-3BA5622FC3AD';
DECLARE @xml NVARCHAR(MAX) = N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>777be283-09f1-4528-bd59-3ba5622fc3ad</Id>
  <Name>LoyaltyClientUpdate</Name>
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
    <Variable>
      <Name>ColNames</Name>
      <DefaultValue>notset</DefaultValue>
      <Modifier>Output</Modifier>
    </Variable>	
    <Variable>
      <Name>ColNamesForSelect</Name>
      <DefaultValue>notset</DefaultValue>
      <Modifier>Output</Modifier>
    </Variable>
    <Variable>
      <Name>ColNamesForSelectTerm</Name>
      <DefaultValue>notset</DefaultValue>
      <Modifier>Output</Modifier>
    </Variable>		
  </Variables>
  <Steps>
  
	<ExecuteQuery>
      <Name>GetCustomFieldNames</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Text>
		DECLARE @colNames NVARCHAR(MAX);
		SET @colNames = STUFF((SELECT distinct '','' + QUOTENAME([Name])
		FROM [dbo].ProfileCustomFields 
		FOR XML PATH(''''), TYPE).value(''.'', ''NVARCHAR(MAX)'')
		,1,1,'''')
		SELECT ISNULL(@colNames, ''[columnThatNotExist]'') AS ColNames
		</Text>
		<ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
      </Source>
      <OutputVariables>
        <FirstRow>
          <FieldToVariable>
            <SourceFieldName>ColNames</SourceFieldName>
            <VariableName>ColNames</VariableName>
            <DefaultValue>notset</DefaultValue>
          </FieldToVariable>
        </FirstRow>
      </OutputVariables>
    </ExecuteQuery>

	<ExecuteQuery>
      <Name>GetCustomFieldNamesForSelect</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Text>
		DECLARE @colNamesForSelect NVARCHAR(MAX);
		SET @colNamesForSelect = (SELECT distinct '',MAX('' + QUOTENAME([Name]) + '') AS '' + QUOTENAME([Name])
		FROM [dbo].ProfileCustomFields 
		FOR XML PATH(''''), TYPE).value(''.'', ''NVARCHAR(MAX)'')
		SELECT ISNULL(@colNamesForSelect, '''') AS ColNamesForSelect
		</Text>
		<ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
      </Source>
      <OutputVariables>
        <FirstRow>
          <FieldToVariable>
            <SourceFieldName>ColNamesForSelect</SourceFieldName>
            <VariableName>ColNamesForSelect</VariableName>
            <DefaultValue>notset</DefaultValue>
          </FieldToVariable>
        </FirstRow>
      </OutputVariables>
    </ExecuteQuery>

	<ExecuteQuery>
      <Name>GetCustomFieldNamesForSelectTerm</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Text>
		DECLARE @colNamesForSelectTerm NVARCHAR(MAX);
		SET @colNamesForSelectTerm = (SELECT distinct '',t.'' + QUOTENAME([Name])
		FROM [dbo].ProfileCustomFields 
		FOR XML PATH(''''), TYPE).value(''.'', ''NVARCHAR(MAX)'')
		SELECT ISNULL(@colNamesForSelectTerm, '''') AS ColNamesForSelectTerm
		</Text>
		<ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
      </Source>
      <OutputVariables>
        <FirstRow>
          <FieldToVariable>
            <SourceFieldName>ColNamesForSelectTerm</SourceFieldName>
            <VariableName>ColNamesForSelectTerm</VariableName>
            <DefaultValue>notset</DefaultValue>
          </FieldToVariable>
        </FirstRow>
      </OutputVariables>
    </ExecuteQuery>	
	
    <ExportCsvFile>
      <Name>Формирование файла с реестром измененых клиентов</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <EndSessionOnEmptySource>true</EndSessionOnEmptySource>
      <Source>
        <Name>БД</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>
		UPDATE [dbo].[LoyaltyClientUpdates] SET [SendEtlSessionId] = @sessionId
				WHERE [SendEtlSessionId] IS NULL AND [UpdateStatus] = 0; 

			DECLARE @ClientsCustomFields TABLE
			  (
				[FieldId] int,
				[ClientId] nvarchar(36),
				[Value] nvarchar(MAX),
				[Name] nvarchar(256)
			  )
			  
			INSERT INTO @ClientsCustomFields ([FieldId], [ClientId], [Value], [Name])
			SELECT [FieldId], [ClientId], [Value], [Name]
			FROM [dbo].[ProfileCustomFieldsValues] vals
			LEFT JOIN [dbo].ProfileCustomFields fields
			ON vals.FieldId = fields.Id; 
			
			WITH LastLoyaltyClientUpdates AS
				(
				SELECT [ClientId], MAX([SeqId]) AS [MaxSeqId]
				FROM [dbo].[LoyaltyClientUpdates]
				WHERE [SendEtlSessionId] = @sessionId AND [UpdateStatus] = 0
				GROUP BY [ClientId]
				)
			SELECT 
				u.[ClientId],
				u.[LastName],
				u.[FirstName],
				u.[MiddleName],
				CONVERT(VARCHAR(10), u.[BirthDate], 120) AS [BirthDate],
				ISNULL(u.[Gender], ''0'') AS [Gender],
				u.[Email],
				SUBSTRING(u.[MobilePhone], 2, LEN(u.[MobilePhone])) AS [MobilePhone],
				u.[ChangedBy]
				$(ColNamesForSelectTerm)
			FROM [dbo].[LoyaltyClientUpdates] u
			JOIN LastLoyaltyClientUpdates lu ON u.[SeqId] = lu.[MaxSeqId]
			LEFT JOIN (SELECT MAX([ClientId]) AS ClientId $(ColNamesForSelect)
						FROM 
						(
							 SELECT FieldId, ClientId, Value, Name
							 FROM @ClientsCustomFields
						) x
						PIVOT 
						(
							MAX(Value)
							FOR [Name] IN ($(ColNames))
						) p
						GROUP BY [ClientId]) t
						ON u.[ClientId]=t.[ClientId];        
        </Text>
        <Parameters>
          <EtlQueryParameter>
            <Name>sessionId</Name>
            <Value>$(SessionId)</Value>
          </EtlQueryParameter>
        </Parameters>
      </Source>
      <Destination>
        <Name>Temp folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\VTB_$(EtlYear)$(EtlMonth)$(EtlDay)_@count.loyanketaPL</FilePath>
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
    </ExportCsvFile>
    <Encrypt>
      <Name>Зашифровать файл</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <WorkingDirectory>$(Temp)\VTB_$(SessionId)</WorkingDirectory>
    </Encrypt>
    <EmailSend>
      <Name>Отправить файл по почте</Name>
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
      <EmailDbStorage>
        <ConnectionString>$(DB)</ConnectionString>
        <SchemaName>etl</SchemaName>
      </EmailDbStorage>
      <Message>
        <From>$(MailToTeradataFrom)</From>
        <To>
          <string>$(MailToTeradataTo)</string>
        </To>
        <Subject>Update_$(EtlYear)_$(EtlMonth)_$(EtlDay)</Subject>
        <AttachmentFileMask>VTB_*.loyanketaPL</AttachmentFileMask>
      </Message>
    </EmailSend>
    <ExecuteQuery>
      <Name>Удаление персональных данных</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>База данных</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>UPDATE [dbo].[LoyaltyClientUpdates]   SET [FirstName] = N''N/A'',[LastName] = N''N/A'',[MiddleName] = N''N/A'',[MobilePhone] = N''N/A'',[Email] = N''N/A''</Text>
        <Parameters>
          <EtlQueryParameter>
            <Name>sessionId</Name>
            <Value>$(SessionId)</Value>
          </EtlQueryParameter>
        </Parameters>
      </Source>
    </ExecuteQuery>
  </Steps>
</EtlPackage>';

UPDATE [etl].[EtlPackages] SET [Text]=@xml WHERE [Id] = @PackageId