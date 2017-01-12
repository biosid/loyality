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

	SELECT	@parentId = [Id]
	  FROM	Geopoints.Location_VIEW lv
	WHERE	[KladrCode] = @parentKladrCode

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

	IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#Toponyms')) 
		DROP TABLE #Toponyms

	CREATE TABLE #Toponyms (
		Toponym NVARCHAR(10) NULL
	)

	INSERT INTO #Toponyms
	SELECT * FROM [dbo].[RuntimeUtilsSplitString] (@toponyms, ',')

	DECLARE @ToponymsCount INT;
	SELECT @ToponymsCount = COUNT(*)
	FROM #Toponyms

	IF(@nameSearchPattern IS NULL)
		BEGIN
			--Поиск без фильтра по имени
			;WITH LocationTreeWithData AS 
			(
				SELECT L.Id, L.Name
				FROM  geopoints.Location_VIEW L						 
				WHERE (@parentId IS NULL OR L.ParentId = @parentId) 
				  AND (@LocationTypesCount = 0 OR L.LocationType IN (SELECT LocationType FROM #LocationTypes))
				  AND (@ToponymsCount = 0 OR L.Toponym IN (SELECT Toponym FROM #Toponyms))
			)
			SELECT t.*
			FROM   geopoints.Location_VIEW t
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
			DECLARE @nameLEN INT = LEN(@nameSearchPattern)
			
			DECLARE @name NVARCHAR(1024)
			SET @name = @nameSearchPattern
				
			DECLARE @nameFT NVARCHAR(1028)
			SET @nameFT = '"' + @nameSearchPattern + '*"'

			;WITH LocationTreeWithData AS 
			(
				SELECT L.Id, 
				L.Name,
				(CASE WHEN LEFT(L.[Name], @nameLEN) = @name THEN 0 ELSE 1 END) st
				FROM  geopoints.Location_VIEW L						 
				WHERE (@parentId IS NULL OR L.ParentId = @parentId) 
				  AND (@LocationTypesCount = 0 OR L.LocationType IN (SELECT LocationType FROM #LocationTypes))
				  AND (@ToponymsCount = 0 OR L.Toponym IN (SELECT Toponym FROM #Toponyms))
				  AND  CONTAINS(L.Name, @nameFT) 
			)
			SELECT	t.*,
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
GO
