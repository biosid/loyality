/****** Object:  StoredProcedure [Geopoints].[GetLocations]    Script Date: 04/07/2014 19:40:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Geopoints].[GetLocations]
(
    @parentKladrCode    NVARCHAR(20) = null,
    @locationType       NVARCHAR(max) = null,
    @toponyms           NVARCHAR(max) = null,
    @nameSearchPattern  NVARCHAR(1024) = null,
    @regionIsCityOnly   BIT = null,
    @skip               INT = NULL,
    @top                INT = NULL
)
AS
BEGIN
    --DECLARE @parentKladrCode    NVARCHAR(20) = null--'5000000000000'
    --DECLARE @locationType       NVARCHAR(max) = '1'
    --DECLARE @toponyms           NVARCHAR(max) = null--'г,д,ул'
    --DECLARE @nameSearchPattern  NVARCHAR(1024) = null--'мала'
    --DECLARE @regionIsCityOnly   BIT = null
    --DECLARE @skip               INT = 0
    --DECLARE @top                INT = 50

    SET @skip = ISNULL(@skip, 0);
    SET @top = ISNULL(@top, 1000);

    DECLARE @parentId UNIQUEIDENTIFIER;
    DECLARE @parentLocationType INT;

    SELECT @parentId = [Id], @parentLocationType = [LocationType]
    FROM Geopoints.Location_VIEW lv
    WHERE [KladrCode] = @parentKladrCode

    --print '@parentId = ' + CAST(@parentId AS NVARCHAR(MAX))
    --print '@parentLocationType = ' + CAST(@parentLocationType AS NVARCHAR(MAX))

    DECLARE @sqlWhere NVARCHAR(max) = '1 = 1'

    IF (@locationType IS NOT NULL)
    BEGIN
        SET @sqlWhere = @sqlWhere + '
AND L.LocationType IN ('+@locationType+') '
    END

    IF (@regionIsCityOnly IS NOT NULL AND @regionIsCityOnly = 1)
    BEGIN
        SET @sqlWhere = @sqlWhere + '
AND (L.LocationType <> 1 OR L.IsCity = 1) '
    END

    IF (@toponyms IS NOT NULL)
    BEGIN
        DECLARE @toponymsQuoted varchar(max)

        SELECT @toponymsQuoted = COALESCE(@toponymsQuoted + ', ', '') + QUOTENAME(Substr, '''')
        FROM [dbo].[RuntimeUtilsSplitString] (@toponyms, ',')

        SET @sqlWhere = @sqlWhere + '
AND L.Toponym IN ('+@toponymsQuoted+') '
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
    DECLARE @nameFilter NVARCHAR(1028) = QUOTENAME(@nameSearchPattern+'*', '"')

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
SELECT    *
FROM
(
    SELECT *
    FROM
    (
        SELECT *, ROW_NUMBER() OVER(' + @sqlOrderBy + ') AS rownumber
        FROM
        (
            SELECT * ,'+@sqlSortColumn+'
            FROM  geopoints.Location_VIEW L
            WHERE ' + @sqlWhere +  '
        ) as sub2
    ) AS sub1
    WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top
) as t
' + @sqlOrderBy

print @sql

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
