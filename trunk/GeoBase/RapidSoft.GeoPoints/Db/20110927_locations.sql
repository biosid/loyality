GO
MergeUtilsDropSPIfExist '[geopoints].[LocationGetLocationsByParent]';
GO
CREATE PROCEDURE [geopoints].[LocationGetLocationsByParent]
(
	@parentID uniqueidentifier,    
	@locationType int,
	@nameSearchPattern NVARCHAR(1024),
    @locale VARCHAR(2),    
    @skip INT = NULL,
    @top INT = NULL
)
AS
BEGIN

	DECLARE @default_top INT SET @default_top = 1000;

	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);

	WITH t AS 
	(
	 SELECT * FROM geopoints.[V_Location]
	 WHERE (geopoints.[V_Location].ParentId = @parentID OR 
	 (@parentID IS NULL AND geopoints.[V_Location].ParentId IS NULL))
	 AND (@locationType IS NULL OR geopoints.[V_Location].LocationType = @locationType)
	 AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern)
	)

	SELECT TOP (@top) * FROM t WHERE 
	t.Id not in (
		SELECT TOP (@skip) t1.id FROM t AS t1 		
		ORDER BY Name		
	) ORDER BY Name
END
GO