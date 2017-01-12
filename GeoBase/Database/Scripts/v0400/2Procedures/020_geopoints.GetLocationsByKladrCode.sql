IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[GetLocationsByKladrCode]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [Geopoints].[GetLocationsByKladrCode]
GO

CREATE PROCEDURE [Geopoints].[GetLocationsByKladrCode]
(
    @kladrCode          NVARCHAR(20) = null,
    @locationType       NVARCHAR(1024) = null,
    @nameSearchPattern  NVARCHAR(1024) = null,
    @locale             NVARCHAR(2) = null,
    @skip               INT = NULL,
    @top                INT = NULL
)
AS
BEGIN

--declare @kladrCode           NVARCHAR(20)
--set @kladrCode = '7200000100000'
--declare     @locationType       INT
----set @locationType = 0
--declare @nameSearchPattern  NVARCHAR(1024)
--declare @locale             NVARCHAR(2)
--declare @skip               INT
--declare @top                INT


DECLARE @default_top INT 
SET @default_top = 1000;
	
SET @skip = ISNULL(@skip, 0);
SET @top = ISNULL(@top, @default_top);
	
DECLARE @ch NVARCHAR
SET @ch = '#'
	
	
DECLARE @nameLEN INT;
SET @nameLEN = LEN(@nameSearchPattern);
	
DECLARE @name NVARCHAR(1024);
SET @name = @nameSearchPattern;
	
SET @nameSearchPattern = [dbo].[WildcardsShield](@nameSearchPattern, @ch);
SET @nameSearchPattern = '%' + @nameSearchPattern + '%'
	
	
DECLARE @nameFT NVARCHAR(1024);
SET @nameFT = '"' + @name + '*"';

	
	
DECLARE @rootLocationId UNIQUEIDENTIFIER;
DECLARE @rootType INT;

IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#LocationTypes')) 
	DROP TABLE #LocationTypes

CREATE TABLE #LocationTypes (
	LocationType INT
)

INSERT INTO #LocationTypes
SELECT * FROM [dbo].[RuntimeUtilsSplitString] (@locationType, ',')

DECLARE @LocationTypesCount INT;
SELECT @LocationTypesCount = COUNT(*)
FROM #LocationTypes
		
if (@KladrCode is null)
begin
	SELECT
		@rootLocationId = Id, 
		@rootType = LocationType
	FROM
		Geopoints.Location_VIEW
	WHERE
		Name = 'Россия' and LocationType = 0;
end
else
begin
	SELECT 
		@rootLocationId = Id,
		@rootType = LocationType
	FROM   
		Geopoints.Location_VIEW
	WHERE  
		KladrCode = @KladrCode;
end	

PRINT @rootLocationId
PRINT @rootType

IF (@rootType = 0)
BEGIN
	--PRINT '@rootType = 0';
	-- NOTE: Для лучшей производительности, если искать только города
	IF(@locationType = '3')
	BEGIN
		--type 0, location 3 
		IF(@name IS NULL)
		BEGIN
		--Поиск без фильтра по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name
				FROM  geopoints.Location_VIEW L						 
				WHERE  isCity = 1
						AND L.CountryId = @rootLocationId
			)
			SELECT t.*
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
			--Поиск с фильтром по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name,
				(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
				FROM  geopoints.Location_VIEW L						 
				WHERE  isCity = 1
						AND  CONTAINS(Name, @nameFT) 
						AND L.CountryId = @rootLocationId
			)
			SELECT t.*,
			(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
										SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
												t2.Id
										FROM   LocationTreeWithData AS t2
									) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY st, NAME
			--конец блока поиска
		END
								
	END
	-- NOTE: Для лучшей производительности, если искать только одного типа
	ELSE IF (@locationType != '3' AND @LocationTypesCount <= 1)
	BEGIN
		--type 0, location <> 3 				
		IF(@name IS NULL)
		BEGIN
		--Поиск без фильтра по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name
				FROM  geopoints.Location_VIEW L						 
				WHERE  (
							@locationType IS NULL
							OR L.LocationType = @LocationType
						)
						AND L.CountryId = @rootLocationId
			)
			SELECT t.*
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
		--Поиск с фильтром по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name,
				(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
				FROM  geopoints.Location_VIEW L						 
				WHERE  (
							@locationType IS NULL
							OR L.LocationType = @LocationType
						)
						AND  CONTAINS(Name, @nameFT) 
						AND L.CountryId = @rootLocationId
			)
			SELECT t.*,
			(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
										SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
												t2.Id
										FROM   LocationTreeWithData AS t2
									) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY st, NAME
			--конец блока поиска
		END
			
	END
	ELSE
	BEGIN
		-- NOTE: Медленно, но для разных типов
		IF(@name IS NULL)
		BEGIN
		
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name
				FROM  geopoints.Location_VIEW L						 
				WHERE	L.CountryId = @rootLocationId
				  AND	(
							(@locationType IS NULL)
							OR
							(isCity = 1 AND 3 IN (SELECT LocationType FROM #LocationTypes))
							OR
							(L.LocationType IN (SELECT LocationType FROM #LocationTypes WHERE LocationType != 3))
						)
			)
			SELECT t.*
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
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name,
				(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
				FROM  geopoints.Location_VIEW L						 
				WHERE  CONTAINS(Name, @nameFT) 
				  AND  L.CountryId = @rootLocationId
				  AND	(
							(@locationType IS NULL)
							OR
							(isCity = 1 AND 3 IN (SELECT LocationType FROM #LocationTypes))
							OR
							(L.LocationType IN (SELECT LocationType FROM #LocationTypes WHERE LocationType != 3))
						)
			)
			SELECT t.*,
			(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
										SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
												t2.Id
										FROM   LocationTreeWithData AS t2
									) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY st, NAME
		END
	
	END

END
ELSE IF (@rootType = 1)
BEGIN
	--PRINT '@rootType = 1';
	-- NOTE: Для лучшей производительности, если искать только города
	IF(@locationType = '3')
	BEGIN

		--type 1, location = 3 	
		IF(@name IS NULL)
		BEGIN
		--Поиск без фильтра по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name
				FROM  geopoints.Location_VIEW L						 
				WHERE  isCity = 1
						AND L.RegionID = @rootLocationId
			)
			SELECT t.*
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
		--Поиск с фильтром по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name,
				(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
				FROM  geopoints.Location_VIEW L						 
				WHERE  isCity = 1
						AND  CONTAINS(Name, @nameFT) 
						AND L.RegionID = @rootLocationId
			)
			SELECT t.*,
			(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
										SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
												t2.Id
										FROM   LocationTreeWithData AS t2
									) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY st, NAME
			--конец блока поиска
		END

	END
	-- NOTE: Для лучшей производительности, если искать только одного типа
	ELSE IF (@locationType != '3' AND @LocationTypesCount <= 1)
	BEGIN
			
		--type 1, location <> 3 			
		IF(@name IS NULL)
		BEGIN
		--Поиск без фильтра по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name
				FROM  geopoints.Location_VIEW L						 
				WHERE  (
							@locationType IS NULL
							OR (LocationType = @LocationType)
						)
						AND L.RegionID = @rootLocationId
			)
			SELECT t.*
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
		--Поиск с фильтром по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name,
				(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
				FROM  geopoints.Location_VIEW L						 
				WHERE  (
							@locationType IS NULL
							OR (LocationType = @LocationType)
						)
						AND  CONTAINS(Name, @nameFT) 
						AND L.RegionID = @rootLocationId
			)
			SELECT t.*,
			(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
										SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
												t2.Id
										FROM   LocationTreeWithData AS t2
									) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY st, NAME
			--конец блока поиска
		END
					
	END
	ELSE 
	BEGIN
		-- NOTE: Медленно, но для разных типов
		IF(@name IS NULL)
		BEGIN
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name
				FROM  geopoints.Location_VIEW L						 
				WHERE	L.RegionID = @rootLocationId
				  AND	(
							(@locationType IS NULL)
							OR
							(isCity = 1 AND 3 IN (SELECT LocationType FROM #LocationTypes))
							OR
							(L.LocationType IN (SELECT LocationType FROM #LocationTypes WHERE LocationType != 3))
						)
			)
			SELECT t.*
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
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name,
				(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
				FROM  geopoints.Location_VIEW L						 
				WHERE	L.RegionID = @rootLocationId
				  AND	CONTAINS(Name, @nameFT)
				  AND  (
							(@locationType IS NULL)
							OR
							(isCity = 1 AND 3 IN (SELECT LocationType FROM #LocationTypes))
							OR
							(L.LocationType IN (SELECT LocationType FROM #LocationTypes WHERE LocationType != 3))
						)
			)
			SELECT t.*,
			(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
										SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
												t2.Id
										FROM   LocationTreeWithData AS t2
									) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY st, NAME
		END
		
	END

END
ELSE IF (@rootType = 2)
BEGIN
	--PRINT '@rootType = 2';
	--NOTE: Для лучшей производительности, если искать только города
	IF(@locationType = '3')
	BEGIN
		--PRINT 'type 2, location = 3'			
		
		IF(@name IS NULL)
		BEGIN
		--Поиск без фильтра по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name
				FROM  geopoints.Location_VIEW L						 
				WHERE  isCity = 1
						AND L.DistrictID = @rootLocationId
			)
			SELECT t.*
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
		--Поиск с фильтром по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name,
				(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
				FROM  geopoints.Location_VIEW L						 
				WHERE  isCity = 1
						AND  CONTAINS(Name, @nameFT) 
						AND L.DistrictID = @rootLocationId
			)
			SELECT t.*,
			(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
										SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
												t2.Id
										FROM   LocationTreeWithData AS t2
									) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY st, NAME
			--конец блока поиска
		END
			
	END
	-- NOTE: Для лучшей производительности, если искать только одного типа
	ELSE IF (@locationType != '3' AND @LocationTypesCount <= 1)
	BEGIN
		
	--PRINT 'type 2, location <> 3'			
	IF(@name IS NULL)
		BEGIN
		--Поиск без фильтра по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name
				FROM  geopoints.Location_VIEW L						 
				WHERE  (
							@locationType IS NULL
							OR (LocationType = @LocationType)
						)
						AND L.DistrictID = @rootLocationId
			)
			SELECT t.*
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
		--Поиск с фильтром по имени
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name,
				(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
				FROM  geopoints.Location_VIEW L						 
				WHERE  (
							@locationType IS NULL
							OR (LocationType = @LocationType)
						)
						AND  CONTAINS(Name, @nameFT) 
						AND L.DistrictID = @rootLocationId
			)
			SELECT t.*,
			(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
										SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
												t2.Id
										FROM   LocationTreeWithData AS t2
									) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY st, NAME
			--конец блока поиска
		END
				
	END
	ELSE 
	BEGIN
		--PRINT 'Медленно, но для разных типов'
		-- NOTE: Медленно, но для разных типов
		IF(@name IS NULL)
		BEGIN
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name
				FROM  geopoints.Location_VIEW L						 
				WHERE	L.DistrictID = @rootLocationId
				  AND	(
							(@locationType IS NULL)
							OR
							(isCity = 1 AND 3 IN (SELECT LocationType FROM #LocationTypes))
							OR
							(L.LocationType IN (SELECT LocationType FROM #LocationTypes WHERE LocationType != 3))
						)
			)
			SELECT t.*
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
			WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name,
				(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
				FROM  geopoints.Location_VIEW L						 
				WHERE	CONTAINS(Name, @nameFT)
				  AND	L.DistrictID = @rootLocationId
				  AND	(
							(@locationType IS NULL)
							OR
							(isCity = 1 AND 3 IN (SELECT LocationType FROM #LocationTypes))
							OR
							(L.LocationType IN (SELECT LocationType FROM #LocationTypes WHERE LocationType != 3))
						)
			)
			SELECT t.*,
			(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
			FROM   Geopoints.Location_VIEW t
			WHERE  t.Id IN (SELECT Id
							FROM   (
										SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
												t2.Id
										FROM   LocationTreeWithData AS t2
									) AS sub1
							WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
			ORDER BY st, NAME
		END
	END
END
ELSE IF (@rootType = 3)
BEGIN
	PRINT '@rootType = 3';
	--type 3, location 						
	IF(@name IS NULL)
	BEGIN
	--Поиск без фильтра по имени
		WITH LocationTreeWithData AS 
		(
			SELECT L.Id, 
			L.Name
			FROM  geopoints.Location_VIEW L						 
			WHERE	L.CityID = @rootLocationId
			  AND	(
						@locationType IS NULL
						OR (L.LocationType IN (SELECT LocationType FROM #LocationTypes))
					)
		)
		SELECT t.*
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
	--Поиск с фильтром по имени
		print @nameFT;
		WITH LocationTreeWithData AS 
		(
			SELECT L.Id, 
			L.Name,
			(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
			FROM  geopoints.Location_VIEW L						 
			WHERE	CONTAINS(Name, @nameFT) 
			  AND	L.CityID = @rootLocationId
			  AND	(
						@locationType IS NULL
						OR (L.LocationType IN (SELECT LocationType FROM #LocationTypes))
					)
		)
		SELECT t.*,
		(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
		FROM   Geopoints.Location_VIEW t
		WHERE  t.Id IN (SELECT Id
						FROM   (
									SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
											t2.Id
									FROM   LocationTreeWithData AS t2
								) AS sub1
						WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
		ORDER BY st, NAME
		--конец блока поиска
	END
				
END
ELSE IF (@rootType = 4)
BEGIN
	PRINT '@rootType = 4';
	--type 4, location 	
	IF(@name IS NULL)
	BEGIN
	--Поиск без фильтра по имени
		WITH LocationTreeWithData AS 
		(
			SELECT L.Id, 
			L.Name
			FROM  geopoints.Location_VIEW L						 
			WHERE	L.TownID = @rootLocationId
			  AND	(
						@locationType IS NULL
						OR (L.LocationType IN (SELECT LocationType FROM #LocationTypes))
					)
		)
		SELECT t.*
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
	--Поиск с фильтром по имени
		WITH LocationTreeWithData AS 
		(
			SELECT L.Id, 
			L.Name,
			(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
			FROM  geopoints.Location_VIEW L						 
			WHERE	CONTAINS(Name, @nameFT)
			  AND	L.TownID = @rootLocationId
			  AND	(
						@locationType IS NULL
						OR (L.LocationType IN (SELECT LocationType FROM #LocationTypes))
					)
		)
		SELECT t.*,
		(CASE WHEN LEFT(t.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
		FROM   Geopoints.Location_VIEW t
		WHERE  t.Id IN (SELECT Id
						FROM   (
									SELECT ROW_NUMBER() OVER(ORDER BY st ASC, [Name] ASC) AS rownumber,
											t2.Id
									FROM   LocationTreeWithData AS t2
								) AS sub1
						WHERE  sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top)
		ORDER BY st, NAME
		--конец блока поиска
	END
 
END
ELSE
BEGIN
	PRINT '@rootType != 1,2,3,4';
	WITH LocationHierachy(Id, ParentID, LEVEL) AS 
	(
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
		WHERE	(
					@locationType IS NULL
					OR (L.LocationType IN (SELECT LocationType FROM #LocationTypes))
				)
		  AND	(
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


