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

	DECLARE @ch NVARCHAR
	SET @ch = '#'
	
	SET @nameSearchPattern = [dbo].[WildcardsShield](@nameSearchPattern, @ch);

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
				AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern ESCAPE @ch)
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
				AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern ESCAPE @ch)
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
				AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern ESCAPE @ch)
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
				AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern ESCAPE @ch)
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
				AND (@nameSearchPattern IS NULL OR geopoints.[V_Location].Name LIKE @nameSearchPattern ESCAPE @ch)
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
	ELSE
		BEGIN
			with LocationHierachy(Id, ParentID, Level) as ( 
					select L.Id, L.ParentId, 0 as Level 
					from Geopoints.V_Location L     
					where L.Id = @rootLocationId 
				union all     
					select L.Id, L.ParentId,Lh.Level + 1     
					from Geopoints.V_Location L
					inner join LocationHierachy Lh   
					on L.ParentId = Lh.Id 
			)
			, LocationTreeWithData AS 
			(
				SELECT L.* FROM Geopoints.V_Location L
				INNER JOIN LocationHierachy LH ON L.Id = Lh.Id
				WHERE (@locationType IS NULL OR L.LocationType = @locationType)
				AND (@nameSearchPattern IS NULL OR L.Name LIKE @nameSearchPattern ESCAPE @ch)						
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