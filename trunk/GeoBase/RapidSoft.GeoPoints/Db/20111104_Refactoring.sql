MergeUtilsDropSPIfExist '[Geopoints].[CreateIPRangesTable]';
GO
CREATE PROCEDURE [Geopoints].[CreateIPRangesTable]
	@TableName NVARCHAR(128)	
AS
BEGIN

	DECLARE @S NVARCHAR(2048)

	SET @S = 'CREATE TABLE ' + @TableName + '(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IPV4From] [bigint] NOT NULL,
	[IPV4To] [bigint] NOT NULL,
	[IPV4FromString] [nvarchar](30) NOT NULL,
	[IPV4ToString] [nvarchar](30) NOT NULL,
	[Company] [nvarchar](255) NULL,
	[LocationId] [uniqueidentifier] NULL,
	'  + [dbo].ShemaUtilsGetStandardColumns(1, 1) +   '
	 CONSTRAINT [PK_' + [dbo].[RuntimeUtilsGetPureTableName](@TableName) + '] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC 
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]';

	EXEC MergeUtilsExecSQL @S;	

END
GO
[MergeUtilsDropSPIfExistAndCreateSynonym] '[Geopoints].[IPRangesCreateTable]', '[Geopoints].[CreateIPRangesTable]';
GO


MergeUtilsDropSPIfExist '[Geopoints].[ApplyIPRangesTableConstraints]';
GO
CREATE PROCEDURE [Geopoints].[ApplyIPRangesTableConstraints]
	@TableName NVARCHAR(128)	
AS
BEGIN
	DECLARE @S NVARCHAR(1024)
 	SET @S = 'CREATE NONCLUSTERED INDEX [IX_IPV4From_IPV4To] ON ' + @TableName + ' 
	(
		[IPV4From] ASC,
		[IPV4To] ASC
	)INCLUDE ( [IPV4FromString],
	[IPV4ToString],
	[Company],
	[LocationId],
	[CreatedDateTime],
	[CreatedUtcDateTime],
	[ModifiedDateTime],
	[ModifiedUtcDateTime],
	[EtlPackageId],
	[EtlSessionId]) ;'
	EXEC MergeUtilsExecSQL @S;		
END
GO
[MergeUtilsDropSPIfExistAndCreateSynonym] '[Geopoints].[IPRangesApplyTableConstraints]', '[Geopoints].[ApplyIPRangesTableConstraints]';
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
[MergeUtilsDropSPIfExistAndCreateSynonym] '[Geopoints].[LocationGetCountries]', '[Geopoints].[GetCountries]';
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
	SELECT TOP (1) * FROM Geopoints.V_Location AS l
		WHERE l.Id = @Id
END
GO
[MergeUtilsDropSPIfExistAndCreateSynonym] '[Geopoints].[LocationGetById]', '[Geopoints].[GetLocationById]';
GO


MergeUtilsDropSPIfExist '[Geopoints].[GetTradePointsByLocation]';
GO
CREATE PROCEDURE [Geopoints].[GetTradePointsByLocation]
(
	@LocationId uniqueidentifier, 
    @locale VARCHAR(2),    
    @skip INT = NULL,
    @top INT = NULL
)
AS
BEGIN
	DECLARE @default_top INT SET @default_top = 1000;

	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);

	SELECT * FROM [Geopoints].V_TradePoint t	
	WHERE t.Name  in (
	
					(SELECT Name FROM (
                            SELECT 
                                    ROW_NUMBER() OVER (ORDER BY t2.Name ASC) AS rownumber,
                                    t2.Name
                            FROM
                                    [Geopoints].V_TradePoint AS t2 
                            INNER JOIN [Geopoints].V_Location vl ON vl.Id=t2.LocationId
                            WHERE vl.LocationType = 2 OR vl.LocationType = 3
                            ) AS sub1 
                            WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
                    )	
	) ORDER BY Name	
END
GO
[MergeUtilsDropSPIfExistAndCreateSynonym] '[Geopoints].[TradePointsGetByLocation]', '[Geopoints].[GetTradePointsByLocation]';
GO


MergeUtilsDropSPIfExist '[Geopoints].[GetLocationsByParent]';
GO
CREATE PROCEDURE [Geopoints].[GetLocationsByParent]
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
[MergeUtilsDropSPIfExistAndCreateSynonym] '[Geopoints].[LocationGetLocationsByParent]', '[Geopoints].[GetLocationsByParent]';
GO


MergeUtilsDropSPIfExist '[Geopoints].[GetLocationsByIP]';
GO
--Ищем только для LocationType 1 (Москва, Питер) и 3,4 ( город и населенный пункт)
--ТАк как в базе содержатся только города
CREATE PROCEDURE [Geopoints].[GetLocationsByIP]
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
		--Если надо вернуть локацию уровнем выше. Например для города вернуть страну
	ELSE IF(@rootType > @locationType)
		BEGIN			
			DECLARE @ResLocationId uniqueIdentifier;			

			IF(@locationType = 0)
				SELECT @ResLocationId = t.CountryId FROM Geopoints.V_Location t WHERE t.Id = @rootLocationId;
			ELSE IF (@locationType = 1)
				SELECT @ResLocationId = t.RegionID FROM Geopoints.V_Location t WHERE t.Id = @rootLocationId;
			ELSE IF (@locationType = 2)
				SELECT @ResLocationId = t.DistrictID FROM Geopoints.V_Location t WHERE t.Id = @rootLocationId;
			ELSE IF (@locationType = 3)
				SELECT @ResLocationId = t.CityID FROM Geopoints.V_Location t WHERE t.Id = @rootLocationId;
			ELSE IF (@locationType = 4)
				SELECT @ResLocationId = t.TownID FROM Geopoints.V_Location t WHERE t.Id = @rootLocationId;

			SELECT * FROM Geopoints.V_Location t 
			WHERE t.Id = @ResLocationId AND (@skip = 0 AND @top > 0);

		END
	ELSE
		BEGIN
			print @rootType
			--Root=Регион, Москва + Питер. Фильтр по RegionID
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
			--Root=Округ. Фильтр по DistrictID
			ELSE IF(@rootType = 2)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,Name FROM Geopoints.V_Location
						WHERE LocationType = @locationType AND DistrictID = @rootLocationId
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
			--Root=Город. Фильтр по CityID
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
			--Root=Поселок. Фильтр по TownID
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
[MergeUtilsDropSPIfExistAndCreateSynonym] '[Geopoints].[IPRangesGetLocationsByIP]', '[Geopoints].[GetLocationsByIP]';
GO
