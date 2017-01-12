IF EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'InsertedUserId' AND Object_ID = Object_ID(N'[prod].[Orders]'))    
BEGIN
	PRINT '������� ������� InsertedUserId �� ������� [prod].[Orders], ��� ��� ����� ������ ��������� ��������'
    ALTER TABLE [prod].[Orders]
    DROP COLUMN [InsertedUserId]
END
GO

IF EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'InsertedUserId' AND Object_ID = Object_ID(N'[prod].[OrdersHistory]'))    
BEGIN
	PRINT '������� ������� InsertedUserId �� ������� [prod].[OrdersHistory], ��� ��� ����� ������ ��������� ��������'
    ALTER TABLE [prod].[OrdersHistory]
    DROP COLUMN [InsertedUserId]
END
GO