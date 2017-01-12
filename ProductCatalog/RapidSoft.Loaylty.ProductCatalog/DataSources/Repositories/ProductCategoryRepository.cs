namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Xml.Linq;

    using Extensions;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Entities;
    using RapidSoft.Loaylty.ProductCatalog.Services;

    internal class ProductCategoryRepository : BaseRepository, IProductCategoriesRepository
    {
        public ProductCategoryRepository()
            : base(DataSourceConfig.ConnectionString)
        {
        }

        public ProductCategoryRepository(string connectionString)
            : base(connectionString)
        {
        }

        public int UpdateChildrenNamePaths(string userId, int categoryId, string oldName, string newName)
        {
            var getAllSubCategoriesParameters = new GetAllSubCategoriesParameters { ParentId = categoryId };
            var subCatResult = new CategoriesSearcher().GetAllSubCategories(getAllSubCategoriesParameters);

            if (!subCatResult.Categories.Any())
            {
                return 0;
            }

            const string UpdateScript = @"UPDATE [prod].[ProductCategories] SET [NamePath] = @namePath, UpdatedUserId = @updatedUserId, UpdatedDate = getdate() WHERE [Id] = @id";

            using (var ctx = DbNewContext(userId))
            {
                foreach (var child in subCatResult.Categories)
                {
                    child.NamePath = child.NamePath.Replace(oldName, newName).Replace("//", "/");    
                   
                    ctx.ExecuteSqlCommand(
                        UpdateScript,
                        new SqlParameter("namePath", child.NamePath),
                        new SqlParameter("updatedUserId", userId), 
                        new SqlParameter("id", child.Id));
                }

                return subCatResult.Categories.Count();
            }
        }

        public ResultBase ChangeCategoriesStatus(string userId, int[] categoryIds, ProductCategoryStatuses status)
        {
            using (var ctx = DbNewContext(userId))
            {
                var res = ctx.ProductCategories.Where(c => categoryIds.Contains(c.Id)).ToList();
                var now = DateTime.Now;

                res.ForEach(c =>
                {
                    c.Status = status; 
                    c.UpdatedUserId = userId;
                    c.UpdatedDate = now;
                });

                ctx.SaveChanges();

                return ResultBase.BuildSuccess();
            }
        }

        public ResultBase DeleteCategory(string userId, int categoryId)
        {
            var getAllSubCategoriesParameters = new GetAllSubCategoriesParameters
                                                    {
                                                        ParentId = categoryId,
                                                        IncludeParent = true,
                                                        CalcTotalCount = true
                                                    };
            var catResult = new CategoriesSearcher().GetAllSubCategories(getAllSubCategoriesParameters);

            if (catResult.TotalCount == 0)
            {
                return ResultBase.BuildNotFound("Категория не найдена");
            }
            
            if (catResult.Categories.Any(c => c.ProductsCount != 0))
            {
                return ResultBase.BuildFail(ResultCodes.ERROR_DELETING_NOT_EMPTY_CATEGORY, "Ошибка удаления не пустой категории");
            }

            new CategoryPermissionRepository().Delete(catResult.Categories.Select(c => c.Id).ToList());

            using (var ctx = DbNewContext(userId))
            {
                foreach (var category in catResult.Categories)
                {
                    ctx.Entry(category).State = EntityState.Deleted;
                }

                ctx.SaveChanges();
            }

            return ResultBase.BuildSuccess();
        }

        public ProductCategory GetById(int id)
        {
            using (var ctx = DbNewContext())
            {
                return GetById(ctx, id);
            }
        }

        public ProductCategory GetById(LoyaltyDBEntities context, int id)
        {
            return context.ProductCategories.FirstOrDefault(c => c.Id == id);
        }
        
        public void UpdateParent(string userId, int categoryId, int? newParentId)
        {
            using (var ctx = DbNewContext(userId))
            {
                var cat = GetById(ctx, categoryId);

                if (cat == null)
                {
                    throw new OperationException("Перемещаемая категория не найдена")
                    {
                        ResultCode = ResultCodes.NOT_FOUND
                    };
                }

                if (cat.ParentId != newParentId)
                {
                    var newParent = newParentId.HasValue
                        ? GetById(ctx, newParentId.Value)
                        : null;

                    if (newParent == null)
                    {
                        throw new OperationException(ResultCodes.PARENT_CATEGORY_NOT_FOUND, "Родительская категория не найдена");
                    }
                    
                    if (newParent != null && newParent.Type == ProductCategoryTypes.Online)
                    {
                        string mess = string.Format(
                            "Не возможно создать подкатегорию в динамической категории {0}",
                            newParent.Id);
                        throw new OperationException(ResultCodes.PARENT_CATEGORY_IS_DYNAMIC, mess);
                    }

                    var newNamePath = ProductCategoriesDataSource.CreatePath(newParent == null ? null : newParent.NamePath, cat.Name);
                    var existedPathCat = ProductCategoriesDataSource.GetProductCategoryByNamePath(newNamePath);

                    if (existedPathCat != null)
                    {
                        var mess = string.Format("Категория {0} уже существует", newNamePath);
                        throw new OperationException(ResultCodes.CATEGORY_WITH_NAME_EXISTS, mess);
                    }

                    cat.ParentId = newParentId;
                    cat.NamePath = newNamePath;

                    UpdateChildrenNamePaths(userId, cat.Id, cat.Name, newNamePath);
                }

                ctx.Entry(cat).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public bool IsDeactivated(int id)
        {
            using (var ctx = DbNewContext())
            {
                return ctx.DeactivatedCategories.Any(dc => dc.Id == id);
            }
        }

        public int[] GetDeactivated(int[] ids)
        {
            using (var ctx = DbNewContext())
            {
                return ids != null && ids.Length > 0
                           ? ctx.DeactivatedCategories
                                .Where(c => ids.Contains(c.Id))
                                .Select(c => c.Id)
                                .ToArray()
                           : ctx.DeactivatedCategories
                                .Select(c => c.Id)
                                .ToArray();
            }
        }

        public ProductCategory Add(ProductCategory category)
        {
            using (var ctx = DbNewContext())
            {
                if (category.CatOrder == 0)
                {
                    var maxOrder = ctx.ProductCategories.Max(x => x.CatOrder);
                    category.CatOrder = maxOrder + 1;
                }

                ctx.ProductCategories.Add(category);
                ctx.SaveChanges();
            }

            return category;
        }

        public ResultBase MoveCategory(MoveCategoryParameters parameters)
        {
            parameters.ThrowIfNull("parameters");

            using (var ctx = DbNewContext(parameters.UserId))
            {
                var cat = this.GetById(ctx, parameters.CategoryId); 

                if (cat == null)
                {
                    throw new OperationException("Перемещаемая категория не найдена")
                    {
                        ResultCode = ResultCodes.NOT_FOUND
                    };
                }

                ProductCategory refCat;
                if (parameters.ReferenceCategoryId.HasValue)
                {
                    refCat = this.GetById(parameters.ReferenceCategoryId.Value);

                    if (refCat == null)
                    {
                        throw new OperationException("Относительная категория не найдена")
                                  {
                                      ResultCode = ResultCodes.NOT_FOUND
                                  };
                    }
                }
                else
                {
                    const string Error = "Идентификатор категории относительно которой осуществляется перенос не может быть null";
                    throw new ArgumentException(Error);
                }

                // Обновляем родителя если необходимо
                if (cat.ParentId != refCat.ParentId)
                {
                    var newParent = refCat.ParentId.HasValue
                        ? this.GetById(refCat.ParentId.Value)
                        : null;

                    if (newParent != null && newParent.Type == ProductCategoryTypes.Online)
                    {
                        string mess = string.Format(
                            "Не возможно создать подкатегорию в динамической категории {0}",
                            newParent.Id);
                        throw new OperationException(ResultCodes.PARENT_CATEGORY_IS_DYNAMIC, mess);
                    }

                    var newNamePath = ProductCategoriesDataSource.CreatePath(newParent == null ? null : newParent.NamePath, cat.Name);
                    var existedPathCat = ProductCategoriesDataSource.GetProductCategoryByNamePath(newNamePath);

                    if (existedPathCat != null)
                    {
                        var mess = string.Format("Категория {0} уже существует", newNamePath);
                        throw new OperationException(ResultCodes.CATEGORY_WITH_NAME_EXISTS, mess);
                    }

                    UpdateChildrenNamePaths(parameters.UserId, cat.Id, cat.NamePath, newNamePath);

                    cat.ParentId = refCat.ParentId;
                    cat.NamePath = newNamePath;
                }
                
                // Вставка категории
                if (parameters.PositionType == CategoryPositionTypes.Before)
                {
                    cat.CatOrder = refCat.CatOrder;
                    refCat.CatOrder++;
                   
                    ctx.Entry(cat).State = ctx.Entry(refCat).State = EntityState.Modified;

                    var followedCats = ctx.ProductCategories.Where(
                         c => ((cat.ParentId == null ? c.ParentId == null : c.ParentId == refCat.ParentId) &&
                             c.CatOrder >= refCat.CatOrder &&
                             c.Id != cat.Id &&
                             c.Id != refCat.Id)).ToList();
                    
                    followedCats.ForEach(c => c.CatOrder++);
                }

                if (parameters.PositionType == CategoryPositionTypes.After)
                {
                    cat.CatOrder = refCat.CatOrder;
                    refCat.CatOrder--;
                    
                    ctx.Entry(cat).State = ctx.Entry(refCat).State = EntityState.Modified;

                    var followedCats = ctx.ProductCategories.Where(
                         c => ((cat.ParentId == null ? c.ParentId == null : c.ParentId == refCat.ParentId) &&
                             c.CatOrder <= refCat.CatOrder &&
                             c.Id != cat.Id &&
                             c.Id != refCat.Id)).ToList();

                    followedCats.ForEach(c => c.CatOrder--);
                }
                
                ctx.SaveChanges();
            }

            return ResultBase.BuildSuccess();
        }

        public ProductCategory GetCategoryByName(string name)
        {
            using (var ctx = DbNewContext())
            {
                return ctx.ProductCategories.FirstOrDefault(c => c.Name == name);
            }
        }

        public IList<CategoryMappingProjection> GetCategoryMappings(int partnerId, string key)
        {
            using (var ctx = DbNewContext())
            {
                var adapter = (IObjectContextAdapter)ctx;
                var objectContext = adapter.ObjectContext;

                var param = new object[]
                            {
                                new SqlParameter("@partnerId", partnerId), 
                                new SqlParameter("@key", key)
                            };

                List<CategoryMappingProjection> retVal =
                    objectContext.ExecuteStoreQuery<CategoryMappingProjection>(
                        "exec prod.GetCategoriesMapping @partnerId, @key", param).ToList();

                return retVal;
            }
        }
    }
}