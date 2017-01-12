DECLARE @PackageId NVARCHAR(50) = N'3E505BA0-59A6-4093-8286-55501C61AE09';
DECLARE @Name NVARCHAR(255) = N'ReceiveAndRegisterBankOffers'
DECLARE @xml NVARCHAR(MAX) = N'<?xml version="1.0" encoding="utf-16"?>
<EtlPackage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>3E505BA0-59A6-4093-8286-55501C61AE09</Id>
  <Name>ReceiveAndRegisterBankOffers</Name>
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
            <SubjectContains>orderRegB</SubjectContains>
          </ReceiveFilter>
        </Filters>
        <AttachmentRegExp>^VTB_[1-2][0-9]{3}[0-1][0-9][0-3][0-9]_[0-9]+\.orderRegB$</AttachmentRegExp>
      </Message>
      <Destination>
        <Name>Temp Folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\Receive</FilePath>
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
      <WorkingDirectory>$(Temp)\VTB_$(SessionId)\Receive</WorkingDirectory>
      <ExceptionStepName>EmailMoveToBoxImap</ExceptionStepName>
    </Decrypt>

    <ImportCsvFileBatch>
      <Name>Чтение файлов вложений</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>Temp folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)\Receive</FilePath>
        <CodePage>1251</CodePage>
        <HasHeaders>true</HasHeaders>
        <FieldDelimiter>;</FieldDelimiter>
      </Source>
      <Destination>
        <Name>Database</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <TableName>dbo.RegisterBankOffers</TableName>
      </Destination>
      <BatchSize>0</BatchSize>
      <Mappings>
        <Mapping>
          <SourceFieldName>PartnerOrderNum</SourceFieldName>
          <DestinationFieldName>PartnerOrderNum</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>ArticleName</SourceFieldName>
          <DestinationFieldName>ArticleName</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>OrderBonusCost</SourceFieldName>
          <DestinationFieldName>OrderBonusCost</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>ExpirationDate</SourceFieldName>
          <DestinationFieldName>ExpirationDate</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>ProductId</SourceFieldName>
          <DestinationFieldName>ProductId</DestinationFieldName>
        </Mapping>
		<Mapping>
          <SourceFieldName>OfferId</SourceFieldName>
          <DestinationFieldName>OfferId</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>ClientID</SourceFieldName>
          <DestinationFieldName>ClientId</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>CardLast4Digits</SourceFieldName>
          <DestinationFieldName>CardLast4Digits</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>OrderAction</SourceFieldName>
          <DestinationFieldName>OrderAction</DestinationFieldName>
        </Mapping>
        <Mapping>
          <DestinationFieldName>EtlSessionId</DestinationFieldName>
          <DefaultValue>$(SessionId)</DefaultValue>
        </Mapping>
      </Mappings>
      <DataLossBehavior>Fail</DataLossBehavior>
    </ImportCsvFileBatch>

    <ExecuteQuery>
      <Name>Переносим запросы на повторное добавление продуктов в таблицу ответов</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>База данных</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>
          WITH Dublicates AS (
          SELECT rbo.[Id]
          ,rbo.[PartnerOrderNum]
          FROM [dbo].[RegisterBankOffers] AS rbo WITH(NOLOCK)
          INNER JOIN [dbo].[BankOffers] AS bo WITH(NOLOCK)
          ON
          bo.Id = rbo.[PartnerOrderNum]
          WHERE rbo.[EtlSessionId] = @sessionId AND rbo.[OrderAction] = ''REGORDER''
          )

          INSERT INTO [dbo].[RegisterBankOffersResponse]
          ([EtlSessionId],[ClientId],[PartnerOrderNum], [OrderActionResult], [InsertedDate])
          SELECT rbo.[EtlSessionId], rbo.[ClientId], rbo.[PartnerOrderNum], 3, GETDATE()
          FROM [dbo].[RegisterBankOffers] AS rbo WITH(NOLOCK)
          INNER JOIN Dublicates AS d
          ON rbo.[Id] = d.Id
        </Text>
        <Parameters>
          <EtlQueryParameter>
            <Name>sessionId</Name>
            <Value>$(SessionId)</Value>
          </EtlQueryParameter>
        </Parameters>
      </Source>
    </ExecuteQuery>

    <ExecuteQuery>
      <Name>Переносим запросы на удаление отсутствующих продуктов в таблицу ответов</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>База данных</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>
          WITH Missing AS (
          SELECT rbo.[PartnerOrderNum]
          FROM [dbo].[RegisterBankOffers] AS rbo WITH(NOLOCK)
          WHERE rbo.[EtlSessionId] = @sessionId AND rbo.[OrderAction] = ''DELORDER''
          EXCEPT
          SELECT [Id] FROM [dbo].[BankOffers] AS bo WITH(NOLOCK)
          )

          INSERT INTO [dbo].[RegisterBankOffersResponse]
          ([EtlSessionId],[ClientId],[PartnerOrderNum], [OrderActionResult], [InsertedDate])
          SELECT rbo.[EtlSessionId], rbo.[ClientId], rbo.[PartnerOrderNum], 2, GETDATE()
          FROM [dbo].[RegisterBankOffers] AS rbo WITH(NOLOCK)
          INNER JOIN Missing AS m
          ON
          rbo.[PartnerOrderNum] = m.PartnerOrderNum
          AND
          rbo.EtlSessionId = @sessionId
        </Text>
        <Parameters>
          <EtlQueryParameter>
            <Name>sessionId</Name>
            <Value>$(SessionId)</Value>
          </EtlQueryParameter>
        </Parameters>
      </Source>
    </ExecuteQuery>

    <ExecuteQuery>
      <Name>Удаляем продукты и пишем в таблицу ответов</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>База данных</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Text>
          WITH Deleting AS (
          SELECT rbo.[PartnerOrderNum]
          FROM [dbo].[RegisterBankOffers] AS rbo WITH(NOLOCK)
          WHERE rbo.[EtlSessionId] = @sessionId AND rbo.[OrderAction] = ''DELORDER''
          EXCEPT
          SELECT rbor.PartnerOrderNum FROM [dbo].[RegisterBankOffersResponse] AS rbor WITH(NOLOCK)
          WHERE rbor.[EtlSessionId] = @sessionId AND rbor.[OrderActionResult] = 2
          )

          DELETE dbo.BankOffers
          WHERE Id IN (SELECT PartnerOrderNum FROM Deleting);

          WITH Deleting1 AS (
          SELECT rbo.[PartnerOrderNum]
          FROM [dbo].[RegisterBankOffers] AS rbo WITH(NOLOCK)
          WHERE rbo.[EtlSessionId] = @sessionId AND rbo.[OrderAction] = ''DELORDER''
          EXCEPT
          SELECT rbor.PartnerOrderNum FROM [dbo].[RegisterBankOffersResponse] AS rbor WITH(NOLOCK)
          WHERE rbor.[EtlSessionId] = @sessionId AND rbor.[OrderActionResult] = 2
          )

          INSERT INTO [dbo].[RegisterBankOffersResponse]
          ([EtlSessionId],[ClientId],[PartnerOrderNum], [OrderActionResult], [InsertedDate])
          SELECT rbo.[EtlSessionId], rbo.[ClientId], rbo.[PartnerOrderNum], 0, GETDATE()
          FROM [dbo].[RegisterBankOffers] AS rbo WITH(NOLOCK)
          INNER JOIN Deleting1 AS d
          ON
          rbo.[PartnerOrderNum] = d.PartnerOrderNum
          AND
          rbo.EtlSessionId = @sessionId
        </Text>
        <Parameters>
          <EtlQueryParameter>
            <Name>sessionId</Name>
            <Value>$(SessionId)</Value>
          </EtlQueryParameter>
        </Parameters>
      </Source>
    </ExecuteQuery>

    <InvokeMethod>
      <Name>Вызов оркестратора</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>assembly</Name>
        <AssemblyName>RapidSoft.VTB24.BankConnector</AssemblyName>
        <TypeName>RapidSoft.VTB24.BankConnector.EtlInvokeHelper</TypeName>
        <MethodName>ProcessBankOffers</MethodName>
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
      <Name>BuildResponseFileName</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Text>
          SELECT TOP 1 [FileName] + ''.response'' as ResponseFileName
          FROM [etl].[EtlIncomingMailAttachments]
          WHERE EtlSessionId = @sessionId
          order by [SeqId] desc
        </Text>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <Parameters>
          <EtlQueryParameter>
            <Name>sessionId</Name>
            <Value>$(SessionId)</Value>
          </EtlQueryParameter>
        </Parameters>
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
        <Text>SELECT RBOR.[PartnerOrderNum], RBOR.[ClientId], RBOR.[OrderActionResult] FROM [dbo].[RegisterBankOffersResponse] AS RBOR WITH(NOLOCK)
INNER JOIN [dbo].[RegisterBankOffers] AS RBO WITH(NOLOCK)
ON
RBO.EtlSessionId = RBOR.EtlSessionId
AND
RBO.PartnerOrderNum = RBOR.PartnerOrderNum
WHERE RBOR.[EtlSessionId] = @sessionId
ORDER BY RBO.Id ASC</Text>
        <Parameters>
          <EtlQueryParameter>
            <Name>sessionId</Name>
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
          <SourceFieldName>PartnerOrderNum</SourceFieldName>
          <DestinationFieldName>PartnerOrderNum</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>ClientId</SourceFieldName>
          <DestinationFieldName>ClientId</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>OrderActionResult</SourceFieldName>
          <DestinationFieldName>OrderActionResult</DestinationFieldName>
        </Mapping>
      </Mappings>
      <EndSessionOnEmptySource>true</EndSessionOnEmptySource>
    </ExportCsvFile>

    <Encrypt>
      <Name>Encrypt</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <WorkingDirectory>$(Temp)\VTB_$(SessionId)\Response</WorkingDirectory>
    </Encrypt>

    <EmailSend>
      <Name>Отправка письма с ответными файлами</Name>
      <TimeoutMilliseconds xsi:nil="true" />
      <Source>
        <Name>Temp Folder</Name>
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
        <Subject>RegisterBankOffers_Response_$(EtlYear)_$(EtlMonth)_$(EtlDay)</Subject>
        <AttachmentFileMask>VTB_*.orderRegB.response</AttachmentFileMask>
      </Message>
    </EmailSend>
  </Steps>
</EtlPackage>'

IF NOT EXISTS(SELECT 1 FROM [etl].[EtlPackages] WHERE Id = @PackageId)
        INSERT [etl].[EtlPackages] ([Id], [Name], [Text], [RunIntervalSeconds], [Enabled])
        VALUES (@PackageId, @Name, @xml, 0, 1)
ELSE
        UPDATE [etl].[EtlPackages] SET [Text]=@xml,[Name]=@Name
        WHERE [Id] = @PackageId
