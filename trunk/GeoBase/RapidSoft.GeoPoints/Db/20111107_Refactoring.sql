sp_rename '[Geopoints].[V_Location]', 'Location_VIEW';
GO
sp_rename '[Geopoints].[V_LocationGeoInfo]', 'LocationGeoInfo_VIEW';
GO
sp_rename '[Geopoints].[V_LocationLocalization]', 'LocationLocalization_VIEW';
GO
sp_rename '[Geopoints].[V_LocationType]', 'LocationType_VIEW';
GO
sp_rename '[Geopoints].[V_TradePoint]', 'TradePoint_VIEW';
GO
sp_rename '[Geopoints].[V_TradePointType]', 'TradePointType_VIEW';
GO

MergeUtilsDropSPIfExist '[Geopoints].[GetCountries]';
GO
CREATE PROCEDURE [Geopoints].[GetCountries]
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
		SELECT Id,Name FROM Geopoints.Location_VIEW
		WHERE geopoints.[Location_VIEW].LocationType = 0			
	)
	SELECT * FROM Geopoints.Location_VIEW AS t WHERE t.Id in (
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


MergeUtilsDropSPIfExist '[Geopoints].[GetLocationById]';
GO
CREATE PROCEDURE [Geopoints].[GetLocationById]
(	
	@Id uniqueidentifier,
    @locale VARCHAR(2)
)
AS
BEGIN
	SELECT TOP(1) * 
	FROM   Geopoints.Location_VIEW AS l
	WHERE  l.Id = @Id
END
GO


MergeUtilsDropSPIfExist '[Geopoints].[GetTradePointsByLocation]';
GO
CREATE PROCEDURE [Geopoints].[GetTradePointsByLocation]
(
    @LocationId  UNIQUEIDENTIFIER,
    @locale      VARCHAR(2),
    @skip        INT = NULL,
    @top         INT = NULL
)
AS
BEGIN
	DECLARE @default_top INT 
	SET @default_top = 1000;
	
	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);
	
	SELECT *
	FROM   [Geopoints].TradePoint_VIEW t
	WHERE  t.Name  IN ((
	                      SELECT NAME
	                      FROM   (
	                                 SELECT ROW_NUMBER() OVER(ORDER BY t2.Name ASC) AS rownumber,
	                                        t2.Name
	                                 FROM   [Geopoints].TradePoint_VIEW AS t2
	                                        INNER JOIN [Geopoints].Location_VIEW vl ON vl.Id = t2.LocationId
	                                 WHERE  vl.LocationType = 2 OR vl.LocationType = 3
	                             ) AS sub1
	                      WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top
	                  ))
	ORDER BY NAME
END
GO


MergeUtilsDropSPIfExist '[Geopoints].[GetLocationsByParent]';
GO
CREATE PROCEDURE [Geopoints].[GetLocationsByParent]
(
    @parentID           UNIQUEIDENTIFIER,
    @locationType       INT,
    @nameSearchPattern  NVARCHAR(1024),
    @locale             VARCHAR(2),
    @skip               INT = NULL,
    @top                INT = NULL
)
AS
BEGIN
	DECLARE @default_top INT 
	SET @default_top = 1000;
	
	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);
	
	DECLARE @ch NVARCHAR
	SET @ch = '#'
	
	SET @nameSearchPattern = [dbo].[WildcardsShield](@nameSearchPattern, @ch);
	
	SET @nameSearchPattern = @nameSearchPattern + '%'
	
	DECLARE @rootLocationId UNIQUEIDENTIFIER;
	DECLARE @rootType INT;
	
	SELECT @rootLocationId = Id,
	       @rootType = LocationType
	FROM   Geopoints.Location_VIEW
	WHERE  Id = @parentID;
	
	IF (@rootType = 0)
	BEGIN
	    WITH LocationTreeWithData AS 
	    (
	        SELECT *
	        FROM   Geopoints.Location_VIEW
	        WHERE  (
	                   @locationType IS NULL
	                   OR geopoints.[Location_VIEW].LocationType = @locationType
	               )
	               AND (
	                       @nameSearchPattern IS NULL
	                       OR geopoints.[Location_VIEW].Name LIKE @nameSearchPattern ESCAPE @ch
	                   )
	               AND CountryId = @rootLocationId
	    )
	    SELECT *
	    FROM   Geopoints.Location_VIEW t
	    WHERE  t.Id IN (SELECT Id
	                    FROM   (
	                               SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
	                                      t2.Id
	                               FROM   LocationTreeWithData AS t2
	                           ) AS sub1
	                    WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
	    ORDER BY NAME
	END
	ELSE 
	IF (@rootType = 1)
	BEGIN
	    WITH LocationTreeWithData AS 
	    (
	        SELECT *
	        FROM   Geopoints.Location_VIEW
	        WHERE  (
	                   @locationType IS NULL
	                   OR geopoints.[Location_VIEW].LocationType = @locationType
	               )
	               AND (
	                       @nameSearchPattern IS NULL
	                       OR geopoints.[Location_VIEW].Name LIKE @nameSearchPattern ESCAPE @ch
	                   )
	               AND RegionID = @rootLocationId
	    )
	    SELECT *
	    FROM   Geopoints.Location_VIEW t
	    WHERE  t.Id IN (SELECT Id
	                    FROM   (
	                               SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
	                                      t2.Id
	                               FROM   LocationTreeWithData AS t2
	                           ) AS sub1
	                    WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
	    ORDER BY NAME
	END
	ELSE 
	IF (@rootType = 2)
	BEGIN
	    WITH LocationTreeWithData AS 
	    (
	        SELECT *
	        FROM   Geopoints.Location_VIEW
	        WHERE  (
	                   @locationType IS NULL
	                   OR geopoints.[Location_VIEW].LocationType = @locationType
	               )
	               AND (
	                       @nameSearchPattern IS NULL
	                       OR geopoints.[Location_VIEW].Name LIKE @nameSearchPattern ESCAPE @ch
	                   )
	               AND DistrictId = @rootLocationId
	    )
	    SELECT *
	    FROM   Geopoints.Location_VIEW t
	    WHERE  t.Id IN (SELECT Id
	                    FROM   (
	                               SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
	                                      t2.Id
	                               FROM   LocationTreeWithData AS t2
	                           ) AS sub1
	                    WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
	    ORDER BY NAME
	END
	ELSE 
	IF (@rootType = 3)
	BEGIN
	    WITH LocationTreeWithData AS 
	    (
	        SELECT *
	        FROM   Geopoints.Location_VIEW
	        WHERE  (
	                   @locationType IS NULL
	                   OR geopoints.[Location_VIEW].LocationType = @locationType
	               )
	               AND (
	                       @nameSearchPattern IS NULL
	                       OR geopoints.[Location_VIEW].Name LIKE @nameSearchPattern ESCAPE @ch
	                   )
	               AND CityID = @rootLocationId
	    )
	    SELECT *
	    FROM   Geopoints.Location_VIEW t
	    WHERE  t.Id IN (SELECT Id
	                    FROM   (
	                               SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
	                                      t2.Id
	                               FROM   LocationTreeWithData AS t2
	                           ) AS sub1
	                    WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
	    ORDER BY NAME
	END
	ELSE 
	IF (@rootType = 4)
	BEGIN
	    WITH LocationTreeWithData AS 
	    (
	        SELECT *
	        FROM   Geopoints.Location_VIEW
	        WHERE  (
	                   @locationType IS NULL
	                   OR geopoints.[Location_VIEW].LocationType = @locationType
	               )
	               AND (
	                       @nameSearchPattern IS NULL
	                       OR geopoints.[Location_VIEW].Name LIKE @nameSearchPattern ESCAPE @ch
	                   )
	               AND TownID = @rootLocationId
	    )
	    SELECT *
	    FROM   Geopoints.Location_VIEW t
	    WHERE  t.Id IN (SELECT Id
	                    FROM   (
	                               SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
	                                      t2.Id
	                               FROM   LocationTreeWithData AS t2
	                           ) AS sub1
	                    WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
	    ORDER BY NAME
	END
	ELSE
	BEGIN
	    WITH LocationHierachy(Id, ParentID, LEVEL) AS (
	        SELECT L.Id,
	               L.ParentId,
	               0 AS LEVEL
	        FROM   Geopoints.Location_VIEW L
	        WHERE  L.Id = @rootLocationId 
	        UNION ALL     
	        SELECT L.Id,
	               L.ParentId,
	               Lh.Level + 1
	        FROM   Geopoints.Location_VIEW L
	               INNER JOIN LocationHierachy Lh ON L.ParentId = Lh.Id
	    )
	    , LocationTreeWithData AS 
	    (
	        SELECT L.* 
	        FROM   Geopoints.Location_VIEW L
	               INNER JOIN LocationHierachy LH ON L.Id = Lh.Id
	        WHERE  (@locationType IS NULL OR L.LocationType = @locationType)
	               AND (
	                       @nameSearchPattern IS NULL
	                       OR L.Name LIKE @nameSearchPattern ESCAPE @ch
	                   )
	    ) 
	    
	    SELECT *
	    FROM   Geopoints.Location_VIEW t
	    WHERE  t.Id IN (SELECT Id
	                    FROM   (
	                               SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
	                                      t2.Id
	                               FROM   LocationTreeWithData AS t2
	                           ) AS sub1
	                    WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
	    ORDER BY NAME
	END
END
GO


MergeUtilsDropSPIfExist '[Geopoints].[GetLocationsByIP]';
GO
--Ищем только для LocationType 1 (Москва, Питер) и 3,4 ( город и населенный пункт)
--ТАк как в базе содержатся только города
CREATE PROCEDURE [Geopoints].[GetLocationsByIP]
(
    @ipINT         BIGINT,
    @locationType  INT,
    @locale        VARCHAR(2),
    @skip          INT = NULL,
    @top           INT = NULL
)
AS
BEGIN
	DECLARE @S NVARCHAR(2048)
	
	DECLARE @rootLocationId UNIQUEIDENTIFIER;
	DECLARE @rootType INT;
	
	SELECT @rootLocationId = r.LocationId,
	       @rootType = L.LocationType
	FROM   Geopoints.IPRanges_VIEW r
	       INNER JOIN Geopoints.Location_VIEW L
	            ON  L.Id = r.LocationId
	WHERE  IPV4From <= @ipINT
	       AND @ipINT <= IPV4To;
	
	DECLARE @default_top INT 
	SET @default_top = 1000;
	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);
	
	
	IF (@rootType = @locationType)
	BEGIN
	    -- Возвращаем рутовую локацию. Если skip > 0 или Top == 0 то возвращаем ничего
	    SELECT *
	    FROM   Geopoints.Location_VIEW
	    WHERE  Id = @rootLocationId
	           AND (@skip = 0 AND @top > 0);
	END
	ELSE IF (@rootType > @locationType)--Если надо вернуть локацию уровнем выше. Например для города вернуть страну
	BEGIN
	    DECLARE @ResLocationId UNIQUEIDENTIFIER;			
	    
	    IF (@locationType = 0)
	        SELECT @ResLocationId = t.CountryId
	        FROM   Geopoints.Location_VIEW t
	        WHERE  t.Id = @rootLocationId;
		ELSE IF (@locationType = 1)
			SELECT @ResLocationId = t.RegionID
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id = @rootLocationId;
		ELSE IF (@locationType = 2)
			SELECT @ResLocationId = t.DistrictID
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id = @rootLocationId;
		ELSE IF (@locationType = 3)
			SELECT @ResLocationId = t.CityID
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id = @rootLocationId;
		ELSE IF (@locationType = 4)
			SELECT @ResLocationId = t.TownID
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id = @rootLocationId;
		
		SELECT *
		FROM   Geopoints.Location_VIEW t
		WHERE  t.Id = @ResLocationId
			   AND (@skip = 0 AND @top > 0);
	END
	ELSE
	BEGIN
		PRINT @rootType
		IF (@rootType = 1)--Root=Регион, Москва + Питер. Фильтр по RegionID
		BEGIN
			WITH intLocationTreeWithData AS 
			(
				SELECT Id,
					   NAME
				FROM   Geopoints.Location_VIEW
				WHERE  LocationType = @locationType
					   AND RegionID = @rootLocationId
			)
			SELECT *
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
									   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
											  t2.Id
									   FROM   intLocationTreeWithData AS t2
								   ) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY NAME
		END
		ELSE 
		IF (@rootType = 2)--Root=Округ. Фильтр по DistrictID
		BEGIN
			WITH intLocationTreeWithData AS 
			(
				SELECT Id,
					   NAME
				FROM   Geopoints.Location_VIEW
				WHERE  LocationType = @locationType
					   AND DistrictID = @rootLocationId
			)
			SELECT *
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
									   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
											  t2.Id
									   FROM   intLocationTreeWithData AS t2
								   ) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY NAME
		END
		ELSE 
		IF (@rootType = 3)--Root=Город. Фильтр по CityID
		BEGIN
			WITH intLocationTreeWithData AS 
			(
				SELECT Id,
					   NAME
				FROM   Geopoints.Location_VIEW
				WHERE  LocationType = @locationType
					   AND CityID = @rootLocationId
			)
			SELECT *
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
									   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
											  t2.Id
									   FROM   intLocationTreeWithData AS t2
								   ) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY NAME
		END
		ELSE 
		IF (@rootType = 4)--Root=Поселок. Фильтр по TownID
		BEGIN
			WITH intLocationTreeWithData AS 
			(
				SELECT Id,
					   NAME
				FROM   Geopoints.Location_VIEW
				WHERE  LocationType = @locationType
					   AND TownID = @rootLocationId
			)
			SELECT *
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
									   SELECT ROW_NUMBER() OVER(ORDER BY [Name] ASC) AS rownumber,
											  t2.Id
									   FROM   intLocationTreeWithData AS t2
								   ) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY NAME
		END
	END
END
GO
