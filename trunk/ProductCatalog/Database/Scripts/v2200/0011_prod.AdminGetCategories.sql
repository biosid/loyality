/****** Object:  StoredProcedure [prod].[AdminGetCategories]    Script Date: 03/20/2015 19:38:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[AdminGetCategories]') AND type in (N'P', N'PC'))
DROP PROCEDURE [prod].[AdminGetCategories]
GO

/****** Object:  StoredProcedure [prod].[AdminGetCategories]    Script Date: 03/20/2015 19:38:49 ******/
CREATE PROCEDURE [prod].[AdminGetCategories]
	@status int = null,
	@parentId int = null,
	@nestingLevel int = null,
	@countToTake int = null,
	@countToSkip int = null,
	@calcTotalCount bit = null,
	@includeParent bit = 0,
	@type int = null,
	@totalCount int out,
	@childrenCount int out
AS
BEGIN
	SET @countToTake = ISNULL(@countToTake, 20)
	SET @countToSkip = ISNULL(@countToSkip, 0)

	DECLARE @NamePath nvarchar(max)
	DECLARE @StartNestingLevel int = 1

	-- NOTE: Если необходимо брать категории от заданой, то находим путь заданной
	IF (@parentId IS NOT NULL)
	BEGIN
		SELECT @NamePath = NamePath, @StartNestingLevel = [prod].[GetCategoryNestingLevel](id, null)
		FROM [prod].[ProductCategories] NOLOCK
		WHERE Id = @parentId
	END 

	-- NOTE: Если необходимо брать только дочки заданой, 
	IF (@includeParent != 1)
	BEGIN
		-- NOTE: то символ "_" гарантирует что LIKE не вернет саму заданую категорию
		SET @NamePath = @NamePath + '_'
		-- NOTE: то + 1 гарантирует правильный отбор заданого уровня
		SET @StartNestingLevel = @StartNestingLevel + 1
	END
	 
	IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#categories')) 
		DROP TABLE #categories

	CREATE TABLE #categories (
		Id int,
		ParentId int,
		NamePath nvarchar(max),
		SubCategoriesCount int,
		ProductsCount int,
		NestingLevel int,
		CatOrder int
	)
	  
	;WITH 
		-- NOTE: Фильтруем по просты признакам и вышисляем уровень вложености
		Categories AS (
		SELECT c.Id
			,c.ParentId
			,c.NamePath
			,c.CatOrder
			,[prod].[GetCategoryNestingLevel](id, null) AS NestingLevel
		FROM prod.ProductCategories c (NOLOCK)
		WHERE (@parentId IS NULL OR c.NamePath LIKE @NamePath + '%')
		  AND (@status IS NULL OR c.[Status] = @status)
		  AND (@type IS NULL or c.[Type] = @type)
	  )
	  , 
	  -- NOTE: Вычисляем кол-во 
	  CategoriesWithSubCategoriesCount AS (
		SELECT c1.Id
			,MIN(c1.ParentId) AS ParentId
			,MIN(c1.NamePath) AS NamePath
			,MIN(c1.NestingLevel) AS NestingLevel 
			,MIN(c1.CatOrder) AS CatOrder
			,COUNT(c2.Id) AS SubCategoriesCount
		FROM Categories c1
		LEFT JOIN Categories c2 ON c1.Id = c2.ParentId
		WHERE (@nestingLevel is null or (c1.NestingLevel - @StartNestingLevel) < @nestingLevel)
		GROUP BY c1.Id
	  )
	  INSERT INTO #categories (Id,ParentId,NamePath,NestingLevel,SubCategoriesCount,CatOrder,ProductsCount)
	  SELECT cwc.Id
			,MIN(cwc.ParentId)
			,MIN(cwc.NamePath)
			,MIN(cwc.NestingLevel)
			,MIN(cwc.SubCategoriesCount)
			,MIN(cwc.CatOrder)
			,COUNT(p.ProductId) AS ProductsCount
	  FROM CategoriesWithSubCategoriesCount cwc
	  LEFT JOIN prod.ProductsFromAllPartners p ON cwc.Id = p.CategoryId
	  --WHERE Id IN (486, 288, 296)
	  GROUP BY cwc.Id

	if (@calcTotalCount = 1)
	begin
		select @totalCount = count(1)
		from #categories c
	end

	select @childrenCount = count(1)
	from #categories c
	where (@parentId is null and c.ParentId is null) or c.ParentId = @parentId

	declare @catOrderIncrement int
	select @catOrderIncrement = min(catOrder) 
	from #categories

	if (@catOrderIncrement > 0) 
		set @catOrderIncrement = 0

	;WITH ProductsCountWithChild AS (
		SELECT	c1.Id
				,MIN(c1.SubCategoriesCount) AS SubCategoriesCount
				,MIN(c1.NestingLevel) AS NestingLevel
				,MIN(c1.CatOrder) AS CatOrder
				,SUM(c2.ProductsCount) AS ProductsCount
		FROM #categories c1
		JOIN #categories c2 ON c2.NamePath LIKE c1.NamePath + '%'
		GROUP BY c1.Id
	)
	select top (@countToTake) pc.*, tt.ProductsCount, tt.SubCategoriesCount, tt.NestingLevel, tt.NameRoot
	from (
		select *, row_number() over (order by NameRoot, CatOrder) as RowNumber 
		from
			(
				select	c.*,
						[dbo].[GetProdCatOrderName] (Id, @catOrderIncrement) as NameRoot
				from ProductsCountWithChild c
			) as t
		) as tt
	JOIN prod.ProductCategories pc ON pc.Id = tt.Id
	where tt.RowNumber > @countToSkip
	order by tt.NameRoot
END

GO