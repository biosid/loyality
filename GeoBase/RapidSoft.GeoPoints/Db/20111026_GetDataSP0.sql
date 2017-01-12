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

	SET @nameSearchPattern = @nameSearchPattern + '%'

	DECLARE @rootLocationId uniqueIdentifier;
	DECLARE @rootType INT;

	SELECT @rootLocationId = Id, @rootType = LocationType FROM  Geopoints.V_Location 
	WHERE Id = @parentID;

	IF(@rootType = 0)
		BEGIN
			WITH LocationTreeWithData AS 
			(
				SELECT * FROM Geopoints.V_Location
				WHERE (@locationType IS NULL OR geopoints.[V_Location].LocationType = @locationType)
				AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern)
				AND CountryId = @rootLocationId
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
	ELSE IF(@rootType = 1)
		BEGIN
			WITH LocationTreeWithData AS 
			(
				SELECT * FROM Geopoints.V_Location
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
	ELSE IF(@rootType = 2)
		BEGIN
			WITH LocationTreeWithData AS 
			(
				SELECT * FROM Geopoints.V_Location
				WHERE (@locationType IS NULL OR geopoints.[V_Location].LocationType = @locationType)
				AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern)					
				AND DistrictId = @rootLocationId
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
				SELECT * FROM Geopoints.V_Location
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
				SELECT * FROM Geopoints.V_Location
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

--------------------------------------------------------------------------------------------------------------

GO
MergeUtilsDropSPIfExist '[Geopoints].[LocationGetById]';
GO
CREATE PROCEDURE [Geopoints].[LocationGetById]
(	
	@Id uniqueidentifier,
    @locale VARCHAR(2)
)
AS
BEGIN
	SELECT TOP (1) * FROM Geopoints.V_Location AS l
		WHERE l.Id = @Id
END
GO




