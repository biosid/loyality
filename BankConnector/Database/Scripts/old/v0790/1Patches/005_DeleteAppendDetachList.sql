DELETE FROM [etl].[EtlVariables] WHERE [EtlPackageId] = N'bacc74b8-f917-4905-b482-4ee90e2c62dd';
DELETE FROM [etl].[EtlMessages] WHERE [EtlPackageId] = N'bacc74b8-f917-4905-b482-4ee90e2c62dd';
DELETE FROM [etl].[EtlSessions] WHERE [EtlPackageId] = N'bacc74b8-f917-4905-b482-4ee90e2c62dd';
DELETE FROM [etl].[EtlPackages] WHERE [Id]=N'bacc74b8-f917-4905-b482-4ee90e2c62dd';
GO