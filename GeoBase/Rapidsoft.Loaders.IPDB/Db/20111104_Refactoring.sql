MergeUtilsDropSPIfExist '[Geopoints].[CreateIPRangesBufferTable]';
GO
CREATE PROCEDURE [Geopoints].[CreateIPRangesBufferTable]
	
AS
BEGIN
	DECLARE @tmp int;
	DECLARE @TableName NVARCHAR(128);
	SET @TableName = 'geopoints.BUFFER_IPRanges';
	DECLARE @sql NVARCHAR(2048);
	IF([dbo].[MergeUtilsCheckTableExist](@TableName) = 0)
	BEGIN
		
		SET  @sql = 'CREATE TABLE ' + @TableName + '(		
			[IPV4From] [bigint] NOT NULL,
			[IPV4To] [bigint] NOT NULL,
			[IPV4FromString] [nvarchar](30) NOT NULL,
			[IPV4ToString] [nvarchar](30) NOT NULL,
			[Country] [nvarchar](64) NULL,
			[Region] [nvarchar](64) NULL,
			[FedRegion] [nvarchar](64) NULL,
			[City] [nvarchar](64) NULL

		) ON [PRIMARY];';
		EXEC dbo.MergeUtilsExecSQL @sql;		
	END

	SET  @sql = 'DELETE FROM ' + @TableName + ';'
	EXEC dbo.MergeUtilsExecSQL @sql;
	
END
GO
[MergeUtilsDropSPIfExistAndCreateSynonym] '[Geopoints].[IPRangesCreateBufferTable]', '[Geopoints].[CreateIPRangesBufferTable]';
GO


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
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer started');

		EXEC [geopoints].CreateIPRangesBufferTable;

		DECLARE @xml XML;
		SET @xml = @xmlData;


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer before buffer fill');

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
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer after buffer fill');

		DECLARE @s NVARCHAR(MAX);


		DECLARE @tbl_IPRanges NVARCHAR(128);
		

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer before table creation');

		SET @tbl_IPRanges = dbo.RuntimeUtilsGetTableNameWithDate('[geopoints].IPRanges');
		EXEC [geopoints].[CreateIPRangesTable] @tbl_IPRanges;


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer after table creation');


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer before data merge');


		SET @s = 'INSERT INTO ' + @tbl_IPRanges + '(IPV4From, IPV4To, IPV4FromString, IPV4ToString, LocationId) 		
		SELECT IPV4From, IPV4To, IPV4FromString, IPV4ToString,
		[geopoints].[SearchLocationForIP](City, Region, FedRegion, Country)
		
		FROM geopoints.BUFFER_IPRanges;';

		EXEC dbo.MergeUtilsExecSQL @s;


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer after data merge');




		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer before apply constraints');

		EXEC [geopoints].[ApplyIPRangesTableConstraints] @tbl_IPRanges;

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer after apply constraints');


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer before delete from buffer');

		--DELETE FROM geopoints.BUFFER_IPRanges;

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer after delete from buffer');


		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer before set view definition');

		EXEC RuntimeUtilsSetViewDefinition '[geopoints].IPRanges_VIEW', @tbl_IPRanges;

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer after set view definition');




		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text])
		VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer finished successfully');

	END TRY
	BEGIN CATCH
		PRINT 'Unexpected error occurred!';
		PRINT ERROR_MESSAGE();		

		INSERT INTO dbo.EtlMessages(EtlPackageId, EtlSessionId, LogDateTime, LogUtcDateTime,  MessageType, [Text], ErrorType)
			VALUES(@packID, @sessionID, GETDATE(), GETUTCDATE(), 0, 'SyncIPRangesFromBuffer finished with error', ERROR_MESSAGE());
	END CATCH


END
GO
[MergeUtilsDropSPIfExistAndCreateSynonym] '[Geopoints].[IPRangesSyncFromBuffer]', '[Geopoints].[SyncIPRangesFromBuffer]';
GO


MergeUtilsDropFuncIfExist '[Geopoints].[SearchLocationForIP]';
GO
CREATE FUNCTION [Geopoints].[SearchLocationForIP]
(
	@City NVARCHAR(128), 	
	@Region NVARCHAR(128), 
	@FedRegion NVARCHAR(128),
	@Country NVARCHAR(128)
)
RETURNS uniqueidentifier
AS
BEGIN

	DECLARE @toponym NVARCHAR(32);
	DECLARE @toponymPosition INT;

	SELECT TOP 1 @toponym = Name FROM [geopoints].[LocationRegionToponymDic] d
	WHERE CHARINDEX(LOWER(NAME), LOWER(@Region)) >= 1
	ORDER BY Name DESC;

	IF(@toponym IS NOT NULL)
	BEGIN

		SET @Region = REPLACE(@Region, @toponym, '');
		SET @Region = LTrim(RTrim(@Region));
	END

	DECLARE @res uniqueidentifier;	

	SELECT TOP 1 @res = Id FROM Geopoints.V_Location loc
	WHERE loc.RegionName = @Region AND loc.Name = @City AND loc.Toponym = N'ã';

	IF(@res IS NULL)
		SELECT TOP 1 @res = Id FROM Geopoints.V_Location loc
		WHERE loc.RegionName IS NULL AND loc.Name = @City AND loc.Toponym = N'ã';
	

	return @res;
END
GO
[MergeUtilsDropFuncIfExistAndCreateSynonym] '[Geopoints].[LocationSearchForIP]', '[Geopoints].[SearchLocationForIP]';
GO
