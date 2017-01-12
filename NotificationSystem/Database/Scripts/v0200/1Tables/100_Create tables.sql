/****** Drop all tables ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[mess].[ClientProfiles]') AND type in (N'U'))
	DROP TABLE [mess].[ClientProfiles]
GO
