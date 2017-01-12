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
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 8, N'�������� ��������� �������� ���������� IP � ��');

		EXEC [geopoints].CreateIPRangesBufferTable;

		DECLARE @xml XML;
		SET @xml = @xmlData;


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 3, N'������ ���������� ������ � �������� �������');

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
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 4, N'���������� ������ � �������� ������� ���������');

		DECLARE @s NVARCHAR(MAX);


		DECLARE @tbl_IPRanges NVARCHAR(128);
		

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 3, N'�������� ����� ������� ���������� IP-�������');

		SET @tbl_IPRanges = dbo.RuntimeUtilsGetTableNameWithDate('[geopoints].IPRanges');
		EXEC [geopoints].[CreateIPRangesTable] @tbl_IPRanges;


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 4, N'�������� ������� ��� ���������� IP-������� ���������');


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 3, N'������ ���������� ������� IP-���������� � ��������� � ���������������');


		SET @s = 'INSERT INTO ' + @tbl_IPRanges + '(IPV4From, IPV4To, IPV4FromString, IPV4ToString, LocationId) 		
		SELECT IPV4From, IPV4To, IPV4FromString, IPV4ToString,
		[geopoints].[SearchLocationForIP](City, Region, FedRegion, Country)
		
		FROM geopoints.BUFFER_IPRanges WITH(NOLOCK);';

		EXEC dbo.MergeUtilsExecSQL @s;


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 4, N'���������� ������� IP-���������� ���������');




		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 3, N'���������� ����������� � ������� ����������');

		EXEC [geopoints].[ApplyIPRangesTableConstraints] @tbl_IPRanges;

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 4, N'���������� ����������� � ������� ���������� ���������');


		--INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		--VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer before delete from buffer');

		--DELETE FROM geopoints.BUFFER_IPRanges;

		--INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		--VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 8, 'SyncIPRangesFromBuffer after delete from buffer');


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 3, N'���������� ������������� ��� ������� ����������');

		EXEC RuntimeUtilsSetViewDefinition '[geopoints].IPRanges_VIEW', @tbl_IPRanges;

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 4, N'���������� ������������� ���������');




		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 8, N'��������� �������� ������ IP-���������� ������� ���������');

	END TRY
	BEGIN CATCH
		PRINT 'Unexpected error occurred!';
		PRINT ERROR_MESSAGE();		

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
			VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 5, N'��������� �������� ������ IP-���������� ��������� c ������� ' + ERROR_MESSAGE());
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
	       AND loc.Toponym = N'�';
	
	IF (@res IS NULL)
	    SELECT TOP 1 @res = Id
	    FROM   Geopoints.Location_VIEW loc
	    WHERE  loc.RegionName IS NULL
	           AND loc.Name = @City
	           AND loc.Toponym = N'�';
	RETURN @res;
END
GO