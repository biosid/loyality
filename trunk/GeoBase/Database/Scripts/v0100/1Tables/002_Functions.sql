/************************************************************
* MergeUtilsCheckColumnExist.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[MergeUtilsCheckColumnExist]';
GO
CREATE FUNCTION [dbo].[MergeUtilsCheckColumnExist] 
(
	@Table_Name NVARCHAR(128),
	@Field_Name NVARCHAR(128)

)
RETURNS bit
AS
BEGIN
	DECLARE @Res bit;

	set @Field_Name = REPLACE(@Field_Name, '[', '')
	set @Field_Name = REPLACE(@Field_Name, ']', '')

	IF EXISTS(SELECT 1 FROM sysobjects, syscolumns WHERE sysobjects.id = syscolumns.id AND sysobjects.id = OBJECT_ID(@Table_Name) AND syscolumns.name LIKE @Field_Name)
		SET @Res = 1;
	ELSE
		SET @Res = 0;

	return @Res;

END



GO

/************************************************************
* MergeUtilsCheckFuncExist.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[MergeUtilsCheckFuncExist]';
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

/************************************************************
* MergeUtilsCheckIndexExist.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[MergeUtilsCheckIndexExist]';
GO

CREATE FUNCTION [dbo].[MergeUtilsCheckIndexExist] 
(
	@Table_Name NVARCHAR(128),
	@Index_Name NVARCHAR(128)
)
RETURNS bit
AS
BEGIN
	DECLARE @Res bit;
	
	IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(@Table_Name) AND name LIKE @Index_Name)
		SET @Res = 1;
	ELSE
		SET @Res = 0;

	return @Res;

END




GO

/************************************************************
* MergeUtilsCheckSPExist.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[MergeUtilsCheckSPExist]';
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

/************************************************************
* MergeUtilsCheckTableExist.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[MergeUtilsCheckTableExist]';
GO
CREATE FUNCTION [dbo].[MergeUtilsCheckTableExist] 
(
	@Table_Name NVARCHAR(128)
)
RETURNS bit
AS
BEGIN
	DECLARE @Res bit;

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@Table_Name) AND type in (N'U'))
		SET @Res = 1;
	ELSE
		SET @Res = 0;

	return @Res;

END



GO

/************************************************************
* MergeUtilsCheckViewExist.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[MergeUtilsCheckViewExist]';
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
GO

/************************************************************
* RuntimeUtilsGenerateRandomStr.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[RuntimeUtilsGenerateRandomStr]';
GO
CREATE FUNCTION [dbo].[RuntimeUtilsGenerateRandomStr]
(
	@RandomNumber FLOAT,
	@Length INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @ValidCharacters varchar(255);
	SET @ValidCharacters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-=+&$ ';

	DECLARE @RandomNumberInt tinyint;
	SET @RandomNumberInt = Convert(tinyint, ((len(@ValidCharacters) - 1) * @RandomNumber + 1));

	DECLARE @CurrentCharacter varchar(1);
	SET @CurrentCharacter = SUBSTRING(@ValidCharacters, @RandomNumberInt, 1);

	RETURN REPLICATE(@CurrentCharacter, @Length);
END


GO

/************************************************************
* RuntimeUtilsGetDateOnly.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[RuntimeUtilsGetDateOnly]';
GO
CREATE FUNCTION [dbo].[RuntimeUtilsGetDateOnly]
(
 @date datetime
)
RETURNS datetime
AS
BEGIN
 
 declare @result datetime
 
 set @result = CAST(FLOOR(CAST(@date AS float)) AS DATETIME)
 
 return @result

END
GO

/************************************************************
* RuntimeUtilsGetPureTableName.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[RuntimeUtilsGetPureTableName]';
GO

--Выделяет имя таблицы из строку схема.имятаблицы. Требутеся чтобы делать имена constaints
CREATE FUNCTION [dbo].[RuntimeUtilsGetPureTableName] 
(
	@Table_Name NVARCHAR(128)	
)
RETURNS NVARCHAR(128)
AS
BEGIN
	DECLARE @res NVARCHAR(128);
	SET @res = @Table_Name;
	SET @res = REPLACE(@res, '[','');
	SET @res = REPLACE(@res, ']','');
	DECLARE @sindex INT;
	SET @sindex = CHARINDEX('.', @res);
	IF(@sindex > 0)
		SET @res = RIGHT(@res, LEN(@res) - @sindex);

	RETURN @res;
END




GO

/************************************************************
* RuntimeUtilsGetSchemaName.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[RuntimeUtilsGetSchemaName]';
GO

--Выделяет имя cхемы из строки схема.имятаблицы
CREATE FUNCTION [dbo].[RuntimeUtilsGetSchemaName]
(
	@Table_Name NVARCHAR(128)	
)
RETURNS NVARCHAR(128)
AS
BEGIN
	DECLARE @res NVARCHAR(128);
	SET @res = @Table_Name;
	SET @res = REPLACE(@res, '[','');
	SET @res = REPLACE(@res, ']','');
	DECLARE @sindex INT;
	SET @sindex = CHARINDEX('.', @res);
	IF(@sindex > 0)
		SET @res = LEFT(@res, @sindex - 1);
	ELSE
		SET @res = 'dbo';
	RETURN @res;
END




GO

/************************************************************
* RuntimeUtilsGetTableNameWithDate.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[RuntimeUtilsGetTableNameWithDate]';
GO
--ИЗ базового имени таблицы делает имя новой таблицы 
CREATE FUNCTION [dbo].[RuntimeUtilsGetTableNameWithDate] 
(
	@Table_Name NVARCHAR(128)	
)
RETURNS NVARCHAR(128)
AS
BEGIN
	DECLARE @Res NVARCHAR(128);
	DECLARE @ResTemp NVARCHAR(128);

	DECLARE @d DATETIME
	SET @d = GETDATE()

	SET @Res = @Table_Name + '_' + CAST(DATEPART(yy, @d) as NVARCHAR(16)) 
	+ dbo.RuntimeUtilsGetTwoDigitsString(DATEPART(mm, @d)) 
	+ dbo.RuntimeUtilsGetTwoDigitsString(DATEPART(dd, @d)) + '_' 

	+ dbo.RuntimeUtilsGetTwoDigitsString(DATEPART(Hh, @d))   
	+ dbo.RuntimeUtilsGetTwoDigitsString(DATEPART(mi, @d))   
	+ dbo.RuntimeUtilsGetTwoDigitsString(DATEPART(ss, @d))

	SET @ResTemp = @Res

	DECLARE @Suff INT
	SET @Suff = 1
	WHILE dbo.MergeUtilsCheckTableExist(@Res) = 1
	BEGIN
		SET @Res = @ResTemp + '_' + CAST(@Suff as nvarchar(3))
		SET @Suff = @Suff + 1
	END

	RETURN @Res
END
GO

/************************************************************
* RuntimeUtilsGetTwoDigitsString.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[RuntimeUtilsGetTwoDigitsString]';
GO
--возвращает строку из двух символов для числа. Если число содержит только одну цифру то подставляет 0 вначало
CREATE FUNCTION [dbo].[RuntimeUtilsGetTwoDigitsString] 
(
	@i int
)
RETURNS NVARCHAR(16)
AS
BEGIN
	DECLARE @res NVARCHAR(16);
	if(@i < 10)
		SET @res = '0' + CAST(@i as NVARCHAR(16))
	ELSE
		SET @res = CAST(@i as NVARCHAR(16))
		

	return @res;

END




GO

/************************************************************
* RuntimeUtilsGetViewTableName.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[RuntimeUtilsGetViewTableName]';
GO
--Получает имя таблицы по которой построена view
CREATE FUNCTION [dbo].[RuntimeUtilsGetViewTableName]
(
	@View_Name NVARCHAR(128)
)
RETURNS NVARCHAR(128)
AS
BEGIN
	DECLARE @result NVARCHAR(MAX)
	
	DECLARE @schemaName NVARCHAR(MAX)
	DECLARE @viewName NVARCHAR(MAX)	
	SELECT @schemaName = [dbo].[RuntimeUtilsGetSchemaName](@View_Name)
	SELECT @viewName = [dbo].[RuntimeUtilsGetPureTableName](@View_Name)
	
	DECLARE @viewText NVARCHAR(MAX)
	
	-- приведение текста view к строке без пробелов, табуляций, переводов строк
	SELECT @viewText = REPLACE(REPLACE(REPLACE(ISNULL(smv.definition, ssmv.definition), CHAR(13) + CHAR(10), ''), CHAR(32), ''), CHAR(9), '')
	FROM   sys.all_views AS v
	       LEFT OUTER JOIN sys.sql_modules AS smv ON  smv.object_id = v.object_id
	       LEFT OUTER JOIN sys.system_sql_modules AS ssmv ON  ssmv.object_id = v.object_id
	WHERE  v.type = N'V'
	       AND v.name = @viewName
	       AND SCHEMA_NAME(v.schema_id) = @schemaName
	
	DECLARE @shift INT	
	DECLARE @from INT
	DECLARE @to INT
	
	SET @shift = 11
	SET @from = CHARINDEX('--#', @viewText, 0)
	SET @to = CHARINDEX('#', @viewText, @from + 3)
	
	IF (@from = 0 OR @to = 0)
	BEGIN
	    -- если коментарий отсутствует, берем название таблицы их текста вьюхи
	    SET @from = CHARINDEX('SELECT*FROM', @viewText, 0)
	    DECLARE @length INT
	    IF CHARINDEX('WITH(NOLOCK)', @viewText, 0) > 0
	        SET @length = LEN(@viewText) - @from - 22
	    ELSE IF CHARINDEX('withschemabinding', @viewText, 0) > 0
	    BEGIN
			SET @shift = 5
			SET @from = CHARINDEX(']FROM', @viewText, 0)
			SET @length = LEN(@viewText) - @from - 1
		END
	    ELSE
	        SET @length = LEN(@viewText) - @from - 10
	    
	    SET @result = SUBSTRING(@viewText, @from + @shift, @length)
	END
	ELSE
	BEGIN
	    SET @result = SUBSTRING(@viewText, @from + 3, @to - @from - 3)
	END
	
	RETURN @result
END
GO

/************************************************************
* SchemaUtilsGetBufferTableName.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[SchemaUtilsGetBufferTableName]';
GO
CREATE FUNCTION [dbo].SchemaUtilsGetBufferTableName 
(
	@Table_Name NVARCHAR(128)	

)
RETURNS NVARCHAR(128)
AS
BEGIN
	DECLARE @Res NVARCHAR(128);

	SET @Res = [dbo].[RuntimeUtilsGetSchemaName](@Table_Name) + '.BUFFER_' + [dbo].[RuntimeUtilsGetPureTableName](@Table_Name);

	return @Res;

END



GO

/************************************************************
* ShemaUtilsGetLastestTableName.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[ShemaUtilsGetLastestTableName]';
GO
CREATE FUNCTION [dbo].ShemaUtilsGetLastestTableName 
(
	@tablename NVARCHAR(128)
)
RETURNS NVARCHAR(128)
AS
BEGIN
	DECLARE @Res NVARCHAR(128);
	DECLARE @T NVARCHAR(128);
	SET @T = [dbo].[RuntimeUtilsGetPureTableName](@tablename);

	SELECT TOP 1 @Res = Name FROM sys.objects WHERE CHARINDEX(LOWER(@T + '_'), LOWER(name)) = 1 
	AND LOWER(@T + '_history') <> LOWER(name)
	AND LOWER(@T + '_buffer') <> LOWER(name)
	AND type in (N'U') 
	ORDER BY name DESC

	return @Res

END



GO

/************************************************************
* ShemaUtilsGetStandardColumns.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[ShemaUtilsGetStandardColumns]';
GO
CREATE FUNCTION [dbo].ShemaUtilsGetStandardColumns 
(
	@withDates bit,
	@withEtl bit
)
RETURNS NVARCHAR(2048)
AS
BEGIN
	DECLARE @Res NVARCHAR(2048);

	SET @Res = '[CreatedDateTime] [datetime] NULL,
	CreatedUtcDateTime [datetime] NULL, 
	ModifiedDateTime [datetime] NULL,
	ModifiedUtcDateTime [datetime] NULL,
	EtlPackageId [uniqueidentifier] NULL,
	EtlSessionId [uniqueidentifier] NULL';

	return @Res;

END



GO

/************************************************************
* WildcardsShield.sql
************************************************************/


MergeUtilsDropFuncIfExist '[dbo].[WildcardsShield]';
GO
CREATE FUNCTION [dbo].[WildcardsShield]
(
	@str NVARCHAR(1024),
	@ch NVARCHAR
)
RETURNS NVARCHAR(1024)
AS
BEGIN
	SET @str = REPLACE(@str,'%',@ch + '%');
	SET @str = REPLACE(@str,'[',@ch + '[');
	SET @str = REPLACE(@str,']',@ch + ']');
	SET @str = REPLACE(@str,'_',@ch + '_');

	RETURN @str;
END


GO

/************************************************************
* GetDistanceBetweenGeoPoints.sql
************************************************************/


MergeUtilsDropFuncIfExist '[Geopoints].[GetDistanceBetweenGeoPoints]';
GO
--Вычисляет расстояние между двумя точками по их координатам
CREATE FUNCTION [Geopoints].[GetDistanceBetweenGeoPoints]
(
	@lng1 numeric(20,12),
	@lat1 numeric(20,12),
	@lng2 numeric(20,12),
	@lat2 numeric(20,12)
)
RETURNS  numeric(21,12)
AS
BEGIN
	DECLARE @radius decimal(20,12)
	SET @radius = 6371

	DECLARE @dLat decimal(20,12)
	DECLARE @dLon decimal(20,12)


	DECLARE @a decimal(20,12)
	DECLARE @c decimal(20,12)

	SET @dLat = radians(@lat2 - @lat1)
	SET @dLon = radians(@lng2 - @lng1);
	SET @lat1 = radians(@lat1);
	SET @lat2 = radians(@lat2);

	SET @a = sin(@dLat/2) * sin(@dLat/2) + sin(@dLon/2) * sin(@dLon/2) * cos(@lat1) * cos(@lat2); 

	SET @c = 2 * atn2(sqrt(@a), sqrt(1-@a)); 

	RETURN @radius * @c
END;


GO

/************************************************************
* SearchLocationForIP.sql
************************************************************/


MergeUtilsDropFuncIfExist '[Geopoints].[SearchLocationForIP]';
GO
CREATE FUNCTION [Geopoints].[SearchLocationForIP]
(
	@City       NVARCHAR(128),
	@Region     NVARCHAR(128),
	@FedRegion  NVARCHAR(128),
	@Country    NVARCHAR(128)
)
RETURNS UNIQUEIDENTIFIER
AS
BEGIN
	DECLARE @toponym NVARCHAR(32);
	DECLARE @toponymPosition INT;
	
	SELECT TOP 1 @toponym = NAME
	FROM   [geopoints].[LocationRegionToponymDic] d WITH(NOLOCK)
	WHERE  CHARINDEX(LOWER(NAME), LOWER(@Region)) >= 1
	ORDER BY NAME DESC;
	
	IF (@toponym IS NOT NULL)
	BEGIN
	    SET @Region = REPLACE(@Region, @toponym, '');
	    SET @Region = LTRIM(RTRIM(@Region));
	END
	
	DECLARE @res UNIQUEIDENTIFIER;	
	
	SELECT TOP 1 @res = Id
	FROM   Geopoints.Location_VIEW loc
	WHERE  loc.RegionName = @Region
	       AND loc.Name = @City
	       AND loc.Toponym = N'г';
	
	IF (@res IS NULL)
	    SELECT TOP 1 @res = Id
	    FROM   Geopoints.Location_VIEW loc
	    WHERE  loc.RegionName IS NULL
	           AND loc.Name = @City
	           AND loc.Toponym = N'г';
	RETURN @res;
END
GO
GO

