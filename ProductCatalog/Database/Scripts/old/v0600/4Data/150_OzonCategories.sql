DECLARE @User nvarchar(50) = 'SCRIPT';
DECLARE @CatId int;
DECLARE @OzonId int = 1;

PRINT 'LEVEL 1'
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	ParentId IS NULL
  AND	[Name] = 'Дом и семья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, NULL, N'Дом и семья', N'/Дом и семья/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	ParentId IS NULL
  AND	[Name] = 'Красота, здоровье, фитнес';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, NULL, N'Красота, здоровье, фитнес', N'/Красота, здоровье, фитнес/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	ParentId IS NULL
  AND	[Name] = 'Подарки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, NULL, N'Подарки', N'/Подарки/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	ParentId IS NULL
  AND	[Name] = 'Техника и связь';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, NULL, N'Техника и связь', N'/Техника и связь/', 1, @User, GETDATE(), 0);
END

PRINT 'LEVEL 2'
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/%'
  AND	[Name] = 'Аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'Аксессуары', N'/Дом и семья/Аксессуары/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/%'
  AND	[Name] = 'Аксессуары и сувениры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 577, N'Аксессуары и сувениры', N'/Подарки/Аксессуары и сувениры/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/%'
  AND	[Name] = 'Бытовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 578, N'Бытовая техника', N'/Техника и связь/Бытовая техника/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/%'
  AND	[Name] = 'Гаджеты, мобильные телефоны';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 578, N'Гаджеты, мобильные телефоны', N'/Техника и связь/Гаджеты, мобильные телефоны/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/%'
  AND	[Name] = 'Книги';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 577, N'Книги', N'/Подарки/Книги/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/%'
  AND	[Name] = 'Музыка и Кино';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 577, N'Музыка и Кино', N'/Подарки/Музыка и Кино/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/%'
  AND	[Name] = 'Софт и игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 578, N'Софт и игры', N'/Техника и связь/Софт и игры/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/%'
  AND	[Name] = 'Техника для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 576, N'Техника для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/%'
  AND	[Name] = 'Товары для автомобилистов';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'Товары для автомобилистов', N'/Дом и семья/Товары для автомобилистов/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/%'
  AND	[Name] = 'Товары для дачи';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'Товары для дачи', N'/Дом и семья/Товары для дачи/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/%'
  AND	[Name] = 'Товары для детей';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'Товары для детей', N'/Дом и семья/Товары для детей/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/%'
  AND	[Name] = 'Товары для дома';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'Товары для дома', N'/Дом и семья/Товары для дома/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/%'
  AND	[Name] = 'Товары для животных';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'Товары для животных', N'/Дом и семья/Товары для животных/', 1, @User, GETDATE(), 0);
END

SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/%'
  AND	[Name] = 'Товары для спорта';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 575, N'Товары для спорта', N'/Дом и семья/Товары для спорта/', 1, @User, GETDATE(), 0);
END

PRINT 'LEVEL 3'
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'GPS навигаторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 587, N'GPS навигаторы', N'/Дом и семья/Товары для автомобилистов/GPS навигаторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'GPS навигаторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 601, N'GPS навигаторы', N'/Дом и семья/Товары для автомобилистов/GPS навигаторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'GPS навигаторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 615, N'GPS навигаторы', N'/Дом и семья/Товары для автомобилистов/GPS навигаторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'GPS навигаторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 629, N'GPS навигаторы', N'/Дом и семья/Товары для автомобилистов/GPS навигаторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'GPS навигаторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 643, N'GPS навигаторы', N'/Дом и семья/Товары для автомобилистов/GPS навигаторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'GPS навигаторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 657, N'GPS навигаторы', N'/Дом и семья/Товары для автомобилистов/GPS навигаторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'GPS навигаторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 671, N'GPS навигаторы', N'/Дом и семья/Товары для автомобилистов/GPS навигаторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'GPS навигаторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 685, N'GPS навигаторы', N'/Дом и семья/Товары для автомобилистов/GPS навигаторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'GPS навигаторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 699, N'GPS навигаторы', N'/Дом и семья/Товары для автомобилистов/GPS навигаторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'LEGO', N'/Дом и семья/Товары для детей/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'LEGO', N'/Дом и семья/Товары для детей/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'LEGO', N'/Дом и семья/Товары для детей/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'LEGO', N'/Дом и семья/Товары для детей/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'LEGO', N'/Дом и семья/Товары для детей/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'LEGO', N'/Дом и семья/Товары для детей/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'LEGO', N'/Дом и семья/Товары для детей/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'LEGO', N'/Дом и семья/Товары для детей/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'LEGO';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'LEGO', N'/Дом и семья/Товары для детей/LEGO/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автоаксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 587, N'Автоаксессуары', N'/Дом и семья/Товары для автомобилистов/Автоаксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автоаксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 601, N'Автоаксессуары', N'/Дом и семья/Товары для автомобилистов/Автоаксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автоаксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 615, N'Автоаксессуары', N'/Дом и семья/Товары для автомобилистов/Автоаксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автоаксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 629, N'Автоаксессуары', N'/Дом и семья/Товары для автомобилистов/Автоаксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автоаксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 643, N'Автоаксессуары', N'/Дом и семья/Товары для автомобилистов/Автоаксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автоаксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 657, N'Автоаксессуары', N'/Дом и семья/Товары для автомобилистов/Автоаксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автоаксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 671, N'Автоаксессуары', N'/Дом и семья/Товары для автомобилистов/Автоаксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автоаксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 685, N'Автоаксессуары', N'/Дом и семья/Товары для автомобилистов/Автоаксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автоаксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 699, N'Автоаксессуары', N'/Дом и семья/Товары для автомобилистов/Автоаксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автокружки, термосы, холодильники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 587, N'Автокружки, термосы, холодильники', N'/Дом и семья/Товары для автомобилистов/Автокружки, термосы, холодильники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автокружки, термосы, холодильники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 601, N'Автокружки, термосы, холодильники', N'/Дом и семья/Товары для автомобилистов/Автокружки, термосы, холодильники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автокружки, термосы, холодильники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 615, N'Автокружки, термосы, холодильники', N'/Дом и семья/Товары для автомобилистов/Автокружки, термосы, холодильники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автокружки, термосы, холодильники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 629, N'Автокружки, термосы, холодильники', N'/Дом и семья/Товары для автомобилистов/Автокружки, термосы, холодильники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автокружки, термосы, холодильники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 643, N'Автокружки, термосы, холодильники', N'/Дом и семья/Товары для автомобилистов/Автокружки, термосы, холодильники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автокружки, термосы, холодильники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 657, N'Автокружки, термосы, холодильники', N'/Дом и семья/Товары для автомобилистов/Автокружки, термосы, холодильники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автокружки, термосы, холодильники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 671, N'Автокружки, термосы, холодильники', N'/Дом и семья/Товары для автомобилистов/Автокружки, термосы, холодильники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автокружки, термосы, холодильники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 685, N'Автокружки, термосы, холодильники', N'/Дом и семья/Товары для автомобилистов/Автокружки, термосы, холодильники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Автокружки, термосы, холодильники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 699, N'Автокружки, термосы, холодильники', N'/Дом и семья/Товары для автомобилистов/Автокружки, термосы, холодильники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Авторские работы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 580, N'Авторские работы', N'/Подарки/Аксессуары и сувениры/Авторские работы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Авторские работы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 594, N'Авторские работы', N'/Подарки/Аксессуары и сувениры/Авторские работы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Авторские работы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 608, N'Авторские работы', N'/Подарки/Аксессуары и сувениры/Авторские работы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Авторские работы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 622, N'Авторские работы', N'/Подарки/Аксессуары и сувениры/Авторские работы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Авторские работы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 636, N'Авторские работы', N'/Подарки/Аксессуары и сувениры/Авторские работы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Авторские работы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 650, N'Авторские работы', N'/Подарки/Аксессуары и сувениры/Авторские работы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Авторские работы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 664, N'Авторские работы', N'/Подарки/Аксессуары и сувениры/Авторские работы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Авторские работы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 678, N'Авторские работы', N'/Подарки/Аксессуары и сувениры/Авторские работы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Авторские работы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 692, N'Авторские работы', N'/Подарки/Аксессуары и сувениры/Авторские работы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 591, N'Аксессуары', N'/Дом и семья/Товары для животных/Аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 605, N'Аксессуары', N'/Дом и семья/Товары для животных/Аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 619, N'Аксессуары', N'/Дом и семья/Товары для животных/Аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 633, N'Аксессуары', N'/Дом и семья/Товары для животных/Аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 647, N'Аксессуары', N'/Дом и семья/Товары для животных/Аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 661, N'Аксессуары', N'/Дом и семья/Товары для животных/Аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 675, N'Аксессуары', N'/Дом и семья/Товары для животных/Аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 689, N'Аксессуары', N'/Дом и семья/Товары для животных/Аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 703, N'Аксессуары', N'/Дом и семья/Товары для животных/Аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Аксессуары для интерьера';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 579, N'Аксессуары для интерьера', N'/Дом и семья/Аксессуары/Аксессуары для интерьера/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Аксессуары для интерьера';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 593, N'Аксессуары для интерьера', N'/Дом и семья/Аксессуары/Аксессуары для интерьера/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Аксессуары для интерьера';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 607, N'Аксессуары для интерьера', N'/Дом и семья/Аксессуары/Аксессуары для интерьера/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Аксессуары для интерьера';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 621, N'Аксессуары для интерьера', N'/Дом и семья/Аксессуары/Аксессуары для интерьера/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Аксессуары для интерьера';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 635, N'Аксессуары для интерьера', N'/Дом и семья/Аксессуары/Аксессуары для интерьера/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Аксессуары для интерьера';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 649, N'Аксессуары для интерьера', N'/Дом и семья/Аксессуары/Аксессуары для интерьера/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Аксессуары для интерьера';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 663, N'Аксессуары для интерьера', N'/Дом и семья/Аксессуары/Аксессуары для интерьера/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Аксессуары для интерьера';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 677, N'Аксессуары для интерьера', N'/Дом и семья/Аксессуары/Аксессуары для интерьера/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Аксессуары для интерьера';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 691, N'Аксессуары для интерьера', N'/Дом и семья/Аксессуары/Аксессуары для интерьера/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Активный отдых и путешествие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 592, N'Активный отдых и путешествие', N'/Дом и семья/Товары для спорта/Активный отдых и путешествие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Активный отдых и путешествие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 606, N'Активный отдых и путешествие', N'/Дом и семья/Товары для спорта/Активный отдых и путешествие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Активный отдых и путешествие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 620, N'Активный отдых и путешествие', N'/Дом и семья/Товары для спорта/Активный отдых и путешествие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Активный отдых и путешествие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 634, N'Активный отдых и путешествие', N'/Дом и семья/Товары для спорта/Активный отдых и путешествие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Активный отдых и путешествие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 648, N'Активный отдых и путешествие', N'/Дом и семья/Товары для спорта/Активный отдых и путешествие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Активный отдых и путешествие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 662, N'Активный отдых и путешествие', N'/Дом и семья/Товары для спорта/Активный отдых и путешествие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Активный отдых и путешествие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 676, N'Активный отдых и путешествие', N'/Дом и семья/Товары для спорта/Активный отдых и путешествие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Активный отдых и путешествие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 690, N'Активный отдых и путешествие', N'/Дом и семья/Товары для спорта/Активный отдых и путешествие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Активный отдых и путешествие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 704, N'Активный отдых и путешествие', N'/Дом и семья/Товары для спорта/Активный отдых и путешествие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Аудио-видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'Аудио-видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Аудио-видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Аудио-видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'Аудио-видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Аудио-видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Аудио-видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'Аудио-видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Аудио-видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Аудио-видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'Аудио-видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Аудио-видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Аудио-видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'Аудио-видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Аудио-видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Аудио-видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'Аудио-видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Аудио-видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Аудио-видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'Аудио-видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Аудио-видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Аудио-видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'Аудио-видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Аудио-видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Аудио-видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'Аудио-видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Аудио-видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Безопасность';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'Безопасность', N'/Дом и семья/Товары для детей/Безопасность/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Безопасность';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'Безопасность', N'/Дом и семья/Товары для детей/Безопасность/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Безопасность';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'Безопасность', N'/Дом и семья/Товары для детей/Безопасность/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Безопасность';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'Безопасность', N'/Дом и семья/Товары для детей/Безопасность/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Безопасность';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'Безопасность', N'/Дом и семья/Товары для детей/Безопасность/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Безопасность';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'Безопасность', N'/Дом и семья/Товары для детей/Безопасность/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Безопасность';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'Безопасность', N'/Дом и семья/Товары для детей/Безопасность/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Безопасность';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'Безопасность', N'/Дом и семья/Товары для детей/Безопасность/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Безопасность';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'Безопасность', N'/Дом и семья/Товары для детей/Безопасность/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Бизнес-литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Бизнес-литература', N'/Подарки/Книги/Бизнес-литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Бизнес-литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Бизнес-литература', N'/Подарки/Книги/Бизнес-литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Бизнес-литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Бизнес-литература', N'/Подарки/Книги/Бизнес-литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Бизнес-литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Бизнес-литература', N'/Подарки/Книги/Бизнес-литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Бизнес-литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Бизнес-литература', N'/Подарки/Книги/Бизнес-литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Бизнес-литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Бизнес-литература', N'/Подарки/Книги/Бизнес-литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Бизнес-литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Бизнес-литература', N'/Подарки/Книги/Бизнес-литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Бизнес-литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Бизнес-литература', N'/Подарки/Книги/Бизнес-литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Бизнес-литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Бизнес-литература', N'/Подарки/Книги/Бизнес-литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Биографическая и мемуарная литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Биографическая и мемуарная литература', N'/Подарки/Книги/Биографическая и мемуарная литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Биографическая и мемуарная литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Биографическая и мемуарная литература', N'/Подарки/Книги/Биографическая и мемуарная литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Биографическая и мемуарная литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Биографическая и мемуарная литература', N'/Подарки/Книги/Биографическая и мемуарная литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Биографическая и мемуарная литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Биографическая и мемуарная литература', N'/Подарки/Книги/Биографическая и мемуарная литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Биографическая и мемуарная литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Биографическая и мемуарная литература', N'/Подарки/Книги/Биографическая и мемуарная литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Биографическая и мемуарная литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Биографическая и мемуарная литература', N'/Подарки/Книги/Биографическая и мемуарная литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Биографическая и мемуарная литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Биографическая и мемуарная литература', N'/Подарки/Книги/Биографическая и мемуарная литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Биографическая и мемуарная литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Биографическая и мемуарная литература', N'/Подарки/Книги/Биографическая и мемуарная литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Биографическая и мемуарная литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Биографическая и мемуарная литература', N'/Подарки/Книги/Биографическая и мемуарная литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Ванна, баня, сауна';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'Ванна, баня, сауна', N'/Дом и семья/Товары для дома/Ванна, баня, сауна/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Ванна, баня, сауна';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'Ванна, баня, сауна', N'/Дом и семья/Товары для дома/Ванна, баня, сауна/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Ванна, баня, сауна';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'Ванна, баня, сауна', N'/Дом и семья/Товары для дома/Ванна, баня, сауна/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Ванна, баня, сауна';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'Ванна, баня, сауна', N'/Дом и семья/Товары для дома/Ванна, баня, сауна/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Ванна, баня, сауна';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'Ванна, баня, сауна', N'/Дом и семья/Товары для дома/Ванна, баня, сауна/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Ванна, баня, сауна';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'Ванна, баня, сауна', N'/Дом и семья/Товары для дома/Ванна, баня, сауна/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Ванна, баня, сауна';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'Ванна, баня, сауна', N'/Дом и семья/Товары для дома/Ванна, баня, сауна/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Ванна, баня, сауна';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'Ванна, баня, сауна', N'/Дом и семья/Товары для дома/Ванна, баня, сауна/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Ванна, баня, сауна';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'Ванна, баня, сауна', N'/Дом и семья/Товары для дома/Ванна, баня, сауна/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Видеоигры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 585, N'Видеоигры', N'/Техника и связь/Софт и игры/Видеоигры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Видеоигры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 599, N'Видеоигры', N'/Техника и связь/Софт и игры/Видеоигры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Видеоигры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 613, N'Видеоигры', N'/Техника и связь/Софт и игры/Видеоигры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Видеоигры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 627, N'Видеоигры', N'/Техника и связь/Софт и игры/Видеоигры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Видеоигры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 641, N'Видеоигры', N'/Техника и связь/Софт и игры/Видеоигры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Видеоигры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 655, N'Видеоигры', N'/Техника и связь/Софт и игры/Видеоигры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Видеоигры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 669, N'Видеоигры', N'/Техника и связь/Софт и игры/Видеоигры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Видеоигры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 683, N'Видеоигры', N'/Техника и связь/Софт и игры/Видеоигры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Видеоигры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 697, N'Видеоигры', N'/Техника и связь/Софт и игры/Видеоигры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Все для ремонта';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'Все для ремонта', N'/Дом и семья/Товары для дома/Все для ремонта/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Все для ремонта';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'Все для ремонта', N'/Дом и семья/Товары для дома/Все для ремонта/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Все для ремонта';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'Все для ремонта', N'/Дом и семья/Товары для дома/Все для ремонта/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Все для ремонта';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'Все для ремонта', N'/Дом и семья/Товары для дома/Все для ремонта/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Все для ремонта';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'Все для ремонта', N'/Дом и семья/Товары для дома/Все для ремонта/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Все для ремонта';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'Все для ремонта', N'/Дом и семья/Товары для дома/Все для ремонта/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Все для ремонта';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'Все для ремонта', N'/Дом и семья/Товары для дома/Все для ремонта/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Все для ремонта';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'Все для ремонта', N'/Дом и семья/Товары для дома/Все для ремонта/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Все для ремонта';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'Все для ремонта', N'/Дом и семья/Товары для дома/Все для ремонта/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детская косметика и гигиена';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'Детская косметика и гигиена', N'/Дом и семья/Товары для детей/Детская косметика и гигиена/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детская косметика и гигиена';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'Детская косметика и гигиена', N'/Дом и семья/Товары для детей/Детская косметика и гигиена/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детская косметика и гигиена';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'Детская косметика и гигиена', N'/Дом и семья/Товары для детей/Детская косметика и гигиена/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детская косметика и гигиена';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'Детская косметика и гигиена', N'/Дом и семья/Товары для детей/Детская косметика и гигиена/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детская косметика и гигиена';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'Детская косметика и гигиена', N'/Дом и семья/Товары для детей/Детская косметика и гигиена/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детская косметика и гигиена';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'Детская косметика и гигиена', N'/Дом и семья/Товары для детей/Детская косметика и гигиена/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детская косметика и гигиена';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'Детская косметика и гигиена', N'/Дом и семья/Товары для детей/Детская косметика и гигиена/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детская косметика и гигиена';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'Детская косметика и гигиена', N'/Дом и семья/Товары для детей/Детская косметика и гигиена/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детская косметика и гигиена';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'Детская косметика и гигиена', N'/Дом и семья/Товары для детей/Детская косметика и гигиена/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Детская литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Детская литература', N'/Подарки/Книги/Детская литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Детская литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Детская литература', N'/Подарки/Книги/Детская литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Детская литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Детская литература', N'/Подарки/Книги/Детская литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Детская литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Детская литература', N'/Подарки/Книги/Детская литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Детская литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Детская литература', N'/Подарки/Книги/Детская литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Детская литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Детская литература', N'/Подарки/Книги/Детская литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Детская литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Детская литература', N'/Подарки/Книги/Детская литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Детская литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Детская литература', N'/Подарки/Книги/Детская литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Детская литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Детская литература', N'/Подарки/Книги/Детская литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детсткий спорт, активный отдых';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'Детсткий спорт, активный отдых', N'/Дом и семья/Товары для детей/Детсткий спорт, активный отдых/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детсткий спорт, активный отдых';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'Детсткий спорт, активный отдых', N'/Дом и семья/Товары для детей/Детсткий спорт, активный отдых/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детсткий спорт, активный отдых';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'Детсткий спорт, активный отдых', N'/Дом и семья/Товары для детей/Детсткий спорт, активный отдых/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детсткий спорт, активный отдых';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'Детсткий спорт, активный отдых', N'/Дом и семья/Товары для детей/Детсткий спорт, активный отдых/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детсткий спорт, активный отдых';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'Детсткий спорт, активный отдых', N'/Дом и семья/Товары для детей/Детсткий спорт, активный отдых/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детсткий спорт, активный отдых';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'Детсткий спорт, активный отдых', N'/Дом и семья/Товары для детей/Детсткий спорт, активный отдых/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детсткий спорт, активный отдых';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'Детсткий спорт, активный отдых', N'/Дом и семья/Товары для детей/Детсткий спорт, активный отдых/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детсткий спорт, активный отдых';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'Детсткий спорт, активный отдых', N'/Дом и семья/Товары для детей/Детсткий спорт, активный отдых/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Детсткий спорт, активный отдых';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'Детсткий спорт, активный отдых', N'/Дом и семья/Товары для детей/Детсткий спорт, активный отдых/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'Для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'Для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'Для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'Для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'Для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'Для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'Для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'Для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'Для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Домашний текстиль';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'Домашний текстиль', N'/Дом и семья/Товары для дома/Домашний текстиль/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Домашний текстиль';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'Домашний текстиль', N'/Дом и семья/Товары для дома/Домашний текстиль/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Домашний текстиль';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'Домашний текстиль', N'/Дом и семья/Товары для дома/Домашний текстиль/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Домашний текстиль';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'Домашний текстиль', N'/Дом и семья/Товары для дома/Домашний текстиль/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Домашний текстиль';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'Домашний текстиль', N'/Дом и семья/Товары для дома/Домашний текстиль/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Домашний текстиль';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'Домашний текстиль', N'/Дом и семья/Товары для дома/Домашний текстиль/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Домашний текстиль';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'Домашний текстиль', N'/Дом и семья/Товары для дома/Домашний текстиль/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Домашний текстиль';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'Домашний текстиль', N'/Дом и семья/Товары для дома/Домашний текстиль/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Домашний текстиль';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'Домашний текстиль', N'/Дом и семья/Товары для дома/Домашний текстиль/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Дорожные фены';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'Дорожные фены', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Дорожные фены/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Дорожные фены';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'Дорожные фены', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Дорожные фены/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Дорожные фены';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'Дорожные фены', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Дорожные фены/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Дорожные фены';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'Дорожные фены', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Дорожные фены/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Дорожные фены';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'Дорожные фены', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Дорожные фены/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Дорожные фены';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'Дорожные фены', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Дорожные фены/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Дорожные фены';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'Дорожные фены', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Дорожные фены/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Дорожные фены';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'Дорожные фены', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Дорожные фены/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Дорожные фены';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'Дорожные фены', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Дорожные фены/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Зарубежная проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Зарубежная проза', N'/Подарки/Книги/Зарубежная проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Зарубежная проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Зарубежная проза', N'/Подарки/Книги/Зарубежная проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Зарубежная проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Зарубежная проза', N'/Подарки/Книги/Зарубежная проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Зарубежная проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Зарубежная проза', N'/Подарки/Книги/Зарубежная проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Зарубежная проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Зарубежная проза', N'/Подарки/Книги/Зарубежная проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Зарубежная проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Зарубежная проза', N'/Подарки/Книги/Зарубежная проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Зарубежная проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Зарубежная проза', N'/Подарки/Книги/Зарубежная проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Зарубежная проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Зарубежная проза', N'/Подарки/Книги/Зарубежная проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Зарубежная проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Зарубежная проза', N'/Подарки/Книги/Зарубежная проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Игровые приставки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 585, N'Игровые приставки', N'/Техника и связь/Софт и игры/Игровые приставки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Игровые приставки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 599, N'Игровые приставки', N'/Техника и связь/Софт и игры/Игровые приставки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Игровые приставки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 613, N'Игровые приставки', N'/Техника и связь/Софт и игры/Игровые приставки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Игровые приставки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 627, N'Игровые приставки', N'/Техника и связь/Софт и игры/Игровые приставки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Игровые приставки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 641, N'Игровые приставки', N'/Техника и связь/Софт и игры/Игровые приставки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Игровые приставки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 655, N'Игровые приставки', N'/Техника и связь/Софт и игры/Игровые приставки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Игровые приставки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 669, N'Игровые приставки', N'/Техника и связь/Софт и игры/Игровые приставки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Игровые приставки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 683, N'Игровые приставки', N'/Техника и связь/Софт и игры/Игровые приставки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Игровые приставки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 697, N'Игровые приставки', N'/Техника и связь/Софт и игры/Игровые приставки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Игрушки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'Игрушки', N'/Дом и семья/Товары для детей/Игрушки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Игрушки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'Игрушки', N'/Дом и семья/Товары для детей/Игрушки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Игрушки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'Игрушки', N'/Дом и семья/Товары для детей/Игрушки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Игрушки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'Игрушки', N'/Дом и семья/Товары для детей/Игрушки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Игрушки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'Игрушки', N'/Дом и семья/Товары для детей/Игрушки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Игрушки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'Игрушки', N'/Дом и семья/Товары для детей/Игрушки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Игрушки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'Игрушки', N'/Дом и семья/Товары для детей/Игрушки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Игрушки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'Игрушки', N'/Дом и семья/Товары для детей/Игрушки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Игрушки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'Игрушки', N'/Дом и семья/Товары для детей/Игрушки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Инструменты для авто';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 587, N'Инструменты для авто', N'/Дом и семья/Товары для автомобилистов/Инструменты для авто/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Инструменты для авто';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 601, N'Инструменты для авто', N'/Дом и семья/Товары для автомобилистов/Инструменты для авто/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Инструменты для авто';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 615, N'Инструменты для авто', N'/Дом и семья/Товары для автомобилистов/Инструменты для авто/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Инструменты для авто';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 629, N'Инструменты для авто', N'/Дом и семья/Товары для автомобилистов/Инструменты для авто/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Инструменты для авто';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 643, N'Инструменты для авто', N'/Дом и семья/Товары для автомобилистов/Инструменты для авто/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Инструменты для авто';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 657, N'Инструменты для авто', N'/Дом и семья/Товары для автомобилистов/Инструменты для авто/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Инструменты для авто';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 671, N'Инструменты для авто', N'/Дом и семья/Товары для автомобилистов/Инструменты для авто/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Инструменты для авто';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 685, N'Инструменты для авто', N'/Дом и семья/Товары для автомобилистов/Инструменты для авто/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Инструменты для авто';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 699, N'Инструменты для авто', N'/Дом и семья/Товары для автомобилистов/Инструменты для авто/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Интерьер';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'Интерьер', N'/Дом и семья/Товары для дома/Интерьер/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Интерьер';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'Интерьер', N'/Дом и семья/Товары для дома/Интерьер/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Интерьер';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'Интерьер', N'/Дом и семья/Товары для дома/Интерьер/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Интерьер';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'Интерьер', N'/Дом и семья/Товары для дома/Интерьер/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Интерьер';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'Интерьер', N'/Дом и семья/Товары для дома/Интерьер/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Интерьер';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'Интерьер', N'/Дом и семья/Товары для дома/Интерьер/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Интерьер';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'Интерьер', N'/Дом и семья/Товары для дома/Интерьер/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Интерьер';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'Интерьер', N'/Дом и семья/Товары для дома/Интерьер/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Интерьер';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'Интерьер', N'/Дом и семья/Товары для дома/Интерьер/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Исторические романы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Исторические романы', N'/Подарки/Книги/Исторические романы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Исторические романы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Исторические романы', N'/Подарки/Книги/Исторические романы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Исторические романы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Исторические романы', N'/Подарки/Книги/Исторические романы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Исторические романы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Исторические романы', N'/Подарки/Книги/Исторические романы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Исторические романы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Исторические романы', N'/Подарки/Книги/Исторические романы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Исторические романы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Исторические романы', N'/Подарки/Книги/Исторические романы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Исторические романы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Исторические романы', N'/Подарки/Книги/Исторические романы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Исторические романы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Исторические романы', N'/Подарки/Книги/Исторические романы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Исторические романы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Исторические романы', N'/Подарки/Книги/Исторические романы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Канцелярские товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 580, N'Канцелярские товары', N'/Подарки/Аксессуары и сувениры/Канцелярские товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Канцелярские товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 594, N'Канцелярские товары', N'/Подарки/Аксессуары и сувениры/Канцелярские товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Канцелярские товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 608, N'Канцелярские товары', N'/Подарки/Аксессуары и сувениры/Канцелярские товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Канцелярские товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 622, N'Канцелярские товары', N'/Подарки/Аксессуары и сувениры/Канцелярские товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Канцелярские товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 636, N'Канцелярские товары', N'/Подарки/Аксессуары и сувениры/Канцелярские товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Канцелярские товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 650, N'Канцелярские товары', N'/Подарки/Аксессуары и сувениры/Канцелярские товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Канцелярские товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 664, N'Канцелярские товары', N'/Подарки/Аксессуары и сувениры/Канцелярские товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Канцелярские товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 678, N'Канцелярские товары', N'/Подарки/Аксессуары и сувениры/Канцелярские товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Канцелярские товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 692, N'Канцелярские товары', N'/Подарки/Аксессуары и сувениры/Канцелярские товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Кино';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 584, N'Кино', N'/Подарки/Музыка и Кино/Кино/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Кино';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 598, N'Кино', N'/Подарки/Музыка и Кино/Кино/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Кино';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 612, N'Кино', N'/Подарки/Музыка и Кино/Кино/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Кино';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 626, N'Кино', N'/Подарки/Музыка и Кино/Кино/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Кино';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 640, N'Кино', N'/Подарки/Музыка и Кино/Кино/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Кино';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 654, N'Кино', N'/Подарки/Музыка и Кино/Кино/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Кино';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 668, N'Кино', N'/Подарки/Музыка и Кино/Кино/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Кино';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 682, N'Кино', N'/Подарки/Музыка и Кино/Кино/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Кино';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 696, N'Кино', N'/Подарки/Музыка и Кино/Кино/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для дома и досуга';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Книги для дома и досуга', N'/Подарки/Книги/Книги для дома и досуга/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для дома и досуга';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Книги для дома и досуга', N'/Подарки/Книги/Книги для дома и досуга/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для дома и досуга';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Книги для дома и досуга', N'/Подарки/Книги/Книги для дома и досуга/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для дома и досуга';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Книги для дома и досуга', N'/Подарки/Книги/Книги для дома и досуга/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для дома и досуга';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Книги для дома и досуга', N'/Подарки/Книги/Книги для дома и досуга/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для дома и досуга';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Книги для дома и досуга', N'/Подарки/Книги/Книги для дома и досуга/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для дома и досуга';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Книги для дома и досуга', N'/Подарки/Книги/Книги для дома и досуга/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для дома и досуга';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Книги для дома и досуга', N'/Подарки/Книги/Книги для дома и досуга/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для дома и досуга';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Книги для дома и досуга', N'/Подарки/Книги/Книги для дома и досуга/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для родителей';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Книги для родителей', N'/Подарки/Книги/Книги для родителей/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для родителей';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Книги для родителей', N'/Подарки/Книги/Книги для родителей/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для родителей';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Книги для родителей', N'/Подарки/Книги/Книги для родителей/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для родителей';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Книги для родителей', N'/Подарки/Книги/Книги для родителей/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для родителей';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Книги для родителей', N'/Подарки/Книги/Книги для родителей/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для родителей';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Книги для родителей', N'/Подарки/Книги/Книги для родителей/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для родителей';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Книги для родителей', N'/Подарки/Книги/Книги для родителей/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для родителей';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Книги для родителей', N'/Подарки/Книги/Книги для родителей/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги для родителей';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Книги для родителей', N'/Подарки/Книги/Книги для родителей/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги о путешествиях, туризме и спорте';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Книги о путешествиях, туризме и спорте', N'/Подарки/Книги/Книги о путешествиях, туризме и спорте/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги о путешествиях, туризме и спорте';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Книги о путешествиях, туризме и спорте', N'/Подарки/Книги/Книги о путешествиях, туризме и спорте/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги о путешествиях, туризме и спорте';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Книги о путешествиях, туризме и спорте', N'/Подарки/Книги/Книги о путешествиях, туризме и спорте/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги о путешествиях, туризме и спорте';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Книги о путешествиях, туризме и спорте', N'/Подарки/Книги/Книги о путешествиях, туризме и спорте/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги о путешествиях, туризме и спорте';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Книги о путешествиях, туризме и спорте', N'/Подарки/Книги/Книги о путешествиях, туризме и спорте/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги о путешествиях, туризме и спорте';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Книги о путешествиях, туризме и спорте', N'/Подарки/Книги/Книги о путешествиях, туризме и спорте/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги о путешествиях, туризме и спорте';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Книги о путешествиях, туризме и спорте', N'/Подарки/Книги/Книги о путешествиях, туризме и спорте/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги о путешествиях, туризме и спорте';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Книги о путешествиях, туризме и спорте', N'/Подарки/Книги/Книги о путешествиях, туризме и спорте/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги о путешествиях, туризме и спорте';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Книги о путешествиях, туризме и спорте', N'/Подарки/Книги/Книги о путешествиях, туризме и спорте/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по изучению иностранных языков';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Книги по изучению иностранных языков', N'/Подарки/Книги/Книги по изучению иностранных языков/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по изучению иностранных языков';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Книги по изучению иностранных языков', N'/Подарки/Книги/Книги по изучению иностранных языков/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по изучению иностранных языков';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Книги по изучению иностранных языков', N'/Подарки/Книги/Книги по изучению иностранных языков/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по изучению иностранных языков';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Книги по изучению иностранных языков', N'/Подарки/Книги/Книги по изучению иностранных языков/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по изучению иностранных языков';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Книги по изучению иностранных языков', N'/Подарки/Книги/Книги по изучению иностранных языков/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по изучению иностранных языков';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Книги по изучению иностранных языков', N'/Подарки/Книги/Книги по изучению иностранных языков/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по изучению иностранных языков';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Книги по изучению иностранных языков', N'/Подарки/Книги/Книги по изучению иностранных языков/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по изучению иностранных языков';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Книги по изучению иностранных языков', N'/Подарки/Книги/Книги по изучению иностранных языков/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по изучению иностранных языков';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Книги по изучению иностранных языков', N'/Подарки/Книги/Книги по изучению иностранных языков/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по искусству и культуре';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Книги по искусству и культуре', N'/Подарки/Книги/Книги по искусству и культуре/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по искусству и культуре';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Книги по искусству и культуре', N'/Подарки/Книги/Книги по искусству и культуре/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по искусству и культуре';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Книги по искусству и культуре', N'/Подарки/Книги/Книги по искусству и культуре/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по искусству и культуре';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Книги по искусству и культуре', N'/Подарки/Книги/Книги по искусству и культуре/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по искусству и культуре';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Книги по искусству и культуре', N'/Подарки/Книги/Книги по искусству и культуре/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по искусству и культуре';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Книги по искусству и культуре', N'/Подарки/Книги/Книги по искусству и культуре/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по искусству и культуре';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Книги по искусству и культуре', N'/Подарки/Книги/Книги по искусству и культуре/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по искусству и культуре';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Книги по искусству и культуре', N'/Подарки/Книги/Книги по искусству и культуре/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по искусству и культуре';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Книги по искусству и культуре', N'/Подарки/Книги/Книги по искусству и культуре/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по общественным и гуманитарным наукам';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Книги по общественным и гуманитарным наукам', N'/Подарки/Книги/Книги по общественным и гуманитарным наукам/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по общественным и гуманитарным наукам';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Книги по общественным и гуманитарным наукам', N'/Подарки/Книги/Книги по общественным и гуманитарным наукам/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по общественным и гуманитарным наукам';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Книги по общественным и гуманитарным наукам', N'/Подарки/Книги/Книги по общественным и гуманитарным наукам/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по общественным и гуманитарным наукам';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Книги по общественным и гуманитарным наукам', N'/Подарки/Книги/Книги по общественным и гуманитарным наукам/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по общественным и гуманитарным наукам';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Книги по общественным и гуманитарным наукам', N'/Подарки/Книги/Книги по общественным и гуманитарным наукам/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по общественным и гуманитарным наукам';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Книги по общественным и гуманитарным наукам', N'/Подарки/Книги/Книги по общественным и гуманитарным наукам/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по общественным и гуманитарным наукам';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Книги по общественным и гуманитарным наукам', N'/Подарки/Книги/Книги по общественным и гуманитарным наукам/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по общественным и гуманитарным наукам';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Книги по общественным и гуманитарным наукам', N'/Подарки/Книги/Книги по общественным и гуманитарным наукам/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Книги по общественным и гуманитарным наукам';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Книги по общественным и гуманитарным наукам', N'/Подарки/Книги/Книги по общественным и гуманитарным наукам/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Компьютерные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 585, N'Компьютерные игры', N'/Техника и связь/Софт и игры/Компьютерные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Компьютерные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 599, N'Компьютерные игры', N'/Техника и связь/Софт и игры/Компьютерные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Компьютерные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 613, N'Компьютерные игры', N'/Техника и связь/Софт и игры/Компьютерные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Компьютерные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 627, N'Компьютерные игры', N'/Техника и связь/Софт и игры/Компьютерные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Компьютерные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 641, N'Компьютерные игры', N'/Техника и связь/Софт и игры/Компьютерные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Компьютерные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 655, N'Компьютерные игры', N'/Техника и связь/Софт и игры/Компьютерные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Компьютерные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 669, N'Компьютерные игры', N'/Техника и связь/Софт и игры/Компьютерные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Компьютерные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 683, N'Компьютерные игры', N'/Техника и связь/Софт и игры/Компьютерные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Компьютерные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 697, N'Компьютерные игры', N'/Техника и связь/Софт и игры/Компьютерные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Крупная бытовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'Крупная бытовая техника', N'/Техника и связь/Бытовая техника/Крупная бытовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Крупная бытовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'Крупная бытовая техника', N'/Техника и связь/Бытовая техника/Крупная бытовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Крупная бытовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'Крупная бытовая техника', N'/Техника и связь/Бытовая техника/Крупная бытовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Крупная бытовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'Крупная бытовая техника', N'/Техника и связь/Бытовая техника/Крупная бытовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Крупная бытовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'Крупная бытовая техника', N'/Техника и связь/Бытовая техника/Крупная бытовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Крупная бытовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'Крупная бытовая техника', N'/Техника и связь/Бытовая техника/Крупная бытовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Крупная бытовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'Крупная бытовая техника', N'/Техника и связь/Бытовая техника/Крупная бытовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Крупная бытовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'Крупная бытовая техника', N'/Техника и связь/Бытовая техника/Крупная бытовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Крупная бытовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'Крупная бытовая техника', N'/Техника и связь/Бытовая техника/Крупная бытовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Кухня';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'Кухня', N'/Дом и семья/Товары для дома/Кухня/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Кухня';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'Кухня', N'/Дом и семья/Товары для дома/Кухня/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Кухня';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'Кухня', N'/Дом и семья/Товары для дома/Кухня/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Кухня';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'Кухня', N'/Дом и семья/Товары для дома/Кухня/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Кухня';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'Кухня', N'/Дом и семья/Товары для дома/Кухня/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Кухня';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'Кухня', N'/Дом и семья/Товары для дома/Кухня/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Кухня';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'Кухня', N'/Дом и семья/Товары для дома/Кухня/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Кухня';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'Кухня', N'/Дом и семья/Товары для дома/Кухня/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Кухня';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'Кухня', N'/Дом и семья/Товары для дома/Кухня/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Ландшафт и садовый декор';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 588, N'Ландшафт и садовый декор', N'/Дом и семья/Товары для дачи/Ландшафт и садовый декор/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Ландшафт и садовый декор';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 602, N'Ландшафт и садовый декор', N'/Дом и семья/Товары для дачи/Ландшафт и садовый декор/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Ландшафт и садовый декор';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 616, N'Ландшафт и садовый декор', N'/Дом и семья/Товары для дачи/Ландшафт и садовый декор/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Ландшафт и садовый декор';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 630, N'Ландшафт и садовый декор', N'/Дом и семья/Товары для дачи/Ландшафт и садовый декор/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Ландшафт и садовый декор';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 644, N'Ландшафт и садовый декор', N'/Дом и семья/Товары для дачи/Ландшафт и садовый декор/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Ландшафт и садовый декор';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 658, N'Ландшафт и садовый декор', N'/Дом и семья/Товары для дачи/Ландшафт и садовый декор/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Ландшафт и садовый декор';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 672, N'Ландшафт и садовый декор', N'/Дом и семья/Товары для дачи/Ландшафт и садовый декор/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Ландшафт и садовый декор';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 686, N'Ландшафт и садовый декор', N'/Дом и семья/Товары для дачи/Ландшафт и садовый декор/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Ландшафт и садовый декор';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 700, N'Ландшафт и садовый декор', N'/Дом и семья/Товары для дачи/Ландшафт и садовый декор/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Медицинская техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'Медицинская техника', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Медицинская техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Медицинская техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'Медицинская техника', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Медицинская техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Медицинская техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'Медицинская техника', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Медицинская техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Медицинская техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'Медицинская техника', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Медицинская техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Медицинская техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'Медицинская техника', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Медицинская техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Медицинская техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'Медицинская техника', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Медицинская техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Медицинская техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'Медицинская техника', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Медицинская техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Медицинская техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'Медицинская техника', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Медицинская техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Медицинская техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'Медицинская техника', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Медицинская техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Модные аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 579, N'Модные аксессуары', N'/Дом и семья/Аксессуары/Модные аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Модные аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 593, N'Модные аксессуары', N'/Дом и семья/Аксессуары/Модные аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Модные аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 607, N'Модные аксессуары', N'/Дом и семья/Аксессуары/Модные аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Модные аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 621, N'Модные аксессуары', N'/Дом и семья/Аксессуары/Модные аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Модные аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 635, N'Модные аксессуары', N'/Дом и семья/Аксессуары/Модные аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Модные аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 649, N'Модные аксессуары', N'/Дом и семья/Аксессуары/Модные аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Модные аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 663, N'Модные аксессуары', N'/Дом и семья/Аксессуары/Модные аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Модные аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 677, N'Модные аксессуары', N'/Дом и семья/Аксессуары/Модные аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Аксессуары/%'
  AND	[Name] = 'Модные аксессуары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 691, N'Модные аксессуары', N'/Дом и семья/Аксессуары/Модные аксессуары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Музыка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 584, N'Музыка', N'/Подарки/Музыка и Кино/Музыка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Музыка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 598, N'Музыка', N'/Подарки/Музыка и Кино/Музыка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Музыка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 612, N'Музыка', N'/Подарки/Музыка и Кино/Музыка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Музыка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 626, N'Музыка', N'/Подарки/Музыка и Кино/Музыка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Музыка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 640, N'Музыка', N'/Подарки/Музыка и Кино/Музыка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Музыка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 654, N'Музыка', N'/Подарки/Музыка и Кино/Музыка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Музыка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 668, N'Музыка', N'/Подарки/Музыка и Кино/Музыка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Музыка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 682, N'Музыка', N'/Подарки/Музыка и Кино/Музыка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Музыка и Кино/%'
  AND	[Name] = 'Музыка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 696, N'Музыка', N'/Подарки/Музыка и Кино/Музыка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Настольные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'Настольные игры', N'/Дом и семья/Товары для детей/Настольные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Настольные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'Настольные игры', N'/Дом и семья/Товары для детей/Настольные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Настольные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'Настольные игры', N'/Дом и семья/Товары для детей/Настольные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Настольные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'Настольные игры', N'/Дом и семья/Товары для детей/Настольные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Настольные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'Настольные игры', N'/Дом и семья/Товары для детей/Настольные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Настольные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'Настольные игры', N'/Дом и семья/Товары для детей/Настольные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Настольные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'Настольные игры', N'/Дом и семья/Товары для детей/Настольные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Настольные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'Настольные игры', N'/Дом и семья/Товары для детей/Настольные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Настольные игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'Настольные игры', N'/Дом и семья/Товары для детей/Настольные игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Научно-техническая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Научно-техническая литература', N'/Подарки/Книги/Научно-техническая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Научно-техническая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Научно-техническая литература', N'/Подарки/Книги/Научно-техническая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Научно-техническая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Научно-техническая литература', N'/Подарки/Книги/Научно-техническая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Научно-техническая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Научно-техническая литература', N'/Подарки/Книги/Научно-техническая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Научно-техническая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Научно-техническая литература', N'/Подарки/Книги/Научно-техническая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Научно-техническая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Научно-техническая литература', N'/Подарки/Книги/Научно-техническая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Научно-техническая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Научно-техническая литература', N'/Подарки/Книги/Научно-техническая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Научно-техническая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Научно-техническая литература', N'/Подарки/Книги/Научно-техническая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Научно-техническая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Научно-техническая литература', N'/Подарки/Книги/Научно-техническая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Наушники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'Наушники', N'/Техника и связь/Гаджеты, мобильные телефоны/Наушники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Наушники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'Наушники', N'/Техника и связь/Гаджеты, мобильные телефоны/Наушники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Наушники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'Наушники', N'/Техника и связь/Гаджеты, мобильные телефоны/Наушники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Наушники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'Наушники', N'/Техника и связь/Гаджеты, мобильные телефоны/Наушники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Наушники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'Наушники', N'/Техника и связь/Гаджеты, мобильные телефоны/Наушники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Наушники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'Наушники', N'/Техника и связь/Гаджеты, мобильные телефоны/Наушники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Наушники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'Наушники', N'/Техника и связь/Гаджеты, мобильные телефоны/Наушники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Наушники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'Наушники', N'/Техника и связь/Гаджеты, мобильные телефоны/Наушники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Наушники';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'Наушники', N'/Техника и связь/Гаджеты, мобильные телефоны/Наушники/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ноутбуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'Ноутбуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ноутбуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ноутбуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'Ноутбуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ноутбуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ноутбуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'Ноутбуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ноутбуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ноутбуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'Ноутбуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ноутбуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ноутбуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'Ноутбуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ноутбуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ноутбуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'Ноутбуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ноутбуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ноутбуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'Ноутбуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ноутбуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ноутбуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'Ноутбуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ноутбуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ноутбуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'Ноутбуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ноутбуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Обучающие программы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 585, N'Обучающие программы', N'/Техника и связь/Софт и игры/Обучающие программы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Обучающие программы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 599, N'Обучающие программы', N'/Техника и связь/Софт и игры/Обучающие программы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Обучающие программы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 613, N'Обучающие программы', N'/Техника и связь/Софт и игры/Обучающие программы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Обучающие программы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 627, N'Обучающие программы', N'/Техника и связь/Софт и игры/Обучающие программы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Обучающие программы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 641, N'Обучающие программы', N'/Техника и связь/Софт и игры/Обучающие программы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Обучающие программы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 655, N'Обучающие программы', N'/Техника и связь/Софт и игры/Обучающие программы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Обучающие программы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 669, N'Обучающие программы', N'/Техника и связь/Софт и игры/Обучающие программы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Обучающие программы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 683, N'Обучающие программы', N'/Техника и связь/Софт и игры/Обучающие программы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Обучающие программы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 697, N'Обучающие программы', N'/Техника и связь/Софт и игры/Обучающие программы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Оригинальные решения';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 580, N'Оригинальные решения', N'/Подарки/Аксессуары и сувениры/Оригинальные решения/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Оригинальные решения';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 594, N'Оригинальные решения', N'/Подарки/Аксессуары и сувениры/Оригинальные решения/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Оригинальные решения';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 608, N'Оригинальные решения', N'/Подарки/Аксессуары и сувениры/Оригинальные решения/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Оригинальные решения';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 622, N'Оригинальные решения', N'/Подарки/Аксессуары и сувениры/Оригинальные решения/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Оригинальные решения';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 636, N'Оригинальные решения', N'/Подарки/Аксессуары и сувениры/Оригинальные решения/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Оригинальные решения';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 650, N'Оригинальные решения', N'/Подарки/Аксессуары и сувениры/Оригинальные решения/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Оригинальные решения';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 664, N'Оригинальные решения', N'/Подарки/Аксессуары и сувениры/Оригинальные решения/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Оригинальные решения';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 678, N'Оригинальные решения', N'/Подарки/Аксессуары и сувениры/Оригинальные решения/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Оригинальные решения';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 692, N'Оригинальные решения', N'/Подарки/Аксессуары и сувениры/Оригинальные решения/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Освещение и электротовары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'Освещение и электротовары', N'/Дом и семья/Товары для дома/Освещение и электротовары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Освещение и электротовары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'Освещение и электротовары', N'/Дом и семья/Товары для дома/Освещение и электротовары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Освещение и электротовары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'Освещение и электротовары', N'/Дом и семья/Товары для дома/Освещение и электротовары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Освещение и электротовары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'Освещение и электротовары', N'/Дом и семья/Товары для дома/Освещение и электротовары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Освещение и электротовары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'Освещение и электротовары', N'/Дом и семья/Товары для дома/Освещение и электротовары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Освещение и электротовары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'Освещение и электротовары', N'/Дом и семья/Товары для дома/Освещение и электротовары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Освещение и электротовары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'Освещение и электротовары', N'/Дом и семья/Товары для дома/Освещение и электротовары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Освещение и электротовары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'Освещение и электротовары', N'/Дом и семья/Товары для дома/Освещение и электротовары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Освещение и электротовары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'Освещение и электротовары', N'/Дом и семья/Товары для дома/Освещение и электротовары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Пикник';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 588, N'Пикник', N'/Дом и семья/Товары для дачи/Пикник/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Пикник';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 602, N'Пикник', N'/Дом и семья/Товары для дачи/Пикник/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Пикник';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 616, N'Пикник', N'/Дом и семья/Товары для дачи/Пикник/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Пикник';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 630, N'Пикник', N'/Дом и семья/Товары для дачи/Пикник/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Пикник';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 644, N'Пикник', N'/Дом и семья/Товары для дачи/Пикник/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Пикник';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 658, N'Пикник', N'/Дом и семья/Товары для дачи/Пикник/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Пикник';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 672, N'Пикник', N'/Дом и семья/Товары для дачи/Пикник/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Пикник';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 686, N'Пикник', N'/Дом и семья/Товары для дачи/Пикник/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Пикник';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 700, N'Пикник', N'/Дом и семья/Товары для дачи/Пикник/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Планшеты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'Планшеты', N'/Техника и связь/Гаджеты, мобильные телефоны/Планшеты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Планшеты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'Планшеты', N'/Техника и связь/Гаджеты, мобильные телефоны/Планшеты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Планшеты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'Планшеты', N'/Техника и связь/Гаджеты, мобильные телефоны/Планшеты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Планшеты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'Планшеты', N'/Техника и связь/Гаджеты, мобильные телефоны/Планшеты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Планшеты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'Планшеты', N'/Техника и связь/Гаджеты, мобильные телефоны/Планшеты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Планшеты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'Планшеты', N'/Техника и связь/Гаджеты, мобильные телефоны/Планшеты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Планшеты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'Планшеты', N'/Техника и связь/Гаджеты, мобильные телефоны/Планшеты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Планшеты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'Планшеты', N'/Техника и связь/Гаджеты, мобильные телефоны/Планшеты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Планшеты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'Планшеты', N'/Техника и связь/Гаджеты, мобильные телефоны/Планшеты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Полив растений';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 588, N'Полив растений', N'/Дом и семья/Товары для дачи/Полив растений/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Полив растений';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 602, N'Полив растений', N'/Дом и семья/Товары для дачи/Полив растений/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Полив растений';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 616, N'Полив растений', N'/Дом и семья/Товары для дачи/Полив растений/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Полив растений';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 630, N'Полив растений', N'/Дом и семья/Товары для дачи/Полив растений/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Полив растений';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 644, N'Полив растений', N'/Дом и семья/Товары для дачи/Полив растений/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Полив растений';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 658, N'Полив растений', N'/Дом и семья/Товары для дачи/Полив растений/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Полив растений';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 672, N'Полив растений', N'/Дом и семья/Товары для дачи/Полив растений/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Полив растений';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 686, N'Полив растений', N'/Дом и семья/Товары для дачи/Полив растений/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Полив растений';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 700, N'Полив растений', N'/Дом и семья/Товары для дачи/Полив растений/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Поэзия';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Поэзия', N'/Подарки/Книги/Поэзия/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Поэзия';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Поэзия', N'/Подарки/Книги/Поэзия/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Поэзия';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Поэзия', N'/Подарки/Книги/Поэзия/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Поэзия';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Поэзия', N'/Подарки/Книги/Поэзия/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Поэзия';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Поэзия', N'/Подарки/Книги/Поэзия/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Поэзия';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Поэзия', N'/Подарки/Книги/Поэзия/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Поэзия';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Поэзия', N'/Подарки/Книги/Поэзия/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Поэзия';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Поэзия', N'/Подарки/Книги/Поэзия/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Поэзия';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Поэзия', N'/Подарки/Книги/Поэзия/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Пылесосы и климат';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'Пылесосы и климат', N'/Техника и связь/Бытовая техника/Пылесосы и климат/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Пылесосы и климат';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'Пылесосы и климат', N'/Техника и связь/Бытовая техника/Пылесосы и климат/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Пылесосы и климат';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'Пылесосы и климат', N'/Техника и связь/Бытовая техника/Пылесосы и климат/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Пылесосы и климат';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'Пылесосы и климат', N'/Техника и связь/Бытовая техника/Пылесосы и климат/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Пылесосы и климат';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'Пылесосы и климат', N'/Техника и связь/Бытовая техника/Пылесосы и климат/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Пылесосы и климат';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'Пылесосы и климат', N'/Техника и связь/Бытовая техника/Пылесосы и климат/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Пылесосы и климат';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'Пылесосы и климат', N'/Техника и связь/Бытовая техника/Пылесосы и климат/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Пылесосы и климат';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'Пылесосы и климат', N'/Техника и связь/Бытовая техника/Пылесосы и климат/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Пылесосы и климат';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'Пылесосы и климат', N'/Техника и связь/Бытовая техника/Пылесосы и климат/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Русская проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Русская проза', N'/Подарки/Книги/Русская проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Русская проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Русская проза', N'/Подарки/Книги/Русская проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Русская проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Русская проза', N'/Подарки/Книги/Русская проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Русская проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Русская проза', N'/Подарки/Книги/Русская проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Русская проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Русская проза', N'/Подарки/Книги/Русская проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Русская проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Русская проза', N'/Подарки/Книги/Русская проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Русская проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Русская проза', N'/Подарки/Книги/Русская проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Русская проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Русская проза', N'/Подарки/Книги/Русская проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Русская проза';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Русская проза', N'/Подарки/Книги/Русская проза/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Рыбалка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 592, N'Рыбалка', N'/Дом и семья/Товары для спорта/Рыбалка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Рыбалка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 606, N'Рыбалка', N'/Дом и семья/Товары для спорта/Рыбалка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Рыбалка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 620, N'Рыбалка', N'/Дом и семья/Товары для спорта/Рыбалка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Рыбалка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 634, N'Рыбалка', N'/Дом и семья/Товары для спорта/Рыбалка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Рыбалка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 648, N'Рыбалка', N'/Дом и семья/Товары для спорта/Рыбалка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Рыбалка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 662, N'Рыбалка', N'/Дом и семья/Товары для спорта/Рыбалка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Рыбалка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 676, N'Рыбалка', N'/Дом и семья/Товары для спорта/Рыбалка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Рыбалка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 690, N'Рыбалка', N'/Дом и семья/Товары для спорта/Рыбалка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Рыбалка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 704, N'Рыбалка', N'/Дом и семья/Товары для спорта/Рыбалка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 588, N'Садовая техника', N'/Дом и семья/Товары для дачи/Садовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 602, N'Садовая техника', N'/Дом и семья/Товары для дачи/Садовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 616, N'Садовая техника', N'/Дом и семья/Товары для дачи/Садовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 630, N'Садовая техника', N'/Дом и семья/Товары для дачи/Садовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 644, N'Садовая техника', N'/Дом и семья/Товары для дачи/Садовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 658, N'Садовая техника', N'/Дом и семья/Товары для дачи/Садовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 672, N'Садовая техника', N'/Дом и семья/Товары для дачи/Садовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 686, N'Садовая техника', N'/Дом и семья/Товары для дачи/Садовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовая техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 700, N'Садовая техника', N'/Дом и семья/Товары для дачи/Садовая техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовые интрументы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 588, N'Садовые интрументы', N'/Дом и семья/Товары для дачи/Садовые интрументы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовые интрументы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 602, N'Садовые интрументы', N'/Дом и семья/Товары для дачи/Садовые интрументы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовые интрументы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 616, N'Садовые интрументы', N'/Дом и семья/Товары для дачи/Садовые интрументы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовые интрументы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 630, N'Садовые интрументы', N'/Дом и семья/Товары для дачи/Садовые интрументы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовые интрументы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 644, N'Садовые интрументы', N'/Дом и семья/Товары для дачи/Садовые интрументы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовые интрументы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 658, N'Садовые интрументы', N'/Дом и семья/Товары для дачи/Садовые интрументы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовые интрументы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 672, N'Садовые интрументы', N'/Дом и семья/Товары для дачи/Садовые интрументы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовые интрументы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 686, N'Садовые интрументы', N'/Дом и семья/Товары для дачи/Садовые интрументы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дачи/%'
  AND	[Name] = 'Садовые интрументы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 700, N'Садовые интрументы', N'/Дом и семья/Товары для дачи/Садовые интрументы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Софт для дома и бизнеса';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 585, N'Софт для дома и бизнеса', N'/Техника и связь/Софт и игры/Софт для дома и бизнеса/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Софт для дома и бизнеса';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 599, N'Софт для дома и бизнеса', N'/Техника и связь/Софт и игры/Софт для дома и бизнеса/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Софт для дома и бизнеса';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 613, N'Софт для дома и бизнеса', N'/Техника и связь/Софт и игры/Софт для дома и бизнеса/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Софт для дома и бизнеса';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 627, N'Софт для дома и бизнеса', N'/Техника и связь/Софт и игры/Софт для дома и бизнеса/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Софт для дома и бизнеса';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 641, N'Софт для дома и бизнеса', N'/Техника и связь/Софт и игры/Софт для дома и бизнеса/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Софт для дома и бизнеса';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 655, N'Софт для дома и бизнеса', N'/Техника и связь/Софт и игры/Софт для дома и бизнеса/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Софт для дома и бизнеса';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 669, N'Софт для дома и бизнеса', N'/Техника и связь/Софт и игры/Софт для дома и бизнеса/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Софт для дома и бизнеса';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 683, N'Софт для дома и бизнеса', N'/Техника и связь/Софт и игры/Софт для дома и бизнеса/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Софт и игры/%'
  AND	[Name] = 'Софт для дома и бизнеса';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 697, N'Софт для дома и бизнеса', N'/Техника и связь/Софт и игры/Софт для дома и бизнеса/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Спорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 592, N'Спорт', N'/Дом и семья/Товары для спорта/Спорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Спорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 606, N'Спорт', N'/Дом и семья/Товары для спорта/Спорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Спорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 620, N'Спорт', N'/Дом и семья/Товары для спорта/Спорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Спорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 634, N'Спорт', N'/Дом и семья/Товары для спорта/Спорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Спорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 648, N'Спорт', N'/Дом и семья/Товары для спорта/Спорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Спорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 662, N'Спорт', N'/Дом и семья/Товары для спорта/Спорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Спорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 676, N'Спорт', N'/Дом и семья/Товары для спорта/Спорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Спорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 690, N'Спорт', N'/Дом и семья/Товары для спорта/Спорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для спорта/%'
  AND	[Name] = 'Спорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 704, N'Спорт', N'/Дом и семья/Товары для спорта/Спорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Средства по уходу';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 591, N'Средства по уходу', N'/Дом и семья/Товары для животных/Средства по уходу/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Средства по уходу';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 605, N'Средства по уходу', N'/Дом и семья/Товары для животных/Средства по уходу/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Средства по уходу';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 619, N'Средства по уходу', N'/Дом и семья/Товары для животных/Средства по уходу/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Средства по уходу';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 633, N'Средства по уходу', N'/Дом и семья/Товары для животных/Средства по уходу/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Средства по уходу';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 647, N'Средства по уходу', N'/Дом и семья/Товары для животных/Средства по уходу/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Средства по уходу';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 661, N'Средства по уходу', N'/Дом и семья/Товары для животных/Средства по уходу/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Средства по уходу';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 675, N'Средства по уходу', N'/Дом и семья/Товары для животных/Средства по уходу/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Средства по уходу';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 689, N'Средства по уходу', N'/Дом и семья/Товары для животных/Средства по уходу/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для животных/%'
  AND	[Name] = 'Средства по уходу';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 703, N'Средства по уходу', N'/Дом и семья/Товары для животных/Средства по уходу/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Стайлеры и щипцы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'Стайлеры и щипцы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Стайлеры и щипцы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Стайлеры и щипцы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'Стайлеры и щипцы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Стайлеры и щипцы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Стайлеры и щипцы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'Стайлеры и щипцы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Стайлеры и щипцы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Стайлеры и щипцы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'Стайлеры и щипцы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Стайлеры и щипцы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Стайлеры и щипцы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'Стайлеры и щипцы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Стайлеры и щипцы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Стайлеры и щипцы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'Стайлеры и щипцы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Стайлеры и щипцы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Стайлеры и щипцы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'Стайлеры и щипцы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Стайлеры и щипцы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Стайлеры и щипцы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'Стайлеры и щипцы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Стайлеры и щипцы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Стайлеры и щипцы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'Стайлеры и щипцы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Стайлеры и щипцы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Сувениры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 580, N'Сувениры', N'/Подарки/Аксессуары и сувениры/Сувениры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Сувениры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 594, N'Сувениры', N'/Подарки/Аксессуары и сувениры/Сувениры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Сувениры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 608, N'Сувениры', N'/Подарки/Аксессуары и сувениры/Сувениры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Сувениры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 622, N'Сувениры', N'/Подарки/Аксессуары и сувениры/Сувениры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Сувениры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 636, N'Сувениры', N'/Подарки/Аксессуары и сувениры/Сувениры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Сувениры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 650, N'Сувениры', N'/Подарки/Аксессуары и сувениры/Сувениры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Сувениры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 664, N'Сувениры', N'/Подарки/Аксессуары и сувениры/Сувениры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Сувениры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 678, N'Сувениры', N'/Подарки/Аксессуары и сувениры/Сувениры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Сувениры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 692, N'Сувениры', N'/Подарки/Аксессуары и сувениры/Сувениры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телевизоры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'Телевизоры', N'/Техника и связь/Гаджеты, мобильные телефоны/Телевизоры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телевизоры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'Телевизоры', N'/Техника и связь/Гаджеты, мобильные телефоны/Телевизоры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телевизоры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'Телевизоры', N'/Техника и связь/Гаджеты, мобильные телефоны/Телевизоры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телевизоры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'Телевизоры', N'/Техника и связь/Гаджеты, мобильные телефоны/Телевизоры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телевизоры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'Телевизоры', N'/Техника и связь/Гаджеты, мобильные телефоны/Телевизоры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телевизоры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'Телевизоры', N'/Техника и связь/Гаджеты, мобильные телефоны/Телевизоры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телевизоры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'Телевизоры', N'/Техника и связь/Гаджеты, мобильные телефоны/Телевизоры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телевизоры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'Телевизоры', N'/Техника и связь/Гаджеты, мобильные телефоны/Телевизоры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телевизоры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'Телевизоры', N'/Техника и связь/Гаджеты, мобильные телефоны/Телевизоры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телефоны';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'Телефоны', N'/Техника и связь/Гаджеты, мобильные телефоны/Телефоны/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телефоны';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'Телефоны', N'/Техника и связь/Гаджеты, мобильные телефоны/Телефоны/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телефоны';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'Телефоны', N'/Техника и связь/Гаджеты, мобильные телефоны/Телефоны/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телефоны';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'Телефоны', N'/Техника и связь/Гаджеты, мобильные телефоны/Телефоны/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телефоны';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'Телефоны', N'/Техника и связь/Гаджеты, мобильные телефоны/Телефоны/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телефоны';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'Телефоны', N'/Техника и связь/Гаджеты, мобильные телефоны/Телефоны/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телефоны';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'Телефоны', N'/Техника и связь/Гаджеты, мобильные телефоны/Телефоны/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телефоны';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'Телефоны', N'/Техника и связь/Гаджеты, мобильные телефоны/Телефоны/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Телефоны';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'Телефоны', N'/Техника и связь/Гаджеты, мобильные телефоны/Телефоны/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для дома';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'Техника для дома', N'/Техника и связь/Бытовая техника/Техника для дома/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для дома';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'Техника для дома', N'/Техника и связь/Бытовая техника/Техника для дома/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для дома';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'Техника для дома', N'/Техника и связь/Бытовая техника/Техника для дома/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для дома';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'Техника для дома', N'/Техника и связь/Бытовая техника/Техника для дома/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для дома';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'Техника для дома', N'/Техника и связь/Бытовая техника/Техника для дома/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для дома';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'Техника для дома', N'/Техника и связь/Бытовая техника/Техника для дома/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для дома';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'Техника для дома', N'/Техника и связь/Бытовая техника/Техника для дома/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для дома';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'Техника для дома', N'/Техника и связь/Бытовая техника/Техника для дома/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для дома';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'Техника для дома', N'/Техника и связь/Бытовая техника/Техника для дома/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Техника для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'Техника для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Техника для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Техника для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'Техника для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Техника для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Техника для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'Техника для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Техника для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Техника для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'Техника для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Техника для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Техника для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'Техника для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Техника для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Техника для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'Техника для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Техника для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Техника для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'Техника для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Техника для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Техника для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'Техника для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Техника для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Техника для красоты и здоровья';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'Техника для красоты и здоровья', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Техника для красоты и здоровья/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для кухни';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'Техника для кухни', N'/Техника и связь/Бытовая техника/Техника для кухни/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для кухни';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'Техника для кухни', N'/Техника и связь/Бытовая техника/Техника для кухни/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для кухни';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'Техника для кухни', N'/Техника и связь/Бытовая техника/Техника для кухни/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для кухни';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'Техника для кухни', N'/Техника и связь/Бытовая техника/Техника для кухни/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для кухни';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'Техника для кухни', N'/Техника и связь/Бытовая техника/Техника для кухни/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для кухни';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'Техника для кухни', N'/Техника и связь/Бытовая техника/Техника для кухни/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для кухни';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'Техника для кухни', N'/Техника и связь/Бытовая техника/Техника для кухни/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для кухни';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'Техника для кухни', N'/Техника и связь/Бытовая техника/Техника для кухни/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Техника для кухни';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'Техника для кухни', N'/Техника и связь/Бытовая техника/Техника для кухни/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Транспорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'Транспорт', N'/Дом и семья/Товары для детей/Транспорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Транспорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'Транспорт', N'/Дом и семья/Товары для детей/Транспорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Транспорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'Транспорт', N'/Дом и семья/Товары для детей/Транспорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Транспорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'Транспорт', N'/Дом и семья/Товары для детей/Транспорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Транспорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'Транспорт', N'/Дом и семья/Товары для детей/Транспорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Транспорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'Транспорт', N'/Дом и семья/Товары для детей/Транспорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Транспорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'Транспорт', N'/Дом и семья/Товары для детей/Транспорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Транспорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'Транспорт', N'/Дом и семья/Товары для детей/Транспорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Транспорт';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'Транспорт', N'/Дом и семья/Товары для детей/Транспорт/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Увлечения и игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 580, N'Увлечения и игры', N'/Подарки/Аксессуары и сувениры/Увлечения и игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Увлечения и игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 594, N'Увлечения и игры', N'/Подарки/Аксессуары и сувениры/Увлечения и игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Увлечения и игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 608, N'Увлечения и игры', N'/Подарки/Аксессуары и сувениры/Увлечения и игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Увлечения и игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 622, N'Увлечения и игры', N'/Подарки/Аксессуары и сувениры/Увлечения и игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Увлечения и игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 636, N'Увлечения и игры', N'/Подарки/Аксессуары и сувениры/Увлечения и игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Увлечения и игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 650, N'Увлечения и игры', N'/Подарки/Аксессуары и сувениры/Увлечения и игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Увлечения и игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 664, N'Увлечения и игры', N'/Подарки/Аксессуары и сувениры/Увлечения и игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Увлечения и игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 678, N'Увлечения и игры', N'/Подарки/Аксессуары и сувениры/Увлечения и игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Аксессуары и сувениры/%'
  AND	[Name] = 'Увлечения и игры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 692, N'Увлечения и игры', N'/Подарки/Аксессуары и сувениры/Увлечения и игры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ультрабуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'Ультрабуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ультрабуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ультрабуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'Ультрабуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ультрабуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ультрабуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'Ультрабуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ультрабуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ультрабуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'Ультрабуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ультрабуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ультрабуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'Ультрабуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ультрабуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ультрабуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'Ультрабуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ультрабуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ультрабуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'Ультрабуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ультрабуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ультрабуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'Ультрабуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ультрабуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Ультрабуки';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'Ультрабуки', N'/Техника и связь/Гаджеты, мобильные телефоны/Ультрабуки/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Фантастическая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 583, N'Фантастическая литература', N'/Подарки/Книги/Фантастическая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Фантастическая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 597, N'Фантастическая литература', N'/Подарки/Книги/Фантастическая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Фантастическая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 611, N'Фантастическая литература', N'/Подарки/Книги/Фантастическая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Фантастическая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 625, N'Фантастическая литература', N'/Подарки/Книги/Фантастическая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Фантастическая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 639, N'Фантастическая литература', N'/Подарки/Книги/Фантастическая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Фантастическая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 653, N'Фантастическая литература', N'/Подарки/Книги/Фантастическая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Фантастическая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 667, N'Фантастическая литература', N'/Подарки/Книги/Фантастическая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Фантастическая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 681, N'Фантастическая литература', N'/Подарки/Книги/Фантастическая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Подарки/Книги/%'
  AND	[Name] = 'Фантастическая литература';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 695, N'Фантастическая литература', N'/Подарки/Книги/Фантастическая литература/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Фото и видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'Фото и видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Фото и видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Фото и видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'Фото и видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Фото и видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Фото и видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'Фото и видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Фото и видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Фото и видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'Фото и видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Фото и видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Фото и видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'Фото и видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Фото и видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Фото и видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'Фото и видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Фото и видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Фото и видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'Фото и видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Фото и видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Фото и видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'Фото и видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Фото и видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Фото и видео техника';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'Фото и видео техника', N'/Техника и связь/Гаджеты, мобильные телефоны/Фото и видео техника/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Хобби и творчество';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'Хобби и творчество', N'/Дом и семья/Товары для детей/Хобби и творчество/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Хобби и творчество';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'Хобби и творчество', N'/Дом и семья/Товары для детей/Хобби и творчество/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Хобби и творчество';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'Хобби и творчество', N'/Дом и семья/Товары для детей/Хобби и творчество/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Хобби и творчество';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'Хобби и творчество', N'/Дом и семья/Товары для детей/Хобби и творчество/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Хобби и творчество';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'Хобби и творчество', N'/Дом и семья/Товары для детей/Хобби и творчество/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Хобби и творчество';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'Хобби и творчество', N'/Дом и семья/Товары для детей/Хобби и творчество/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Хобби и творчество';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'Хобби и творчество', N'/Дом и семья/Товары для детей/Хобби и творчество/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Хобби и творчество';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'Хобби и творчество', N'/Дом и семья/Товары для детей/Хобби и творчество/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Хобби и творчество';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'Хобби и творчество', N'/Дом и семья/Товары для детей/Хобби и творчество/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Хозяйственные товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 590, N'Хозяйственные товары', N'/Дом и семья/Товары для дома/Хозяйственные товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Хозяйственные товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 604, N'Хозяйственные товары', N'/Дом и семья/Товары для дома/Хозяйственные товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Хозяйственные товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 618, N'Хозяйственные товары', N'/Дом и семья/Товары для дома/Хозяйственные товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Хозяйственные товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 632, N'Хозяйственные товары', N'/Дом и семья/Товары для дома/Хозяйственные товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Хозяйственные товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 646, N'Хозяйственные товары', N'/Дом и семья/Товары для дома/Хозяйственные товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Хозяйственные товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 660, N'Хозяйственные товары', N'/Дом и семья/Товары для дома/Хозяйственные товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Хозяйственные товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 674, N'Хозяйственные товары', N'/Дом и семья/Товары для дома/Хозяйственные товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Хозяйственные товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 688, N'Хозяйственные товары', N'/Дом и семья/Товары для дома/Хозяйственные товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для дома/%'
  AND	[Name] = 'Хозяйственные товары';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 702, N'Хозяйственные товары', N'/Дом и семья/Товары для дома/Хозяйственные товары/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Часы, погодные станции';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'Часы, погодные станции', N'/Техника и связь/Бытовая техника/Часы, погодные станции/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Часы, погодные станции';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'Часы, погодные станции', N'/Техника и связь/Бытовая техника/Часы, погодные станции/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Часы, погодные станции';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'Часы, погодные станции', N'/Техника и связь/Бытовая техника/Часы, погодные станции/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Часы, погодные станции';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'Часы, погодные станции', N'/Техника и связь/Бытовая техника/Часы, погодные станции/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Часы, погодные станции';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'Часы, погодные станции', N'/Техника и связь/Бытовая техника/Часы, погодные станции/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Часы, погодные станции';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'Часы, погодные станции', N'/Техника и связь/Бытовая техника/Часы, погодные станции/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Часы, погодные станции';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'Часы, погодные станции', N'/Техника и связь/Бытовая техника/Часы, погодные станции/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Часы, погодные станции';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'Часы, погодные станции', N'/Техника и связь/Бытовая техника/Часы, погодные станции/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Часы, погодные станции';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'Часы, погодные станции', N'/Техника и связь/Бытовая техника/Часы, погодные станции/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Школа, обучение, развитие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 589, N'Школа, обучение, развитие', N'/Дом и семья/Товары для детей/Школа, обучение, развитие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Школа, обучение, развитие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 603, N'Школа, обучение, развитие', N'/Дом и семья/Товары для детей/Школа, обучение, развитие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Школа, обучение, развитие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 617, N'Школа, обучение, развитие', N'/Дом и семья/Товары для детей/Школа, обучение, развитие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Школа, обучение, развитие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 631, N'Школа, обучение, развитие', N'/Дом и семья/Товары для детей/Школа, обучение, развитие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Школа, обучение, развитие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 645, N'Школа, обучение, развитие', N'/Дом и семья/Товары для детей/Школа, обучение, развитие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Школа, обучение, развитие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 659, N'Школа, обучение, развитие', N'/Дом и семья/Товары для детей/Школа, обучение, развитие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Школа, обучение, развитие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 673, N'Школа, обучение, развитие', N'/Дом и семья/Товары для детей/Школа, обучение, развитие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Школа, обучение, развитие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 687, N'Школа, обучение, развитие', N'/Дом и семья/Товары для детей/Школа, обучение, развитие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для детей/%'
  AND	[Name] = 'Школа, обучение, развитие';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 701, N'Школа, обучение, развитие', N'/Дом и семья/Товары для детей/Школа, обучение, развитие/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Щетки, скребки, омыватели, канистры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 587, N'Щетки, скребки, омыватели, канистры', N'/Дом и семья/Товары для автомобилистов/Щетки, скребки, омыватели, канистры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Щетки, скребки, омыватели, канистры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 601, N'Щетки, скребки, омыватели, канистры', N'/Дом и семья/Товары для автомобилистов/Щетки, скребки, омыватели, канистры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Щетки, скребки, омыватели, канистры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 615, N'Щетки, скребки, омыватели, канистры', N'/Дом и семья/Товары для автомобилистов/Щетки, скребки, омыватели, канистры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Щетки, скребки, омыватели, канистры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 629, N'Щетки, скребки, омыватели, канистры', N'/Дом и семья/Товары для автомобилистов/Щетки, скребки, омыватели, канистры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Щетки, скребки, омыватели, канистры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 643, N'Щетки, скребки, омыватели, канистры', N'/Дом и семья/Товары для автомобилистов/Щетки, скребки, омыватели, канистры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Щетки, скребки, омыватели, канистры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 657, N'Щетки, скребки, омыватели, канистры', N'/Дом и семья/Товары для автомобилистов/Щетки, скребки, омыватели, канистры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Щетки, скребки, омыватели, канистры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 671, N'Щетки, скребки, омыватели, канистры', N'/Дом и семья/Товары для автомобилистов/Щетки, скребки, омыватели, канистры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Щетки, скребки, омыватели, канистры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 685, N'Щетки, скребки, омыватели, канистры', N'/Дом и семья/Товары для автомобилистов/Щетки, скребки, омыватели, канистры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Дом и семья/Товары для автомобилистов/%'
  AND	[Name] = 'Щетки, скребки, омыватели, канистры';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 699, N'Щетки, скребки, омыватели, канистры', N'/Дом и семья/Товары для автомобилистов/Щетки, скребки, омыватели, канистры/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Электрическая зубная щетка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'Электрическая зубная щетка', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Электрическая зубная щетка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Электрическая зубная щетка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'Электрическая зубная щетка', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Электрическая зубная щетка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Электрическая зубная щетка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'Электрическая зубная щетка', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Электрическая зубная щетка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Электрическая зубная щетка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'Электрическая зубная щетка', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Электрическая зубная щетка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Электрическая зубная щетка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'Электрическая зубная щетка', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Электрическая зубная щетка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Электрическая зубная щетка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'Электрическая зубная щетка', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Электрическая зубная щетка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Электрическая зубная щетка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'Электрическая зубная щетка', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Электрическая зубная щетка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Электрическая зубная щетка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'Электрическая зубная щетка', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Электрическая зубная щетка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Электрическая зубная щетка';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'Электрическая зубная щетка', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Электрическая зубная щетка/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Электроинструменты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 581, N'Электроинструменты', N'/Техника и связь/Бытовая техника/Электроинструменты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Электроинструменты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 595, N'Электроинструменты', N'/Техника и связь/Бытовая техника/Электроинструменты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Электроинструменты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 609, N'Электроинструменты', N'/Техника и связь/Бытовая техника/Электроинструменты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Электроинструменты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 623, N'Электроинструменты', N'/Техника и связь/Бытовая техника/Электроинструменты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Электроинструменты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 637, N'Электроинструменты', N'/Техника и связь/Бытовая техника/Электроинструменты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Электроинструменты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 651, N'Электроинструменты', N'/Техника и связь/Бытовая техника/Электроинструменты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Электроинструменты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 665, N'Электроинструменты', N'/Техника и связь/Бытовая техника/Электроинструменты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Электроинструменты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 679, N'Электроинструменты', N'/Техника и связь/Бытовая техника/Электроинструменты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Бытовая техника/%'
  AND	[Name] = 'Электроинструменты';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 693, N'Электроинструменты', N'/Техника и связь/Бытовая техника/Электроинструменты/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Электронные книги';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 582, N'Электронные книги', N'/Техника и связь/Гаджеты, мобильные телефоны/Электронные книги/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Электронные книги';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 596, N'Электронные книги', N'/Техника и связь/Гаджеты, мобильные телефоны/Электронные книги/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Электронные книги';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 610, N'Электронные книги', N'/Техника и связь/Гаджеты, мобильные телефоны/Электронные книги/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Электронные книги';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 624, N'Электронные книги', N'/Техника и связь/Гаджеты, мобильные телефоны/Электронные книги/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Электронные книги';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 638, N'Электронные книги', N'/Техника и связь/Гаджеты, мобильные телефоны/Электронные книги/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Электронные книги';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 652, N'Электронные книги', N'/Техника и связь/Гаджеты, мобильные телефоны/Электронные книги/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Электронные книги';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 666, N'Электронные книги', N'/Техника и связь/Гаджеты, мобильные телефоны/Электронные книги/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Электронные книги';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 680, N'Электронные книги', N'/Техника и связь/Гаджеты, мобильные телефоны/Электронные книги/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Техника и связь/Гаджеты, мобильные телефоны/%'
  AND	[Name] = 'Электронные книги';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 694, N'Электронные книги', N'/Техника и связь/Гаджеты, мобильные телефоны/Электронные книги/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Эпиляторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 586, N'Эпиляторы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Эпиляторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Эпиляторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 600, N'Эпиляторы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Эпиляторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Эпиляторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 614, N'Эпиляторы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Эпиляторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Эпиляторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 628, N'Эпиляторы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Эпиляторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Эпиляторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 642, N'Эпиляторы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Эпиляторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Эпиляторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 656, N'Эпиляторы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Эпиляторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Эпиляторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 670, N'Эпиляторы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Эпиляторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Эпиляторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 684, N'Эпиляторы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Эпиляторы/', 1, @User, GETDATE(), 0);
END
SET @CatId = NULL;
SELECT	@CatId = [Id]
 FROM	[prod].[ProductCategories]
WHERE	[NamePath] LIKE '/Красота, здоровье, фитнес/Техника для красоты и здоровья/%'
  AND	[Name] = 'Эпиляторы';

IF @CatId IS NULL
BEGIN
INSERT INTO [prod].[ProductCategories]
	([Type], [ParentId], [Name], [NamePath], [Status], [InsertedUserId], [InsertedDate], [CatOrder]) 
VALUES 
	(0, 698, N'Эпиляторы', N'/Красота, здоровье, фитнес/Техника для красоты и здоровья/Эпиляторы/', 1, @User, GETDATE(), 0);
END

PRINT 'Удаляем маппинг и разрешения ОЗОНа'
DELETE FROM [prod].[PartnerProductCategoryLinks]
WHERE [PartnerId] = @OzonId

DELETE FROM [prod].[ProductCategoriesPermissions]
WHERE [PartnerId] = @OzonId

PRINT 'Новые разрешения ОЗОНа'
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

PRINT 'Новый маппинг ОЗОНа'
INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(575, @OzonId, '/Специальный каталог/Дом и семья/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(579, @OzonId, '/Специальный каталог/Дом и семья/Аксессуары/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(599, @OzonId, '/Специальный каталог/Дом и семья/Аксессуары/Аксессуары для интерьера/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(633, @OzonId, '/Специальный каталог/Дом и семья/Аксессуары/Модные аксессуары/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(587, @OzonId, '/Специальный каталог/Дом и семья/Товары для автомобилистов/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(593, @OzonId, '/Специальный каталог/Дом и семья/Товары для автомобилистов/GPS навигаторы/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(595, @OzonId, '/Специальный каталог/Дом и семья/Товары для автомобилистов/Автоаксессуары/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(596, @OzonId, '/Специальный каталог/Дом и семья/Товары для автомобилистов/Автокружки, термосы, холодильники/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(617, @OzonId, '/Специальный каталог/Дом и семья/Товары для автомобилистов/Инструменты для авто/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(669, @OzonId, '/Специальный каталог/Дом и семья/Товары для автомобилистов/Щетки, скребки, омыватели, канистры/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(588, @OzonId, '/Специальный каталог/Дом и семья/Товары для дачи/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(631, @OzonId, '/Специальный каталог/Дом и семья/Товары для дачи/Ландшафт и садовый декор/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(642, @OzonId, '/Специальный каталог/Дом и семья/Товары для дачи/Пикник/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(644, @OzonId, '/Специальный каталог/Дом и семья/Товары для дачи/Полив растений/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(649, @OzonId, '/Специальный каталог/Дом и семья/Товары для дачи/Садовая техника/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(650, @OzonId, '/Специальный каталог/Дом и семья/Товары для дачи/Садовые интрументы/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(589, @OzonId, '/Специальный каталог/Дом и семья/Товары для детей/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(594, @OzonId, '/Специальный каталог/Дом и семья/Товары для детей/LEGO/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(602, @OzonId, '/Специальный каталог/Дом и семья/Товары для детей/Безопасность/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(608, @OzonId, '/Специальный каталог/Дом и семья/Товары для детей/Детская косметика и гигиена/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(610, @OzonId, '/Специальный каталог/Дом и семья/Товары для детей/Детсткий спорт, активный отдых/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(616, @OzonId, '/Специальный каталог/Дом и семья/Товары для детей/Игрушки/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(635, @OzonId, '/Специальный каталог/Дом и семья/Товары для детей/Настольные игры/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(660, @OzonId, '/Специальный каталог/Дом и семья/Товары для детей/Транспорт/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(665, @OzonId, '/Специальный каталог/Дом и семья/Товары для детей/Хобби и творчество/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(668, @OzonId, '/Специальный каталог/Дом и семья/Товары для детей/Школа, обучение, развитие/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(590, @OzonId, '/Специальный каталог/Дом и семья/Товары для дома/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(605, @OzonId, '/Специальный каталог/Дом и семья/Товары для дома/Ванна, баня, сауны/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(607, @OzonId, '/Специальный каталог/Дом и семья/Товары для дома/Все для ремонта/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(612, @OzonId, '/Специальный каталог/Дом и семья/Товары для дома/Домашний текстиль/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(618, @OzonId, '/Специальный каталог/Дом и семья/Товары для дома/Интерьер/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(630, @OzonId, '/Специальный каталог/Дом и семья/Товары для дома/Кухня/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(641, @OzonId, '/Специальный каталог/Дом и семья/Товары для дома/Освещение и электротовары/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(666, @OzonId, '/Специальный каталог/Дом и семья/Товары для дома/Хозяйственные товары/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(591, @OzonId, '/Специальный каталог/Дом и семья/Товары для животных/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(598, @OzonId, '/Специальный каталог/Дом и семья/Товары для животных/Аксессуары/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(653, @OzonId, '/Специальный каталог/Дом и семья/Товары для животных/Средства по уходу/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(592, @OzonId, '/Специальный каталог/Дом и семья/Товары для спорта/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(600, @OzonId, '/Специальный каталог/Дом и семья/Товары для спорта/Активный отдых и путешествие/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(648, @OzonId, '/Специальный каталог/Дом и семья/Товары для спорта/Рыбалка/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(652, @OzonId, '/Специальный каталог/Дом и семья/Товары для спорта/Спорт/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(576, @OzonId, '/Специальный каталог/Красота, здоровье, фитнес/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(576, @OzonId, '/Специальный каталог/Красота, здоровье, фитнес/Красота, здоровье, фитнес/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(611, @OzonId, '/Специальный каталог/Красота, здоровье, фитнес/Красота, здоровье, фитнес/Для красоты и здоровья/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(613, @OzonId, '/Специальный каталог/Красота, здоровье, фитнес/Красота, здоровье, фитнес/Техника для красоты и здоровья/Дорожные фены/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(632, @OzonId, '/Специальный каталог/Красота, здоровье, фитнес/Красота, здоровье, фитнес/Техника для красоты и здоровья/Медицинская техника/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(654, @OzonId, '/Специальный каталог/Красота, здоровье, фитнес/Красота, здоровье, фитнес/Техника для красоты и здоровья/Стайлеры и щипцы/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(670, @OzonId, '/Специальный каталог/Красота, здоровье, фитнес/Красота, здоровье, фитнес/Техника для красоты и здоровья/Электрическая зубная щетка/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(673, @OzonId, '/Специальный каталог/Красота, здоровье, фитнес/Красота, здоровье, фитнес/Техника для красоты и здоровья/Эпиляторы/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(577, @OzonId, '/Специальный каталог/Подарки/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(580, @OzonId, '/Специальный каталог/Подарки/Аксессуары/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(580, @OzonId, '/Специальный каталог/Подарки/Другие подарки/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(597, @OzonId, '/Специальный каталог/Подарки/Другие подарки/Авторские работы/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(620, @OzonId, '/Специальный каталог/Подарки/Другие подарки/Канцелярские товары/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(640, @OzonId, '/Специальный каталог/Подарки/Другие подарки/Оригинальные решения/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(655, @OzonId, '/Специальный каталог/Подарки/Другие подарки/Сувениры/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(661, @OzonId, '/Специальный каталог/Подарки/Другие подарки/Увлечения и игры/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(583, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(603, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Бизнес-литература/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(604, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Биографическая и мемуарная литература/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(609, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Детская литература/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(614, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Зарубежная проза/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(619, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Исторические романы/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(622, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Книги для дома и досуга/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(623, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Книги для родителей/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(624, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Книги о путешествиях, туризме и спорте/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(625, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Книги по изучению иностранных языков/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(626, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Книги по искусству и культуре/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(627, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Книги по общественным и гуманитарным наукам/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(636, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Научно-техническая литература/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(645, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Поэзия/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(647, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Русская проза/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(663, @OzonId, '/Специальный каталог/Подарки/Книги, пресса/Фантастическая литература/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(584, @OzonId, '/Специальный каталог/Музыка и Кино/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(621, @OzonId, '/Специальный каталог/Кино/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(634, @OzonId, '/Специальный каталог/Музыка и Кино/Музыка/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(578, @OzonId, '/Специальный каталог/Техника и связь/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(581, @OzonId, '/Специальный каталог/Техника и связь/Бытовая техника/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(629, @OzonId, '/Специальный каталог/Техника и связь/Бытовая техника/Крупная бытовая техника/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(646, @OzonId, '/Специальный каталог/Техника и связь/Бытовая техника/Пылесосы и климат/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(658, @OzonId, '/Специальный каталог/Техника и связь/Бытовая техника/Техника для дома/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(659, @OzonId, '/Специальный каталог/Техника и связь/Бытовая техника/Техника для кухни/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(667, @OzonId, '/Специальный каталог/Техника и связь/Бытовая техника/Часы, погодные станции/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(671, @OzonId, '/Специальный каталог/Техника и связь/Бытовая техника/Электроинструменты/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(582, @OzonId, '/Специальный каталог/Техника и связь/Гаджеты, мобильные телефоны/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(601, @OzonId, '/Специальный каталог/Техника и связь/Гаджеты, мобильные телефоны/Аудио-видео техника/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(637, @OzonId, '/Специальный каталог/Техника и связь/Гаджеты, мобильные телефоны/Наушники/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(638, @OzonId, '/Специальный каталог/Техника и связь/Гаджеты, мобильные телефоны/Ноутбуки/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(643, @OzonId, '/Специальный каталог/Техника и связь/Гаджеты, мобильные телефоны/Планшеты/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(656, @OzonId, '/Специальный каталог/Техника и связь/Гаджеты, мобильные телефоны/Телевизоры/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(657, @OzonId, '/Специальный каталог/Техника и связь/Гаджеты, мобильные телефоны/Телефоны/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(662, @OzonId, '/Специальный каталог/Техника и связь/Гаджеты, мобильные телефоны/Ультрабуки/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(664, @OzonId, '/Специальный каталог/Техника и связь/Гаджеты, мобильные телефоны/Фото и видео техника/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(672, @OzonId, '/Специальный каталог/Техника и связь/Гаджеты, мобильные телефоны/Электронные книги/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(585, @OzonId, '/Специальный каталог/Техника и связь/Софт и игры/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(606, @OzonId, '/Специальный каталог/Техника и связь/Софт и игры/Видеоигры/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(615, @OzonId, '/Специальный каталог/Техника и связь/Софт и игры/Игровые приставки/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(628, @OzonId, '/Специальный каталог/Техника и связь/Софт и игры/Компьютерные игры/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(639, @OzonId, '/Специальный каталог/Техника и связь/Софт и игры/Обучающие программы/', 1, @User, GETDATE())

INSERT INTO [prod].[PartnerProductCategoryLinks]
	([ProductCategoryId],[PartnerId], [NamePath], [IncludeSubcategories], [InsertedUserId], [InsertedDate])
VALUES
	(651, @OzonId, '/Специальный каталог/Техника и связь/Софт и игры/Софт для дома и бизнеса/', 1, @User, GETDATE())

