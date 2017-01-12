DECLARE @User nvarchar(50) = 'SCRIPT';
DECLARE @CatId int;
DECLARE @OzonId int = 1;

PRINT 'LEVEL 1'
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	ParentId IS NULL
  AND	[Name] = '��� � �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, NULL, N'��� � �����', N'/��� � �����/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	ParentId IS NULL
  AND	[Name] = '�������, ��������, ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, NULL, N'�������, ��������, ������', N'/�������, ��������, ������/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	ParentId IS NULL
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, NULL, N'�������', N'/�������/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	ParentId IS NULL
  AND	[Name] = '������� � �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, NULL, N'������� � �����', N'/������� � �����/', 1, @User, GETDATE(), 0);
END

PRINT 'LEVEL 2'
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'����������', N'/��� � �����/����������/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/%'
  AND	[Name] = '���������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 577, N'���������� � ��������', N'/�������/���������� � ��������/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/%'
  AND	[Name] = '������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 578, N'������� �������', N'/������� � �����/������� �������/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/%'
  AND	[Name] = '�������, ��������� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 578, N'�������, ��������� ��������', N'/������� � �����/�������, ��������� ��������/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 577, N'�����', N'/�������/�����/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/%'
  AND	[Name] = '������ � ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 577, N'������ � ����', N'/�������/������ � ����/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/%'
  AND	[Name] = '���� � ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 578, N'���� � ����', N'/������� � �����/���� � ����/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/%'
  AND	[Name] = '������� ��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 576, N'������� ��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/%'
  AND	[Name] = '������ ��� ��������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'������ ��� ��������������', N'/��� � �����/������ ��� ��������������/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/%'
  AND	[Name] = '������ ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'������ ��� ����', N'/��� � �����/������ ��� ����/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/%'
  AND	[Name] = '������ ��� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'������ ��� �����', N'/��� � �����/������ ��� �����/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/%'
  AND	[Name] = '������ ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'������ ��� ����', N'/��� � �����/������ ��� ����/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/%'
  AND	[Name] = '������ ��� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'������ ��� ��������', N'/��� � �����/������ ��� ��������/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/%'
  AND	[Name] = '������ ��� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'������ ��� ������', N'/��� � �����/������ ��� ������/', 1, @User, GETDATE(), 0);
END

PRINT 'LEVEL 3'
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = 'GPS ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 587, N'GPS ����������', N'/��� � �����/������ ��� ��������������/GPS ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = 'GPS ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 601, N'GPS ����������', N'/��� � �����/������ ��� ��������������/GPS ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = 'GPS ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 615, N'GPS ����������', N'/��� � �����/������ ��� ��������������/GPS ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = 'GPS ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 629, N'GPS ����������', N'/��� � �����/������ ��� ��������������/GPS ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = 'GPS ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 643, N'GPS ����������', N'/��� � �����/������ ��� ��������������/GPS ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = 'GPS ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 657, N'GPS ����������', N'/��� � �����/������ ��� ��������������/GPS ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = 'GPS ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 671, N'GPS ����������', N'/��� � �����/������ ��� ��������������/GPS ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = 'GPS ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 685, N'GPS ����������', N'/��� � �����/������ ��� ��������������/GPS ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = 'GPS ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 699, N'GPS ����������', N'/��� � �����/������ ��� ��������������/GPS ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'LEGO', N'/��� � �����/������ ��� �����/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'LEGO', N'/��� � �����/������ ��� �����/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'LEGO', N'/��� � �����/������ ��� �����/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'LEGO', N'/��� � �����/������ ��� �����/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'LEGO', N'/��� � �����/������ ��� �����/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'LEGO', N'/��� � �����/������ ��� �����/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'LEGO', N'/��� � �����/������ ��� �����/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'LEGO', N'/��� � �����/������ ��� �����/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'LEGO', N'/��� � �����/������ ��� �����/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '��������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 587, N'��������������', N'/��� � �����/������ ��� ��������������/��������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '��������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 601, N'��������������', N'/��� � �����/������ ��� ��������������/��������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '��������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 615, N'��������������', N'/��� � �����/������ ��� ��������������/��������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '��������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 629, N'��������������', N'/��� � �����/������ ��� ��������������/��������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '��������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 643, N'��������������', N'/��� � �����/������ ��� ��������������/��������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '��������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 657, N'��������������', N'/��� � �����/������ ��� ��������������/��������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '��������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 671, N'��������������', N'/��� � �����/������ ��� ��������������/��������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '��������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 685, N'��������������', N'/��� � �����/������ ��� ��������������/��������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '��������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 699, N'��������������', N'/��� � �����/������ ��� ��������������/��������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������, �������, ������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 587, N'����������, �������, ������������', N'/��� � �����/������ ��� ��������������/����������, �������, ������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������, �������, ������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 601, N'����������, �������, ������������', N'/��� � �����/������ ��� ��������������/����������, �������, ������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������, �������, ������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 615, N'����������, �������, ������������', N'/��� � �����/������ ��� ��������������/����������, �������, ������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������, �������, ������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 629, N'����������, �������, ������������', N'/��� � �����/������ ��� ��������������/����������, �������, ������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������, �������, ������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 643, N'����������, �������, ������������', N'/��� � �����/������ ��� ��������������/����������, �������, ������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������, �������, ������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 657, N'����������, �������, ������������', N'/��� � �����/������ ��� ��������������/����������, �������, ������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������, �������, ������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 671, N'����������, �������, ������������', N'/��� � �����/������ ��� ��������������/����������, �������, ������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������, �������, ������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 685, N'����������, �������, ������������', N'/��� � �����/������ ��� ��������������/����������, �������, ������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������, �������, ������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 699, N'����������, �������, ������������', N'/��� � �����/������ ��� ��������������/����������, �������, ������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 580, N'��������� ������', N'/�������/���������� � ��������/��������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 594, N'��������� ������', N'/�������/���������� � ��������/��������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 608, N'��������� ������', N'/�������/���������� � ��������/��������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 622, N'��������� ������', N'/�������/���������� � ��������/��������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 636, N'��������� ������', N'/�������/���������� � ��������/��������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 650, N'��������� ������', N'/�������/���������� � ��������/��������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 664, N'��������� ������', N'/�������/���������� � ��������/��������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 678, N'��������� ������', N'/�������/���������� � ��������/��������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 692, N'��������� ������', N'/�������/���������� � ��������/��������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 591, N'����������', N'/��� � �����/������ ��� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 605, N'����������', N'/��� � �����/������ ��� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 619, N'����������', N'/��� � �����/������ ��� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 633, N'����������', N'/��� � �����/������ ��� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 647, N'����������', N'/��� � �����/������ ��� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 661, N'����������', N'/��� � �����/������ ��� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 675, N'����������', N'/��� � �����/������ ��� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 689, N'����������', N'/��� � �����/������ ��� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 703, N'����������', N'/��� � �����/������ ��� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '���������� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 579, N'���������� ��� ���������', N'/��� � �����/����������/���������� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '���������� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 593, N'���������� ��� ���������', N'/��� � �����/����������/���������� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '���������� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 607, N'���������� ��� ���������', N'/��� � �����/����������/���������� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '���������� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 621, N'���������� ��� ���������', N'/��� � �����/����������/���������� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '���������� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 635, N'���������� ��� ���������', N'/��� � �����/����������/���������� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '���������� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 649, N'���������� ��� ���������', N'/��� � �����/����������/���������� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '���������� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 663, N'���������� ��� ���������', N'/��� � �����/����������/���������� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '���������� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 677, N'���������� ��� ���������', N'/��� � �����/����������/���������� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '���������� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 691, N'���������� ��� ���������', N'/��� � �����/����������/���������� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������� ����� � �����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 592, N'�������� ����� � �����������', N'/��� � �����/������ ��� ������/�������� ����� � �����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������� ����� � �����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 606, N'�������� ����� � �����������', N'/��� � �����/������ ��� ������/�������� ����� � �����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������� ����� � �����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 620, N'�������� ����� � �����������', N'/��� � �����/������ ��� ������/�������� ����� � �����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������� ����� � �����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 634, N'�������� ����� � �����������', N'/��� � �����/������ ��� ������/�������� ����� � �����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������� ����� � �����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 648, N'�������� ����� � �����������', N'/��� � �����/������ ��� ������/�������� ����� � �����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������� ����� � �����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 662, N'�������� ����� � �����������', N'/��� � �����/������ ��� ������/�������� ����� � �����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������� ����� � �����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 676, N'�������� ����� � �����������', N'/��� � �����/������ ��� ������/�������� ����� � �����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������� ����� � �����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 690, N'�������� ����� � �����������', N'/��� � �����/������ ��� ������/�������� ����� � �����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������� ����� � �����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 704, N'�������� ����� � �����������', N'/��� � �����/������ ��� ������/�������� ����� � �����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '�����-����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'�����-����� �������', N'/������� � �����/�������, ��������� ��������/�����-����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '�����-����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'�����-����� �������', N'/������� � �����/�������, ��������� ��������/�����-����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '�����-����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'�����-����� �������', N'/������� � �����/�������, ��������� ��������/�����-����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '�����-����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'�����-����� �������', N'/������� � �����/�������, ��������� ��������/�����-����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '�����-����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'�����-����� �������', N'/������� � �����/�������, ��������� ��������/�����-����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '�����-����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'�����-����� �������', N'/������� � �����/�������, ��������� ��������/�����-����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '�����-����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'�����-����� �������', N'/������� � �����/�������, ��������� ��������/�����-����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '�����-����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'�����-����� �������', N'/������� � �����/�������, ��������� ��������/�����-����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '�����-����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'�����-����� �������', N'/������� � �����/�������, ��������� ��������/�����-����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'������������', N'/��� � �����/������ ��� �����/������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'������������', N'/��� � �����/������ ��� �����/������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'������������', N'/��� � �����/������ ��� �����/������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'������������', N'/��� � �����/������ ��� �����/������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'������������', N'/��� � �����/������ ��� �����/������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'������������', N'/��� � �����/������ ��� �����/������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'������������', N'/��� � �����/������ ��� �����/������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'������������', N'/��� � �����/������ ��� �����/������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'������������', N'/��� � �����/������ ��� �����/������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'������-����������', N'/�������/�����/������-����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'������-����������', N'/�������/�����/������-����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'������-����������', N'/�������/�����/������-����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'������-����������', N'/�������/�����/������-����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'������-����������', N'/�������/�����/������-����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'������-����������', N'/�������/�����/������-����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'������-����������', N'/�������/�����/������-����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'������-����������', N'/�������/�����/������-����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'������-����������', N'/�������/�����/������-����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� � ��������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'�������������� � ��������� ����������', N'/�������/�����/�������������� � ��������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� � ��������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'�������������� � ��������� ����������', N'/�������/�����/�������������� � ��������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� � ��������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'�������������� � ��������� ����������', N'/�������/�����/�������������� � ��������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� � ��������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'�������������� � ��������� ����������', N'/�������/�����/�������������� � ��������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� � ��������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'�������������� � ��������� ����������', N'/�������/�����/�������������� � ��������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� � ��������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'�������������� � ��������� ����������', N'/�������/�����/�������������� � ��������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� � ��������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'�������������� � ��������� ����������', N'/�������/�����/�������������� � ��������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� � ��������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'�������������� � ��������� ����������', N'/�������/�����/�������������� � ��������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� � ��������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'�������������� � ��������� ����������', N'/�������/�����/�������������� � ��������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����, ����, �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'�����, ����, �����', N'/��� � �����/������ ��� ����/�����, ����, �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����, ����, �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'�����, ����, �����', N'/��� � �����/������ ��� ����/�����, ����, �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����, ����, �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'�����, ����, �����', N'/��� � �����/������ ��� ����/�����, ����, �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����, ����, �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'�����, ����, �����', N'/��� � �����/������ ��� ����/�����, ����, �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����, ����, �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'�����, ����, �����', N'/��� � �����/������ ��� ����/�����, ����, �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����, ����, �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'�����, ����, �����', N'/��� � �����/������ ��� ����/�����, ����, �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����, ����, �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'�����, ����, �����', N'/��� � �����/������ ��� ����/�����, ����, �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����, ����, �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'�����, ����, �����', N'/��� � �����/������ ��� ����/�����, ����, �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����, ����, �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'�����, ����, �����', N'/��� � �����/������ ��� ����/�����, ����, �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 585, N'���������', N'/������� � �����/���� � ����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 599, N'���������', N'/������� � �����/���� � ����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 613, N'���������', N'/������� � �����/���� � ����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 627, N'���������', N'/������� � �����/���� � ����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 641, N'���������', N'/������� � �����/���� � ����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 655, N'���������', N'/������� � �����/���� � ����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 669, N'���������', N'/������� � �����/���� � ����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 683, N'���������', N'/������� � �����/���� � ����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 697, N'���������', N'/������� � �����/���� � ����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��� ��� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'��� ��� �������', N'/��� � �����/������ ��� ����/��� ��� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��� ��� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'��� ��� �������', N'/��� � �����/������ ��� ����/��� ��� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��� ��� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'��� ��� �������', N'/��� � �����/������ ��� ����/��� ��� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��� ��� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'��� ��� �������', N'/��� � �����/������ ��� ����/��� ��� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��� ��� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'��� ��� �������', N'/��� � �����/������ ��� ����/��� ��� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��� ��� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'��� ��� �������', N'/��� � �����/������ ��� ����/��� ��� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��� ��� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'��� ��� �������', N'/��� � �����/������ ��� ����/��� ��� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��� ��� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'��� ��� �������', N'/��� � �����/������ ��� ����/��� ��� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��� ��� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'��� ��� �������', N'/��� � �����/������ ��� ����/��� ��� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������� ��������� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'������� ��������� � �������', N'/��� � �����/������ ��� �����/������� ��������� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������� ��������� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'������� ��������� � �������', N'/��� � �����/������ ��� �����/������� ��������� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������� ��������� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'������� ��������� � �������', N'/��� � �����/������ ��� �����/������� ��������� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������� ��������� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'������� ��������� � �������', N'/��� � �����/������ ��� �����/������� ��������� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������� ��������� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'������� ��������� � �������', N'/��� � �����/������ ��� �����/������� ��������� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������� ��������� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'������� ��������� � �������', N'/��� � �����/������ ��� �����/������� ��������� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������� ��������� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'������� ��������� � �������', N'/��� � �����/������ ��� �����/������� ��������� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������� ��������� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'������� ��������� � �������', N'/��� � �����/������ ��� �����/������� ��������� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '������� ��������� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'������� ��������� � �������', N'/��� � �����/������ ��� �����/������� ��������� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'������� ����������', N'/�������/�����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'������� ����������', N'/�������/�����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'������� ����������', N'/�������/�����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'������� ����������', N'/�������/�����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'������� ����������', N'/�������/�����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'������� ����������', N'/�������/�����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'������� ����������', N'/�������/�����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'������� ����������', N'/�������/�����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'������� ����������', N'/�������/�����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������� �����, �������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'�������� �����, �������� �����', N'/��� � �����/������ ��� �����/�������� �����, �������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������� �����, �������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'�������� �����, �������� �����', N'/��� � �����/������ ��� �����/�������� �����, �������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������� �����, �������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'�������� �����, �������� �����', N'/��� � �����/������ ��� �����/�������� �����, �������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������� �����, �������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'�������� �����, �������� �����', N'/��� � �����/������ ��� �����/�������� �����, �������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������� �����, �������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'�������� �����, �������� �����', N'/��� � �����/������ ��� �����/�������� �����, �������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������� �����, �������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'�������� �����, �������� �����', N'/��� � �����/������ ��� �����/�������� �����, �������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������� �����, �������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'�������� �����, �������� �����', N'/��� � �����/������ ��� �����/�������� �����, �������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������� �����, �������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'�������� �����, �������� �����', N'/��� � �����/������ ��� �����/�������� �����, �������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������� �����, �������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'�������� �����, �������� �����', N'/��� � �����/������ ��� �����/�������� �����, �������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'�������� ��������', N'/��� � �����/������ ��� ����/�������� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'�������� ��������', N'/��� � �����/������ ��� ����/�������� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'�������� ��������', N'/��� � �����/������ ��� ����/�������� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'�������� ��������', N'/��� � �����/������ ��� ����/�������� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'�������� ��������', N'/��� � �����/������ ��� ����/�������� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'�������� ��������', N'/��� � �����/������ ��� ����/�������� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'�������� ��������', N'/��� � �����/������ ��� ����/�������� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'�������� ��������', N'/��� � �����/������ ��� ����/�������� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'�������� ��������', N'/��� � �����/������ ��� ����/�������� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'�������� ����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'�������� ����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'�������� ����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'�������� ����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'�������� ����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'�������� ����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'�������� ����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'�������� ����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'�������� ����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '���������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'���������� �����', N'/�������/�����/���������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '���������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'���������� �����', N'/�������/�����/���������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '���������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'���������� �����', N'/�������/�����/���������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '���������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'���������� �����', N'/�������/�����/���������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '���������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'���������� �����', N'/�������/�����/���������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '���������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'���������� �����', N'/�������/�����/���������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '���������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'���������� �����', N'/�������/�����/���������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '���������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'���������� �����', N'/�������/�����/���������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '���������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'���������� �����', N'/�������/�����/���������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 585, N'������� ���������', N'/������� � �����/���� � ����/������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 599, N'������� ���������', N'/������� � �����/���� � ����/������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 613, N'������� ���������', N'/������� � �����/���� � ����/������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 627, N'������� ���������', N'/������� � �����/���� � ����/������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 641, N'������� ���������', N'/������� � �����/���� � ����/������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 655, N'������� ���������', N'/������� � �����/���� � ����/������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 669, N'������� ���������', N'/������� � �����/���� � ����/������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 683, N'������� ���������', N'/������� � �����/���� � ����/������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 697, N'������� ���������', N'/������� � �����/���� � ����/������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'�������', N'/��� � �����/������ ��� �����/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'�������', N'/��� � �����/������ ��� �����/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'�������', N'/��� � �����/������ ��� �����/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'�������', N'/��� � �����/������ ��� �����/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'�������', N'/��� � �����/������ ��� �����/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'�������', N'/��� � �����/������ ��� �����/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'�������', N'/��� � �����/������ ��� �����/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'�������', N'/��� � �����/������ ��� �����/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'�������', N'/��� � �����/������ ��� �����/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 587, N'����������� ��� ����', N'/��� � �����/������ ��� ��������������/����������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 601, N'����������� ��� ����', N'/��� � �����/������ ��� ��������������/����������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 615, N'����������� ��� ����', N'/��� � �����/������ ��� ��������������/����������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 629, N'����������� ��� ����', N'/��� � �����/������ ��� ��������������/����������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 643, N'����������� ��� ����', N'/��� � �����/������ ��� ��������������/����������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 657, N'����������� ��� ����', N'/��� � �����/������ ��� ��������������/����������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 671, N'����������� ��� ����', N'/��� � �����/������ ��� ��������������/����������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 685, N'����������� ��� ����', N'/��� � �����/������ ��� ��������������/����������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '����������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 699, N'����������� ��� ����', N'/��� � �����/������ ��� ��������������/����������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'��������', N'/��� � �����/������ ��� ����/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'��������', N'/��� � �����/������ ��� ����/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'��������', N'/��� � �����/������ ��� ����/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'��������', N'/��� � �����/������ ��� ����/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'��������', N'/��� � �����/������ ��� ����/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'��������', N'/��� � �����/������ ��� ����/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'��������', N'/��� � �����/������ ��� ����/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'��������', N'/��� � �����/������ ��� ����/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'��������', N'/��� � �����/������ ��� ����/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'������������ ������', N'/�������/�����/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'������������ ������', N'/�������/�����/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'������������ ������', N'/�������/�����/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'������������ ������', N'/�������/�����/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'������������ ������', N'/�������/�����/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'������������ ������', N'/�������/�����/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'������������ ������', N'/�������/�����/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'������������ ������', N'/�������/�����/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'������������ ������', N'/�������/�����/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 580, N'������������ ������', N'/�������/���������� � ��������/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 594, N'������������ ������', N'/�������/���������� � ��������/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 608, N'������������ ������', N'/�������/���������� � ��������/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 622, N'������������ ������', N'/�������/���������� � ��������/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 636, N'������������ ������', N'/�������/���������� � ��������/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 650, N'������������ ������', N'/�������/���������� � ��������/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 664, N'������������ ������', N'/�������/���������� � ��������/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 678, N'������������ ������', N'/�������/���������� � ��������/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 692, N'������������ ������', N'/�������/���������� � ��������/������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 584, N'����', N'/�������/������ � ����/����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 598, N'����', N'/�������/������ � ����/����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 612, N'����', N'/�������/������ � ����/����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 626, N'����', N'/�������/������ � ����/����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 640, N'����', N'/�������/������ � ����/����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 654, N'����', N'/�������/������ � ����/����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 668, N'����', N'/�������/������ � ����/����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 682, N'����', N'/�������/������ � ����/����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 696, N'����', N'/�������/������ � ����/����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'����� ��� ���� � ������', N'/�������/�����/����� ��� ���� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'����� ��� ���� � ������', N'/�������/�����/����� ��� ���� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'����� ��� ���� � ������', N'/�������/�����/����� ��� ���� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'����� ��� ���� � ������', N'/�������/�����/����� ��� ���� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'����� ��� ���� � ������', N'/�������/�����/����� ��� ���� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'����� ��� ���� � ������', N'/�������/�����/����� ��� ���� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'����� ��� ���� � ������', N'/�������/�����/����� ��� ���� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'����� ��� ���� � ������', N'/�������/�����/����� ��� ���� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'����� ��� ���� � ������', N'/�������/�����/����� ��� ���� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'����� ��� ���������', N'/�������/�����/����� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'����� ��� ���������', N'/�������/�����/����� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'����� ��� ���������', N'/�������/�����/����� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'����� ��� ���������', N'/�������/�����/����� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'����� ��� ���������', N'/�������/�����/����� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'����� ��� ���������', N'/�������/�����/����� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'����� ��� ���������', N'/�������/�����/����� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'����� ��� ���������', N'/�������/�����/����� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� ��� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'����� ��� ���������', N'/�������/�����/����� ��� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� � ������������, ������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'����� � ������������, ������� � ������', N'/�������/�����/����� � ������������, ������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� � ������������, ������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'����� � ������������, ������� � ������', N'/�������/�����/����� � ������������, ������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� � ������������, ������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'����� � ������������, ������� � ������', N'/�������/�����/����� � ������������, ������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� � ������������, ������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'����� � ������������, ������� � ������', N'/�������/�����/����� � ������������, ������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� � ������������, ������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'����� � ������������, ������� � ������', N'/�������/�����/����� � ������������, ������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� � ������������, ������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'����� � ������������, ������� � ������', N'/�������/�����/����� � ������������, ������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� � ������������, ������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'����� � ������������, ������� � ������', N'/�������/�����/����� � ������������, ������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� � ������������, ������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'����� � ������������, ������� � ������', N'/�������/�����/����� � ������������, ������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� � ������������, ������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'����� � ������������, ������� � ������', N'/�������/�����/����� � ������������, ������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� �������� ����������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'����� �� �������� ����������� ������', N'/�������/�����/����� �� �������� ����������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� �������� ����������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'����� �� �������� ����������� ������', N'/�������/�����/����� �� �������� ����������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� �������� ����������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'����� �� �������� ����������� ������', N'/�������/�����/����� �� �������� ����������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� �������� ����������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'����� �� �������� ����������� ������', N'/�������/�����/����� �� �������� ����������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� �������� ����������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'����� �� �������� ����������� ������', N'/�������/�����/����� �� �������� ����������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� �������� ����������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'����� �� �������� ����������� ������', N'/�������/�����/����� �� �������� ����������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� �������� ����������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'����� �� �������� ����������� ������', N'/�������/�����/����� �� �������� ����������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� �������� ����������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'����� �� �������� ����������� ������', N'/�������/�����/����� �� �������� ����������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� �������� ����������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'����� �� �������� ����������� ������', N'/�������/�����/����� �� �������� ����������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ��������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'����� �� ��������� � ��������', N'/�������/�����/����� �� ��������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ��������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'����� �� ��������� � ��������', N'/�������/�����/����� �� ��������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ��������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'����� �� ��������� � ��������', N'/�������/�����/����� �� ��������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ��������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'����� �� ��������� � ��������', N'/�������/�����/����� �� ��������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ��������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'����� �� ��������� � ��������', N'/�������/�����/����� �� ��������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ��������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'����� �� ��������� � ��������', N'/�������/�����/����� �� ��������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ��������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'����� �� ��������� � ��������', N'/�������/�����/����� �� ��������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ��������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'����� �� ��������� � ��������', N'/�������/�����/����� �� ��������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ��������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'����� �� ��������� � ��������', N'/�������/�����/����� �� ��������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ������������ � ������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'����� �� ������������ � ������������ ������', N'/�������/�����/����� �� ������������ � ������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ������������ � ������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'����� �� ������������ � ������������ ������', N'/�������/�����/����� �� ������������ � ������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ������������ � ������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'����� �� ������������ � ������������ ������', N'/�������/�����/����� �� ������������ � ������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ������������ � ������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'����� �� ������������ � ������������ ������', N'/�������/�����/����� �� ������������ � ������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ������������ � ������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'����� �� ������������ � ������������ ������', N'/�������/�����/����� �� ������������ � ������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ������������ � ������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'����� �� ������������ � ������������ ������', N'/�������/�����/����� �� ������������ � ������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ������������ � ������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'����� �� ������������ � ������������ ������', N'/�������/�����/����� �� ������������ � ������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ������������ � ������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'����� �� ������������ � ������������ ������', N'/�������/�����/����� �� ������������ � ������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '����� �� ������������ � ������������ ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'����� �� ������������ � ������������ ������', N'/�������/�����/����� �� ������������ � ������������ ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������������ ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 585, N'������������ ����', N'/������� � �����/���� � ����/������������ ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������������ ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 599, N'������������ ����', N'/������� � �����/���� � ����/������������ ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������������ ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 613, N'������������ ����', N'/������� � �����/���� � ����/������������ ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������������ ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 627, N'������������ ����', N'/������� � �����/���� � ����/������������ ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������������ ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 641, N'������������ ����', N'/������� � �����/���� � ����/������������ ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������������ ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 655, N'������������ ����', N'/������� � �����/���� � ����/������������ ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������������ ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 669, N'������������ ����', N'/������� � �����/���� � ����/������������ ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������������ ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 683, N'������������ ����', N'/������� � �����/���� � ����/������������ ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '������������ ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 697, N'������������ ����', N'/������� � �����/���� � ����/������������ ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'������� ������� �������', N'/������� � �����/������� �������/������� ������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'������� ������� �������', N'/������� � �����/������� �������/������� ������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'������� ������� �������', N'/������� � �����/������� �������/������� ������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'������� ������� �������', N'/������� � �����/������� �������/������� ������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'������� ������� �������', N'/������� � �����/������� �������/������� ������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'������� ������� �������', N'/������� � �����/������� �������/������� ������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'������� ������� �������', N'/������� � �����/������� �������/������� ������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'������� ������� �������', N'/������� � �����/������� �������/������� ������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'������� ������� �������', N'/������� � �����/������� �������/������� ������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'�����', N'/��� � �����/������ ��� ����/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'�����', N'/��� � �����/������ ��� ����/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'�����', N'/��� � �����/������ ��� ����/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'�����', N'/��� � �����/������ ��� ����/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'�����', N'/��� � �����/������ ��� ����/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'�����', N'/��� � �����/������ ��� ����/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'�����', N'/��� � �����/������ ��� ����/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'�����', N'/��� � �����/������ ��� ����/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'�����', N'/��� � �����/������ ��� ����/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� � ������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 588, N'�������� � ������� �����', N'/��� � �����/������ ��� ����/�������� � ������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� � ������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 602, N'�������� � ������� �����', N'/��� � �����/������ ��� ����/�������� � ������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� � ������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 616, N'�������� � ������� �����', N'/��� � �����/������ ��� ����/�������� � ������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� � ������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 630, N'�������� � ������� �����', N'/��� � �����/������ ��� ����/�������� � ������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� � ������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 644, N'�������� � ������� �����', N'/��� � �����/������ ��� ����/�������� � ������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� � ������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 658, N'�������� � ������� �����', N'/��� � �����/������ ��� ����/�������� � ������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� � ������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 672, N'�������� � ������� �����', N'/��� � �����/������ ��� ����/�������� � ������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� � ������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 686, N'�������� � ������� �����', N'/��� � �����/������ ��� ����/�������� � ������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '�������� � ������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 700, N'�������� � ������� �����', N'/��� � �����/������ ��� ����/�������� � ������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '����������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'����������� �������', N'/�������, ��������, ������/������� ��� ������� � ��������/����������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '����������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'����������� �������', N'/�������, ��������, ������/������� ��� ������� � ��������/����������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '����������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'����������� �������', N'/�������, ��������, ������/������� ��� ������� � ��������/����������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '����������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'����������� �������', N'/�������, ��������, ������/������� ��� ������� � ��������/����������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '����������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'����������� �������', N'/�������, ��������, ������/������� ��� ������� � ��������/����������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '����������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'����������� �������', N'/�������, ��������, ������/������� ��� ������� � ��������/����������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '����������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'����������� �������', N'/�������, ��������, ������/������� ��� ������� � ��������/����������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '����������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'����������� �������', N'/�������, ��������, ������/������� ��� ������� � ��������/����������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '����������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'����������� �������', N'/�������, ��������, ������/������� ��� ������� � ��������/����������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '������ ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 579, N'������ ����������', N'/��� � �����/����������/������ ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '������ ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 593, N'������ ����������', N'/��� � �����/����������/������ ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '������ ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 607, N'������ ����������', N'/��� � �����/����������/������ ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '������ ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 621, N'������ ����������', N'/��� � �����/����������/������ ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '������ ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 635, N'������ ����������', N'/��� � �����/����������/������ ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '������ ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 649, N'������ ����������', N'/��� � �����/����������/������ ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '������ ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 663, N'������ ����������', N'/��� � �����/����������/������ ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '������ ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 677, N'������ ����������', N'/��� � �����/����������/������ ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/����������/%'
  AND	[Name] = '������ ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 691, N'������ ����������', N'/��� � �����/����������/������ ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 584, N'������', N'/�������/������ � ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 598, N'������', N'/�������/������ � ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 612, N'������', N'/�������/������ � ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 626, N'������', N'/�������/������ � ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 640, N'������', N'/�������/������ � ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 654, N'������', N'/�������/������ � ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 668, N'������', N'/�������/������ � ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 682, N'������', N'/�������/������ � ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/������ � ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 696, N'������', N'/�������/������ � ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'���������� ����', N'/��� � �����/������ ��� �����/���������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'���������� ����', N'/��� � �����/������ ��� �����/���������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'���������� ����', N'/��� � �����/������ ��� �����/���������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'���������� ����', N'/��� � �����/������ ��� �����/���������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'���������� ����', N'/��� � �����/������ ��� �����/���������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'���������� ����', N'/��� � �����/������ ��� �����/���������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'���������� ����', N'/��� � �����/������ ��� �����/���������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'���������� ����', N'/��� � �����/������ ��� �����/���������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'���������� ����', N'/��� � �����/������ ��� �����/���������� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'������-����������� ����������', N'/�������/�����/������-����������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'������-����������� ����������', N'/�������/�����/������-����������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'������-����������� ����������', N'/�������/�����/������-����������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'������-����������� ����������', N'/�������/�����/������-����������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'������-����������� ����������', N'/�������/�����/������-����������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'������-����������� ����������', N'/�������/�����/������-����������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'������-����������� ����������', N'/�������/�����/������-����������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'������-����������� ����������', N'/�������/�����/������-����������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������-����������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'������-����������� ����������', N'/�������/�����/������-����������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '��������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 585, N'��������� ���������', N'/������� � �����/���� � ����/��������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '��������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 599, N'��������� ���������', N'/������� � �����/���� � ����/��������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '��������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 613, N'��������� ���������', N'/������� � �����/���� � ����/��������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '��������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 627, N'��������� ���������', N'/������� � �����/���� � ����/��������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '��������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 641, N'��������� ���������', N'/������� � �����/���� � ����/��������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '��������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 655, N'��������� ���������', N'/������� � �����/���� � ����/��������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '��������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 669, N'��������� ���������', N'/������� � �����/���� � ����/��������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '��������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 683, N'��������� ���������', N'/������� � �����/���� � ����/��������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '��������� ���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 697, N'��������� ���������', N'/������� � �����/���� � ����/��������� ���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 580, N'������������ �������', N'/�������/���������� � ��������/������������ �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 594, N'������������ �������', N'/�������/���������� � ��������/������������ �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 608, N'������������ �������', N'/�������/���������� � ��������/������������ �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 622, N'������������ �������', N'/�������/���������� � ��������/������������ �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 636, N'������������ �������', N'/�������/���������� � ��������/������������ �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 650, N'������������ �������', N'/�������/���������� � ��������/������������ �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 664, N'������������ �������', N'/�������/���������� � ��������/������������ �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 678, N'������������ �������', N'/�������/���������� � ��������/������������ �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '������������ �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 692, N'������������ �������', N'/�������/���������� � ��������/������������ �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������� � �������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'��������� � �������������', N'/��� � �����/������ ��� ����/��������� � �������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������� � �������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'��������� � �������������', N'/��� � �����/������ ��� ����/��������� � �������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������� � �������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'��������� � �������������', N'/��� � �����/������ ��� ����/��������� � �������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������� � �������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'��������� � �������������', N'/��� � �����/������ ��� ����/��������� � �������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������� � �������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'��������� � �������������', N'/��� � �����/������ ��� ����/��������� � �������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������� � �������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'��������� � �������������', N'/��� � �����/������ ��� ����/��������� � �������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������� � �������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'��������� � �������������', N'/��� � �����/������ ��� ����/��������� � �������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������� � �������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'��������� � �������������', N'/��� � �����/������ ��� ����/��������� � �������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '��������� � �������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'��������� � �������������', N'/��� � �����/������ ��� ����/��������� � �������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 588, N'������', N'/��� � �����/������ ��� ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 602, N'������', N'/��� � �����/������ ��� ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 616, N'������', N'/��� � �����/������ ��� ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 630, N'������', N'/��� � �����/������ ��� ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 644, N'������', N'/��� � �����/������ ��� ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 658, N'������', N'/��� � �����/������ ��� ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 672, N'������', N'/��� � �����/������ ��� ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 686, N'������', N'/��� � �����/������ ��� ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 700, N'������', N'/��� � �����/������ ��� ����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '����� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 588, N'����� ��������', N'/��� � �����/������ ��� ����/����� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '����� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 602, N'����� ��������', N'/��� � �����/������ ��� ����/����� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '����� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 616, N'����� ��������', N'/��� � �����/������ ��� ����/����� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '����� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 630, N'����� ��������', N'/��� � �����/������ ��� ����/����� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '����� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 644, N'����� ��������', N'/��� � �����/������ ��� ����/����� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '����� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 658, N'����� ��������', N'/��� � �����/������ ��� ����/����� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '����� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 672, N'����� ��������', N'/��� � �����/������ ��� ����/����� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '����� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 686, N'����� ��������', N'/��� � �����/������ ��� ����/����� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '����� ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 700, N'����� ��������', N'/��� � �����/������ ��� ����/����� ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'������', N'/�������/�����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'������', N'/�������/�����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'������', N'/�������/�����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'������', N'/�������/�����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'������', N'/�������/�����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'������', N'/�������/�����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'������', N'/�������/�����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'������', N'/�������/�����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'������', N'/�������/�����/������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '�������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'�������� � ������', N'/������� � �����/������� �������/�������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '�������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'�������� � ������', N'/������� � �����/������� �������/�������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '�������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'�������� � ������', N'/������� � �����/������� �������/�������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '�������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'�������� � ������', N'/������� � �����/������� �������/�������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '�������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'�������� � ������', N'/������� � �����/������� �������/�������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '�������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'�������� � ������', N'/������� � �����/������� �������/�������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '�������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'�������� � ������', N'/������� � �����/������� �������/�������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '�������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'�������� � ������', N'/������� � �����/������� �������/�������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '�������� � ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'�������� � ������', N'/������� � �����/������� �������/�������� � ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'������� �����', N'/�������/�����/������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'������� �����', N'/�������/�����/������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'������� �����', N'/�������/�����/������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'������� �����', N'/�������/�����/������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'������� �����', N'/�������/�����/������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'������� �����', N'/�������/�����/������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'������� �����', N'/�������/�����/������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'������� �����', N'/�������/�����/������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'������� �����', N'/�������/�����/������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 592, N'�������', N'/��� � �����/������ ��� ������/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 606, N'�������', N'/��� � �����/������ ��� ������/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 620, N'�������', N'/��� � �����/������ ��� ������/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 634, N'�������', N'/��� � �����/������ ��� ������/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 648, N'�������', N'/��� � �����/������ ��� ������/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 662, N'�������', N'/��� � �����/������ ��� ������/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 676, N'�������', N'/��� � �����/������ ��� ������/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 690, N'�������', N'/��� � �����/������ ��� ������/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 704, N'�������', N'/��� � �����/������ ��� ������/�������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 588, N'������� �������', N'/��� � �����/������ ��� ����/������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 602, N'������� �������', N'/��� � �����/������ ��� ����/������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 616, N'������� �������', N'/��� � �����/������ ��� ����/������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 630, N'������� �������', N'/��� � �����/������ ��� ����/������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 644, N'������� �������', N'/��� � �����/������ ��� ����/������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 658, N'������� �������', N'/��� � �����/������ ��� ����/������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 672, N'������� �������', N'/��� � �����/������ ��� ����/������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 686, N'������� �������', N'/��� � �����/������ ��� ����/������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 700, N'������� �������', N'/��� � �����/������ ��� ����/������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 588, N'������� ����������', N'/��� � �����/������ ��� ����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 602, N'������� ����������', N'/��� � �����/������ ��� ����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 616, N'������� ����������', N'/��� � �����/������ ��� ����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 630, N'������� ����������', N'/��� � �����/������ ��� ����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 644, N'������� ����������', N'/��� � �����/������ ��� ����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 658, N'������� ����������', N'/��� � �����/������ ��� ����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 672, N'������� ����������', N'/��� � �����/������ ��� ����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 686, N'������� ����������', N'/��� � �����/������ ��� ����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 700, N'������� ����������', N'/��� � �����/������ ��� ����/������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���� ��� ���� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 585, N'���� ��� ���� � �������', N'/������� � �����/���� � ����/���� ��� ���� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���� ��� ���� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 599, N'���� ��� ���� � �������', N'/������� � �����/���� � ����/���� ��� ���� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���� ��� ���� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 613, N'���� ��� ���� � �������', N'/������� � �����/���� � ����/���� ��� ���� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���� ��� ���� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 627, N'���� ��� ���� � �������', N'/������� � �����/���� � ����/���� ��� ���� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���� ��� ���� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 641, N'���� ��� ���� � �������', N'/������� � �����/���� � ����/���� ��� ���� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���� ��� ���� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 655, N'���� ��� ���� � �������', N'/������� � �����/���� � ����/���� ��� ���� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���� ��� ���� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 669, N'���� ��� ���� � �������', N'/������� � �����/���� � ����/���� ��� ���� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���� ��� ���� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 683, N'���� ��� ���� � �������', N'/������� � �����/���� � ����/���� ��� ���� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/���� � ����/%'
  AND	[Name] = '���� ��� ���� � �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 697, N'���� ��� ���� � �������', N'/������� � �����/���� � ����/���� ��� ���� � �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 592, N'�����', N'/��� � �����/������ ��� ������/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 606, N'�����', N'/��� � �����/������ ��� ������/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 620, N'�����', N'/��� � �����/������ ��� ������/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 634, N'�����', N'/��� � �����/������ ��� ������/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 648, N'�����', N'/��� � �����/������ ��� ������/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 662, N'�����', N'/��� � �����/������ ��� ������/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 676, N'�����', N'/��� � �����/������ ��� ������/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 690, N'�����', N'/��� � �����/������ ��� ������/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ������/%'
  AND	[Name] = '�����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 704, N'�����', N'/��� � �����/������ ��� ������/�����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '�������� �� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 591, N'�������� �� �����', N'/��� � �����/������ ��� ��������/�������� �� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '�������� �� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 605, N'�������� �� �����', N'/��� � �����/������ ��� ��������/�������� �� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '�������� �� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 619, N'�������� �� �����', N'/��� � �����/������ ��� ��������/�������� �� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '�������� �� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 633, N'�������� �� �����', N'/��� � �����/������ ��� ��������/�������� �� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '�������� �� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 647, N'�������� �� �����', N'/��� � �����/������ ��� ��������/�������� �� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '�������� �� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 661, N'�������� �� �����', N'/��� � �����/������ ��� ��������/�������� �� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '�������� �� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 675, N'�������� �� �����', N'/��� � �����/������ ��� ��������/�������� �� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '�������� �� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 689, N'�������� �� �����', N'/��� � �����/������ ��� ��������/�������� �� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������/%'
  AND	[Name] = '�������� �� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 703, N'�������� �� �����', N'/��� � �����/������ ��� ��������/�������� �� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� � �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'�������� � �����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� � �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� � �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'�������� � �����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� � �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� � �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'�������� � �����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� � �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� � �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'�������� � �����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� � �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� � �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'�������� � �����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� � �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� � �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'�������� � �����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� � �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� � �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'�������� � �����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� � �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� � �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'�������� � �����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� � �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '�������� � �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'�������� � �����', N'/�������, ��������, ������/������� ��� ������� � ��������/�������� � �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 580, N'��������', N'/�������/���������� � ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 594, N'��������', N'/�������/���������� � ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 608, N'��������', N'/�������/���������� � ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 622, N'��������', N'/�������/���������� � ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 636, N'��������', N'/�������/���������� � ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 650, N'��������', N'/�������/���������� � ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 664, N'��������', N'/�������/���������� � ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 678, N'��������', N'/�������/���������� � ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 692, N'��������', N'/�������/���������� � ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'��������', N'/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'������� ��� ����', N'/������� � �����/������� �������/������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'������� ��� ����', N'/������� � �����/������� �������/������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'������� ��� ����', N'/������� � �����/������� �������/������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'������� ��� ����', N'/������� � �����/������� �������/������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'������� ��� ����', N'/������� � �����/������� �������/������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'������� ��� ����', N'/������� � �����/������� �������/������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'������� ��� ����', N'/������� � �����/������� �������/������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'������� ��� ����', N'/������� � �����/������� �������/������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'������� ��� ����', N'/������� � �����/������� �������/������� ��� ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������� ��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'������� ��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/������� ��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������� ��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'������� ��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/������� ��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������� ��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'������� ��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/������� ��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������� ��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'������� ��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/������� ��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������� ��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'������� ��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/������� ��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������� ��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'������� ��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/������� ��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������� ��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'������� ��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/������� ��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������� ��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'������� ��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/������� ��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������� ��� ������� � ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'������� ��� ������� � ��������', N'/�������, ��������, ������/������� ��� ������� � ��������/������� ��� ������� � ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'������� ��� �����', N'/������� � �����/������� �������/������� ��� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'������� ��� �����', N'/������� � �����/������� �������/������� ��� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'������� ��� �����', N'/������� � �����/������� �������/������� ��� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'������� ��� �����', N'/������� � �����/������� �������/������� ��� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'������� ��� �����', N'/������� � �����/������� �������/������� ��� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'������� ��� �����', N'/������� � �����/������� �������/������� ��� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'������� ��� �����', N'/������� � �����/������� �������/������� ��� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'������� ��� �����', N'/������� � �����/������� �������/������� ��� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������� ��� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'������� ��� �����', N'/������� � �����/������� �������/������� ��� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'���������', N'/��� � �����/������ ��� �����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'���������', N'/��� � �����/������ ��� �����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'���������', N'/��� � �����/������ ��� �����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'���������', N'/��� � �����/������ ��� �����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'���������', N'/��� � �����/������ ��� �����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'���������', N'/��� � �����/������ ��� �����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'���������', N'/��� � �����/������ ��� �����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'���������', N'/��� � �����/������ ��� �����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'���������', N'/��� � �����/������ ��� �����/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� � ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 580, N'��������� � ����', N'/�������/���������� � ��������/��������� � ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� � ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 594, N'��������� � ����', N'/�������/���������� � ��������/��������� � ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� � ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 608, N'��������� � ����', N'/�������/���������� � ��������/��������� � ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� � ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 622, N'��������� � ����', N'/�������/���������� � ��������/��������� � ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� � ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 636, N'��������� � ����', N'/�������/���������� � ��������/��������� � ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� � ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 650, N'��������� � ����', N'/�������/���������� � ��������/��������� � ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� � ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 664, N'��������� � ����', N'/�������/���������� � ��������/��������� � ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� � ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 678, N'��������� � ����', N'/�������/���������� � ��������/��������� � ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/���������� � ��������/%'
  AND	[Name] = '��������� � ����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 692, N'��������� � ����', N'/�������/���������� � ��������/��������� � ����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'����������', N'/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'�������������� ����������', N'/�������/�����/�������������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'�������������� ����������', N'/�������/�����/�������������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'�������������� ����������', N'/�������/�����/�������������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'�������������� ����������', N'/�������/�����/�������������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'�������������� ����������', N'/�������/�����/�������������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'�������������� ����������', N'/�������/�����/�������������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'�������������� ����������', N'/�������/�����/�������������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'�������������� ����������', N'/�������/�����/�������������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������/�����/%'
  AND	[Name] = '�������������� ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'�������������� ����������', N'/�������/�����/�������������� ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '���� � ����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'���� � ����� �������', N'/������� � �����/�������, ��������� ��������/���� � ����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '���� � ����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'���� � ����� �������', N'/������� � �����/�������, ��������� ��������/���� � ����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '���� � ����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'���� � ����� �������', N'/������� � �����/�������, ��������� ��������/���� � ����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '���� � ����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'���� � ����� �������', N'/������� � �����/�������, ��������� ��������/���� � ����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '���� � ����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'���� � ����� �������', N'/������� � �����/�������, ��������� ��������/���� � ����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '���� � ����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'���� � ����� �������', N'/������� � �����/�������, ��������� ��������/���� � ����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '���� � ����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'���� � ����� �������', N'/������� � �����/�������, ��������� ��������/���� � ����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '���� � ����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'���� � ����� �������', N'/������� � �����/�������, ��������� ��������/���� � ����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '���� � ����� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'���� � ����� �������', N'/������� � �����/�������, ��������� ��������/���� � ����� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '����� � ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'����� � ����������', N'/��� � �����/������ ��� �����/����� � ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '����� � ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'����� � ����������', N'/��� � �����/������ ��� �����/����� � ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '����� � ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'����� � ����������', N'/��� � �����/������ ��� �����/����� � ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '����� � ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'����� � ����������', N'/��� � �����/������ ��� �����/����� � ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '����� � ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'����� � ����������', N'/��� � �����/������ ��� �����/����� � ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '����� � ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'����� � ����������', N'/��� � �����/������ ��� �����/����� � ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '����� � ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'����� � ����������', N'/��� � �����/������ ��� �����/����� � ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '����� � ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'����� � ����������', N'/��� � �����/������ ��� �����/����� � ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '����� � ����������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'����� � ����������', N'/��� � �����/������ ��� �����/����� � ����������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'������������� ������', N'/��� � �����/������ ��� ����/������������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'������������� ������', N'/��� � �����/������ ��� ����/������������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'������������� ������', N'/��� � �����/������ ��� ����/������������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'������������� ������', N'/��� � �����/������ ��� ����/������������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'������������� ������', N'/��� � �����/������ ��� ����/������������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'������������� ������', N'/��� � �����/������ ��� ����/������������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'������������� ������', N'/��� � �����/������ ��� ����/������������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'������������� ������', N'/��� � �����/������ ��� ����/������������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ����/%'
  AND	[Name] = '������������� ������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'������������� ������', N'/��� � �����/������ ��� ����/������������� ������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '����, �������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'����, �������� �������', N'/������� � �����/������� �������/����, �������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '����, �������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'����, �������� �������', N'/������� � �����/������� �������/����, �������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '����, �������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'����, �������� �������', N'/������� � �����/������� �������/����, �������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '����, �������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'����, �������� �������', N'/������� � �����/������� �������/����, �������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '����, �������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'����, �������� �������', N'/������� � �����/������� �������/����, �������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '����, �������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'����, �������� �������', N'/������� � �����/������� �������/����, �������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '����, �������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'����, �������� �������', N'/������� � �����/������� �������/����, �������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '����, �������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'����, �������� �������', N'/������� � �����/������� �������/����, �������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '����, �������� �������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'����, �������� �������', N'/������� � �����/������� �������/����, �������� �������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�����, ��������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'�����, ��������, ��������', N'/��� � �����/������ ��� �����/�����, ��������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�����, ��������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'�����, ��������, ��������', N'/��� � �����/������ ��� �����/�����, ��������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�����, ��������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'�����, ��������, ��������', N'/��� � �����/������ ��� �����/�����, ��������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�����, ��������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'�����, ��������, ��������', N'/��� � �����/������ ��� �����/�����, ��������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�����, ��������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'�����, ��������, ��������', N'/��� � �����/������ ��� �����/�����, ��������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�����, ��������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'�����, ��������, ��������', N'/��� � �����/������ ��� �����/�����, ��������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�����, ��������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'�����, ��������, ��������', N'/��� � �����/������ ��� �����/�����, ��������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�����, ��������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'�����, ��������, ��������', N'/��� � �����/������ ��� �����/�����, ��������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� �����/%'
  AND	[Name] = '�����, ��������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'�����, ��������, ��������', N'/��� � �����/������ ��� �����/�����, ��������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '�����, �������, ���������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 587, N'�����, �������, ���������, ��������', N'/��� � �����/������ ��� ��������������/�����, �������, ���������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '�����, �������, ���������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 601, N'�����, �������, ���������, ��������', N'/��� � �����/������ ��� ��������������/�����, �������, ���������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '�����, �������, ���������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 615, N'�����, �������, ���������, ��������', N'/��� � �����/������ ��� ��������������/�����, �������, ���������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '�����, �������, ���������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 629, N'�����, �������, ���������, ��������', N'/��� � �����/������ ��� ��������������/�����, �������, ���������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '�����, �������, ���������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 643, N'�����, �������, ���������, ��������', N'/��� � �����/������ ��� ��������������/�����, �������, ���������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '�����, �������, ���������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 657, N'�����, �������, ���������, ��������', N'/��� � �����/������ ��� ��������������/�����, �������, ���������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '�����, �������, ���������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 671, N'�����, �������, ���������, ��������', N'/��� � �����/������ ��� ��������������/�����, �������, ���������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '�����, �������, ���������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 685, N'�����, �������, ���������, ��������', N'/��� � �����/������ ��� ��������������/�����, �������, ���������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/��� � �����/������ ��� ��������������/%'
  AND	[Name] = '�����, �������, ���������, ��������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 699, N'�����, �������, ���������, ��������', N'/��� � �����/������ ��� ��������������/�����, �������, ���������, ��������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������������� ������ �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'������������� ������ �����', N'/�������, ��������, ������/������� ��� ������� � ��������/������������� ������ �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������������� ������ �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'������������� ������ �����', N'/�������, ��������, ������/������� ��� ������� � ��������/������������� ������ �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������������� ������ �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'������������� ������ �����', N'/�������, ��������, ������/������� ��� ������� � ��������/������������� ������ �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������������� ������ �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'������������� ������ �����', N'/�������, ��������, ������/������� ��� ������� � ��������/������������� ������ �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������������� ������ �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'������������� ������ �����', N'/�������, ��������, ������/������� ��� ������� � ��������/������������� ������ �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������������� ������ �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'������������� ������ �����', N'/�������, ��������, ������/������� ��� ������� � ��������/������������� ������ �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������������� ������ �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'������������� ������ �����', N'/�������, ��������, ������/������� ��� ������� � ��������/������������� ������ �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������������� ������ �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'������������� ������ �����', N'/�������, ��������, ������/������� ��� ������� � ��������/������������� ������ �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '������������� ������ �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'������������� ������ �����', N'/�������, ��������, ������/������� ��� ������� � ��������/������������� ������ �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'������������������', N'/������� � �����/������� �������/������������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'������������������', N'/������� � �����/������� �������/������������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'������������������', N'/������� � �����/������� �������/������������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'������������������', N'/������� � �����/������� �������/������������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'������������������', N'/������� � �����/������� �������/������������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'������������������', N'/������� � �����/������� �������/������������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'������������������', N'/������� � �����/������� �������/������������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'������������������', N'/������� � �����/������� �������/������������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/������� �������/%'
  AND	[Name] = '������������������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'������������������', N'/������� � �����/������� �������/������������������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'����������� �����', N'/������� � �����/�������, ��������� ��������/����������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'����������� �����', N'/������� � �����/�������, ��������� ��������/����������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'����������� �����', N'/������� � �����/�������, ��������� ��������/����������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'����������� �����', N'/������� � �����/�������, ��������� ��������/����������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'����������� �����', N'/������� � �����/�������, ��������� ��������/����������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'����������� �����', N'/������� � �����/�������, ��������� ��������/����������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'����������� �����', N'/������� � �����/�������, ��������� ��������/����������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'����������� �����', N'/������� � �����/�������, ��������� ��������/����������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/������� � �����/�������, ��������� ��������/%'
  AND	[Name] = '����������� �����';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'����������� �����', N'/������� � �����/�������, ��������� ��������/����������� �����/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'���������', N'/�������, ��������, ������/������� ��� ������� � ��������/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'���������', N'/�������, ��������, ������/������� ��� ������� � ��������/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'���������', N'/�������, ��������, ������/������� ��� ������� � ��������/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'���������', N'/�������, ��������, ������/������� ��� ������� � ��������/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'���������', N'/�������, ��������, ������/������� ��� ������� � ��������/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'���������', N'/�������, ��������, ������/������� ��� ������� � ��������/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'���������', N'/�������, ��������, ������/������� ��� ������� � ��������/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'���������', N'/�������, ��������, ������/������� ��� ������� � ��������/���������/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/�������, ��������, ������/������� ��� ������� � ��������/%'
  AND	[Name] = '���������';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'���������', N'/�������, ��������, ������/������� ��� ������� � ��������/���������/', 1, @User, GETDATE(), 0);
END

PRINT '������� ������� � ���������� �����'
DELETE FROM [prod].[PartnerProductCategoryLinks]
WHERE [PartnerId] = @OzonId

DELETE FROM [prod].[ProductCategoriesPermissions]
WHERE [PartnerId] = @OzonId

PRINT '����� ���������� �����'
INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 575, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 579, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 599, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 633, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 587, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 593, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 595, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 596, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 617, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 669, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 588, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 631, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 642, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 644, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 649, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 650, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 589, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 594, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 602, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 608, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 610, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 616, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 635, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 660, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 665, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 668, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 590, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 605, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 607, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 612, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 618, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 630, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 641, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 666, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 591, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 598, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 653, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 592, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 600, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 648, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 652, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 576, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 611, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 613, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 632, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 654, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 670, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 673, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 577, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 580, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 597, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 620, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 640, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 655, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 661, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 583, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 603, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 604, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 609, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 614, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 619, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 622, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 623, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 624, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 625, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 626, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 627, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 636, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 645, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 647, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 663, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 584, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 621, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 634, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 578, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 581, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 629, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 646, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 658, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 659, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 667, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 671, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 582, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 601, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 637, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 638, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 643, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 656, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 657, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 662, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 664, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 672, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 585, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 606, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 615, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 628, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 639, @User, GETDATE())

INSERT INTO [prod].[ProductCategoriesPermissions]
	([PartnerId],[CategoryId],[InsertedUserId],[InsertedDate])
VALUES
	(@OzonId, 651, @User, GETDATE())

PRINT '����� ������� �����'
INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(575, @OzonId, '/����������� �������/��� � �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(579, @OzonId, '/����������� �������/��� � �����/����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(599, @OzonId, '/����������� �������/��� � �����/����������/���������� ��� ���������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(633, @OzonId, '/����������� �������/��� � �����/����������/������ ����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(587, @OzonId, '/����������� �������/��� � �����/������ ��� ��������������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(593, @OzonId, '/����������� �������/��� � �����/������ ��� ��������������/GPS ����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(595, @OzonId, '/����������� �������/��� � �����/������ ��� ��������������/��������������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(596, @OzonId, '/����������� �������/��� � �����/������ ��� ��������������/����������, �������, ������������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(617, @OzonId, '/����������� �������/��� � �����/������ ��� ��������������/����������� ��� ����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(669, @OzonId, '/����������� �������/��� � �����/������ ��� ��������������/�����, �������, ���������, ��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(588, @OzonId, '/����������� �������/��� � �����/������ ��� ����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(631, @OzonId, '/����������� �������/��� � �����/������ ��� ����/�������� � ������� �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(642, @OzonId, '/����������� �������/��� � �����/������ ��� ����/������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(644, @OzonId, '/����������� �������/��� � �����/������ ��� ����/����� ��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(649, @OzonId, '/����������� �������/��� � �����/������ ��� ����/������� �������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(650, @OzonId, '/����������� �������/��� � �����/������ ��� ����/������� ����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(589, @OzonId, '/����������� �������/��� � �����/������ ��� �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(594, @OzonId, '/����������� �������/��� � �����/������ ��� �����/LEGO/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(602, @OzonId, '/����������� �������/��� � �����/������ ��� �����/������������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(608, @OzonId, '/����������� �������/��� � �����/������ ��� �����/������� ��������� � �������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(610, @OzonId, '/����������� �������/��� � �����/������ ��� �����/�������� �����, �������� �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(616, @OzonId, '/����������� �������/��� � �����/������ ��� �����/�������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(635, @OzonId, '/����������� �������/��� � �����/������ ��� �����/���������� ����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(660, @OzonId, '/����������� �������/��� � �����/������ ��� �����/���������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(665, @OzonId, '/����������� �������/��� � �����/������ ��� �����/����� � ����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(668, @OzonId, '/����������� �������/��� � �����/������ ��� �����/�����, ��������, ��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(590, @OzonId, '/����������� �������/��� � �����/������ ��� ����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(605, @OzonId, '/����������� �������/��� � �����/������ ��� ����/�����, ����, �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(607, @OzonId, '/����������� �������/��� � �����/������ ��� ����/��� ��� �������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(612, @OzonId, '/����������� �������/��� � �����/������ ��� ����/�������� ��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(618, @OzonId, '/����������� �������/��� � �����/������ ��� ����/��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(630, @OzonId, '/����������� �������/��� � �����/������ ��� ����/�����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(641, @OzonId, '/����������� �������/��� � �����/������ ��� ����/��������� � �������������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(666, @OzonId, '/����������� �������/��� � �����/������ ��� ����/������������� ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(591, @OzonId, '/����������� �������/��� � �����/������ ��� ��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(598, @OzonId, '/����������� �������/��� � �����/������ ��� ��������/����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(653, @OzonId, '/����������� �������/��� � �����/������ ��� ��������/�������� �� �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(592, @OzonId, '/����������� �������/��� � �����/������ ��� ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(600, @OzonId, '/����������� �������/��� � �����/������ ��� ������/�������� ����� � �����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(648, @OzonId, '/����������� �������/��� � �����/������ ��� ������/�������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(652, @OzonId, '/����������� �������/��� � �����/������ ��� ������/�����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(576, @OzonId, '/����������� �������/�������, ��������, ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(576, @OzonId, '/����������� �������/�������, ��������, ������/�������, ��������, ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(611, @OzonId, '/����������� �������/�������, ��������, ������/�������, ��������, ������/��� ������� � ��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(613, @OzonId, '/����������� �������/�������, ��������, ������/�������, ��������, ������/������� ��� ������� � ��������/�������� ����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(632, @OzonId, '/����������� �������/�������, ��������, ������/�������, ��������, ������/������� ��� ������� � ��������/����������� �������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(654, @OzonId, '/����������� �������/�������, ��������, ������/�������, ��������, ������/������� ��� ������� � ��������/�������� � �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(670, @OzonId, '/����������� �������/�������, ��������, ������/�������, ��������, ������/������� ��� ������� � ��������/������������� ������ �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(673, @OzonId, '/����������� �������/�������, ��������, ������/�������, ��������, ������/������� ��� ������� � ��������/���������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(577, @OzonId, '/����������� �������/�������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(580, @OzonId, '/����������� �������/�������/����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(580, @OzonId, '/����������� �������/�������/������ �������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(597, @OzonId, '/����������� �������/�������/������ �������/��������� ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(620, @OzonId, '/����������� �������/�������/������ �������/������������ ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(640, @OzonId, '/����������� �������/�������/������ �������/������������ �������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(655, @OzonId, '/����������� �������/�������/������ �������/��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(661, @OzonId, '/����������� �������/�������/������ �������/��������� � ����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(583, @OzonId, '/����������� �������/�������/�����, ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(603, @OzonId, '/����������� �������/�������/�����, ������/������-����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(604, @OzonId, '/����������� �������/�������/�����, ������/�������������� � ��������� ����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(609, @OzonId, '/����������� �������/�������/�����, ������/������� ����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(614, @OzonId, '/����������� �������/�������/�����, ������/���������� �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(619, @OzonId, '/����������� �������/�������/�����, ������/������������ ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(622, @OzonId, '/����������� �������/�������/�����, ������/����� ��� ���� � ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(623, @OzonId, '/����������� �������/�������/�����, ������/����� ��� ���������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(624, @OzonId, '/����������� �������/�������/�����, ������/����� � ������������, ������� � ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(625, @OzonId, '/����������� �������/�������/�����, ������/����� �� �������� ����������� ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(626, @OzonId, '/����������� �������/�������/�����, ������/����� �� ��������� � ��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(627, @OzonId, '/����������� �������/�������/�����, ������/����� �� ������������ � ������������ ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(636, @OzonId, '/����������� �������/�������/�����, ������/������-����������� ����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(645, @OzonId, '/����������� �������/�������/�����, ������/������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(647, @OzonId, '/����������� �������/�������/�����, ������/������� �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(663, @OzonId, '/����������� �������/�������/�����, ������/�������������� ����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(584, @OzonId, '/����������� �������/������ � ����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(621, @OzonId, '/����������� �������/����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(634, @OzonId, '/����������� �������/������ � ����/������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(578, @OzonId, '/����������� �������/������� � �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(581, @OzonId, '/����������� �������/������� � �����/������� �������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(629, @OzonId, '/����������� �������/������� � �����/������� �������/������� ������� �������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(646, @OzonId, '/����������� �������/������� � �����/������� �������/�������� � ������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(658, @OzonId, '/����������� �������/������� � �����/������� �������/������� ��� ����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(659, @OzonId, '/����������� �������/������� � �����/������� �������/������� ��� �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(667, @OzonId, '/����������� �������/������� � �����/������� �������/����, �������� �������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(671, @OzonId, '/����������� �������/������� � �����/������� �������/������������������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(582, @OzonId, '/����������� �������/������� � �����/�������, ��������� ��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(601, @OzonId, '/����������� �������/������� � �����/�������, ��������� ��������/�����-����� �������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(637, @OzonId, '/����������� �������/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(638, @OzonId, '/����������� �������/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(643, @OzonId, '/����������� �������/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(656, @OzonId, '/����������� �������/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(657, @OzonId, '/����������� �������/������� � �����/�������, ��������� ��������/��������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(662, @OzonId, '/����������� �������/������� � �����/�������, ��������� ��������/����������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(664, @OzonId, '/����������� �������/������� � �����/�������, ��������� ��������/���� � ����� �������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(672, @OzonId, '/����������� �������/������� � �����/�������, ��������� ��������/����������� �����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(585, @OzonId, '/����������� �������/������� � �����/���� � ����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(606, @OzonId, '/����������� �������/������� � �����/���� � ����/���������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(615, @OzonId, '/����������� �������/������� � �����/���� � ����/������� ���������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(628, @OzonId, '/����������� �������/������� � �����/���� � ����/������������ ����/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(639, @OzonId, '/����������� �������/������� � �����/���� � ����/��������� ���������/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(651, @OzonId, '/����������� �������/������� � �����/���� � ����/���� ��� ���� � �������/', 1, @User, GETDATE())

