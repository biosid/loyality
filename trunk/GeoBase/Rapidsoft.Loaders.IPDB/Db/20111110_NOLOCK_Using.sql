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
			VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 5, N'Процедура разноски данных IP-диапазонов завершена c ошибкой ' + ERROR_MESSAGE());
	END CATCH
END
GO


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