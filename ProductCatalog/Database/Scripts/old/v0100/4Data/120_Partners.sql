DECLARE @User nvarchar(50) = 'SCRIPT'

SET IDENTITY_INSERT [prod].[Partners] ON

PRINT '������ online �������� "��������"'
INSERT INTO [prod].[Partners]
	([Id],[Description],
	 [InsertedUserId],[UpdatedUserId],
	 [Name],[Type],[Status],[ThrustLevel],
	 [InsertedDate],[UpdatedDate],
	 [FixPriceSupported],[IsCarrier])
	VALUES
	(5,'��������',
	 @User, NULL,
	 '��������',0 /* Online */,1,0,
	 GETDATE(),NULL,
	 NULL,0)

PRINT '������ ������� ����'
INSERT INTO [prod].[Partners]
	([Id],[Description],
	 [InsertedUserId],[UpdatedUserId],
	 [Name],[Type],[Status],[ThrustLevel],
	 [InsertedDate],[UpdatedDate],
	 [FixPriceSupported],[IsCarrier],[ImportDeliveryRatesEtlPackageId])
VALUES
	(6,'������ ����',
	 @User, NULL,
	 '������ ����', 2 /* Offline */,1,0,
	 GETDATE(),NULL,
	 NULL,1,'777ff1a8-3fbf-4127-96d3-70a0fa7fd05c')

PRINT '������ �������� ����'
INSERT INTO [prod].[Partners]
	([Id],[Description],
	 [InsertedUserId],[UpdatedUserId],
	 [Name],[Type],[Status],[ThrustLevel],
	 [InsertedDate],[UpdatedDate],
	 [FixPriceSupported],[IsCarrier],[CarrierId])
 VALUES
	(1,'����',
	 @User, NULL,
	 '����', 2 /* Offline */,1,0,
	 GETDATE(),NULL,
	 1,0, 6 /* ������ ���� */)

SET IDENTITY_INSERT [prod].[Partners] OFF


