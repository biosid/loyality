GO
MergeUtilsDropSPIfExist '[geopoints].[IPLocationCreateBufferTable]';
GO

CREATE PROCEDURE [geopoints].IPLocationCreateBufferTable	
	
AS
BEGIN
	DECLARE @tmp int;
	DECLARE @TableName NVARCHAR(128);
	SET @TableName = 'geopoints.BUFFER_IPLocation';
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

GO
MergeUtilsDropSPIfExist '[geopoints].[IPLocationSyncFromBuffer]';
GO

CREATE PROCEDURE [geopoints].IPLocationSyncFromBuffer
	@packID uniqueidentifier,
	@sessionID uniqueidentifier,
	@xmlData NVARCHAR(MAX)
AS
BEGIN

	EXEC [geopoints].IPLocationCreateBufferTable;

	DECLARE @xml XML;
	SET @xml = @xmlData;

	INSERT INTO geopoints.BUFFER_IPLocation(IPV4From, IPV4To, IPV4FromString, IPV4ToString, 
	City, Country, Region, FedRegion)

	SELECT 
	  tab.col.value('@fri','BIGINT') AS IPfromInt, 
	  tab.col.value('@toi','BIGINT')AS IPtoInt, 
  
	  tab.col.value('@frs','VARCHAR(32)') AS IPfromString, 
	  tab.col.value('@tos','VARCHAR(32)') AS IPtoString, 
  
	  tab.col.value('@cty','VARCHAR(64)') AS City, 
	  tab.col.value('@ctr','VARCHAR(64)') AS Country, 
	  tab.col.value('@reg','VARCHAR(64)') AS Region, 
	  tab.col.value('@fed','VARCHAR(64)') AS FedRegion  
	FROM @xml.nodes('//r') tab(col)

END