MergeUtilsDropSPIfExist '[Geopoints].[IPRangesGetLocationsByIP]';
GO
--Ищем только для LocationType 1 (Москва, Питер) и 3,4 ( город и населенный пункт)
--ТАк как в базе содержатся только города
CREATE PROCEDURE [Geopoints].[IPRangesGetLocationsByIP]
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

	SELECT @rootLocationId = r.LocationId, @rootType = L.LocationType FROM Geopoints.IPRanges_VIEW r
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
			print @rootType
			IF(@rootType = 1)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,Name FROM Geopoints.V_Location
						WHERE LocationType = @locationType AND RegionID = @rootLocationId
					)
					SELECT * FROM Geopoints.V_Location t WHERE t.Id in (
							SELECT Id FROM 
							(
							SELECT 
									ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
							FROM
									intLocationTreeWithData AS t2                                                                                 
							) 
							AS sub1 
							WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
						) ORDER BY Name
				END
			ELSE IF(@rootType = 3)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,Name FROM Geopoints.V_Location
						WHERE LocationType = @locationType AND CityID = @rootLocationId
					)
					SELECT * FROM Geopoints.V_Location t WHERE t.Id in (
							SELECT Id FROM 
							(
							SELECT 
									ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
							FROM
									intLocationTreeWithData AS t2  
							) 
							AS sub1 
							WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
						) ORDER BY Name
				END
			ELSE IF(@rootType = 4)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,Name FROM Geopoints.V_Location
						WHERE LocationType = @locationType AND TownID = @rootLocationId
					)
					SELECT * FROM Geopoints.V_Location t WHERE t.Id in (
							SELECT Id FROM 
							(
							SELECT 
									ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
							FROM
									intLocationTreeWithData AS t2                                                                                 
							) 
							AS sub1 
							WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
						) ORDER BY Name
				END
			

		END

END
GO
--------------------------------------------------------------------------------------------
MergeUtilsDropSPIfExist '[Geopoints].[LocationGetLocationsByParent]';
GO
CREATE PROCEDURE [Geopoints].[LocationGetLocationsByParent]
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

	SELECT @rootLocationId = Id, @rootType = LocationType FROM  Geopoints.V_Location 
	WHERE Id = @parentID;

	IF(@rootType = 1)
		BEGIN
			WITH LocationTreeWithData AS 
			(
				SELECT Id,Name FROM Geopoints.V_Location
				WHERE (@locationType IS NULL OR geopoints.[V_Location].LocationType = @locationType)
				AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern)					
				AND RegionID = @rootLocationId
			)
			SELECT * FROM Geopoints.V_Location t WHERE t.Id in (
							SELECT Id FROM 
							(
							SELECT 
									ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
							FROM
									LocationTreeWithData AS t2                                                                                 
							) 
							AS sub1 
							WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
			) ORDER BY Name
		END
	ELSE IF(@rootType = 3)
		BEGIN
			WITH LocationTreeWithData AS 
			(
				SELECT Id,Name FROM Geopoints.V_Location
				WHERE (@locationType IS NULL OR geopoints.[V_Location].LocationType = @locationType)
				AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern)					
				AND CityID = @rootLocationId
			)
			SELECT * FROM Geopoints.V_Location t WHERE t.Id in (
							SELECT Id FROM 
							(
							SELECT 
									ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
							FROM
									LocationTreeWithData AS t2                                                                                 
							) 
							AS sub1 
							WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
			) ORDER BY Name
		END
	ELSE IF(@rootType = 4)
		BEGIN
			WITH LocationTreeWithData AS 
			(
				SELECT Id,Name FROM Geopoints.V_Location
				WHERE (@locationType IS NULL OR geopoints.[V_Location].LocationType = @locationType)
				AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern)					
				AND TownID = @rootLocationId					
			)
			SELECT * FROM Geopoints.V_Location t WHERE t.Id in (
							SELECT Id FROM 
							(
							SELECT 
									ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
							FROM
									LocationTreeWithData AS t2                                                                                 
							) 
							AS sub1 
							WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
			) ORDER BY Name
		END	
END
GO
--------------------------------------------------------------------------------------------------------------

MergeUtilsDropSPIfExist '[Geopoints].[LocationGetCountries]';
GO
CREATE PROCEDURE [Geopoints].[LocationGetCountries]
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
		SELECT Id,Name FROM Geopoints.V_Location
		WHERE geopoints.[V_Location].LocationType = 0			
	)
	SELECT * FROM Geopoints.V_Location AS t WHERE t.Id in (
		SELECT Id FROM 
			(
			SELECT 
					ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
			FROM
					LocationTreeWithData AS t2                                                                                 
			) 
			AS sub1 
			WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
	) ORDER BY Name

END
GO



