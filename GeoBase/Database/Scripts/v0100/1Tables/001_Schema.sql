/************************************************************
* StartScript.sql
************************************************************/

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

CREATE FUNCTION [dbo].[MergeUtilsCheckFuncExist] 
(	
	@Name NVARCHAR(256)
)
RETURNS bit
AS
BEGIN
	DECLARE @Res bit;

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@Name) AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
		SET @Res = 1;
	ELSE
		SET @Res = 0;

	return @Res;

END
GO

CREATE FUNCTION [dbo].[MergeUtilsCheckSPExist] 
(	
	@Name NVARCHAR(256)
)
RETURNS bit
AS
BEGIN
	DECLARE @Res bit;

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@Name) AND type in (N'P', N'PC'))
		SET @Res = 1;
	ELSE
		SET @Res = 0;

	return @Res;

END
GO

CREATE FUNCTION [dbo].[MergeUtilsCheckViewExist] 
(	
	@Name NVARCHAR(256)
)
RETURNS bit
AS
BEGIN
	DECLARE @Res bit;

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@Name) AND type in (N'V'))
		SET @Res = 1;
	ELSE
		SET @Res = 0;

	return @Res;

END
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


GO

/************************************************************
* Create_Geopoints_Schema.sql
************************************************************/

IF schema_id('Geopoints') is null
BEGIN
	EXECUTE('CREATE SCHEMA Geopoints');
END;
GO

