ALTER PROCEDURE [Geopoints].[ProcessingKladr]
@EtlPackageId uniqueidentifier,
@EtlSessionId uniqueidentifier
AS
BEGIN
	-- Перенос новых данных КЛАДР в таблицу [BufferCityLocation], [BufferStreetLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[KLADR]->[BufferCityLocation]');
	INSERT INTO [Geopoints].[BufferCityLocation]
			   ([LocationType],  [Name], [KladrCode], 
			   [Toponym], [RegionCode],
			   [DistrictCode], [CityCode], [TownCode],
			   [CreatedDateTime], [CreatedUtcDateTime])
	SELECT 
	[LEVEL] as LocationType,
	[NAME] as Name,
	[CODE] as KladrCode,
	[SOCR] as [Toponym],
	(CASE WHEN [LEVEL] > 1 THEN [Region] ELSE NULL END) as [RegionCode],
	(CASE WHEN [LEVEL] > 2 AND SUBSTRING([CODE], 3, 3) <> '000' THEN [District] ELSE NULL END) as [DistrictCode],
	(CASE WHEN [LEVEL] > 3 AND SUBSTRING([CODE], 6, 3) <> '000' THEN [City] ELSE NULL END) as [CityCode],
	NULL as [TownCode],
	GETDATE(), GETUTCDATE()
	FROM
		(SELECT [NAME], [SOCR], [CODE],
		(case 
			when SUBSTRING([CODE], 3, 9) = '000000000' then 1
			when SUBSTRING([CODE], 6, 6) = '000000' then 2
			when SUBSTRING([CODE], 9, 3) = '000' then 3
			else 4 END
		) as [LEVEL],
		SUBSTRING([CODE], 1, 2) + '00000000000' as Region,
		SUBSTRING([CODE], 1, 5) + '00000000' as District,
		SUBSTRING([CODE], 1, 8) + '00000' as City
		FROM Geopoints.KLADR) as tk
		
		
		
			
		
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[KLADR]->[Geopoints].[BufferCityLocation]');
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[STREET]->[Geopoints].[BufferStreetLocation]');
	INSERT INTO [Geopoints].[BufferStreetLocation]
			   ([LocationType],  [Name], [KladrCode], 
			   [Toponym], [ParentCode],
			   [CreatedDateTime], [CreatedUtcDateTime])
	SELECT
	5 as LocationType,
	[NAME] as Name,
	[CODE] as KladrCode,
	[SOCR] as [Toponym],
	SUBSTRING([CODE], 1, 11) + '00' as ParentCode,
	GETDATE(), GETUTCDATE()
	FROM Geopoints.STREET
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[STREET]->[Geopoints].[BufferStreetLocation]');

	-- Перенос текущей версии в таблицу Geopoints.BufferDestinationLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[V_Location]->[Geopoints].[BufferDestinationLocation]');
	INSERT INTO [Geopoints].[BufferDestinationLocation]
			   ([Id], [ParentId], [LocationType], [Name]
			   ,[Toponym], [KladrCode]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym], [TownName], [TownId], [TownToponym]
			   ,[EtlPackageId], [EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime], [ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT [Id], [ParentId], [LocationType], [Name]
	   ,[Toponym], [KladrCode]
	   ,[RegionName], [RegionId], [RegionToponym]
	   ,[DistrictName], [DistrictId], [DistrictToponym]
	   ,[CityName], [CityId], [CityToponym], [TownName], [TownId], [TownToponym],
	   [EtlPackageId], [EtlSessionId], [CreatedDateTime], [CreatedUtcDateTime], 
	   [ModifiedDateTime], [ModifiedUtcDateTime]
	FROM Geopoints.V_Location
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[V_Location]->[Geopoints].[BufferDestinationLocation]');

	-- Актуализация кодов в текущей версии КЛАДР Geopoints.BufferDestinationLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[ALTNAMES]->[Geopoints].[BufferDestinationLocation]');
	UPDATE bdl
	SET bdl.KladrCode = an.NEWCODE
	FROM [Geopoints].[BufferDestinationLocation] bdl
	JOIN Geopoints.ALTNAMES an ON bdl.KladrCode = an.OLDCODE
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[ALTNAMES]->[Geopoints].[BufferDestinationLocation]');

	-- Актуализация Id, CreatedDateTime, CreatedUtcDateTime

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[BufferDestinationLocation]->[Geopoints].[BufferCityLocation]');
	UPDATE sl
	SET
		sl.Id = dl.Id,
		sl.CreatedDateTime = dl.CreatedDateTime,
		sl.CreatedUtcDateTime = sl.CreatedUtcDateTime
	FROM [Geopoints].[BufferCityLocation] sl
	JOIN [Geopoints].[BufferDestinationLocation] dl ON sl.KladrCode = dl.KladrCode
	WHERE dl.EtlPackageId = @EtlPackageId
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[BufferDestinationLocation]->[Geopoints].[BufferCityLocation]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[BufferDestinationLocation]->[Geopoints].[BufferStreetLocation]');
	UPDATE sl
	SET
		sl.Id = dl.Id,
		sl.CreatedDateTime = dl.CreatedDateTime,
		sl.CreatedUtcDateTime = sl.CreatedUtcDateTime
	FROM [Geopoints].[BufferStreetLocation] sl
	JOIN [Geopoints].[BufferDestinationLocation] dl ON sl.KladrCode = dl.KladrCode
	WHERE dl.EtlPackageId = @EtlPackageId
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[BufferDestinationLocation]->[Geopoints].[BufferStreetLocation]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferCityLocation]. SET NEW ID');
	UPDATE Geopoints.BufferCityLocation
	SET Id = NewId()
	WHERE Id IS NULL
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferCityLocation]. SET NEW ID');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferStreetLocation]. SET NEW ID');
	UPDATE Geopoints.BufferStreetLocation
	SET Id = NewId()
	WHERE Id IS NULL
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferStreetLocation]. SET NEW ID');

	-- UPDATE Region, District, City in Geopoints.BufferCityLocation

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferCityLocation]. UPDATE REGION');
	UPDATE bl
	SET 
		bl.RegionName = blr.Name,
		bl.RegionId = blr.Id,
		bl.RegionToponym = blr.Toponym,
		bl.ParentId = blr.Id
	FROM Geopoints.BufferCityLocation bl
	JOIN Geopoints.BufferCityLocation blr ON bl.RegionCode = blr.KladrCode;
	
	
	UPDATE bl
	SET 
		bl.RegionName = bl.Name,
		bl.RegionToponym = bl.Toponym,
		bl.RegionId = bl.Id,
		bl.RegionCode = bl.KladrCode
	FROM Geopoints.BufferCityLocation bl
	WHERE bl.LocationType = 1;
	
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferCityLocation]. UPDATE REGION');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferCityLocation]. UPDATE DISTRICT');
	UPDATE bl
	SET 
		bl.DistrictName = blr.Name,
		bl.DistrictId = blr.Id,
		bl.DistrictToponym = blr.Toponym,
		bl.ParentId = blr.Id
	FROM Geopoints.BufferCityLocation bl
	JOIN Geopoints.BufferCityLocation blr ON bl.DistrictCode = blr.KladrCode;
	
	UPDATE bl
	SET 
		bl.DistrictName = bl.Name,
		bl.DistrictToponym = bl.Toponym,
		bl.DistrictId = bl.Id,
		bl.DistrictCode = bl.KladrCode
	FROM Geopoints.BufferCityLocation bl
	WHERE bl.LocationType = 2;
	
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferCityLocation]. UPDATE DISTRICT');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferCityLocation]. UPDATE CITY');
	UPDATE bl
	SET 
		bl.CityName = blr.Name,
		bl.CityId = blr.Id,
		bl.CityToponym = blr.Toponym,
		bl.ParentId = blr.Id
	FROM Geopoints.BufferCityLocation bl
	JOIN Geopoints.BufferCityLocation blr ON bl.CityCode = blr.KladrCode;
	
	
	UPDATE bl
	SET 
		bl.CityName = bl.Name,
		bl.CityToponym = bl.Toponym,
		bl.CityId = bl.Id,
		bl.CityCode = bl.KladrCode
	FROM Geopoints.BufferCityLocation bl
	WHERE bl.LocationType = 3;
	
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferCityLocation]. UPDATE CITY');



	UPDATE bl
	SET 
		bl.TownName = bl.Name,
		bl.TownToponym = bl.Toponym,
		bl.TownId = bl.Id,
		bl.TownCode = bl.KladrCode
	FROM Geopoints.BufferCityLocation bl
	WHERE bl.LocationType = 4;
	
	
	-- UPDATE Region, District, City, Town in Geopoints.BufferCityLocation
	
	

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferStreetLocation]. UPDATE REGION, DISTRICT, CITY, TOWN');
	UPDATE bl
	SET 
		bl.RegionName = (CASE blr.LocationType WHEN 1 THEN blr.Name ELSE blr.RegionName END),
		bl.RegionId = (CASE blr.LocationType WHEN 1 THEN blr.Id ELSE blr.RegionId END),
		bl.RegionToponym = (CASE blr.LocationType WHEN 1 THEN blr.Toponym ELSE blr.RegionToponym END),
		bl.DistrictName = (CASE blr.LocationType WHEN 2 THEN blr.Name ELSE blr.DistrictName END),
		bl.DistrictId = (CASE blr.LocationType WHEN 2 THEN blr.Id ELSE blr.DistrictId END),
		bl.DistrictToponym = (CASE blr.LocationType WHEN 2 THEN blr.Toponym ELSE blr.DistrictToponym END),
		bl.CityName = (CASE blr.LocationType WHEN 3 THEN blr.Name ELSE blr.CityName END),
		bl.CityId = (CASE blr.LocationType WHEN 3 THEN blr.Id ELSE blr.CityId END),
		bl.CityToponym = (CASE blr.LocationType WHEN 3 THEN blr.Toponym ELSE blr.CityToponym END),
		bl.TownName = (CASE blr.LocationType WHEN 4 THEN blr.Name ELSE blr.TownName END),
		bl.TownId = (CASE blr.LocationType WHEN 4 THEN blr.Id ELSE blr.TownId END),
		bl.TownToponym = (CASE blr.LocationType WHEN 4 THEN blr.Toponym ELSE blr.TownToponym END),
		bl.ParentId = blr.Id
	FROM Geopoints.BufferStreetLocation bl
	JOIN Geopoints.BufferCityLocation blr ON blr.KladrCode = bl.ParentCode;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferStreetLocation]. UPDATE REGION, DISTRICT, CITY, TOWN');

END;