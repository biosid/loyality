IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[GetLocations]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [Geopoints].[GetLocations]
GO

CREATE PROCEDURE [Geopoints].[GetLocations]
(
    @parentKladrCode	NVARCHAR(20) = null,
    @locationType       NVARCHAR(max) = null,
    @toponyms			NVARCHAR(max) = null,
    @nameSearchPattern  NVARCHAR(1024) = null,
    @skip               INT = NULL,
    @top                INT = NULL
)
AS
BEGIN
	--DECLARE @parentKladrCode	NVARCHAR(20) = null--'5000000000000'
	--DECLARE @locationType		NVARCHAR(max) = '1'
	--DECLARE @toponyms			NVARCHAR(max) = null--'г,д,ул'
	--DECLARE @nameSearchPattern	NVARCHAR(1024) = null--'мала'
	--DECLARE @skip               INT = 0
	--DECLARE @top                INT = 50
		
	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, 1000);

	DECLARE @parentId UNIQUEIDENTIFIER;
	DECLARE @parentLocationType INT;

	SELECT	@parentId = [Id], @parentLocationType = [LocationType]
	  FROM	Geopoints.Location_VIEW lv
	WHERE	[KladrCode] = @parentKladrCode
	
	--print '@parentId = ' + CAST(@parentId AS NVARCHAR(MAX))
	--print '@parentLocationType = ' + CAST(@parentLocationType AS NVARCHAR(MAX))
	
	DECLARE @sqlWhere NVARCHAR(max) = '1 = 1'
	
	IF (@locationType IS NOT NULL)
	BEGIN
		IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#LocationTypes')) 
			DROP TABLE #LocationTypes
			
		CREATE TABLE #LocationTypes (LocationType INT)
		
		INSERT INTO #LocationTypes
		SELECT * FROM [dbo].[RuntimeUtilsSplitString] (@locationType, ',')
		
		IF EXISTS (SELECT TOP 1 1 FROM #LocationTypes)
			SET @sqlWhere = @sqlWhere + '
AND L.LocationType IN (SELECT LocationType FROM #LocationTypes) '
	END
	
	IF (@toponyms IS NOT NULL)
	BEGIN
		IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#Toponyms')) 
			DROP TABLE #Toponyms

		CREATE TABLE #Toponyms (Toponym NVARCHAR(10))

		INSERT INTO #Toponyms
		SELECT * FROM [dbo].[RuntimeUtilsSplitString] (@toponyms, ',')
		
		IF EXISTS (SELECT TOP 1 1 FROM #Toponyms)
			SET @sqlWhere = @sqlWhere + '
AND L.Toponym IN (SELECT Toponym FROM #Toponyms) '
	END

	IF (@parentLocationType IS NOT NULL)
	BEGIN
		IF (@parentLocationType = 1)
			SET @sqlWhere = @sqlWhere + '
AND L.RegionId = @parentId '
		ELSE IF (@parentLocationType = 2)
			SET @sqlWhere = @sqlWhere + '
AND L.DistrictId = @parentId '
		ELSE IF (@parentLocationType = 3)
			SET @sqlWhere = @sqlWhere + '
AND L.CityId = @parentId '
		ELSE IF (@parentLocationType = 4)
			SET @sqlWhere = @sqlWhere + '
AND L.TownId = @parentId '
	END

	DECLARE @sqlSortColumn NVARCHAR(256)
	DECLARE @sqlOrderBy NVARCHAR(256)
	
	DECLARE @nameLen INT = LEN(@nameSearchPattern)
	DECLARE @nameFilter NVARCHAR(1028) = '"' + @nameSearchPattern + '*"'
	
	IF (@nameSearchPattern IS NOT NULL) 
	BEGIN
		SET @sqlSortColumn = '(CASE WHEN LEFT([Name], @nameLen) = @nameSearchPattern THEN 0 ELSE 1 END) Sort'
		SET @sqlOrderBy = 'ORDER BY [Sort], [Name]'
		SET @sqlWhere = @sqlWhere + '
AND CONTAINS(L.Name, @nameFilter) '
	END
	ELSE
	BEGIN
		SET @sqlSortColumn = '1 AS Sort'
		SET @sqlOrderBy = 'ORDER BY [Name]'
	END
	
	DECLARE @sql NVARCHAR(max) = '
;WITH LocationTreeWithData AS 
(
	SELECT L.Id, L.Name,' + @sqlSortColumn + '
	FROM  geopoints.Location_VIEW L						 
	WHERE ' + @sqlWhere +  '
)
SELECT	t.*,' + @sqlSortColumn + '
FROM   Geopoints.Location_VIEW t
WHERE  t.Id IN (SELECT Id
				FROM   (
							SELECT ROW_NUMBER() OVER(' + @sqlOrderBy + ') AS rownumber,
									t2.Id
							FROM   LocationTreeWithData AS t2
						) AS sub1
				WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
' + @sqlOrderBy

	EXECUTE sp_executesql 
	@sql, 
	N'@parentId UNIQUEIDENTIFIER, 
	@nameSearchPattern NVARCHAR(1024), 
	@nameLen INT,
	@nameFilter NVARCHAR(1028),
	@skip INT,
	@top INT', 
	@parentId = @parentId,
	@nameSearchPattern = @nameSearchPattern,
	@nameLen = @nameLen,
	@nameFilter = @nameFilter,
	@skip = @skip,
	@top = @top

END

GO


