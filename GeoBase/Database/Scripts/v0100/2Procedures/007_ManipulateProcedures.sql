/************************************************************
* AddEtlStepSource.sql
************************************************************/

MergeUtilsDropSPIfExist '[dbo].[AddEtlStepSource]';
GO

CREATE PROCEDURE [dbo].[AddEtlStepSource]
(
    @Name             NVARCHAR(250),
    @EtlSessionId     UNIQUEIDENTIFIER,
    @EtlPackageId     UNIQUEIDENTIFIER,
    @EtlStepId        UNIQUEIDENTIFIER,
    @Content          NVARCHAR(MAX) = NULL,
    @IsDestination    BIT,
    @Type             NVARCHAR(255),
    @Path             NVARCHAR(2000),
    @IsBase64Content  BIT
)
AS
BEGIN
	
	DECLARE @size BIGINT
	IF @Content IS NOT NULL
	BEGIN
		SET @size = LEN(@Content)
	END
	
	INSERT INTO [dbo].[EtlStepSources]
	  (
	    [EtlPackageId],
	    [EtlSessionId],
	    [EtlStepId],
	    [Name],
	    [IsDestination],
	    [Type],
	    [Path],
	    [LogDateTime],
	    [LogUtcDateTime],
	    [IsBase64Content],
	    [ContentSizeBytes],
	    [Content]
	  )
	VALUES
	  (
	    @EtlPackageId,
	    @EtlSessionId,
	    @EtlStepId,
	    @Name,
	    @IsDestination,
	    @Type,
	    @Path,
	    GETDATE(),
	    GETUTCDATE(),
	    @IsBase64Content,
	    @size,
	    @Content
	  )
END
GO



GO

/************************************************************
* AddSynchronizationJob.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[AddSynchronizationJob]';
GO




CREATE PROCEDURE [dbo].[AddSynchronizationJob]
(
	@ProcedureName				nvarchar(250),
	@EtlSessionId				uniqueidentifier,
	@EtlPackageId				uniqueidentifier,
	@XmlData					nvarchar(MAX) = null
)
AS
BEGIN
	
	insert into dbo.SynchronizationJobs(Id, ProcedureName, EtlSessionId, EtlPackageId, XmlData, CreatedDateTime, CreatedUtcDateTime)
	values (newid(), @ProcedureName, @EtlSessionId, @EtlPackageId, @XmlData, GETDATE(), GETUTCDATE())

END



GO

/************************************************************
* MergeUtilsCreateColumnIfNotExist.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[MergeUtilsCreateColumnIfNotExist]';
GO

CREATE PROCEDURE [dbo].MergeUtilsCreateColumnIfNotExist
	@TableName NVARCHAR(128),
	@ColumnName NVARCHAR(128),
	@TypeSQL NVARCHAR(256)
AS
BEGIN
	DECLARE @S NVARCHAR(1024)


	IF dbo.MergeUtilsCheckColumnExist(@TableName, @ColumnName) = 0
	BEGIN	
		SET @S = 'ALTER TABLE ' + @TableName + ' ADD ' + @ColumnName + ' ' +  @TypeSQL + ';';
		EXEC MergeUtilsExecSQL @S;		
	END
END



GO

/************************************************************
* MergeUtilsCreateFK.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[MergeUtilsCreateFK]';
GO

--Создание внешнего ключа
CREATE PROCEDURE [MergeUtilsCreateFK]
(
    @TableNameForeign NVARCHAR(128),
    @ColumnNameForeign NVARCHAR(128),
    @TableNamePrimary NVARCHAR(128),
    @ColumnNamePrimary NVARCHAR(128)
)
AS
BEGIN
	DECLARE @S NVARCHAR(1024)
	
	SET @S = 'ALTER TABLE ' + @TableNameForeign + ' WITH CHECK ADD CONSTRAINT
		 [FK_' + [dbo].[RuntimeUtilsGetPureTableName](@TableNameForeign) + '_' + [dbo].[RuntimeUtilsGetPureTableName](@TableNamePrimary) + ']
		 FOREIGN KEY ([' + @ColumnNameForeign + ']) REFERENCES ' + @TableNamePrimary + '([' + @ColumnNamePrimary + '])'

	EXEC MergeUtilsExecSQL @S;
END;


GO

/************************************************************
* MergeUtilsDropColumnIfExist.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[MergeUtilsDropColumnIfExist]';
GO

CREATE PROCEDURE [dbo].[MergeUtilsDropColumnIfExist]
	@Table NVARCHAR(128),
	@Name NVARCHAR(128)
AS
BEGIN
	IF dbo.MergeUtilsCheckColumnExist(@Table, @Name) = 1
	BEGIN
		DECLARE @S NVARCHAR(256)
		SET @S = 'ALTER TABLE ' + @Table + ' DROP COLUMN ' + @Name;
		EXEC MergeUtilsExecSQL @S;
	END
		
END


GO

/************************************************************
* MergeUtilsDropFK.sql
************************************************************/

MergeUtilsDropSPIfExist '[dbo].[MergeUtilsDropFK]';
GO
--Удаление всех внешних ключей из таблицы
CREATE PROCEDURE [dbo].[MergeUtilsDropFK]
	@TableName NVARCHAR(128)
AS
BEGIN

	DECLARE keys_cursor CURSOR FOR 
	SELECT FKName FROM
	(	SELECT
			  OBJECT_NAME(constraint_object_id) AS FKName,
			  OBJECT_NAME(parent_object_id) AS TableName
			FROM
			  sys.foreign_key_columns  ) t
	WHERE t.TableName = [dbo].RuntimeUtilsGetPureTableName(@TableName);

	OPEN keys_cursor;

	DECLARE @FKName NVARCHAR(MAX);
	DECLARE @S NVARCHAR(MAX);

	FETCH NEXT FROM keys_cursor INTO @FKName;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @S = N'ALTER TABLE ' + @TableName + N' DROP CONSTRAINT ' + @FKName;
		EXEC [dbo].MergeUtilsExecSQL @S;
		FETCH NEXT FROM keys_cursor INTO @FKName;
	END;

	CLOSE keys_cursor;
	DEALLOCATE keys_cursor;
END;
GO

/************************************************************
* MergeUtilsDropFuncIfExist.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[MergeUtilsDropFuncIfExist]';
GO
CREATE PROCEDURE [dbo].[MergeUtilsDropFuncIfExist]
	@Name NVARCHAR(128)
AS
BEGIN
	IF(dbo.MergeUtilsCheckFuncExist(@Name) = 1)
	BEGIN
		DECLARE @S NVARCHAR(256)
		SET @S = 'DROP FUNCTION ' + @Name;
		EXEC MergeUtilsExecSQL @S;
	END
		
END





GO

/************************************************************
* MergeUtilsDropFuncIfExistAndCreateSynonym.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[MergeUtilsDropFuncIfExistAndCreateSynonym]';
GO
CREATE PROCEDURE [dbo].[MergeUtilsDropFuncIfExistAndCreateSynonym]
	@FName NVARCHAR(128),
	@NewFName NVARCHAR(128)
AS
BEGIN
	IF OBJECT_ID(@FName) = OBJECT_ID(@NewFName)
	BEGIN
		RETURN
	END
	
	EXEC MergeUtilsDropFuncIfExist @FName;
	DECLARE @S NVARCHAR(256)
	IF  EXISTS (SELECT * FROM sys.synonyms WHERE object_id = OBJECT_ID(@FName))
	BEGIN
		SET @S = 'DROP SYNONYM ' + @FName;
		EXEC MergeUtilsExecSQL @S;
	END
	
	SET @S = 'CREATE SYNONYM ' + @FName + ' FOR ' + @NewFName;
	EXEC MergeUtilsExecSQL @S;
END


GO

/************************************************************
* MergeUtilsDropSPIfExist.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[MergeUtilsDropSPIfExist]';
GO
CREATE PROCEDURE [dbo].[MergeUtilsDropSPIfExist]
	@Name NVARCHAR(128)
AS
BEGIN
	IF(dbo.MergeUtilsCheckSPExist(@Name) = 1)
	BEGIN
		DECLARE @S NVARCHAR(256)
		SET @S = 'DROP PROCEDURE ' + @Name;
		EXEC MergeUtilsExecSQL @S;
	END
		
END


GO

/************************************************************
* MergeUtilsDropSPIfExistAndCreateSynonym.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[MergeUtilsDropSPIfExistAndCreateSynonym]';
GO
CREATE PROCEDURE [dbo].[MergeUtilsDropSPIfExistAndCreateSynonym]
	@SPName NVARCHAR(128),
	@NewSPName NVARCHAR(128)
AS
BEGIN
	IF OBJECT_ID(@SPName) = OBJECT_ID(@NewSPName)
	BEGIN
		RETURN
	END
	
	EXEC MergeUtilsDropSPIfExist @SPName;
	DECLARE @S NVARCHAR(256)
	IF  EXISTS (SELECT * FROM sys.synonyms WHERE object_id = OBJECT_ID(@SPName))
	BEGIN
		SET @S = 'DROP SYNONYM ' + @SPName;
		EXEC MergeUtilsExecSQL @S;
	END
	
	SET @S = 'CREATE SYNONYM ' + @SPName + ' FOR ' + @NewSPName;
	EXEC MergeUtilsExecSQL @S;
END


GO

/************************************************************
* MergeUtilsDropTableIfExist.sql
************************************************************/

MergeUtilsDropSPIfExist '[dbo].[MergeUtilsDropTableIfExist]';
GO

CREATE PROCEDURE [dbo].[MergeUtilsDropTableIfExist]
	@Table NVARCHAR(MAX)
AS
BEGIN
	DECLARE @sql NVARCHAR(MAX)
	
	WHILE EXISTS(SELECT * FROM sys.foreign_keys WHERE referenced_object_id = OBJECT_ID(@Table))
	BEGIN
	    SELECT TOP 1 @sql = 'ALTER TABLE ' + SCHEMA_NAME(SCHEMA_ID) + '.' +
	           OBJECT_NAME(parent_object_id) + ' DROP CONSTRAINT ' + NAME
	    FROM   sys.foreign_keys
	    WHERE  referenced_object_id = OBJECT_ID(@Table)
	    
	    EXEC dbo.MergeUtilsExecSQL @sql
	END
	
	IF ([dbo].MergeUtilsCheckTableExist(@Table) = 1)
	BEGIN
	    SET @sql = 'DROP TABLE ' + @Table
	    EXEC dbo.MergeUtilsExecSQL @sql
	END
END


GO

/************************************************************
* MergeUtilsDropTriggerIfExist.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[MergeUtilsDropTriggerIfExist]';
GO

CREATE PROCEDURE [dbo].[MergeUtilsDropTriggerIfExist]
	@Name NVARCHAR(128)
AS
BEGIN
	IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(@Name))
	BEGIN
		DECLARE @S NVARCHAR(256)
		SET @S = 'DROP TRIGGER ' + @Name;
		EXEC MergeUtilsExecSQL @S;
	END
		
END


GO

/************************************************************
* MergeUtilsDropViewIfExist.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[MergeUtilsDropViewIfExist]';
GO
CREATE PROCEDURE [dbo].[MergeUtilsDropViewIfExist]
	@Name NVARCHAR(128)
AS
BEGIN
	IF(dbo.[MergeUtilsCheckViewExist](@Name) = 1)
	BEGIN
		DECLARE @S NVARCHAR(256)
		SET @S = 'DROP VIEW ' + @Name;
		EXEC MergeUtilsExecSQL @S;
	END
		
END
GO


GO

/************************************************************
* MergeUtilsExecSQL.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[MergeUtilsExecSQL]';
GO

CREATE PROCEDURE [dbo].[MergeUtilsExecSQL]
	@SQL NTEXT
AS
BEGIN
	IF ISNULL(DATALENGTH(@SQL), 0) = 0
	BEGIN
		RAISERROR(N'Query string cannot be null', 16, 2)
		RETURN
	END

	EXEC sp_executesql @SQL
END




GO

/************************************************************
* ProcessSynchroznizationJobs.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[ProcessSynchroznizationJobs]';
GO



CREATE PROCEDURE [dbo].[ProcessSynchroznizationJobs]
AS
BEGIN

	declare @SynchroznizationJobId uniqueidentifier
	declare @ProcedureName nvarchar(MAX)	
	declare @EtlSessionId uniqueidentifier
	declare @EtlPackageId uniqueidentifier		
	declare @XmlData nvarchar(MAX)
	
	declare @query nvarchar(MAX)
	declare @error bit
	
	while exists (select * from dbo.SynchronizationJobs)
	begin
		set @error = 0
		
		select top 1
			@SynchroznizationJobId = Id,
			@ProcedureName = ProcedureName,			
			@EtlSessionId = EtlSessionId,
			@EtlPackageId = EtlPackageId,
			@XmlData = XmlData
		from
			dbo.SynchronizationJobs
		order by 
			SequentalId
		
		begin try
			set @query = 
				N'exec ' + @ProcedureName + 
				N' @EtlSessionId = ''' + cast(@EtlSessionId as nvarchar(MAX)) + N'''' +
				N',@EtlPackageId = ''' + cast(@EtlPackageId as nvarchar(MAX)) + N'''' +
				case when @XmlData is null then N'' else N',@XmlDataParam = N''' + cast(REPLACE(@XmlData,'''','&apos;') as nvarchar(MAX)) + N'''' end
				
			waitfor delay N'00:00:01'				
	
			exec MergeUtilsExecSQL @query;
		
		end try
		begin catch
			set @error = 1
		
			if (ERROR_SEVERITY() < 17)
			begin
				declare @packId uniqueidentifier
				select @packId = EtlPackageId from EtlSessions where EtlSessionId = @EtlSessionId
			
				INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
				VALUES(@packId, @etlSessionId, GETDATE(), GETUTCDATE(), 5, ERROR_MESSAGE());

				INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
				VALUES(@packId, @etlSessionId, GETDATE(), GETUTCDATE(), 2, N'Непредвиденая ошибка в процедуре синхронизации');
			end			
		end catch

		update	dbo.EtlSessions
		set		EndDateTime = GETDATE(),
				EndUtcDateTime = GETUTCDATE(),
				EndMessage = N'Сессия завершена',
				[Status] = case when @error = 0 then 2 else 5 end
		where	EtlSessionId = @EtlSessionId

		delete from dbo.SynchronizationJobs
		where Id = @SynchroznizationJobId

	end

END
GO

/************************************************************
* RuntimeUtilsGenerateRandomString.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[RuntimeUtilsGenerateRandomString]';
GO

CREATE PROCEDURE [dbo].[RuntimeUtilsGenerateRandomString]
(
    @Length int
)
AS
BEGIN
	DECLARE @ValidCharacters varchar(255);
	SET @ValidCharacters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-=+&$ ';
	DECLARE @ValidCharactersLength int;
	SET @ValidCharactersLength = len(@ValidCharacters);
	DECLARE @CurrentCharacter varchar(1);
	SET @CurrentCharacter = '';
	DECLARE @RandomNumberInt tinyint;
	SET @RandomNumberInt = 0;

	SET @RandomNumberInt = Convert(tinyint, ((@ValidCharactersLength - 1) * Rand() + 1));

	SET @CurrentCharacter = SUBSTRING(@ValidCharacters, @RandomNumberInt, 1);

	SELECT REPLICATE(@CurrentCharacter, @Length)
END


GO

/************************************************************
* RuntimeUtilsGenerateRandomStringFloatLength.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[RuntimeUtilsGenerateRandomStringFloatLength]';
GO
CREATE PROCEDURE [dbo].[RuntimeUtilsGenerateRandomStringFloatLength]
	@MaxLength int	
AS
BEGIN
	SET @MaxLength = (ABS(CHECKSUM(NEWID())) % @MaxLength) + 1;
		
	DECLARE @ValidCharacters varchar(255);
	SET @ValidCharacters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-=+&$ ';

	DECLARE @RandomNumberInt tinyint;
	SET @RandomNumberInt = Convert(tinyint, ((len(@ValidCharacters) - 1) * Rand() + 1));

	DECLARE @CurrentCharacter varchar(1);
	SET @CurrentCharacter = SUBSTRING(@ValidCharacters, @RandomNumberInt, 1);

	SELECT REPLICATE(@CurrentCharacter, @MaxLength)
END


GO

/************************************************************
* RuntimeUtilsRefreshViewByLatestTable.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[RuntimeUtilsRefreshViewByLatestTable]';
GO


CREATE PROCEDURE [dbo].RuntimeUtilsRefreshViewByLatestTable
	@BaseTableName NVARCHAR(128)
AS
BEGIN

	DECLARE @ViewName NVARCHAR(128);
	SET @ViewName = @BaseTableName + '_VIEW';

	DECLARE @ShemaName NVARCHAR(128);
	SET @ShemaName = [dbo].[RuntimeUtilsGetSchemaName](@BaseTableName);

	DECLARE @TableName NVARCHAR(128);
	SET @TableName = [dbo].[ShemaUtilsGetLastestTableName](@BaseTableName);
	SET @TableName = @ShemaName + '.' + @TableName;
	
	EXEC [dbo].[RuntimeUtilsSetViewDefinition] @ViewName, @TableName
END



GO

/************************************************************
* RuntimeUtilsSetViewDefinition.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[RuntimeUtilsSetViewDefinition]';
GO
CREATE PROCEDURE [dbo].[RuntimeUtilsSetViewDefinition]
	@ViewName NVARCHAR(128),
	@TableName NVARCHAR(128)
AS
BEGIN
	DECLARE @S NVARCHAR(256)
	IF EXISTS (
	       SELECT *
	       FROM   sys.views
	       WHERE  OBJECT_ID = OBJECT_ID(@ViewName)
	   )
	BEGIN
	    SET @S = '
			--#' + @TableName + '#
			ALTER VIEW ' + @ViewName + '
			AS
			SELECT *
			FROM  ' + @TableName + ' WITH(NOLOCK)';
	    
	    EXEC MergeUtilsExecSQL @S;
	END
	ELSE
	BEGIN
	    SET @S = '
			--#' + @TableName + '#
			CREATE VIEW ' + @ViewName + '
			AS
			SELECT *
			FROM  ' + @TableName + ' WITH(NOLOCK)';
	    
	    EXEC MergeUtilsExecSQL @S;
	END
END


GO

/************************************************************
* RuntimeUtilsSplitString.sql
************************************************************/

EXEC [dbo].MergeUtilsDropFuncIfExist N'[dbo].RuntimeUtilsSplitString';
GO
CREATE FUNCTION [dbo].RuntimeUtilsSplitString(@String AS VARCHAR(8000), @sep AS CHAR(1))
  RETURNS TABLE
AS
	RETURN
		WITH Pieces(pn, START, STOP)
		AS (
			SELECT 1, 1, CHARINDEX(@sep, @String)
			UNION ALL
			SELECT pn + 1 AS pn, STOP + 1 AS START, CHARINDEX(@sep, @String, STOP + 1) AS STOP
			FROM   Pieces p
			WHERE  p.STOP > 0
		)
		SELECT Substr FROM
		(
		SELECT CAST(SUBSTRING(@String, START, CASE WHEN STOP > 0 THEN STOP -START ELSE 1000 END) AS NVARCHAR(100)) AS Substr
			FROM Pieces) AS t
		WHERE LEN(t.Substr) > 0
GO

/************************************************************
* RuntimeUtilsUpdateAllViewsInScheme.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[RuntimeUtilsUpdateAllViewsInScheme]';
GO
CREATE PROCEDURE [dbo].[RuntimeUtilsUpdateAllViewsInScheme]
	@SchemaName NVARCHAR(128)
AS
BEGIN
DECLARE @viewName NVARCHAR(128)
DECLARE @tableName NVARCHAR(128)
DECLARE cur CURSOR  
FOR
    SELECT TABLE_SCHEMA + '.' + TABLE_NAME
    FROM   INFORMATION_SCHEMA.[VIEWS]
    WHERE  TABLE_SCHEMA = @SchemaName

	OPEN cur
	FETCH NEXT FROM cur
	INTO @viewName
	
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @tableName = [dbo].[RuntimeUtilsGetViewTableName] (@viewName);
    
    EXEC [dbo].[RuntimeUtilsSetViewDefinition] @viewName, @tableName
    
    FETCH NEXT FROM cur
    INTO @viewName
END
	CLOSE cur
	DEALLOCATE cur
END


GO

/************************************************************
* SchemaUtilsApplyStandardColumns.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[SchemaUtilsApplyStandardColumns]';
GO

CREATE PROCEDURE [dbo].[SchemaUtilsApplyStandardColumns]
	@TableName NVARCHAR(128),
	@withDates bit,
	@withEtl bit
AS
BEGIN
	
	EXEC dbo.MergeUtilsCreateColumnIfNotExist @TableName, 'CreatedDateTime', '[datetime] NULL';
	EXEC dbo.MergeUtilsCreateColumnIfNotExist @TableName, 'CreatedUtcDateTime', '[datetime] NULL';
	EXEC dbo.MergeUtilsCreateColumnIfNotExist @TableName, 'ModifiedDateTime', '[datetime] NULL';
	EXEC dbo.MergeUtilsCreateColumnIfNotExist @TableName, 'ModifiedUtcDateTime', '[datetime] NULL';

	EXEC dbo.MergeUtilsCreateColumnIfNotExist @TableName, 'EtlPackageId', '[uniqueidentifier] NULL';
	EXEC dbo.MergeUtilsCreateColumnIfNotExist @TableName, 'EtlSessionId', '[uniqueidentifier] NULL';
END



GO

/************************************************************
* SchemaUtilsApplyVersionColumns.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[SchemaUtilsApplyVersionColumns]';
GO
--Добавляет колонки версии для таблицы
CREATE PROCEDURE [dbo].[SchemaUtilsApplyVersionColumns]
	@TableName NVARCHAR(128)
AS
BEGIN
	
	EXEC dbo.MergeUtilsCreateColumnIfNotExist @TableName, 'VersionId', '[int] IDENTITY(1, 1) NOT NULL';
	EXEC dbo.MergeUtilsCreateColumnIfNotExist @TableName, 'Deleted', '[bit] NOT NULL';	
END



GO

/************************************************************
* SchemaUtilsDropOldTables.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[SchemaUtilsDropOldTables]';
GO

CREATE PROCEDURE dbo.SchemaUtilsDropOldTables
	@basetablename NVARCHAR(128)
AS
BEGIN
	DECLARE @sql NVARCHAR(1024);
	DECLARE @Res NVARCHAR(128);
	DECLARE @T NVARCHAR(128);
	DECLARE @CurTable NVARCHAR(128);
	DECLARE @CurSchema NVARCHAR(128);

	SET @T = [dbo].[RuntimeUtilsGetPureTableName](@basetablename);

	SET @CurTable = [dbo].[ShemaUtilsGetLastestTableName] (@basetablename);
	SET @CurSchema = dbo.RuntimeUtilsGetSchemaName (@basetablename);


	DECLARE @DropTablesCursor CURSOR
	SET @DropTablesCursor = CURSOR FOR
	SELECT Name FROM sys.objects WHERE CHARINDEX(LOWER(@T + '_'), LOWER(name)) = 1 
	AND LOWER(@T + '_history') <> LOWER(name)
	AND LOWER(@T + '_buffer') <> LOWER(name)
	AND type in (N'U') 
	AND LOWER(name) <> [dbo].[RuntimeUtilsGetPureTableName](@CurTable)
	
	OPEN @DropTablesCursor;


	DECLARE @droptablename NVARCHAR(128);
	FETCH NEXT FROM @DropTablesCursor INTO @droptablename;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @sql = 'DROP TABLE ' + @CurSchema + '.' + @droptablename + ' ;';
		EXEC dbo.MergeUtilsExecSQL @sql;
		FETCH NEXT FROM @DropTablesCursor INTO @droptablename;

	END
	CLOSE @DropTablesCursor

		
END

GO

/************************************************************
* SchemaUtilsDropPrimaryKey.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[SchemaUtilsDropPrimaryKey]';
GO

CREATE PROCEDURE [dbo].[SchemaUtilsDropPrimaryKey]
    @tableName NVARCHAR(255)
AS
    DECLARE @pkName NVARCHAR(255);

    SET @pkName= (
        SELECT [name] FROM sysobjects
            WHERE [xtype] = 'PK'
            AND [parent_obj] = OBJECT_ID(@tableName)
    )

    DECLARE @dropSql varchar(4000)

    SET @dropSql=
        'ALTER TABLE ' + @tableName + '
            DROP CONSTRAINT [' + @PkName + ']';

    EXEC MergeUtilsExecSQL @dropSql;


GO

/************************************************************
* SchemaUtilsInitVersionTable.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[SchemaUtilsInitVersionTable]';
GO

--создает и инициализирует таблицу для хранения версий. 
--@PureTableName - чистое имя таблицы: [shema].[table]; 
--@CreateTableProc - имя процедуры для создания базовой таблицы. ДОЛЖНА ПОДДЕРЖИВАТЬ СОЗДАНИЕ ТАБЛИЦЫ БЕЗ IDENTITY!:  [shema].EntityCreateTable
--@ColumnsSQL - колонки которые нужну скопировать из основной таблицы в версионую: Id, Name, Code, Date
--Example: EXEC [dbo].SchemaUtilsInitVersionTable '[bankdict].CardLockReason', '[bankdict].CardLockReasonCreateTable', 'Id, SiebelCode, RBOCode, Description';
CREATE PROCEDURE [dbo].[SchemaUtilsInitVersionTable]
	@PureTableName NVARCHAR(128),
	@CreateTableProc NVARCHAR(128),
	@ColumnsSQL NVARCHAR(2048)
AS
BEGIN
	DECLARE @VersionTableName NVARCHAR(128);
	SET @VersionTableName = @PureTableName + '_HISTORY';

	DECLARE @ViewName NVARCHAR(128);
	SET @ViewName = @PureTableName + '_VIEW';
	
	IF([dbo].[MergeUtilsCheckTableExist] (@VersionTableName) = 0)
	BEGIN
		DECLARE @sql NVARCHAR(2048);		
		SET @sql = 'EXEC ' + @CreateTableProc + ' ''' + @VersionTableName + ''', 1;';		
		EXEC MergeUtilsExecSQL @sql;

		EXEC [dbo].[SchemaUtilsApplyVersionColumns] @VersionTableName;

		EXEC [dbo].[SchemaUtilsDropPrimaryKey] @VersionTableName;

		SET @sql = 'INSERT INTO ' + @VersionTableName + '(' + @ColumnsSQL + ', CreatedDateTime, CreatedUtcDateTime, ModifiedDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId, Deleted) SELECT ' + @ColumnsSQL + ', CreatedDateTime, CreatedUtcDateTime, ModifiedDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId, 0 FROM ' + @ViewName + ';';
		EXEC MergeUtilsExecSQL @sql;
	END
END



GO

/************************************************************
* Utility_ClearBufferTables.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[Utility_ClearBufferTables]';
GO

CREATE PROCEDURE [dbo].[Utility_ClearBufferTables]
AS
BEGIN
	
	delete from anyway.BUFFER_Locations
	delete from anyway.BUFFER_LocationsLink
	delete from anyway.BUFFER_LocationsMap
	delete from gdict.BUFFER_BIK
	delete from Geopoints.BUFFER_FinstreamLocations
	delete from Geopoints.BUFFER_FinstreamLocationsLink
	delete from Geopoints.BUFFER_FinstreamLocationsMap
	delete from Geopoints.BUFFER_FinstreamServicePoints
	delete from Geopoints.BUFFER_GoogleGeocodingCache
	delete from Geopoints.BUFFER_YandexGeocodingCache
	delete from prodcat.BUFFER_FeeRegions
	delete from prodcat.BUFFER_Product
	delete from prodcat.BUFFER_ProductFees
	delete from prodcat.BUFFER_ProductParameters
	
END


GO

/************************************************************
* Utility_DropTables.sql
************************************************************/


MergeUtilsDropSPIfExist '[dbo].[Utility_DropTables]';
GO

-- удаляет все устаревшие таблицы создаваемые при неблокирующей загрузке
-- параметр @count указывает сколько таблиц (помимо используемой) оставлять
CREATE PROCEDURE [dbo].[Utility_DropTables]
	@count int = 0
AS
BEGIN
	
	declare @viewname nvarchar(MAX)
	declare @schema_id int
	declare @tabname nvarchar(MAX)
	declare @currentTable nvarchar(MAX)
	
	declare view_cur cursor
	for select [schema_id], name from sys.views
	where name not in
	(
		'ConfigurationParameters_VIEW'
	)

	open view_cur
	fetch next from view_cur
	into @schema_id, @viewname
	
	while @@FETCH_STATUS = 0
	begin
	
		if (RIGHT(@viewname, 5) = '_VIEW')
			set @tabname = left(@viewname, LEN(@viewname) - 5)
		else
			continue
		
		set @viewname = SCHEMA_NAME(@schema_id) + '.' + @viewname
		
		set @currentTable = dbo.RuntimeUtilsGetPureTableName([dbo].[RuntimeUtilsGetViewTableName](@viewname))

		declare del_cur cursor for
		select	schema_name([schema_id]) + '.' + name as name from sys.tables where [schema_id] = @schema_id and name like '%' + @tabname + '_20%' and name != @currentTable
				and [object_id] not in (select top (@count) [object_id] from sys.tables where [schema_id] = @schema_id and name like '%' + @tabname + '_20%' and name != @currentTable order by name desc)
		order by name
		
		open del_cur
		fetch next from del_cur
		into @tabname
		while @@FETCH_STATUS = 0
		begin
			begin try
				exec [dbo].[MergeUtilsDropTableIfExist] @tabname
	 			print 'table ' + @tabname + ' droped'
			end try
			begin catch
				DECLARE @ErrorMessage NVARCHAR(4000);
				DECLARE @ErrorSeverity INT;
				DECLARE @ErrorState INT;

				SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorSeverity = ERROR_SEVERITY(),@ErrorState = ERROR_STATE();

				RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);		
			end catch
		
		fetch next from del_cur
		into @tabname
		
		end
		
		close del_cur
		deallocate del_cur			
		
	
		fetch next from view_cur
		into @schema_id, @viewname
	
	end	
	
	close view_cur
	deallocate view_cur
	
END
GO



GO

/************************************************************
* deleted_GetTradePointsByLocation.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[GetTradePointsByLocation]';
GO

GO

/************************************************************
* GetCountries.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[GetCountries]';
GO
CREATE PROCEDURE [Geopoints].[GetCountries]
(	
    @locale NVARCHAR(2), 
	@nameSearchPattern NVARCHAR(1024),
    @skip INT = NULL,
    @top INT = NULL
)
AS
BEGIN
	DECLARE @default_top INT SET @default_top = 1000;

	DECLARE @nameFT NVARCHAR(1024);
	SET @nameSearchPattern = ISNULL(@nameSearchPattern, '');
	SET @nameFT = '"' + @nameSearchPattern + '*"';
	
	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);

	IF (@nameSearchPattern = '')
	BEGIN
		WITH LocationTreeWithData AS 
		(
			SELECT Id,Name FROM Geopoints.Location_VIEW
			WHERE geopoints.[Location_VIEW].LocationType = 0
		)
		SELECT * FROM Geopoints.Location_VIEW AS t WHERE t.Id in (
			SELECT Id FROM 
				(
				SELECT 
						ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
				FROM
						LocationTreeWithData AS t2                                                                                 
				) 
				AS sub1 
				WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
		) ORDER BY Name
	END
	ELSE
	BEGIN
		WITH LocationTreeWithData AS 
		(
			SELECT Id,Name FROM Geopoints.Location_VIEW
			WHERE geopoints.[Location_VIEW].LocationType = 0
				AND CONTAINS(geopoints.[Location_VIEW].Name, @nameFT)
		)
		SELECT * FROM Geopoints.Location_VIEW AS t WHERE t.Id in (
			SELECT Id FROM 
				(
				SELECT 
						ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
				FROM
						LocationTreeWithData AS t2                                                                                 
				) 
				AS sub1 
				WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
		) ORDER BY Name
	END
END


GO

/************************************************************
* GetKladrLocationsForServicePointsService.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[GetKladrLocationsForServicePointsService]';
GO


CREATE PROCEDURE [Geopoints].[GetKladrLocationsForServicePointsService]	
	
AS
BEGIN

	select	Id, LocationType, Name, Toponym, KladrCode
	from	Geopoints.Location_VIEW
	where	LocationType in (0, 1, 3)
	
END




GO

/************************************************************
* GetLocationByCoordinates.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[GetLocationByCoordinates]';
GO

CREATE PROCEDURE [Geopoints].[GetLocationByCoordinates]
(	
    @locale NVARCHAR(2),
    @GeoSystem NVARCHAR(MAX),
    @LocationType INT,
    @Radius INT,
	@Lat	DECIMAL(10,6),
	@Lng	DECIMAL(10,6)
)
AS
BEGIN
	DECLARE @LocationLat DECIMAL(10, 6)
	DECLARE @LocationLng DECIMAL(10, 6)
	
	SET @LocationLat = @Lat
	SET @LocationLng = @Lng
	SET @GeoSystem = NULLIF(@GeoSystem, '')
	SET @Radius = ISNULL(@Radius, 500)
	
	DECLARE @BetwLat DECIMAL(10, 6)
	SET @BetwLat = 100 -- Расстояние между параллелями 111 км
	DECLARE @LatDelta DECIMAL(10, 6)
	DECLARE @LngDelta DECIMAL(10, 6)
	
	DECLARE @R DECIMAL(10, 6)
	SET @R = CONVERT(DECIMAL(16, 6), @Radius)
	DECLARE @LatShift DECIMAL(10, 6)
	SET @LatShift = ABS(@LocationLat) -(@R / 3) / @BetwLat
	IF @LatShift < 0
	    SET @LatShift = 0
	
	SET @LatDelta = @R / @BetwLat 
	SET @LngDelta = @R / [Geopoints].[GetDistanceBetweenGeoPoints] (0, @LatShift, 1, @LatShift)
	
	DECLARE @LatD1 DECIMAL(10, 6)
	DECLARE @LngD1 DECIMAL(10, 6)
	DECLARE @LatD2 DECIMAL(10, 6)
	DECLARE @LngD2 DECIMAL(10, 6)
	
	SET @LatD1 = @LocationLat - @LatDelta
	SET @LngD1 = @LocationLng - @LngDelta
	SET @LatD2 = @LocationLat + @LatDelta
	SET @LngD2 = @LocationLng + @LngDelta

	IF @LocationType = 3
	BEGIN
		SELECT TOP 1 l.*,
					[Geopoints].[GetDistanceBetweenGeoPoints](@LocationLng, @LocationLat, LGI.Lng, LGI.Lat) AS Distance
		FROM [Geopoints].[LocationGeoInfo_VIEW] LGI
			inner join Geopoints.Location_VIEW L ON L.Id = LGI.Id
		WHERE	(@GeoSystem IS NULL OR LGI.GeoSystem = @geoSystem)
				AND L.IsCity = 1
				AND LGI.Lng > @LngD1
				AND LGI.Lng < @LngD2
				AND LGI.Lat > @LatD1
				AND LGI.Lat < @LatD2
		ORDER BY Distance
	END
	ELSE
	BEGIN
		SELECT TOP 1 l.*,
					[Geopoints].[GetDistanceBetweenGeoPoints](@LocationLng, @LocationLat, LGI.Lng, LGI.Lat) AS Distance
		FROM [Geopoints].[LocationGeoInfo_VIEW] LGI
			inner join Geopoints.Location_VIEW L ON L.Id = LGI.Id
		WHERE	(@GeoSystem IS NULL OR LGI.GeoSystem = @geoSystem)
				AND (@LocationType IS NULL OR L.LocationType = @LocationType)
				AND LGI.Lng > @LngD1
				AND LGI.Lng < @LngD2
				AND LGI.Lat > @LatD1
				AND LGI.Lat < @LatD2
		ORDER BY Distance	
	END
END


GO

/************************************************************
* GetLocationByExternalId.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[GetLocationByExternalId]';
GO

CREATE PROCEDURE [Geopoints].[GetLocationByExternalId]
(	
    @locale NVARCHAR(2),
	@ExternalId NVARCHAR(50)
)
AS
BEGIN

	IF(EXISTS(SELECT 1 FROM Geopoints.ServicePoints_VIEW AS l
	WHERE  l.ExternalId = @ExternalId))
		BEGIN
			SELECT TOP(1) * 
			FROM   Geopoints.ServicePoints_VIEW AS l
			WHERE  l.ExternalId = @ExternalId
		END

	ELSE
		BEGIN
			SELECT TOP(1) * 
			FROM   Geopoints.Location_VIEW AS l
			WHERE  l.ExternalId = @ExternalId
		END
		
END


GO

/************************************************************
* GetLocationById.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[GetLocationById]';
GO
CREATE PROCEDURE [Geopoints].[GetLocationById]
(	
	@Id uniqueidentifier,
    @locale NVARCHAR(2)
)
AS
BEGIN

	IF(EXISTS(SELECT 1 FROM   Geopoints.Location_VIEW AS l
	WHERE  l.Id = @Id))
		BEGIN

			SELECT TOP(1) * 
			FROM   Geopoints.Location_VIEW AS l
			WHERE  l.Id = @Id
		END

	ELSE
	
		BEGIN

			SELECT TOP(1) * 
			FROM   Geopoints.ServicePoints_VIEW AS l
			WHERE  l.Id = @Id
		END



END


GO

/************************************************************
* GetLocationByKladrCode.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[GetLocationByKladrCode]';
GO
CREATE PROCEDURE [Geopoints].[GetLocationByKladrCode]
(	
    @locale NVARCHAR(2),
	@KladrCode NVARCHAR(20)
)
AS
BEGIN
		SELECT l.* 
		FROM   Geopoints.Location_VIEW l
		WHERE  l.KladrCode = @KladrCode
END
GO

/************************************************************
* GetLocationsByIP.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[GetLocationsByIP]';
GO
--Ищем только для LocationType 1 (Москва, Питер) и 3,4 ( город и населенный пункт)
--ТАк как в базе содержатся только города
CREATE PROCEDURE [Geopoints].[GetLocationsByIP]
(
    @ipINT         BIGINT,
    @locationType  INT,
    @locale        NVARCHAR(2),
    @skip          INT = NULL,
    @top           INT = NULL
)
AS
BEGIN
	DECLARE @S NVARCHAR(2048)
	
	DECLARE @rootLocationId UNIQUEIDENTIFIER;
	DECLARE @rootType INT;
	
	SELECT @rootLocationId = r.LocationId,
	       @rootType = L.LocationType
	FROM   Geopoints.IPRanges_VIEW r
	       INNER JOIN Geopoints.Location_VIEW L
	            ON  L.Id = r.LocationId
	WHERE  IPV4From <= @ipINT
	       AND @ipINT <= IPV4To;
	
	DECLARE @default_top INT 
	SET @default_top = 1000;
	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);
	DECLARE @ServicePointLocationType INT;
	SET @ServicePointLocationType = 8;
	
	IF (@rootType = @locationType)
	BEGIN
	    -- Возвращаем рутовую локацию. Если skip > 0 или Top == 0 то возвращаем ничего
	    SELECT *
	    FROM   Geopoints.Location_VIEW
	    WHERE  Id = @rootLocationId
	           AND (@skip = 0 AND @top > 0);
	END
	ELSE IF (@rootType > @locationType)--Если надо вернуть локацию уровнем выше. Например для города вернуть страну
	BEGIN
	    DECLARE @ResLocationId UNIQUEIDENTIFIER;			
	    
	    IF (@locationType = 0)
	        SELECT @ResLocationId = t.CountryId
	        FROM   Geopoints.Location_VIEW t
	        WHERE  t.Id = @rootLocationId;
		ELSE IF (@locationType = 1)
			SELECT @ResLocationId = t.RegionID
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id = @rootLocationId;
		ELSE IF (@locationType = 2)
			SELECT @ResLocationId = t.DistrictID
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id = @rootLocationId;
		ELSE IF (@locationType = 3)
			SELECT @ResLocationId = t.CityID
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id = @rootLocationId;
		ELSE IF (@locationType = 4)
			SELECT @ResLocationId = t.TownID
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id = @rootLocationId;
		
		SELECT *
		FROM   Geopoints.Location_VIEW t
		WHERE  t.Id = @ResLocationId
			   AND (@skip = 0 AND @top > 0);
	END
	ELSE
	BEGIN
		PRINT @rootType
		IF (@rootType = 1)--Root=Регион, Москва + Питер. Фильтр по RegionID
		BEGIN
		
			IF(@locationType = @ServicePointLocationType)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,
							   NAME
						FROM   Geopoints.ServicePoints_VIEW
						WHERE  LocationType = @locationType
							   AND RegionID = @rootLocationId
					)
					SELECT *
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   intLocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME				
			
				END
			ELSE
			BEGIN
				IF (@LocationType = 3)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,
							   NAME
						FROM   Geopoints.Location_VIEW
						WHERE  isCity = 1 AND RegionID = @rootLocationId
					)
					SELECT *
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   intLocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME			
				END
				ELSE
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,
							   NAME
						FROM   Geopoints.Location_VIEW
						WHERE  LocationType = @LocationType AND RegionID = @rootLocationId
					)
					SELECT *
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   intLocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME			
				END
			END
			
			
		END
		ELSE 
		IF (@rootType = 2)--Root=Округ. Фильтр по DistrictID
		BEGIN
		
			IF(@locationType = @ServicePointLocationType)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,
							   NAME
						FROM   Geopoints.ServicePoints_VIEW
						WHERE  LocationType = @locationType
							   AND DistrictID = @rootLocationId
					)
					SELECT *
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   intLocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
				
				END
			ELSE
				BEGIN
					IF (@LocationType = 3)
					BEGIN
						WITH intLocationTreeWithData AS 
						(
							SELECT Id,
								   NAME
							FROM   Geopoints.Location_VIEW
							WHERE  isCity = 1 AND DistrictID = @rootLocationId
						)
						SELECT *
						FROM   Geopoints.Location_VIEW t
						WHERE  t.Id IN (SELECT Id
										FROM   (
												   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
														  t2.Id
												   FROM   intLocationTreeWithData AS t2
											   ) AS sub1
										WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
						ORDER BY NAME
					END
					ELSE
						BEGIN
							WITH intLocationTreeWithData AS 
							(
								SELECT Id,
									   NAME
								FROM   Geopoints.Location_VIEW
								WHERE  LocationType = @locationType AND DistrictID = @rootLocationId
							)
							SELECT *
							FROM   Geopoints.Location_VIEW t
							WHERE  t.Id IN (SELECT Id
											FROM   (
													   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
															  t2.Id
													   FROM   intLocationTreeWithData AS t2
												   ) AS sub1
											WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
							ORDER BY NAME
						END
			
				END
		END
		ELSE 
		IF (@rootType = 3)--Root=Город. Фильтр по CityID
		BEGIN
		
			IF(@locationType = @ServicePointLocationType)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,
							   NAME
						FROM   Geopoints.ServicePoints_VIEW
						WHERE  LocationType = @locationType
							   AND CityID = @rootLocationId
					)
					SELECT *
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   intLocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME				
				
				END			
			ELSE
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,
							   NAME
						FROM   Geopoints.Location_VIEW
						WHERE  LocationType = @locationType
							   AND CityID = @rootLocationId
					)
					SELECT *
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   intLocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
				END
		END
		ELSE 
		IF (@rootType = 4)--Root=Поселок. Фильтр по TownID
		BEGIN
			IF(@locationType = @ServicePointLocationType)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,
							   NAME
						FROM   Geopoints.ServicePoints_VIEW
						WHERE  LocationType = @locationType
							   AND TownID = @rootLocationId
					)
					SELECT *
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   intLocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME												
				END
			ELSE
				BEGIN
			
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,
							   NAME
						FROM   Geopoints.Location_VIEW
						WHERE  LocationType = @locationType
							   AND TownID = @rootLocationId
					)
					SELECT *
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   intLocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
				END
		END
	END
END


GO

/************************************************************
* GetLocationsByParent.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[GetLocationsByParent]';
GO
CREATE PROCEDURE [Geopoints].[GetLocationsByParent]
(
    @parentID           UNIQUEIDENTIFIER,
    @locationType       INT,
    @nameSearchPattern  NVARCHAR(1024),
    @locale             NVARCHAR(2),
    @skip               INT = NULL,
    @top                INT = NULL
)
AS
BEGIN
	DECLARE @default_top INT 
	SET @default_top = 1000;
	
	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);
	
	DECLARE @ch NVARCHAR
	SET @ch = '#'
	
	
	DECLARE @nameLEN INT;
	SET @nameLEN = LEN(@nameSearchPattern);
	
	DECLARE @name NVARCHAR(1024);
	SET @name = @nameSearchPattern;
	
	SET @nameSearchPattern = [dbo].[WildcardsShield](@nameSearchPattern, @ch);
	SET @nameSearchPattern = '%' + @nameSearchPattern + '%'
	
	
	DECLARE @nameFT NVARCHAR(1024);
	SET @nameFT = '"' + @name + '*"';

	
	
	
	DECLARE @rootLocationId UNIQUEIDENTIFIER;
	DECLARE @rootType INT;
	
	SELECT @rootLocationId = Id,
	       @rootType = LocationType
	FROM   Geopoints.Location_VIEW
	WHERE  Id = @parentID;
	
	
	DECLARE @ServicePointLocationType INT;
	SET @ServicePointLocationType = 8;
	
	
	IF (@rootType = 0)
	BEGIN
	
		IF(@locationType = @ServicePointLocationType)
			BEGIN
--type 0, service 
					IF(@name IS NULL)
					BEGIN
					--Поиск без фильтра по имени
						WITH LocationTreeWithData AS 
						(
							SELECT L.Id, 
							L.Name
							FROM  geopoints.ServicePoints_VIEW L						 
							WHERE  (@locationType IS NULL
							   OR l.LocationType = @locationType)
								   AND L.CountryId = @rootLocationId
						)
						SELECT t.*
						FROM   Geopoints.ServicePoints_VIEW t
						WHERE  t.Id IN (SELECT Id
										FROM   (
												   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
														  t2.Id
												   FROM   LocationTreeWithData AS t2
											   ) AS sub1
										WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
						ORDER BY NAME
					END	
					ELSE
					BEGIN
					--Поиск с фильтром по имени
						WITH LocationTreeWithData AS 
						(
							SELECT L.Id, 
							L.Name,
							(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
							FROM  geopoints.ServicePoints_VIEW L						 
							WHERE  (@locationType IS NULL
							   OR l.LocationType = @locationType)
								   AND  CONTAINS(Name, @nameFT) 
								   AND L.CountryId = @rootLocationId
						)
						SELECT t.*,
						(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM   Geopoints.ServicePoints_VIEW t
						WHERE  t.Id IN (SELECT Id
										FROM   (
												   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
														  t2.Id
												   FROM   LocationTreeWithData AS t2
											   ) AS sub1
										WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
						ORDER BY st, NAME
						--конец блока поиска
					END
					
			END
		
		ELSE
			BEGIN
--type 0, location 3 			
				IF(@locationType = 3)
				BEGIN
				
				
					IF(@name IS NULL)
					BEGIN
					--Поиск без фильтра по имени
						WITH LocationTreeWithData AS 
						(
							SELECT L.Id, 
							L.Name
							FROM  geopoints.Location_VIEW L						 
							WHERE  isCity = 1
								   AND L.CountryId = @rootLocationId
						)
						SELECT t.*
						FROM   Geopoints.Location_VIEW t
						WHERE  t.Id IN (SELECT Id
										FROM   (
												   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
														  t2.Id
												   FROM   LocationTreeWithData AS t2
											   ) AS sub1
										WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
						ORDER BY NAME
					END	
					ELSE
					BEGIN
					--Поиск с фильтром по имени
						WITH LocationTreeWithData AS 
						(
							SELECT L.Id, 
							L.Name,
							(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
							FROM  geopoints.Location_VIEW L						 
							WHERE  isCity = 1
								   AND  CONTAINS(Name, @nameFT) 
								   AND L.CountryId = @rootLocationId
						)
						SELECT t.*,
						(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM   Geopoints.Location_VIEW t
						WHERE  t.Id IN (SELECT Id
										FROM   (
												   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
														  t2.Id
												   FROM   LocationTreeWithData AS t2
											   ) AS sub1
										WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
						ORDER BY st, NAME
						--конец блока поиска
					END
										
				END
				ELSE
				BEGIN
--type 0, location <> 3 				
					IF(@name IS NULL)
					BEGIN
					--Поиск без фильтра по имени
						WITH LocationTreeWithData AS 
						(
							SELECT L.Id, 
							L.Name
							FROM  geopoints.Location_VIEW L						 
							WHERE  (
									   @locationType IS NULL
									   OR L.LocationType = @LocationType
								   )
								   AND L.CountryId = @rootLocationId
						)
						SELECT t.*
						FROM   Geopoints.Location_VIEW t
						WHERE  t.Id IN (SELECT Id
										FROM   (
												   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
														  t2.Id
												   FROM   LocationTreeWithData AS t2
											   ) AS sub1
										WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
						ORDER BY NAME
					END	
					ELSE
					BEGIN
					--Поиск с фильтром по имени
						WITH LocationTreeWithData AS 
						(
							SELECT L.Id, 
							L.Name,
							(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
							FROM  geopoints.Location_VIEW L						 
							WHERE  (
									   @locationType IS NULL
									   OR L.LocationType = @LocationType
								   )
								   AND  CONTAINS(Name, @nameFT) 
								   AND L.CountryId = @rootLocationId
						)
						SELECT t.*,
						(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM   Geopoints.Location_VIEW t
						WHERE  t.Id IN (SELECT Id
										FROM   (
												   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
														  t2.Id
												   FROM   LocationTreeWithData AS t2
											   ) AS sub1
										WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
						ORDER BY st, NAME
						--конец блока поиска
					END
					
					
				END
			END
	END
	ELSE 
	IF (@rootType = 1)
	BEGIN
--type 1, service 		
		IF(@locationType = @ServicePointLocationType)	
			BEGIN
			
				IF(@name IS NULL)
				BEGIN
				--Поиск без фильтра по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name
						FROM  geopoints.ServicePoints_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND L.RegionID = @rootLocationId
					)
					SELECT t.*
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
				END	
				ELSE
				BEGIN
				--Поиск с фильтром по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name,
						(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM  geopoints.ServicePoints_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND  CONTAINS(Name, @nameFT) 
							   AND L.RegionID = @rootLocationId
					)
					SELECT t.*,
					(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY st, NAME
					--конец блока поиска
				END
				
				
					
			END
		ELSE
		BEGIN
--type 1, location = 3 		
			IF(@locationType = 3)
			BEGIN
			
			
					IF(@name IS NULL)
					BEGIN
					--Поиск без фильтра по имени
						WITH LocationTreeWithData AS 
						(
							SELECT L.Id, 
							L.Name
							FROM  geopoints.Location_VIEW L						 
							WHERE  isCity = 1
								   AND L.RegionID = @rootLocationId
						)
						SELECT t.*
						FROM   Geopoints.Location_VIEW t
						WHERE  t.Id IN (SELECT Id
										FROM   (
												   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
														  t2.Id
												   FROM   LocationTreeWithData AS t2
											   ) AS sub1
										WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
						ORDER BY NAME
					END	
					ELSE
					BEGIN
					--Поиск с фильтром по имени
						WITH LocationTreeWithData AS 
						(
							SELECT L.Id, 
							L.Name,
							(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
							FROM  geopoints.Location_VIEW L						 
							WHERE  isCity = 1
								   AND  CONTAINS(Name, @nameFT) 
								   AND L.RegionID = @rootLocationId
						)
						SELECT t.*,
						(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM   Geopoints.Location_VIEW t
						WHERE  t.Id IN (SELECT Id
										FROM   (
												   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
														  t2.Id
												   FROM   LocationTreeWithData AS t2
											   ) AS sub1
										WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
						ORDER BY st, NAME
						--конец блока поиска
					END
			END
			ELSE
			BEGIN
			
--type 1, location <> 3 			
				IF(@name IS NULL)
				BEGIN
				--Поиск без фильтра по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name
						FROM  geopoints.Location_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND L.RegionID = @rootLocationId
					)
					SELECT t.*
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
				END	
				ELSE
				BEGIN
				--Поиск с фильтром по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name,
						(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM  geopoints.Location_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND  CONTAINS(Name, @nameFT) 
							   AND L.RegionID = @rootLocationId
					)
					SELECT t.*,
					(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY st, NAME
					--конец блока поиска
				END
					
			END
		END
	END
	ELSE 
	IF (@rootType = 2)
	BEGIN
	
		IF(@locationType = @ServicePointLocationType)	
			BEGIN
--type 2, service			
			IF(@name IS NULL)
				BEGIN
				--Поиск без фильтра по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name
						FROM  geopoints.ServicePoints_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND L.DistrictID = @rootLocationId
					)
					SELECT t.*
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
				END	
				ELSE
				BEGIN
				--Поиск с фильтром по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name,
						(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM  geopoints.ServicePoints_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND  CONTAINS(Name, @nameFT) 
							   AND L.DistrictID = @rootLocationId
					)
					SELECT t.*,
					(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY st, NAME
					--конец блока поиска
				END
				
			END
		
		ELSE
			BEGIN
			IF(@locationType = 3)
			BEGIN
--type 2, location = 3 			
			
					IF(@name IS NULL)
					BEGIN
					--Поиск без фильтра по имени
						WITH LocationTreeWithData AS 
						(
							SELECT L.Id, 
							L.Name
							FROM  geopoints.Location_VIEW L						 
							WHERE  isCity = 1
								   AND L.DistrictID = @rootLocationId
						)
						SELECT t.*
						FROM   Geopoints.Location_VIEW t
						WHERE  t.Id IN (SELECT Id
										FROM   (
												   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
														  t2.Id
												   FROM   LocationTreeWithData AS t2
											   ) AS sub1
										WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
						ORDER BY NAME
					END	
					ELSE
					BEGIN
					--Поиск с фильтром по имени
						WITH LocationTreeWithData AS 
						(
							SELECT L.Id, 
							L.Name,
							(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
							FROM  geopoints.Location_VIEW L						 
							WHERE  isCity = 1
								   AND  CONTAINS(Name, @nameFT) 
								   AND L.DistrictID = @rootLocationId
						)
						SELECT t.*,
						(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM   Geopoints.Location_VIEW t
						WHERE  t.Id IN (SELECT Id
										FROM   (
												   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
														  t2.Id
												   FROM   LocationTreeWithData AS t2
											   ) AS sub1
										WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
						ORDER BY st, NAME
						--конец блока поиска
					END
			END
			ELSE
			BEGIN
			
--type 2, location <> 3 			
				IF(@name IS NULL)
				BEGIN
				--Поиск без фильтра по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name
						FROM  geopoints.Location_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND L.DistrictID = @rootLocationId
					)
					SELECT t.*
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
				END	
				ELSE
				BEGIN
				--Поиск с фильтром по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name,
						(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM  geopoints.Location_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND  CONTAINS(Name, @nameFT) 
							   AND L.DistrictID = @rootLocationId
					)
					SELECT t.*,
					(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY st, NAME
					--конец блока поиска
				END
					
			END
		END
	END
	ELSE 
	IF (@rootType = 3)
	BEGIN
	
		IF(@locationType = @ServicePointLocationType)	
			BEGIN
--type 3, service			
				IF(@name IS NULL)
				BEGIN
				--Поиск без фильтра по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name
						FROM  geopoints.ServicePoints_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND L.CityID = @rootLocationId
					)
					SELECT t.*
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
				END	
				ELSE
				BEGIN
				--Поиск с фильтром по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name,
						(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM  geopoints.ServicePoints_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND  CONTAINS(Name, @nameFT) 
							   AND L.CityID = @rootLocationId
					)
					SELECT t.*,
					(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY st, NAME
					--конец блока поиска
				END
			END
		
		ELSE		
			BEGIN	
--type 3, location 						
				IF(@name IS NULL)
				BEGIN
				--Поиск без фильтра по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name
						FROM  geopoints.Location_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND L.CityID = @rootLocationId
					)
					SELECT t.*
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
				END	
				ELSE
				BEGIN
				--Поиск с фильтром по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name,
						(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM  geopoints.Location_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND  CONTAINS(Name, @nameFT) 
							   AND L.CityID = @rootLocationId
					)
					SELECT t.*,
					(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY st, NAME
					--конец блока поиска
				END
				
			END
	END
	ELSE 
	IF (@rootType = 4)
	BEGIN
	
		IF(@locationType = @ServicePointLocationType)	
			BEGIN
--type 4, service 					
				IF(@name IS NULL)
				BEGIN
				--Поиск без фильтра по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name
						FROM  geopoints.ServicePoints_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND L.TownID = @rootLocationId
					)
					SELECT t.*
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
				END	
				ELSE
				BEGIN
				--Поиск с фильтром по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name,
						(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM  geopoints.ServicePoints_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND  CONTAINS(Name, @nameFT) 
							   AND L.TownID = @rootLocationId
					)
					SELECT t.*,
					(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
					FROM   Geopoints.ServicePoints_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY st, NAME
					--конец блока поиска
				END
				
			END
		
		ELSE
			BEGIN
 --type 4, location 	
				IF(@name IS NULL)
				BEGIN
				--Поиск без фильтра по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name
						FROM  geopoints.Location_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND L.TownID = @rootLocationId
					)
					SELECT t.*
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
				END	
				ELSE
				BEGIN
				--Поиск с фильтром по имени
					WITH LocationTreeWithData AS 
					(
						SELECT L.Id, 
						L.Name,
						(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
						FROM  geopoints.Location_VIEW L						 
						WHERE  (
								   @locationType IS NULL
								   OR (LocationType = @LocationType)
								)
							   AND  CONTAINS(Name, @nameFT) 
							   AND L.TownID = @rootLocationId
					)
					SELECT t.*,
					(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY st, NAME
					--конец блока поиска
				END
 
	    END
	END
	ELSE
	BEGIN
	
		IF(@locationType = @ServicePointLocationType)	
		BEGIN
						WITH LocationHierachy(Id, ParentID, LEVEL) AS (
					SELECT L.Id,
						   L.ParentId,
						   0 AS LEVEL
					FROM   Geopoints.Location_VIEW L
					WHERE  L.Id = @rootLocationId 
					UNION ALL     
					SELECT L.Id,
						   L.ParentId,
						   Lh.Level + 1
					FROM   Geopoints.Location_VIEW L
						   INNER JOIN LocationHierachy Lh ON L.ParentId = Lh.Id
				)
				, LocationTreeWithData AS 
				(
					SELECT L.* 
					FROM   Geopoints.ServicePoints_VIEW L
						   INNER JOIN LocationHierachy LH ON L.ParentId = Lh.Id
					WHERE  (@locationType IS NULL OR L.LocationType = @locationType)
						   AND (
								   @nameSearchPattern IS NULL
								   OR L.Name LIKE @nameSearchPattern ESCAPE @ch
							   )
				) 
			    
				SELECT *
				FROM   Geopoints.ServicePoints_VIEW t
				WHERE  t.Id IN (SELECT Id
								FROM   (
										   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
												  t2.Id
										   FROM   LocationTreeWithData AS t2
									   ) AS sub1
								WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
				ORDER BY NAME
		
		END
		
			ELSE
				BEGIN	
					 WITH LocationHierachy(Id, ParentID, LEVEL) AS (
						SELECT L.Id,
							   L.ParentId,
							   0 AS LEVEL
						FROM   Geopoints.Location_VIEW L
						WHERE  L.Id = @rootLocationId 
						UNION ALL     
						SELECT L.Id,
							   L.ParentId,
							   Lh.Level + 1
						FROM   Geopoints.Location_VIEW L
							   INNER JOIN LocationHierachy Lh ON L.ParentId = Lh.Id
					)
					, LocationTreeWithData AS 
					(
						SELECT L.* 
						FROM   Geopoints.Location_VIEW L
							   INNER JOIN LocationHierachy LH ON L.Id = Lh.Id
						WHERE  (@locationType IS NULL
							   OR (LocationType = @LocationType))
							   AND (
									   @nameSearchPattern IS NULL
									   OR L.Name LIKE @nameSearchPattern ESCAPE @ch
								   )
					) 
				    
					SELECT *
					FROM   Geopoints.Location_VIEW t
					WHERE  t.Id IN (SELECT Id
									FROM   (
											   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
													  t2.Id
											   FROM   LocationTreeWithData AS t2
										   ) AS sub1
									WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
					ORDER BY NAME
			END
	END
END
GO

/************************************************************
* ProcessingKladr.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[ProcessingKladr]';
GO


CREATE PROCEDURE [Geopoints].[ProcessingKladr]
@EtlPackageId uniqueidentifier,
@EtlSessionId uniqueidentifier
AS
BEGIN
	DECLARE @RussiaId UNIQUEIDENTIFIER
	SET @RussiaId = '6f661444-deae-4318-ae35-e149f322fc0b'

	-- Перенос новых данных КЛАДР в таблицу [BUFFER_CityLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало переноса новых данных КЛАДР в таблицу [BUFFER_CityLocation]');

	INSERT INTO [Geopoints].[BUFFER_CityLocation]
			   ([LocationType],  [Name], [KladrCode], [Index],
			   [Toponym], [CountryId], [ParentId], [RegionCode],
			   [DistrictCode], [CityCode], [TownCode],
			   [CreatedDateTime], [CreatedUtcDateTime], [IsCity])
	SELECT 
	[LEVEL] as LocationType,
	[NAME] as Name,
	[CODE] as KladrCode,
	[INDEX] as [Index],
	[SOCR] as [Toponym],
	@RussiaId as [CountryId],
	(CASE WHEN [LEVEL] = 1 THEN @RussiaId ELSE NULL END) as [ParentId],
	(CASE WHEN [LEVEL] > 1 THEN [Region] ELSE NULL END) as [RegionCode],
	(CASE WHEN [LEVEL] > 2 AND SUBSTRING([CODE], 3, 3) <> '000' THEN [District] ELSE NULL END) as [DistrictCode],
	(CASE WHEN [LEVEL] > 3 AND SUBSTRING([CODE], 6, 3) <> '000' THEN [City] ELSE NULL END) as [CityCode],
	NULL as [TownCode],
	GETDATE(), GETUTCDATE(),
	(CASE WHEN [SOCR] = N'г' THEN 1 ELSE 0 END) as IsCity		
	FROM
		(SELECT [NAME], [SOCR], [CODE],
		(case 
			when SUBSTRING([CODE], 3, 9) = '000000000' then 1
			when SUBSTRING([CODE], 6, 6) = '000000' then 2
			when SUBSTRING([CODE], 9, 3) = '000' then 3
			else 4 END
		) as [LEVEL],
		SUBSTRING([CODE], 1, 2) + '00000000000' as Region,
		SUBSTRING([CODE], 1, 5) + '00000000' as District,
		SUBSTRING([CODE], 1, 8) + '00000' as City,
		[INDEX]
		FROM Geopoints.BUFFER_KLADR WITH(NOLOCK)) as tk
		WHERE SUBSTRING([CODE], 12, 2) = '00'

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Перенос новых данных КЛАДР в таблицу [BUFFER_CityLocation] завершен');



	-- Перенос новых данных КЛАДР в таблицу [BUFFER_StreetLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало переноса новых данных КЛАДР в таблицу [BUFFER_StreetLocation]');

	INSERT INTO [Geopoints].[BUFFER_StreetLocation]
			   ([LocationType],  [Name], [KladrCode], [Index],
			   [Toponym], [ParentCode], [CountryId],
			   [CreatedDateTime], [CreatedUtcDateTime])
	SELECT
	5 as LocationType,
	[NAME] as Name,
	[CODE] as KladrCode,
	[INDEX] as [Index],
	[SOCR] as [Toponym],
	SUBSTRING([CODE], 1, 11) + '00' as ParentCode,
	@RussiaId as [CountryId],
	GETDATE(), GETUTCDATE()
	FROM Geopoints.BUFFER_STREET WITH(NOLOCK)
	WHERE SUBSTRING([CODE], 16, 2) = '00'

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Перенос новых данных КЛАДР в таблицу [BUFFER_StreetLocation] успешно завершен');



	-- Перенос текущей версии в таблицу Geopoints.BUFFER_DestinationLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало переноса текущей версии в таблицу Geopoints.BUFFER_DestinationLocation');

	INSERT INTO [Geopoints].[BUFFER_DestinationLocation](
		[Id], [ParentId], [LocationType], [Name],
		[Toponym], [KladrCode], [Index], [CountryId],
		[RegionName], [RegionId], [RegionToponym],
		[DistrictName], [DistrictId], [DistrictToponym],
		[CityName], [CityId], [CityToponym],
		[TownName], [TownId], [TownToponym],
		[EtlPackageId], [EtlSessionId],
		[CreatedDateTime], [CreatedUtcDateTime],
		[ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT
		[Id], [ParentId], [LocationType], [Name],
		[Toponym], [KladrCode], [Index], [CountryId],
		[RegionName], [RegionId], [RegionToponym],
		[DistrictName], [DistrictId], [DistrictToponym],
		[CityName], [CityId], [CityToponym],
		[TownName], [TownId], [TownToponym],
		[EtlPackageId], [EtlSessionId],
		[CreatedDateTime], [CreatedUtcDateTime], 
		[ModifiedDateTime], [ModifiedUtcDateTime]
	FROM Geopoints.Location_VIEW
	
	IF NOT EXISTS(SELECT TOP(1) Id FROM [Geopoints].[BUFFER_DestinationLocation] WITH(NOLOCK) WHERE Id = @RussiaId)
		INSERT INTO [Geopoints].[BUFFER_DestinationLocation]
		(
			id,
			locationtype,
			name,
			createddatetime,
			createdutcdatetime,
			modifieddatetime,
			modifiedutcdatetime
		)
		VALUES
		(
			@RussiaId,
			0,
			N'Россия',
			getdate(),
			getutcdate(),
			getdate(),
			getutcdate()
		)

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Перенос текущей версии в таблицу Geopoints.BUFFER_DestinationLocation завершен');


	-- Актуализация кодов в текущей версии КЛАДР Geopoints.BUFFER_DestinationLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало актуализации кодов в текущей версии КЛАДР Geopoints.BUFFER_DestinationLocation');
	
	UPDATE bdl
	SET bdl.KladrCode = an.NEWCODE
	FROM [Geopoints].[BUFFER_DestinationLocation] bdl
	JOIN Geopoints.BUFFER_ALTNAMES an ON bdl.KladrCode = an.OLDCODE
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Актуализация кодов в текущей версии КЛАДР Geopoints.BUFFER_DestinationLocation завершена');


	-- Актуализация Id, CreatedDateTime, CreatedUtcDateTime в [Geopoints].[BUFFER_CityLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало актуализации Id, CreatedDateTime, CreatedUtcDateTime в [Geopoints].[BUFFER_CityLocation]');
	
	UPDATE sl
	SET
		sl.Id = dl.Id,
		sl.CreatedDateTime = dl.CreatedDateTime,
		sl.CreatedUtcDateTime = dl.CreatedUtcDateTime,
		sl.IsCity = (CASE WHEN sl.Toponym = N'г' THEN 1 ELSE 0 END)
	FROM [Geopoints].[BUFFER_CityLocation] sl
	JOIN [Geopoints].[BUFFER_DestinationLocation] dl ON sl.KladrCode = dl.KladrCode
	WHERE dl.EtlPackageId = @EtlPackageId
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Актуализация Id, CreatedDateTime, CreatedUtcDateTime в [Geopoints].[BUFFER_CityLocation] завершена');


	-- Актуализация Id, CreatedDateTime, CreatedUtcDateTime в [Geopoints].[BUFFER_StreetLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало актуализации Id, CreatedDateTime, CreatedUtcDateTime в [Geopoints].[BUFFER_StreetLocation]');
	
	UPDATE sl
	SET
		sl.Id = dl.Id,
		sl.CreatedDateTime = dl.CreatedDateTime,
		sl.CreatedUtcDateTime = dl.CreatedUtcDateTime
	FROM [Geopoints].[BUFFER_StreetLocation] sl
	JOIN [Geopoints].[BUFFER_DestinationLocation] dl ON sl.KladrCode = dl.KladrCode
	WHERE dl.EtlPackageId = @EtlPackageId
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Актуализация Id, CreatedDateTime, CreatedUtcDateTime в [Geopoints].[BUFFER_StreetLocation] завершена');


	-- Актуализация Id в [Geopoints].[BUFFER_CityLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало актуализации Id в [Geopoints].[BUFFER_CityLocation]');
	
	UPDATE Geopoints.BUFFER_CityLocation
	SET Id = NewId()
	WHERE Id IS NULL
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Актуализация Id в [Geopoints].[BUFFER_CityLocation] завершена');


	-- Актуализация Id в [Geopoints].[BUFFER_StreetLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало актуализации Id в [Geopoints].[BUFFER_StreetLocation]');

	UPDATE Geopoints.BUFFER_StreetLocation
	SET Id = NewId()
	WHERE Id IS NULL

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Актуализация Id в [Geopoints].[BUFFER_StreetLocation] завершена');


	-- UPDATE Region in Geopoints.BUFFER_CityLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало обновления поля Region для дочерних местоположений');

	UPDATE bl
	SET 
		bl.RegionName = blr.Name,
		bl.RegionId = blr.Id,
		bl.RegionToponym = blr.Toponym,
		bl.ParentId = blr.Id
	FROM Geopoints.BUFFER_CityLocation bl
	JOIN Geopoints.BUFFER_CityLocation blr ON bl.RegionCode = blr.KladrCode;

	UPDATE bl
	SET 
		bl.RegionName = bl.Name,
		bl.RegionToponym = bl.Toponym,
		bl.RegionId = bl.Id,
		bl.RegionCode = bl.KladrCode
	FROM Geopoints.BUFFER_CityLocation bl
	WHERE bl.LocationType = 1;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Завершено обновление поля Region для дочерних местоположений');


	-- UPDATE District in Geopoints.BUFFER_CityLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало обновления поля District для дочерних местоположений');

	UPDATE bl
	SET 
		bl.DistrictName = blr.Name,
		bl.DistrictId = blr.Id,
		bl.DistrictToponym = blr.Toponym,
		bl.ParentId = blr.Id
	FROM Geopoints.BUFFER_CityLocation bl
	JOIN Geopoints.BUFFER_CityLocation blr ON bl.DistrictCode = blr.KladrCode;

	UPDATE bl
	SET 
		bl.DistrictName = bl.Name,
		bl.DistrictToponym = bl.Toponym,
		bl.DistrictId = bl.Id,
		bl.DistrictCode = bl.KladrCode
	FROM Geopoints.BUFFER_CityLocation bl
	WHERE bl.LocationType = 2;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Завершено обновление поля District для дочерних местоположений');


	-- UPDATE City in Geopoints.BUFFER_CityLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало обновления поля City для дочерних местоположений');

	UPDATE bl
	SET 
		bl.CityName = blr.Name,
		bl.CityId = blr.Id,
		bl.CityToponym = blr.Toponym,
		bl.ParentId = blr.Id
	FROM Geopoints.BUFFER_CityLocation bl
	JOIN Geopoints.BUFFER_CityLocation blr ON bl.CityCode = blr.KladrCode;

	UPDATE bl
	SET 
		bl.CityName = bl.Name,
		bl.CityToponym = bl.Toponym,
		bl.CityId = bl.Id,
		bl.CityCode = bl.KladrCode
	FROM Geopoints.BUFFER_CityLocation bl
	WHERE bl.LocationType = 3;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Завершено обновление поля City для дочерних местоположений');


	-- UPDATE Town in Geopoints.BUFFER_CityLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало обновления поля Town для дочерних местоположений');

	UPDATE bl
	SET 
		bl.TownName = bl.Name,
		bl.TownToponym = bl.Toponym,
		bl.TownId = bl.Id,
		bl.TownCode = bl.KladrCode
	FROM Geopoints.BUFFER_CityLocation bl
	WHERE bl.LocationType = 4;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Завершено обновление поля Town для дочерних местоположений');


	-- UPDATE Region, District, City, Town in Geopoints.BUFFER_StreetLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало обновления полей для таблицы BUFFER_StreetLocation');

	UPDATE bl
	SET 
		bl.CountryId = '6f661444-deae-4318-ae35-e149f322fc0b',
		bl.RegionName = (CASE blr.LocationType WHEN 1 THEN blr.Name ELSE blr.RegionName END),
		bl.RegionId = (CASE blr.LocationType WHEN 1 THEN blr.Id ELSE blr.RegionId END),
		bl.RegionToponym = (CASE blr.LocationType WHEN 1 THEN blr.Toponym ELSE blr.RegionToponym END),
		bl.DistrictName = (CASE blr.LocationType WHEN 2 THEN blr.Name ELSE blr.DistrictName END),
		bl.DistrictId = (CASE blr.LocationType WHEN 2 THEN blr.Id ELSE blr.DistrictId END),
		bl.DistrictToponym = (CASE blr.LocationType WHEN 2 THEN blr.Toponym ELSE blr.DistrictToponym END),
		bl.CityName = (CASE blr.LocationType WHEN 3 THEN blr.Name ELSE blr.CityName END),
		bl.CityId = (CASE blr.LocationType WHEN 3 THEN blr.Id ELSE blr.CityId END),
		bl.CityToponym = (CASE blr.LocationType WHEN 3 THEN blr.Toponym ELSE blr.CityToponym END),
		bl.TownName = (CASE blr.LocationType WHEN 4 THEN blr.Name ELSE blr.TownName END),
		bl.TownId = (CASE blr.LocationType WHEN 4 THEN blr.Id ELSE blr.TownId END),
		bl.TownToponym = (CASE blr.LocationType WHEN 4 THEN blr.Toponym ELSE blr.TownToponym END),
		bl.ParentId = blr.Id
	FROM Geopoints.BUFFER_StreetLocation bl
	JOIN Geopoints.BUFFER_CityLocation blr ON blr.KladrCode = bl.ParentCode;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Завершено обновление полей для таблицы BUFFER_StreetLocation');

END;

GO

/************************************************************
* RecreateBaseKlandTable.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[RecreateBaseKlandTable]';
GO
CREATE PROCEDURE [Geopoints].[RecreateBaseKlandTable]
@EtlPackageId uniqueidentifier,
@EtlSessionId uniqueidentifier
AS
BEGIN
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Удаление таблицы [Geopoints].[BUFFER_KLADR]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[BUFFER_KLADR]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[BUFFER_KLADR]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Завершено удаление таблицы [Geopoints].[BUFFER_KLADR]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало создания таблицы [Geopoints].[BUFFER_KLADR]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[BUFFER_KLADR](
		[NAME] [nvarchar](40) NOT NULL,
		[SOCR] [nvarchar](10) NULL,
		[CODE] [nvarchar](13) NOT NULL,
		[INDEX] [nvarchar](6) NULL,
		[GNINMB] [nvarchar](4) NULL,
		[UNO] [nvarchar](4) NULL,
		[OCATD] [nvarchar](11) NULL,
		[STATUS] [nvarchar](1) NULL
	) ON [PRIMARY]';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Создана таблица  [Geopoints].[BUFFER_KLADR]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Удаление таблицы [Geopoints].[BUFFER_STREET]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[BUFFER_STREET]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[BUFFER_STREET]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Завершено удаление таблицы [Geopoints].[BUFFER_STREET]');
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало создания таблицы [Geopoints].[BUFFER_STREET]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[BUFFER_STREET](
		[NAME] [nvarchar](40) NOT NULL,
		[SOCR] [nvarchar](10) NULL,
		[CODE] [nvarchar](17) NOT NULL,
		[INDEX] [nvarchar](6) NULL,
		[GNINMB] [nvarchar](4) NULL,
		[UNO] [nvarchar](4) NULL,
		[OCATD] [nvarchar](11) NULL
	) ON [PRIMARY]';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Создана таблица  [Geopoints].[BUFFER_STREET]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Удаление таблицы [Geopoints].[BUFFER_ALTNAMES]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[BUFFER_ALTNAMES]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[BUFFER_ALTNAMES]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Завершено удаление таблицы [Geopoints].[BUFFER_ALTNAMES]');
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало создания таблицы [Geopoints].[BUFFER_ALTNAMES]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[BUFFER_ALTNAMES](
		[OLDCODE] [nvarchar](19) NOT NULL,
		[NEWCODE] [nvarchar](19) NOT NULL,
		[LEVEL] [int] NOT NULL
	) ON [PRIMARY]';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Создана таблица  [Geopoints].[BUFFER_ALTNAMES]');
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Удаление таблицы [Geopoints].[BUFFER_CityLocation]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[BUFFER_CityLocation]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[BUFFER_CityLocation]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Завершено удаление таблицы [Geopoints].[BUFFER_CityLocation]');
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало создания таблицы [Geopoints].[BUFFER_CityLocation]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[BUFFER_CityLocation](
		[Id] [uniqueidentifier] NULL,
		[ParentId] [uniqueidentifier] NULL,
		[LocationType] [int] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Toponym] [nvarchar](10) NULL,
		[KladrCode] [nvarchar](13) NULL,
		[Index] [nvarchar](6) NULL,
		[CountryId] [uniqueidentifier] NULL,
		[RegionName] [nvarchar](255) NULL,
		[RegionCode] [nvarchar](255) NULL,
		[RegionId] [uniqueidentifier] NULL,
		[RegionToponym] [nvarchar](10) NULL,
		[DistrictName] [nvarchar](255) NULL,
		[DistrictCode] [nvarchar](255) NULL,
		[DistrictId] [uniqueidentifier] NULL,
		[DistrictToponym] [nvarchar](10) NULL,
		[CityName] [nvarchar](255) NULL,
		[CityCode] [nvarchar](255) NULL,
		[CityId] [uniqueidentifier] NULL,
		[CityToponym] [nvarchar](10) NULL,
		[TownName] [nvarchar](255) NULL,
		[TownCode] [nvarchar](255) NULL,
		[TownId] [uniqueidentifier] NULL,
		[TownToponym] [nvarchar](10) NULL,
		[CreatedDateTime] [datetime] NULL,
		[CreatedUtcDateTime] [datetime] NULL,
		[IsCity] [BIT] NOT NULL DEFAULT 0
	)';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Создана таблица  [Geopoints].[BUFFER_CityLocation]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Удаление таблицы [Geopoints].[BUFFER_StreetLocation]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[BUFFER_StreetLocation]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[BUFFER_StreetLocation]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Завершено удаление таблицы [Geopoints].[BUFFER_StreetLocation]');
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало создания таблицы [Geopoints].[BUFFER_StreetLocation]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[BUFFER_StreetLocation](
		[Id] [uniqueidentifier] NULL,
		[ParentId] [uniqueidentifier] NULL,
		[ParentCode] [nvarchar](255) NULL,
		[LocationType] [int] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Toponym] [nvarchar](10) NULL,
		[KladrCode] [nvarchar](17) NULL,
		[Index] [nvarchar](6) NULL,
		[CountryId] [uniqueidentifier] NULL,
		[RegionName] [nvarchar](255) NULL,
		[RegionCode] [nvarchar](255) NULL,
		[RegionId] [uniqueidentifier] NULL,
		[RegionToponym] [nvarchar](10) NULL,
		[DistrictName] [nvarchar](255) NULL,
		[DistrictCode] [nvarchar](255) NULL,
		[DistrictId] [uniqueidentifier] NULL,
		[DistrictToponym] [nvarchar](10) NULL,
		[CityName] [nvarchar](255) NULL,
		[CityCode] [nvarchar](255) NULL,
		[CityId] [uniqueidentifier] NULL,
		[CityToponym] [nvarchar](10) NULL,
		[TownName] [nvarchar](255) NULL,
		[TownCode] [nvarchar](255) NULL,
		[TownId] [uniqueidentifier] NULL,
		[TownToponym] [nvarchar](10) NULL,
		[CreatedDateTime] [datetime] NULL,
		[CreatedUtcDateTime] [datetime] NULL
	)';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Создана таблица  [Geopoints].[BUFFER_StreetLocation]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Удаление таблицы [Geopoints].[BUFFER_DestinationLocation]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[BUFFER_DestinationLocation]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[BUFFER_DestinationLocation]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Завершено удаление таблицы [Geopoints].[BUFFER_DestinationLocation]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало создания таблицы [Geopoints].[BUFFER_DestinationLocation]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[BUFFER_DestinationLocation](
		[Id] [uniqueidentifier] NULL,
		[ParentId] [uniqueidentifier] NULL,
		[LocationType] [int] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Toponym] [nvarchar](10) NULL,
		[KladrCode] [nvarchar](20) NULL,
		[Index] [nvarchar](6) NULL,
		[CountryId] [uniqueidentifier] NULL,
		[RegionName] [nvarchar](255) NULL,
		[RegionCode] [nvarchar](255) NULL,
		[RegionId] [uniqueidentifier] NULL,
		[RegionToponym] [nvarchar](10) NULL,
		[DistrictName] [nvarchar](255) NULL,
		[DistrictCode] [nvarchar](255) NULL,
		[DistrictId] [uniqueidentifier] NULL,
		[DistrictToponym] [nvarchar](10) NULL,
		[CityName] [nvarchar](255) NULL,
		[CityCode] [nvarchar](255) NULL,
		[CityId] [uniqueidentifier] NULL,
		[CityToponym] [nvarchar](10) NULL,
		[TownName] [nvarchar](255) NULL,
		[TownCode] [nvarchar](255) NULL,
		[TownId] [uniqueidentifier] NULL,
		[TownToponym] [nvarchar](10) NULL,
		[EtlPackageId] [uniqueidentifier] NULL,
		[EtlSessionId] [uniqueidentifier] NULL,
		[CreatedDateTime] [datetime] NOT NULL,
		[CreatedUtcDateTime] [datetime] NOT NULL,
		[ModifiedDateTime] [datetime] NOT NULL,
		[ModifiedUtcDateTime] [datetime] NOT NULL
	)';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Создана таблица  [Geopoints].[BUFFER_DestinationLocation]');
END

GO

/************************************************************
* RemapKladrView.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[RemapKladrView]';
GO
CREATE PROCEDURE [geopoints].[RemapKladrView]
@Date nvarchar(20)
AS
BEGIN
	DECLARE @tableName NVARCHAR(128);
	DECLARE @S NVARCHAR(MAX);
	SET @S=N' VIEW [geopoints].[Location_VIEW] With schemabinding  
			AS
			SELECT [Id]
      ,[ParentId]
      ,[ExternalId]
      ,[LocationType]
      ,[Name]
      ,[Toponym]
      ,[KladrCode]
      ,[Index]
      ,[CountryId]
      ,[RegionName]
      ,[RegionId]
      ,[RegionToponym]
      ,[DistrictName]
      ,[DistrictId]
      ,[DistrictToponym]
      ,[CityName]
      ,[CityId]
      ,[CityToponym]
      ,[TownName]
      ,[TownId]
      ,[TownToponym]
      ,[IsCity]
      ,[CreatedDateTime]
      ,[CreatedUtcDateTime]
      ,[ModifiedDateTime]
      ,[ModifiedUtcDateTime]
      ,[EtlPackageId]
      ,[EtlSessionId]
			FROM  ';
	SET @tableName = '[Geopoints].[Location_' + @Date + ']';
	IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[geopoints].[Location_VIEW]'))
	BEGIN
		SET @S = N'--#' + @tableName + '#
		ALTER ' + @S + @tableName;
	END
	ELSE BEGIN
		SET @S = N'--#' + @tableName + '#
		CREATE ' + @S + @tableName;;
	END;
	EXEC [dbo].MergeUtilsExecSQL @S;
	
	CREATE UNIQUE CLUSTERED INDEX [IX_MAIN] ON [geopoints].[Location_VIEW] (
        [Id] ASC	)
        
	IF NOT EXISTS (SELECT * FROM sysfulltextcatalogs ftc WHERE ftc.name = N'FTCatalog')
	CREATE FULLTEXT CATALOG [FTCatalog]WITH ACCENT_SENSITIVITY = OFF
	AS DEFAULT
	AUTHORIZATION [dbo]

	CREATE FULLTEXT INDEX ON [geopoints].Location_VIEW(Name) KEY INDEX IX_MAIN;	
	
END;
GO

/************************************************************
* RemapSevicePointsView.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[RemapServicePointsView]';
GO
CREATE PROCEDURE [geopoints].[RemapServicePointsView]
@TableName nvarchar(128),
@CreateFullTextIndex bit = 1
AS
BEGIN
	DECLARE @S NVARCHAR(MAX);
	SET @S=N' VIEW [geopoints].[ServicePoints_VIEW] With schemabinding  
			AS
			SELECT [Id]
      ,[ParentId]
      ,[ExternalId]
      ,[LocationType]
      ,[InstantTransferSystem]
      ,[Name]
      ,[Code]
      ,[Address]
      ,[PhoneNumber]
      ,[Schedule]
      ,[Unaddressed]
      ,[Toponym]
      ,[KladrCode]
      ,[CountryId]
      ,[RegionName]
      ,[RegionId]
      ,[RegionToponym]
      ,[DistrictName]
      ,[DistrictId]
      ,[DistrictToponym]
      ,[CityName]
      ,[CityId]
      ,[CityToponym]
      ,[TownName]
      ,[TownId]
      ,[TownToponym]
      ,[CreatedDateTime]
      ,[CreatedUtcDateTime]
      ,[ModifiedDateTime]
      ,[ModifiedUtcDateTime]
      ,[EtlPackageId]
      ,[EtlSessionId]
      ,[Summa]
      ,[Currency]
      ,[MaxSumma]
      ,[Description]
			FROM  ';
	IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[geopoints].[ServicePoints_VIEW]'))
	BEGIN
		SET @S = N'--#' + @TableName + '#
		ALTER ' + @S + @TableName;
	END
	ELSE BEGIN
		SET @S = N'--#' + @TableName + '#
		CREATE ' + @S + @TableName;
	END;
	EXEC [dbo].MergeUtilsExecSQL @S;
	
	CREATE UNIQUE CLUSTERED INDEX [IX_MAIN] ON [geopoints].[ServicePoints_VIEW] (
        [Id] ASC	)
        
	IF NOT EXISTS (SELECT * FROM sysfulltextcatalogs ftc WHERE ftc.name = N'FTCatalog')
	CREATE FULLTEXT CATALOG [FTCatalog]WITH ACCENT_SENSITIVITY = OFF
	AS DEFAULT
	AUTHORIZATION [dbo]

	IF @CreateFullTextIndex = 1
		CREATE FULLTEXT INDEX ON [geopoints].ServicePoints_VIEW(Name) KEY INDEX IX_MAIN;	
	
END;
GO

/************************************************************
* SaveDataToLocality.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[SaveDataToLocality]';
GO
CREATE PROCEDURE [Geopoints].[SaveDataToLocality]
@EtlPackageId uniqueidentifier,
@EtlSessionId uniqueidentifier,
@Date nvarchar(20)
AS
BEGIN
	
	DECLARE @TableName NVARCHAR(128)
	SET @TableName = '[Geopoints].[Location_' + @Date + ']';
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало создания таблицы местоположений [Geopoints].[Location_' + @Date + ']');
	
	EXEC [Geopoints].[CreateLocationTable] @TableName;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Таблица местоположений [Geopoints].[Location_' + @Date + N'] создана');
	
	DECLARE @ModifiedDateTime datetime;
	DECLARE @ModifiedUtcDateTime datetime;

	SET @ModifiedDateTime = GETDATE()
	SET @ModifiedUtcDateTime = GETUTCDATE()

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало переноса данных из [Geopoints].[BUFFER_DestinationLocation] в [Geopoints].[Location_' + @Date + ']');
	
	DECLARE @BUFFER_DestinationLocation2Location nvarchar(1500);
	DECLARE @BUFFER_DestinationLocation2LocationParm NVARCHAR(500);
	SET @BUFFER_DestinationLocation2Location = N'INSERT INTO [Geopoints].[Location_' + @Date + ']
			   ([Id], [ParentId], [LocationType], [Name]
			   ,[Toponym], [KladrCode], [Index], [CountryId]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT [Id], [ParentId], [LocationType], [Name]
			   ,[Toponym], [KladrCode], [Index], [CountryId]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime]
	FROM [Geopoints].[BUFFER_DestinationLocation] WITH(NOLOCK)
	WHERE EtlPackageId IS NULL OR [EtlPackageId] <> @EtlPackageId';
	SET @BUFFER_DestinationLocation2LocationParm = N'@EtlPackageId uniqueidentifier';
	EXEC sp_executesql @BUFFER_DestinationLocation2Location, @BUFFER_DestinationLocation2LocationParm, @EtlPackageId = @EtlPackageId;
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Перенос данных из [Geopoints].[BUFFER_DestinationLocation] в [Geopoints].[Location_' + @Date + N'] завершен');


	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало переноса данных из [Geopoints].[BUFFER_CityLocation] в [Geopoints].[Location_' + @Date + ']');

	DECLARE @BUFFER_CityLocation2Location nvarchar(1500);
	DECLARE @BUFFER_CityLocation2LocationParm NVARCHAR(500);
	SET @BUFFER_CityLocation2Location = N'INSERT INTO [Geopoints].[Location_' + @Date + ']
			   ([Id], [ParentId], [LocationType], [Name]
			   ,[Toponym], [KladrCode], [Index], [CountryId]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime], [IsCity])
	SELECT [Id], [ParentId], [LocationType], [Name]
			   ,[Toponym], [KladrCode], [Index], [CountryId]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,@EtlPackageId as [EtlPackageId], @EtlSessionId as [EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,@ModifiedDateTime as [ModifiedDateTime], @ModifiedUtcDateTime as [ModifiedUtcDateTime]
			   ,[IsCity]
	FROM [Geopoints].[BUFFER_CityLocation] WITH(NOLOCK)';
	SET @BUFFER_CityLocation2LocationParm = N'@EtlPackageId uniqueidentifier, @EtlSessionId uniqueidentifier, @ModifiedDateTime datetime, @ModifiedUtcDateTime datetime';
	EXEC sp_executesql @BUFFER_CityLocation2Location, @BUFFER_CityLocation2LocationParm, @EtlPackageId = @EtlPackageId,
	@EtlSessionId = @EtlSessionId, @ModifiedDateTime = @ModifiedDateTime, @ModifiedUtcDateTime = @ModifiedUtcDateTime;
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Перенос данных из [Geopoints].[BUFFER_CityLocation] в [Geopoints].[Location_' + @Date + N'] завершен');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало переноса данных из [Geopoints].[BUFFER_StreetLocation] в [Geopoints].[Location_' + @Date + ']');
	
	DECLARE @BUFFER_StreetLocation2Location nvarchar(1500);
	DECLARE @BUFFER_StreetLocation2LocationParm NVARCHAR(500);
	
	SET @BUFFER_StreetLocation2Location = N'INSERT INTO [Geopoints].[Location_' + @Date + ']
			   ([Id], [ParentId], [LocationType], [Name]
			   , [Toponym], [KladrCode], [Index], [CountryId]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT [Id], [ParentId], [LocationType], [Name]
	   ,[Toponym], [KladrCode], [Index], [CountryId]
	   ,[RegionName], [RegionId], [RegionToponym]
	   ,[DistrictName], [DistrictId], [DistrictToponym]
	   ,[CityName], [CityId], [CityToponym]
	   ,[TownName], [TownId], [TownToponym]
	   ,@EtlPackageId as [EtlPackageId], @EtlSessionId as [EtlSessionId]
	   ,[CreatedDateTime], [CreatedUtcDateTime]
	   ,@ModifiedDateTime as [ModifiedDateTime], @ModifiedUtcDateTime as [ModifiedUtcDateTime]
	FROM [Geopoints].[BUFFER_StreetLocation] WITH(NOLOCK)';
	SET @BUFFER_StreetLocation2LocationParm = N'@EtlPackageId uniqueidentifier, @EtlSessionId uniqueidentifier, @ModifiedDateTime datetime, @ModifiedUtcDateTime datetime';
	EXEC sp_executesql @BUFFER_StreetLocation2Location, @BUFFER_StreetLocation2LocationParm, @EtlPackageId = @EtlPackageId,
	@EtlSessionId = @EtlSessionId, @ModifiedDateTime = @ModifiedDateTime, @ModifiedUtcDateTime = @ModifiedUtcDateTime

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Перенос данных из [Geopoints].[BUFFER_StreetLocation] в [Geopoints].[Location_' + @Date + N'] завершен');


	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),3, N'Начало применения ограничений к [Geopoints].[Location_' + @Date + ']');
	
	EXEC [Geopoints].[ApplyLocationTableConstraints] @TableName;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),4, N'Применение ограничений к [Geopoints].[Location_' + @Date + N'] успешно завершено');
END;
GO

/************************************************************
* SyncIPRangesFromBuffer.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[SyncIPRangesFromBuffer]';
GO
CREATE PROCEDURE [Geopoints].[SyncIPRangesFromBuffer]
	@packID uniqueidentifier,
	@sessionID uniqueidentifier,
	@xmlData NVARCHAR(MAX)
AS
BEGIN

	BEGIN TRY

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 8, N'Запущена процедура разноски диапазонов IP в БД');

		EXEC [geopoints].CreateIPRangesBufferTable;

		DECLARE @xml XML;
		SET @xml = @xmlData;


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 3, N'Начало добавления данных в буферную таблицу');

		INSERT INTO geopoints.BUFFER_IPRanges(IPV4From, IPV4To, IPV4FromString, IPV4ToString, 
		City, Country, Region, FedRegion)

		SELECT 
		  tab.col.value('@fri','BIGINT') AS IPfromInt, 
		  tab.col.value('@toi','BIGINT')AS IPtoInt, 
  
		  tab.col.value('@frs','NVARCHAR(32)') AS IPfromString, 
		  tab.col.value('@tos','NVARCHAR(32)') AS IPtoString, 
  
		  tab.col.value('@cty','NVARCHAR(64)') AS City, 
		  tab.col.value('@ctr','NVARCHAR(64)') AS Country, 
		  tab.col.value('@reg','NVARCHAR(64)') AS Region, 
		  tab.col.value('@fed','NVARCHAR(64)') AS FedRegion  
		FROM @xml.nodes('//r') tab(col)


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 4, N'Добавление данных в буферную таблицу завершено');

		DECLARE @s NVARCHAR(MAX);


		DECLARE @tbl_IPRanges NVARCHAR(128);
		

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 3, N'Создание новой таблицы диапазонов IP-адресов');

		SET @tbl_IPRanges = dbo.RuntimeUtilsGetTableNameWithDate('[geopoints].IPRanges');
		EXEC [geopoints].[CreateIPRangesTable] @tbl_IPRanges;


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 4, N'Создание таблицы для диапазонов IP-адресов завершено');


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 3, N'Начало наполнения таблицы IP-диапазонов с привязкой к местоположениям');


		SET @s = 'INSERT INTO ' + @tbl_IPRanges + '(IPV4From, IPV4To, IPV4FromString, IPV4ToString, LocationId) 		
		SELECT IPV4From, IPV4To, IPV4FromString, IPV4ToString,
		[geopoints].[SearchLocationForIP](City, Region, FedRegion, Country)
		
		FROM geopoints.BUFFER_IPRanges WITH(NOLOCK);';

		EXEC dbo.MergeUtilsExecSQL @s;


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 4, N'Наполнение таблицы IP-диапазонов завершено');




		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 3, N'Применение ограничений к таблице диапазонов');

		EXEC [geopoints].[ApplyIPRangesTableConstraints] @tbl_IPRanges;

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 4, N'Применение ограничений к таблице диапазонов завершено');


		--INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		--VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer before delete from buffer');

		--DELETE FROM geopoints.BUFFER_IPRanges;

		--INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		--VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 8, 'SyncIPRangesFromBuffer after delete from buffer');


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 3, N'Обновление представления для таблицы диапазонов');

		EXEC RuntimeUtilsSetViewDefinition '[geopoints].IPRanges_VIEW', @tbl_IPRanges;

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 4, N'Обновление представления завершено');




		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 8, N'Процедура разноски данных IP-диапазонов успешно завершена');

	END TRY
	BEGIN CATCH
		PRINT 'Unexpected error occurred!';
		PRINT ERROR_MESSAGE();		

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
			VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 5, N'Процедура разноски данных IP-диапазонов завершена с ошибкой ' + ERROR_MESSAGE());
	END CATCH
END
GO

/************************************************************
* SyncServicePointsFromBuffer.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[SyncServicePointsFromBuffer]';
GO


CREATE PROCEDURE [Geopoints].[SyncServicePointsFromBuffer]
	@EtlPackageId		uniqueidentifier,
	@EtlSessionId		uniqueidentifier
AS
BEGIN	
	declare @logmsg nvarchar(MAX)
	
	set @logmsg = N'Запуск процедуры сихронизации буферных данных по точкам обслуживания.'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	DECLARE @tmp int;			
	DECLARE @err INT
	DECLARE @tranStart BIT;
	DECLARE @rows int
	SET @tranStart = 0;

	BEGIN TRY
	DECLARE @s NVARCHAR(MAX);


	declare @servicePointPackageId uniqueidentifier
	set @servicePointPackageId = 'C8BE0E2E-9AF5-4131-B5B2-63C95F7B1441'

	declare @servicePointsRootCategoryId uniqueidentifier
	set @servicePointsRootCategoryId = (select cast(Value as uniqueidentifier) from prodcat.ConfigurationParameters_VIEW where Name = N'ServicePointsRootCategory')

	if @servicePointsRootCategoryId is null
	begin
		raiserror('Корневая категория не задана в таблице конфигурации', 16,2)
		return
	end

	------ ДОБАВЛЯЕМ ROOT КАТЕГОРИЮ ЕСЛИ ОНА ОТСУТСТВУЕТ ------
	if not exists (select * from prodcat.ProductCategory_VIEW where Id = @servicePointsRootCategoryId)
		insert into prodcat.ProductCategory_VIEW (Id, [Order], ExternalId, Name, CreatedDateTime, ModifiedDateTime, CreatedUtcDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId)
		values (@servicePointsRootCategoryId, 0, N'ServicePointsRootCategory', N'Корневая категория точек обслуживания', GETDATE(), GETDATE(), GETUTCDATE(), GETUTCDATE(), @EtlPackageId, @EtlSessionId)


	------ ВАЛИДАЦИЯ ПОСТУПИВШИХ ДАННЫХ ------
	
		
	------ СОЗДАНИЕ ТАБЛИЦ ------
	
	-- Создание таблицы Locations
	set @logmsg = N'Создание таблицы Locations'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	DECLARE @AnywayLocationsTName NVARCHAR(128);
	SET @AnywayLocationsTName = dbo.RuntimeUtilsGetTableNameWithDate(N'[anyway].Locations');
	EXEC [anyway].[CreateLocationsTable] @AnywayLocationsTName;

	set @logmsg = N'Создана таблица ' + @AnywayLocationsTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);


	-- Создание таблицы ServicePointsTable
	set @logmsg = N'Создание таблицы ServicePointsTable'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	DECLARE @AnywayServicePointsTName NVARCHAR(128);
	SET @AnywayServicePointsTName = dbo.RuntimeUtilsGetTableNameWithDate('[Geopoints].ServicePoints');
	EXEC [Geopoints].[CreateServicePointsTable] @AnywayServicePointsTName;

	set @logmsg = N'Создана таблица ' + @AnywayServicePointsTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);


	-- Создание таблицы LocationsLink
	set @logmsg = N'Создание таблицы LocationsLink'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	DECLARE @AnywayLocationsLinkTName NVARCHAR(128);
	SET @AnywayLocationsLinkTName = dbo.RuntimeUtilsGetTableNameWithDate(N'[anyway].LocationsLink');
	EXEC [anyway].[CreateLocationsLinkTable] @AnywayLocationsLinkTName;

	set @logmsg = N'Создана таблица ' + @AnywayLocationsLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);


	-- Создание таблицы LocationsMap
	set @logmsg = N'Создание таблицы LocationsMap'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	DECLARE @AnywayLocationsMapTName NVARCHAR(128);
	SET @AnywayLocationsMapTName = dbo.RuntimeUtilsGetTableNameWithDate('[anyway].LocationsMap');
	EXEC [anyway].[CreateLocationsMapTable] @AnywayLocationsMapTName;

	set @logmsg = N'Создана таблица ' + @AnywayLocationsMapTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);


	-- Создание таблицы ProductVendor
	set @logmsg = N'Создание таблицы ProductVendor'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	DECLARE @ProductVendorTName NVARCHAR(128);
	SET @ProductVendorTName = dbo.RuntimeUtilsGetTableNameWithDate(N'[prodcat].ProductVendor');
	EXEC [prodcat].[CreateProductVendorTable] @ProductVendorTName;

	set @logmsg = N'Создана таблица ' + @ProductVendorTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);


	-- Создание таблицы Product
	set @logmsg = N'Создание таблицы Product'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	DECLARE @ProductTName NVARCHAR(128);
	SET @ProductTName = dbo.RuntimeUtilsGetTableNameWithDate('[prodcat].Product');
	EXEC [prodcat].[CreateProductTable] @ProductTName;

	set @logmsg = N'Создана таблица ' + @ProductTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	
	-- Создание таблицы ProductFieldInfo
	set @logmsg = N'Создание таблицы ProductFieldInfo'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	DECLARE @ProductFieldInfoTName NVARCHAR(128);
	SET @ProductFieldInfoTName = dbo.RuntimeUtilsGetTableNameWithDate(N'[prodcat].ProductFieldInfo');
	EXEC [prodcat].[CreateProductFieldInfoTable] @ProductFieldInfoTName;

	set @logmsg = N'Создана таблица ' + @ProductFieldInfoTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	
	-- Создание таблицы ProductCategoryLink
	set @logmsg = N'Создание таблицы ProductCategoryLink'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	DECLARE @ProductCategoryLinkTName NVARCHAR(128);
	SET @ProductCategoryLinkTName = dbo.RuntimeUtilsGetTableNameWithDate(N'[prodcat].ProductCategoryLink');
	EXEC [prodcat].[CreateProductCategoryLinkTable] @ProductCategoryLinkTName;

	set @logmsg = N'Создана таблица ' + @ProductCategoryLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	
	
	
	-- Создание таблицы ProductLocationLink
	set @logmsg = N'Создание таблицы ProductLocationLink'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	DECLARE @ProductLocationLinkTName NVARCHAR(128);
	SET @ProductLocationLinkTName = dbo.RuntimeUtilsGetTableNameWithDate(N'[prodcat].ProductLocationLink');
	EXEC [prodcat].[CreateProductLocationLinkTable] @ProductLocationLinkTName;

	set @logmsg = N'Создана таблица ' + @ProductLocationLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	

	------ ОБНОВЛЕНИЕ ИДЕНТИФИКАТОРОВ ДЛЯ СУЩЕСТВУЮЩИХ ОБЪЕКТОВ ------

	set @logmsg = N'Обновление родительских идентификаторов для BUFFER_LocationsLink'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	update	anyway.BUFFER_LocationsLink
	set		ParentId = v.Id
	from	anyway.BUFFER_LocationsLink ll
			inner join anyway.BUFFER_Locations l on l.Id = ll.ParentId
			inner join anyway.Locations_VIEW v on l.ExternalId = v.ExternalId and l.LocationType = v.LocationType
	where	ll.EtlSessionId = @EtlSessionId

	set @logmsg = N'Обновлено ' + CONVERT(nvarchar(MAX), @@ROWCOUNT) + ' записей'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	
	set @logmsg = N'Обновление идентификаторов для BUFFER_LocationsLink'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	update	anyway.BUFFER_LocationsLink
	set		Id = v.Id
	from	anyway.BUFFER_LocationsLink ll
			inner join anyway.BUFFER_Locations l on l.Id = ll.Id
			inner join anyway.Locations_VIEW v on l.ExternalId = v.ExternalId and l.LocationType = v.LocationType
	where	ll.EtlSessionId = @EtlSessionId

	set @logmsg = N'Обновлено ' + CONVERT(nvarchar(MAX), @@ROWCOUNT) + ' записей'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	
	
	
	set @logmsg = N'Обновление идентификаторов для BUFFER_LocationsMap'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	update	anyway.BUFFER_LocationsMap
	set		Id = v.Id
	from	anyway.BUFFER_LocationsMap m
			inner join anyway.BUFFER_Locations l on l.Id = m.Id
			inner join anyway.Locations_VIEW v on l.ExternalId = v.ExternalId and l.LocationType = v.LocationType
	where	m.EtlSessionId = @EtlSessionId

	set @logmsg = N'Обновлено ' + CONVERT(nvarchar(MAX), @@ROWCOUNT) + ' записей'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	
		

	set @logmsg = N'Обновление идентификаторов для BUFFER_Locations'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	update	anyway.BUFFER_Locations
	set		Id = s.Id
	from	anyway.BUFFER_Locations t
			inner join anyway.Locations_VIEW s on t.ExternalId = s.ExternalId and t.LocationType = s.LocationType
	where	t.EtlSessionId = @EtlSessionId

	set @logmsg = N'Обновлено ' + CONVERT(nvarchar(MAX), @@ROWCOUNT) + ' записей'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);


	--------- КОПИРОВАНИЕ СУЩЕСТВУЮЩИХ ДАННЫХ ----------	

	-- копирование данных ProductVendor
	set @logmsg = N'Копирование данных в таблицу ' + @ProductVendorTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	SET @s = N'
	
	DECLARE @servicePointPackageId uniqueidentifier;
	SET @servicePointPackageId = CAST(N''' + CAST(@servicePointPackageId as NVARCHAR(64)) + ''' AS uniqueidentifier);
		
	INSERT INTO ' + @ProductVendorTName + N'(
		Id, 
		ExternalId, 
		[ChannelId],
		Name, 
		CreatedDateTime, 
		ModifiedDateTime, 
		CreatedUtcDateTime, 
		ModifiedUtcDateTime,
		EtlPackageId,
		EtlSessionId ) 
	SELECT  
		Id, 
		ExternalId, 
		[ChannelId],
		Name, 
		CreatedDateTime, 
		ModifiedDateTime, 
		CreatedUtcDateTime, 
		ModifiedUtcDateTime,
		EtlPackageId,
		EtlSessionId
	FROM [prodcat].ProductVendor_VIEW
	WHERE EtlPackageId != @servicePointPackageId'
	
	EXEC dbo.MergeUtilsExecSQL @s;
	
	set @logmsg = N'Скопировано ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в таблицу ' + @ProductVendorTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	


	-- копирование данных Product
	set @logmsg = N'Копирование данных в таблицу ' + @ProductTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	SET @s = N'
	
	DECLARE @servicePointPackageId uniqueidentifier;
	SET @servicePointPackageId = CAST(N''' + CAST(@servicePointPackageId as NVARCHAR(64)) + ''' AS uniqueidentifier);	
	
	INSERT INTO ' + @ProductTName + N'(
		Id, 
		[Order],
		ExternalId, 
		[ChannelId],
		[MCC],
		[Statistics],
		ProviderId, 
		Name, 
		EngName, 
		Currency, 
		Logo, 
		CreatedDateTime, 
		ModifiedDateTime, 
		CreatedUtcDateTime, 
		ModifiedUtcDateTime,
		EtlPackageId,
		EtlSessionId ) 
	SELECT 
		Id, 
		[Order],
		ExternalId, 
		[ChannelId],
		[MCC],
		[Statistics],		
		ProviderId, 
		Name, 
		EngName, 
		Currency, 
		Logo, 
		CreatedDateTime, 
		ModifiedDateTime, 
		CreatedUtcDateTime, 
		ModifiedUtcDateTime,
		EtlPackageId,
		EtlSessionId
	FROM [prodcat].Product_VIEW
	WHERE EtlPackageId != @servicePointPackageId'
	EXEC dbo.MergeUtilsExecSQL @s;
	
	set @logmsg = N'Скопировано ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в таблицу ' + @ProductTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	
	
	
	-- копирование параметров
	-- переносим все параметры кроме тех, операторы которых поступили в рамках данной сессии
	-- эти параметры будут пересозданы далее
	set @logmsg = N'Копирование данных в таблицу ' + @ProductFieldInfoTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
		
	SET @s = N'

	DECLARE @servicePointPackageId uniqueidentifier;
	SET @servicePointPackageId = CAST(N''' + CAST(@servicePointPackageId as NVARCHAR(64)) + ''' AS uniqueidentifier);		
	
	INSERT INTO ' + @ProductFieldInfoTName + N'(
		Id,
		[Order],
		ProductId, 
		Namespace, 
		Name, 
		DataType, 
		Direction, 
		DisplayName, 
		Description, 
		Nullable, 
		MaxLength, 
		Pattern, 
		Mask, 
		AllowedValues, 
		Maximum, 
		Minimum, 
		Factor, 
		[Head],
		ErrorMessage, 
		CreatedDateTime, 
		ModifiedDateTime, 
		CreatedUtcDateTime, 
		ModifiedUtcDateTime,
		EtlPackageId,
		EtlSessionId ) 
	SELECT 
		Id,
		[Order],
		ProductId, 
		Namespace, 
		Name, 
		DataType, 
		Direction, 
		DisplayName, 
		Description, 
		Nullable, 
		MaxLength, 
		Pattern, 
		Mask, 
		AllowedValues, 
		Maximum, 
		Minimum, 
		Factor, 
		[Head],
		ErrorMessage, 
		CreatedDateTime, 
		ModifiedDateTime, 
		CreatedUtcDateTime, 
		ModifiedUtcDateTime,
		EtlPackageId,
		EtlSessionId
	FROM [prodcat].ProductFieldInfo_VIEW
	WHERE ProductId in
		(	select	Id
			from	[prodcat].Product_VIEW
			where	EtlPackageId != @servicePointPackageId)'
	EXEC dbo.MergeUtilsExecSQL @s;	

	set @logmsg = N'Скопировано ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в таблицу ' + @ProductFieldInfoTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	
	
	
	-- копирование данных ProductCategoryLink
	set @logmsg = N'Копирование данных в таблицу ' + @ProductCategoryLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
		
	SET @s = N'

	DECLARE @servicePointPackageId uniqueidentifier;
	SET @servicePointPackageId = CAST(N''' + CAST(@servicePointPackageId as NVARCHAR(64)) + ''' AS uniqueidentifier);	
	
	INSERT INTO ' + @ProductCategoryLinkTName + N'(
		Id, 
		CategoryId, 
		CreatedDateTime, 
		ModifiedDateTime, 
		CreatedUtcDateTime, 
		ModifiedUtcDateTime,
		EtlPackageId,
		EtlSessionId )
	SELECT 
		Id, 
		CategoryId, 
		CreatedDateTime, 
		ModifiedDateTime, 
		CreatedUtcDateTime, 
		ModifiedUtcDateTime,
		EtlPackageId,
		EtlSessionId
	FROM [prodcat].ProductCategoryLink_VIEW
	WHERE Id in
		(	select	Id
			from	[prodcat].Product_VIEW
			where	EtlPackageId != @servicePointPackageId)'
	EXEC dbo.MergeUtilsExecSQL @s;

	set @logmsg = N'Скопировано ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в таблицу ' + @ProductCategoryLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);		
	
	
	-- копирование данных ProductLocationLink
	set @logmsg = N'Копирование данных в таблицу ' + @ProductLocationLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
		
	SET @s = N'

	DECLARE @servicePointPackageId uniqueidentifier;
	SET @servicePointPackageId = CAST(N''' + CAST(@servicePointPackageId as NVARCHAR(64)) + ''' AS uniqueidentifier);	
	
	INSERT INTO ' + @ProductLocationLinkTName + N'(
		Id, 
		LocationId, 
		CreatedDateTime, 
		ModifiedDateTime, 
		CreatedUtcDateTime, 
		ModifiedUtcDateTime,
		EtlPackageId,
		EtlSessionId ) 		 
	SELECT 
		Id, 
		LocationId, 
		CreatedDateTime, 
		ModifiedDateTime, 
		CreatedUtcDateTime, 
		ModifiedUtcDateTime,
		EtlPackageId,
		EtlSessionId
	FROM prodcat.ProductLocationLink_VIEW
	WHERE Id in
		(	select	Id
			from	[prodcat].Product_VIEW
			where	EtlPackageId != @servicePointPackageId)'
	EXEC dbo.MergeUtilsExecSQL @s;

	set @logmsg = N'Скопировано ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в таблицу ' + @ProductLocationLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);


	--------- ВСТАВКА ДАННЫХ ---------	

	-- добавляем данные по локациям финстрим
	set @logmsg = N'Вставка данных в таблицу ' + @AnywayLocationsTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	
	
	SET @s = N'

	DECLARE @EtlPackageId uniqueidentifier;
	SET @EtlPackageId = CAST(N''' + CAST(@EtlPackageId as NVARCHAR(64)) + '''  AS uniqueidentifier);
	DECLARE @EtlSessionId uniqueidentifier;
	SET @EtlSessionId = CAST(N''' + CAST(@EtlSessionId as NVARCHAR(64)) + ''' AS uniqueidentifier);

	INSERT INTO ' + @AnywayLocationsTName + N'(Id, LocationType, ExternalId, InstantTransferSystem, Name, [Code], Address, PhoneNumber, Schedule, Unaddressed, CreatedDateTime, CreatedUtcDateTime, ModifiedDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId)
	select	s.Id, s.LocationType, ExternalId, InstantTransferSystem, Name, [Code], Address, PhoneNumber, Schedule, Unaddressed, s.CreatedDateTime, s.CreatedUtcDateTime, s.CreatedDateTime, s.CreatedUtcDateTime, @EtlPackageId, @EtlSessionId
	from	anyway.BUFFER_Locations s
	where	s.EtlSessionId = @EtlSessionId'

	--print(@s)
	EXEC dbo.MergeUtilsExecSQL @s;	

	set @logmsg = N'Вставлено ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в таблицу ' + @AnywayLocationsTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	


	-- добавляем данные LocationsLink
	set @logmsg = N'Вставка данных в таблицу ' + @AnywayLocationsLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
		
	SET @s = N'

	DECLARE @EtlPackageId uniqueidentifier;
	SET @EtlPackageId = CAST(N''' + CAST(@EtlPackageId as NVARCHAR(64)) + '''  AS uniqueidentifier);
	DECLARE @EtlSessionId uniqueidentifier;
	SET @EtlSessionId = CAST(N''' + CAST(@EtlSessionId as NVARCHAR(64)) + ''' AS uniqueidentifier);	

	INSERT INTO ' + @AnywayLocationsLinkTName + N'(ParentId, Id, CreatedDateTime, CreatedUtcDateTime, ModifiedDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId)
	select	s.ParentId, s.Id, s.CreatedDateTime, s.CreatedUtcDateTime, s.CreatedDateTime, s.CreatedUtcDateTime, @EtlPackageId, @EtlSessionId
	from	anyway.BUFFER_LocationsLink s
	where	s.EtlSessionId = @EtlSessionId'
	--print(@s)
	EXEC dbo.MergeUtilsExecSQL @s;	

	set @logmsg = N'Вставлено ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в таблиу ' + @AnywayLocationsLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	

	-- добавляем данные LocationsMap
	set @logmsg = N'Вставка данных в таблицу ' + @AnywayLocationsMapTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
		
	SET @s = N'

	DECLARE @EtlPackageId uniqueidentifier;
	SET @EtlPackageId = CAST(N''' + CAST(@EtlPackageId as NVARCHAR(64)) + '''  AS uniqueidentifier);
	DECLARE @EtlSessionId uniqueidentifier;
	SET @EtlSessionId = CAST(N''' + CAST(@EtlSessionId as NVARCHAR(64)) + ''' AS uniqueidentifier);	

	INSERT INTO ' + @AnywayLocationsMapTName + N'(Id, LocationId, CreatedDateTime, CreatedUtcDateTime, ModifiedDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId)
	select	s.Id, s.LocationId, s.CreatedDateTime, s.CreatedUtcDateTime, s.CreatedDateTime, s.CreatedUtcDateTime, @EtlPackageId, @EtlSessionId
	from	anyway.BUFFER_LocationsMap s 
	where	s.EtlSessionId = @EtlSessionId'

	--print(@s)
	EXEC dbo.MergeUtilsExecSQL @s;	

	set @logmsg = N'Вставлено ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в таблиу ' + @AnywayLocationsMapTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	
	-- добавляем данные по точкам обслуживания
	set @logmsg = N'Вставка данных в таблицу ' + @AnywayServicePointsTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	
	
	SET @s = N'	

	DECLARE @EtlPackageId uniqueidentifier;
	SET @EtlPackageId = CAST(N''' + CAST(@EtlPackageId as NVARCHAR(64)) + '''  AS uniqueidentifier);
	DECLARE @EtlSessionId uniqueidentifier;
	SET @EtlSessionId = CAST(N''' + CAST(@EtlSessionId as NVARCHAR(64)) + ''' AS uniqueidentifier);	

	insert into ' + @AnywayServicePointsTName + N' (Id, ParentId, ExternalId, LocationType, [Code], InstantTransferSystem, Name, [Address], PhoneNumber, Schedule, [Currency], [Summa], [MaxSumma], [Description], Unaddressed, Toponym, KladrCode, CountryId, RegionName, RegionId, RegionToponym, DistrictName, DistrictId, DistrictToponym, CityName, CityId, CityToponym, TownName, TownId, TownToponym, CreatedDateTime, CreatedUtcDateTime, ModifiedDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId)
	select	l.Id, 
			loc2.Id,
			l.ExternalId, 
			l.LocationType, 
			l.[Code],
			l.InstantTransferSystem,
			l.Name,
			l.[Address],
			l.PhoneNumber,
			l.Schedule,
			pr.ProviderCurrency as Currency,
			pr.Summa,
			pr.MaxSumma,
			pr.[Description],
			l.Unaddressed,
			null as Toponym, 
			null as KladrCode,
			case	when loc4.LocationType = 0 then loc4.Id
					when loc3.LocationType = 0 then loc3.Id
					when loc2.LocationType = 0 then loc2.Id else null end as CountryId,
			case	when loc4.LocationType = 1 then loc4.RegionName
					when loc3.LocationType = 1 then loc3.RegionName
					when loc2.LocationType = 1 then loc2.RegionName else null end as RegionName,
			case	when loc4.LocationType = 1 then loc4.RegionId
					when loc3.LocationType = 1 then loc3.RegionId
					when loc2.LocationType = 1 then loc2.RegionId else null end as RegionId,										
			case	when loc4.LocationType = 1 then loc4.RegionToponym
					when loc3.LocationType = 1 then loc3.RegionToponym
					when loc2.LocationType = 1 then loc2.RegionToponym else null end as RegionToponym,
			null as DistrictName, null as DistrictId, null as DistrictToponym,
			case	when loc4.IsCity = 1 then loc4.CityName
					when loc3.IsCity = 1 then loc3.CityName
					when loc2.IsCity = 1 then loc2.CityName else null end as CityName,
			case	when loc4.IsCity = 1 then loc4.CityId
					when loc3.IsCity = 1 then loc3.CityId
					when loc2.IsCity = 1 then loc2.CityId else null end as CityId,										
			case	when loc4.IsCity = 1 then loc4.CityToponym
					when loc3.IsCity = 1 then loc3.CityToponym
					when loc2.IsCity = 1 then loc2.CityToponym else null end as CityToponym,
			null,null,null,
			l.CreatedDateTime, l.CreatedUtcDateTime, l.CreatedDateTime, l.CreatedUtcDateTime, @EtlPackageId, @EtlSessionId
	from	' + @AnywayLocationsTName + ' l
			inner join ' + @AnywayLocationsLinkTName + ' link1 on link1.Id = l.Id
			inner join ' + @AnywayLocationsMapTName + ' map2 on map2.Id = link1.ParentId
			inner join Geopoints.Location_VIEW loc2 on map2.LocationId = loc2.Id
			
			left join ' + @AnywayLocationsLinkTName + ' link2 on link2.Id = link1.ParentId 
			left join ' + @AnywayLocationsMapTName + ' map3 on map3.Id = link2.ParentId
			left join Geopoints.Location_VIEW loc3 on map3.LocationId = loc3.Id

			left join ' + @AnywayLocationsLinkTName + ' link3 on link3.Id = link2.ParentId
			left join ' + @AnywayLocationsMapTName + ' map4 on map4.Id = link3.ParentId
			left join Geopoints.Location_VIEW loc4 on map4.LocationId = loc4.Id			
			
			inner join prodcat.BUFFER_Product pr on pr.OperatorCode = l.ExternalId
	where	l.LocationType = 8
			and pr.EtlSessionId = @EtlSessionId'

	--print(@s)
	EXEC dbo.MergeUtilsExecSQL @s;		

	set @logmsg = N'Вставлено ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в таблиу ' + @AnywayServicePointsTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	
	
	
	-- добавляем данные Product и ProductVendor	
	set @logmsg = N'Вставка данных в таблицы ' + @ProductVendorTName + N' и ' + @ProductTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	
	

	SET @s = N'

	DECLARE @EtlPackageId uniqueidentifier;
	SET @EtlPackageId = CAST(N''' + CAST(@EtlPackageId as NVARCHAR(64)) + '''  AS uniqueidentifier);
	DECLARE @EtlSessionId uniqueidentifier;
	SET @EtlSessionId = CAST(N''' + CAST(@EtlSessionId as NVARCHAR(64)) + ''' AS uniqueidentifier);

	declare @tmp_insert table
	(
		Id					uniqueidentifier,
		ExternalId			nvarchar(MAX),
		Name				nvarchar(MAX),
		CreatedDateTime		datetime,
		ModifiedDateTime	datetime, 
		CreatedUtcDateTime	datetime,
		ModifiedUtcDateTime	datetime
	)

	INSERT INTO ' + @ProductVendorTName + N'(Id, ExternalId, Name, CreatedDateTime, ModifiedDateTime, CreatedUtcDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId)
	output	inserted.Id, inserted.ExternalId, inserted.Name, inserted.CreatedDateTime, inserted.ModifiedDateTime, inserted.CreatedUtcDateTime, inserted.ModifiedUtcDateTime
	into @tmp_insert
	select	NEWID(), s.OperatorCode, isnull(s.Name_RU, s.Name_EN), GETDATE(), GETDATE(), GETUTCDATE(), GETUTCDATE(), @EtlPackageId, @EtlSessionId
	from	prodcat.BUFFER_Product s
	where	EtlSessionId = @EtlSessionId

	INSERT INTO ' + @ProductTName + N'(Id, [Order], ExternalId, [MCC], [Statistics], ProviderId, Name, EngName, Currency, Logo, CreatedDateTime, ModifiedDateTime, CreatedUtcDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId)
	SELECT	NEWID(), [Order], s.OperatorCode, [MCC], [Statistics], t.Id, s.Name_RU, s.Name_EN, cast(s.ProviderCurrency as nvarchar(50)), s.Logo, GETDATE(), GETDATE(), GETUTCDATE(), GETUTCDATE(), @EtlPackageId, @EtlSessionId
	from	prodcat.BUFFER_Product s
			inner join @tmp_insert t on s.OperatorCode = t.ExternalId
	where	s.EtlSessionId = @EtlSessionId'

	--print(@s)
	EXEC dbo.MergeUtilsExecSQL @s;	

	set @logmsg = N'Вставлено ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в каждую из таблиц ' + @ProductVendorTName + N' и ' + @ProductTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	


	-- добавляем данные по параметрам операторов
	set @logmsg = N'Вставка данных в таблицу ' + @ProductFieldInfoTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	
	
	SET @s = N'	

	DECLARE @EtlPackageId uniqueidentifier;
	SET @EtlPackageId = CAST(N''' + CAST(@EtlPackageId as NVARCHAR(64)) + '''  AS uniqueidentifier);
	DECLARE @EtlSessionId uniqueidentifier;
	SET @EtlSessionId = CAST(N''' + CAST(@EtlSessionId as NVARCHAR(64)) + ''' AS uniqueidentifier);	

	insert into ' + @ProductFieldInfoTName + N' (Id, [Order], ProductId, [Namespace], Name, DataType, Direction, DisplayName, [Description], Nullable, MaxLength, Pattern, Mask, AllowedValues, Maximum, Minimum, Factor, [Head], ErrorMessage, CreatedDateTime, ModifiedDateTime, CreatedUtcDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId)
	select	NEWID(), ps.[Order], p.Id, null, ps.Name, ps.[Type], isnull(ps.Direction, ''1''), ps.Title, ps.Comment, cast(isnull(ps.Required, ''0'') as tinyint)^1, nullif(nullif(ps.MaxLength, ''0''), ''''), nullif(ltrim(rtrim(ps.RegularExpression)), ''''), nullif(ltrim(rtrim(ps.Mask)), ''''), null, null, null, null, [Head], null, GETDATE(), GETDATE(), GETUTCDATE(), GETUTCDATE(), @EtlPackageId, @EtlSessionId
	from	prodcat.BUFFER_ProductParameters ps
			inner join prodcat.BUFFER_Product s on ps.EtlSessionId = s.EtlSessionId and ps.OperatorCode = s.OperatorCode
			inner join ' + @ProductTName + N' p on p.EtlSessionId = s.EtlSessionId and ps.OperatorCode = p.ExternalId	
	where	ps.EtlSessionId = @EtlSessionId'

	--print(@s)
	EXEC dbo.MergeUtilsExecSQL @s;		

	set @logmsg = N'Вставлено ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в таблиу ' + @ProductFieldInfoTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	
	-- добавляем данные ProductCategoryLink
	set @logmsg = N'Вставка данных в таблицу ' + @ProductCategoryLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
		
	SET @s = N'

	DECLARE @EtlPackageId uniqueidentifier;
	SET @EtlPackageId = CAST(N''' + CAST(@EtlPackageId as NVARCHAR(64)) + '''  AS uniqueidentifier);
	DECLARE @EtlSessionId uniqueidentifier;
	SET @EtlSessionId = CAST(N''' + CAST(@EtlSessionId as NVARCHAR(64)) + ''' AS uniqueidentifier);	
	DECLARE @servicePointsRootCategoryId uniqueidentifier;
	SET @servicePointsRootCategoryId = CAST(N''' + CAST(@servicePointsRootCategoryId as NVARCHAR(64)) + ''' AS uniqueidentifier);		

	INSERT INTO ' + @ProductCategoryLinkTName + N'(Id, CategoryId, CreatedDateTime, ModifiedDateTime, CreatedUtcDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId)
	select	p.Id, @servicePointsRootCategoryId, GETDATE(), GETDATE(), GETUTCDATE(), GETUTCDATE(), @EtlPackageId, @EtlSessionId
	from	prodcat.BUFFER_Product s 
			inner join ' + @ProductTName + N' p on s.OperatorCode = p.ExternalId	
	where	s.EtlSessionId = @EtlSessionId'

	--print(@s)
	EXEC dbo.MergeUtilsExecSQL @s;	

	set @logmsg = N'Вставлено ' + cast(@@ROWCOUNT as nvarchar(MAX)) + N' записей в таблиу ' + @ProductCategoryLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	
	-- добавляем данные ProductLocationLink
	set @logmsg = N'Вставка данных в таблицу ' + @ProductLocationLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
		
	SET @s = N'

	DECLARE @EtlPackageId uniqueidentifier;
	SET @EtlPackageId = CAST(N''' + CAST(@EtlPackageId as NVARCHAR(64)) + '''  AS uniqueidentifier);
	DECLARE @EtlSessionId uniqueidentifier;
	SET @EtlSessionId = CAST(N''' + CAST(@EtlSessionId as NVARCHAR(64)) + ''' AS uniqueidentifier);	

	INSERT INTO ' + @ProductLocationLinkTName + N'(Id, LocationId, CreatedDateTime, ModifiedDateTime, CreatedUtcDateTime, ModifiedUtcDateTime, EtlPackageId, EtlSessionId)
	select	a.Id, a.LocationId, GETDATE(), GETDATE(), GETUTCDATE(), GETUTCDATE(), @EtlPackageId, @EtlSessionId
	from
	(
	select	p.Id, l.Id as LocationId
	from	' + @ProductTName + ' p
			inner join ' + @AnywayLocationsTName + ' l on p.ExternalId = l.ExternalId
	where	p.EtlSessionId = @EtlSessionId
	) as a'

	EXEC dbo.MergeUtilsExecSQL @s;	
	set @rows = @@rowcount

	set @logmsg = N'Вставлено ' + cast(@rows as nvarchar(MAX)) + N' записей в таблиу ' + @ProductLocationLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);	

	--------- CONSTRAINTS ---------	
	
	declare @ProductCategoryTName nvarchar(MAX)
	declare @ComissionTName nvarchar(MAX)
	declare @ProductLogoTName nvarchar(MAX)
	
	set @ProductCategoryTName = dbo.RuntimeUtilsGetViewTableName('prodcat.ProductCategory_VIEW')
	set @ComissionTName = dbo.RuntimeUtilsGetViewTableName('prodcat.Comission_VIEW')
	set @ProductLogoTName = dbo.RuntimeUtilsGetViewTableName('prodcat.ProductLogo_VIEW')
	
	set @logmsg = N'Создание constraints. Commission table: ' + @ComissionTName + ', ProductCategoryLink table: ' + @ProductCategoryLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	-- ServicePoints constraints	
	set @logmsg = N'Создание constraints для таблицы ' + @AnywayServicePointsTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	EXEC [Geopoints].[ApplyServicePointsTableConstraints] @AnywayServicePointsTName
	
	-- Product constraints	
	set @logmsg = N'Создание constraints для таблицы ' + @ProductTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	EXEC [prodcat].[ApplyProductTableConstraints] @ProductTName, @ProductVendorTName

	-- ProductFieldInfo constraints	
	set @logmsg = N'Создание constraints для таблицы ' + @ProductFieldInfoTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	EXEC [prodcat].[ApplyProductFieldInfoTableConstraints] @ProductFieldInfoTName, @ProductTName

	-- Commission constraints	
	set @logmsg = N'Создание constraints для таблицы ' + @ComissionTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	EXEC [prodcat].[ApplyComissionTableConstraints] @ComissionTName, @ProductTName, @ProductVendorTName, N'<empty>'

	-- ProductCategoryLink constraints	
	set @logmsg = N'Создание constraints для таблицы ' + @ProductCategoryLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	EXEC [prodcat].[ApplyProductCategoryLinkTableConstraints] @ProductCategoryLinkTName, @ProductTName, @ProductCategoryTName

	-- ProductLocationLink constraints	
	set @logmsg = N'Создание constraints для таблицы ' + @ProductLocationLinkTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	EXEC [prodcat].[ApplyProductLocationLinkTableConstraints] @ProductLocationLinkTName, @ProductTName, N'<empty>'

	-- ProductLogo constraints	
	set @logmsg = N'Создание constraints для таблицы ' + @ProductLogoTName
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

	EXEC [prodcat].[ApplyProductLogoTableConstraints] @ProductLogoTName, @ProductTName


	set @logmsg = N'Обновление представлений'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);

		BEGIN TRAN
			SET @tranStart = 1;
			--Создание представлений
			EXEC RuntimeUtilsSetViewDefinition N'anyway.Locations_VIEW', @AnywayLocationsTName;
			EXEC RuntimeUtilsSetViewDefinition N'anyway.LocationsLink_VIEW', @AnywayLocationsLinkTName;		
			EXEC RuntimeUtilsSetViewDefinition N'anyway.LocationsMap_VIEW', @AnywayLocationsMapTName;		
			EXEC [geopoints].RemapServicePointsView @AnywayServicePointsTName, 0;					
			
			EXEC RuntimeUtilsSetViewDefinition N'[prodcat].Product_VIEW', @ProductTName;
			EXEC RuntimeUtilsSetViewDefinition N'[prodcat].ProductVendor_VIEW', @ProductVendorTName;		
			EXEC RuntimeUtilsSetViewDefinition N'[prodcat].ProductFieldInfo_VIEW', @ProductFieldInfoTName;		
			EXEC RuntimeUtilsSetViewDefinition N'[prodcat].ProductLocationLink_VIEW', @ProductLocationLinkTName;			
			EXEC RuntimeUtilsSetViewDefinition N'[prodcat].ProductCategoryLink_VIEW', @ProductCategoryLinkTName;			
			COMMIT TRAN

	CREATE FULLTEXT INDEX ON [geopoints].ServicePoints_VIEW(Name) KEY INDEX IX_MAIN;

	set @logmsg = N'Представления обновлены'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 8, @logmsg);
	
	SET @tranStart = 0;

	set @logmsg = N'Процедура синхронизации завершена'
	INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
	VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 2, @logmsg);
	
	END TRY
	BEGIN CATCH

		PRINT N'Unexpected error occurred!';
		PRINT ERROR_MESSAGE();	
	
		IF(@tranStart = 1)
			ROLLBACK TRAN;

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(), 5, cast(ERROR_MESSAGE() as nvarchar(1000)));

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH

 

END

GO

/************************************************************
* Utility_DropServicePointsTables.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[Utility_DropServicePointsTables]';
GO




GO

