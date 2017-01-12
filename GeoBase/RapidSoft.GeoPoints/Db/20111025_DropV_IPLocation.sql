	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Geopoints].V_IPLocation') AND type in (N'V'))
	BEGIN
		DROP VIEW [Geopoints].V_IPLocation
	END
	ELSE
	BEGIN
		print('Представление V_IPLocation уже удалено.')
	END
	IF ([dbo].MergeUtilsCheckTableExist('[geopoints].IPLocation')=1)
	BEGIN
		DROP TABLE [geopoints].IPLocation;
	END	
	ELSE
	BEGIN
		print('Таблица IPLocation уже удалена.')
	END
