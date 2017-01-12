MergeUtilsDropSPIfExist '[geopoints].[IPRangesGetLocationsByIP]';

GO

--Ищем только для LocationType 1 (Москва, Питер) и 3,4 ( город и населенный пункт)
--ТАк как в базе содержатся только города
CREATE PROCEDURE [geopoints].[IPRangesGetLocationsByIP]
	@ipINT BIGINT,
	@locationType INT,
	@locale VARCHAR(2),    
    @skip INT = NULL,
    @top INT = NULL
AS
BEGIN

	DECLARE @S NVARCHAR(2048)

	DECLARE @rootLocationId uniqueIdentifier;
	DECLARE @rootType INT;

	SELECT TOP 1 @rootLocationId = r.LocationId, @rootType = L.LocationType FROM Geopoints.IPRanges_VIEW r
	INNER JOIN Geopoints.V_Location	L ON L.Id = r.LocationId
	WHERE IPV4From <= @ipINT AND @ipINT <= IPV4To;


	

	DECLARE @default_top INT SET @default_top = 1000;
	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);


	IF(@rootType = @locationType)
		BEGIN
			-- Возвращаем рутовую локацию. Если skip > 0 или Top == 0 то возвращаем ничего
			SELECT * FROM Geopoints.V_Location
			WHERE Id = @rootLocationId AND (@skip = 0 AND @top > 0);

		END
	ELSE
		BEGIN

			IF(@rootType = 1)
			BEGIN
				WITH LocationTreeWithData AS 
				(
					SELECT * FROM Geopoints.V_Location
					WHERE LocationType = @locationType AND RegionID = @rootLocationId
				)
				SELECT TOP (@top) * FROM LocationTreeWithData t WHERE t.Id not in (SELECT TOP (@skip) t1.id FROM LocationTreeWithData AS t1 ORDER BY Name) ORDER BY Name
			END


			IF(@rootType = 3)
			BEGIN
				WITH LocationTreeWithData AS 
				(
					SELECT * FROM Geopoints.V_Location
					WHERE LocationType = @locationType AND CityID = @rootLocationId
				)
				SELECT TOP (@top) * FROM LocationTreeWithData t WHERE t.Id not in (SELECT TOP (@skip) t1.id FROM LocationTreeWithData AS t1 ORDER BY Name) ORDER BY Name
			END

			IF(@rootType = 4)
			BEGIN
				WITH LocationTreeWithData AS 
				(
					SELECT * FROM Geopoints.V_Location
					WHERE LocationType = @locationType AND TownID = @rootLocationId
				)
				SELECT TOP (@top) * FROM LocationTreeWithData t WHERE t.Id not in (SELECT TOP (@skip) t1.id FROM LocationTreeWithData AS t1 ORDER BY Name) ORDER BY Name
			END
			

		END

END

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


	DECLARE @rootLocationId uniqueIdentifier;
	DECLARE @rootType INT;

	SELECT TOP 1 @rootLocationId = Id, @rootType = LocationType FROM  Geopoints.V_Location 
	WHERE Id = @parentID;

	IF(@rootType = 1)
	BEGIN
		WITH LocationTreeWithData AS 
		(
			SELECT * FROM Geopoints.V_Location
			WHERE (@locationType IS NULL OR geopoints.[V_Location].LocationType = @locationType)
			AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern)					
			AND RegionID = @rootLocationId
		)
		SELECT TOP (@top) * FROM LocationTreeWithData t WHERE t.Id not in (SELECT TOP (@skip) t1.id FROM LocationTreeWithData AS t1 ORDER BY Name) ORDER BY Name
	END


	IF(@rootType = 3)
	BEGIN
		WITH LocationTreeWithData AS 
		(
			SELECT * FROM Geopoints.V_Location
			WHERE (@locationType IS NULL OR geopoints.[V_Location].LocationType = @locationType)
			AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern)					
			AND CityID = @rootLocationId
		)
		SELECT TOP (@top) * FROM LocationTreeWithData t WHERE t.Id not in (SELECT TOP (@skip) t1.id FROM LocationTreeWithData AS t1 ORDER BY Name) ORDER BY Name
	END

	IF(@rootType = 4)
	BEGIN
		WITH LocationTreeWithData AS 
		(
			SELECT * FROM Geopoints.V_Location
			WHERE (@locationType IS NULL OR geopoints.[V_Location].LocationType = @locationType)
			AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern)					
			AND TownID = @rootLocationId					
		)
		SELECT TOP (@top) * FROM LocationTreeWithData t WHERE t.Id not in (SELECT TOP (@skip) t1.id FROM LocationTreeWithData AS t1 ORDER BY Name) ORDER BY Name
	END	
END
GO
MergeUtilsDropSPIfExist '[geopoints].[LocationGetCountries]';
GO
CREATE PROCEDURE [geopoints].[LocationGetCountries]
(	
    @locale VARCHAR(2),    
    @skip INT = NULL,
    @top INT = NULL
)
AS
BEGIN

	DECLARE @default_top INT SET @default_top = 1000;

	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);

	WITH LocationTreeWithData AS 
	(
		SELECT * FROM Geopoints.V_Location
		WHERE geopoints.[V_Location].LocationType = 0			
	)
	SELECT TOP (@top) * FROM LocationTreeWithData t WHERE t.Id not in (SELECT TOP (@skip) t1.id FROM LocationTreeWithData AS t1 ORDER BY Name) ORDER BY Name

END