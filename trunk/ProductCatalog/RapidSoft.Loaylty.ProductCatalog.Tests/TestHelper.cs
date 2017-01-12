namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Security.Principal;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Entities;
    using RapidSoft.Loaylty.ProductCatalog.ImportTests;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    public static class TestHelper
    {
        public const string ProductId = "-=TEST=-";
        public const string CategoryName = TestDataStore.TestCategoryName;
        public const string UserId = TestDataStore.TestUserId;

        public static ProductCategory NewCategory(int? parentId = null, ProductCategoryStatuses status = ProductCategoryStatuses.Active)
        {
            var parameters = new CreateCategoryParameters
                                 {
                                     UserId = UserId,
                                     Name = CategoryName + Guid.NewGuid(),
                                     Status = status,
                                     Type = ProductCategoryTypes.Static,
                                     OnlineCategoryUrl = "url",
                                     ParentCategoryId = parentId
                                 };

            var createCategoryResult = MockFactory.GetCatalogAdminService().CreateCategory(parameters);

            var productCategory = createCategoryResult.Category;

            if (!createCategoryResult.Success && createCategoryResult.ResultCode == ResultCodes.CATEGORY_WITH_NAME_EXISTS)
            {
                productCategory = new ProductCategoryRepository().GetCategoryByName(parameters.Name);
            }

            MockFactory.GetCatalogAdminService().SetCategoriesPermissions(new SetCategoriesPermissionsParameters()
            {
                AddCategoriesId = new int[] {productCategory.Id},
                PartnerId = TestDataStore.OzonPartnerId,
                UserId = TestDataStore.TestUserId
            });
            
            return productCategory;
        }

        private static string GetTableSuffixSql()
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                var tableSuffixSql =
                    String.Format(
                        "SELECT [Key] FROM [prod].[PartnerProductCatalogs] WHERE [IsActive] = 1 AND [PartnerId] = {0}", TestDataStore.PartnerId);
                string tableSuffix;
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = tableSuffixSql;
                    tableSuffix = comm.ExecuteScalar().ToString();
                }

                return tableSuffix;
            }
        }

        public static void DeleteTestProduct(string tableSuffix = null)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                if (tableSuffix == null)
                {
                    tableSuffix = GetTableSuffixSql();
                }

                var deleteTestProductSql =
                        String.Format(
                            "DELETE FROM [prod].[Products_{1}] WHERE [PartnerProductId] LIKE '{0}%'",
                            ProductId,
                            tableSuffix);

                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = deleteTestProductSql;
                    comm.ExecuteNonQuery();
                }
            }
        }

        public static string CreateTestProduct(int index, int weight, int price = 50, bool cleanBefore = false, ProductCategory category = null)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                string tableSuffix = GetTableSuffixSql();

                if (cleanBefore)
                {
                    DeleteTestProduct(tableSuffix);
                }

                var retVal = ProductId + index.ToString(CultureInfo.InvariantCulture);

                var cat = category ?? NewCategory();

                var insertTestProductSql = String.Format(
@"INSERT INTO [prod].[Products_{3}]
([ProductId],Name,[InsertedDate],[PartnerId],[PartnerProductId],[PriceRUR]
,[CurrencyId],[CategoryId],[Status],[ModerationStatus],[Weight],[Available])
VALUES
('{2}_{0}','{0}',GETDATE(),{2},'{0}',{4},'RUR',{5},1,2,{1},1)", 
           retVal, 
           weight, 
           TestDataStore.PartnerId, 
           tableSuffix, 
           price,
           cat.Id);

                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = insertTestProductSql;
                    comm.ExecuteNonQuery();
                }

                return String.Format("{1}_{0}", retVal, TestDataStore.PartnerId);
            }
        }

        public static void CreateDeliveryMatrix()
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                var testRepo = new TestDeliveryRatesRepository();

                testRepo.DeleteByPartnerIdAndKladrCodeLike(TestDataStore.PartnerId, "98%");

                testRepo.Create(TestDataStore.PartnerId, 0, 5000, 50, "9800000000000");
                testRepo.Create(TestDataStore.PartnerId, 5001, 10000, 100, "9800000000000");
                testRepo.Create(TestDataStore.PartnerId, 0, 4000, 30, "9800000100000");
            }
        }

        public static PhoneNumber GetTestPhone()
        {
            return new PhoneNumber { CountryCode = "7", CityCode = "495", LocalNumber = "4561278" };
        }

        public static Contact GetTestContact(PhoneNumber phone = null)
        {
            return new Contact
                       {
                           FirstName = "Иван",
                           LastName = "Иванов",
                           MiddleName = "Иванович",
                           Phone = phone ?? GetTestPhone(),
                           Email = "a@a.a"
                       };
        }

        public static Contact[] GetTestContacts(PhoneNumber phone = null)
        {
            return new[] { GetTestContact(phone) };
        }

        public static DeliveryDto BuildDeliveryInfo(string kladr)
        {
            var contact = new Contact
            {
                Email = "Email@ru.ru",
                FirstName = "FirstName",
                LastName = "LastName",
                Phone = new PhoneNumber() { LocalNumber = "9751680", CityCode = "234", CountryCode = "7" }
            };
            return new DeliveryDto()
                {
                    Address = new DeliveryAddress()
                    {
                        StreetTitle = "ул. Островетянова",
                        House = "26б"                  
                    },
                    Contact = contact
                };
        }

        public static string GetAnyProductId(params string[] skipProductId)
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                IQueryable<ProductSortProjection> query = ctx.ProductSortProjections;
                
                if (skipProductId != null && skipProductId.Length > 0)
                {
                    query = query.Where(x => !skipProductId.Contains(x.ProductId));
                }

                var products = query.Take(100).ToArray();
                return products.Last().ProductId;
            }
        }

        public static Product BuildTestProduct()
        {
            var product = new Product
            {
                ProductId = "ProductId",
                PartnerProductId = "PartnerProductId",
                PartnerId = 1,
                Name = "Name",
                CategoryId = 56,
                CategoryName = "CategoryName",
                CategoryNamePath = "CategoryNamePath",
                Status = ProductStatuses.Active,
                ModerationStatus = ProductModerationStatuses.Applied,
                StatusChangedCause = "StatusChangedCause",
                StatusChangedDate = new DateTime(2010, 11, 9, 8, 7, 6, 5),
                StatusChangedUser = "StatusChangedUser",
                LastChangedDate = new DateTime(2010, 10, 5, 4, 3, 2, 1),
                LastChangedUser = "LastChangedUser",
                Description = @"Description",
                PartnerCategoryId = "55",
                Available = true,
                CurrencyId = "CurrencyId",
                Type = "Type",
                Url = "Url",
                Pictures = new[]
                {
                    "Pictures"
                },
                Vendor = "Vendor",
                VendorCode = "VendorCode",
                Bid = 5,
                CBid = 6,
                TypePrefix = "TypePrefix",
                Model = "Model",
                Store = false,
                Pickup = true,
                Delivery = false,
                LocalDeliveryCost = 100,
                SalesNotes = "SalesNotes SalesNotes SalesNotes SalesNotes SalesNotes SalesNotes конец",
                ManufacturerWarranty = false,
                CountryOfOrigin = "CountryOfOrigin",
                Downloadable = false,
                Adult = "Adult",
                Barcode = new[]
                {
                    "Barcode"
                },
                Param = new[]
                {
                    new ProductParam
                    {
                        Name = "Name",
                        Unit = "Unit",
                        Value = "Value"
                    }
                },
                Author = "Author",
                Publisher = "Publisher",
                Series = "Series",
                Year = 200,
                ISBN = "ISBN",
                Volume = 5,
                Part = 3,
                Language = "Language",
                Binding = "Binding",
                PageExtent = 2,
                TableOfContents = "TableOfContents",
                PerformedBy = "PerformedBy",
                PerformanceType = "PerformanceType",
                Format = "Format",
                Storage = "Storage",
                RecordingLength = "RecordingLength",
                Artist = "Artist",
                Media = "Media",
                Starring = "Starring",
                Director = "Director",
                OriginalName = "OriginalName",
                Country = "Country",
                WorldRegion = "WorldRegion",
                Region = "Region",
                Days = 4,
                DataTour = "DataTour",
                HotelStars = "HotelStars",
                Room = "Room",
                Meal = "Meal",
                Included = "Included",
                Transport = "Transport",
                Place = "Place",
                HallPlan = "HallPlan",
                Date = new DateTime(2010, 5, 4, 3, 2, 1, 0),
                IsPremiere = false,
                IsKids = true,
                Weight = 50,
                InsertedDate = new DateTime(2010, 5, 5, 5, 5, 5, 5),
                UpdatedDate = new DateTime(2010, 4, 4, 4, 4, 4, 4),
                PriceRUR = 5.5m,
                PriceBase = 4.4m,
                Price = 3.3m,
                IsActionPrice = true
            };
            return product;
        }

        public static IVtb24Principal BuildSuperUser()
        {
            return new SuperPrincipal { Identity = new SuperIdentity() };
        }

        public class SuperIdentity : IIdentity
        {
            public string Name
            {
                get
                {
                    return "User";
                }
            }

            public string AuthenticationType
            {
                get
                {
                    return "AuthenticationType";
                }
            }

            public bool IsAuthenticated
            {
                get
                {
                    return true;
                }
            }
        }

        public class SuperPrincipal : IVtb24Principal
        {
            public IIdentity Identity { get; set; }

            public IList<string> List = new List<string>
                                        {
                                            ArmPermissions.ARMProductCatalogLogin,
                                            ArmPermissions.ProductCategories,
                                            ArmPermissions.ProductCategoriesManage,
                                            ArmPermissions.ProductCategoryLinks,
                                            ArmPermissions.Partners,
                                            ArmPermissions.PartnersCreateUpdateDelete,
                                            ArmPermissions.PartnersDeliveryMatrix,
                                            ArmPermissions.Products,
                                            ArmPermissions.ProductsImportCatalog,
                                            ArmPermissions.ProductsCreateUpdate,
                                            ArmPermissions.ProductsDelete,
                                            ArmPermissions.ProductsAssignAudience,
                                            ArmPermissions.ProductsChangeProductCategory,
                                            ArmPermissions.ProductsModerate,
                                            ArmPermissions.ProductsRecommend,
                                            ArmPermissions.OrdersChangeOrderStatus,
                                            ArmPermissions.Orders,
                                        };
            
            public bool HasPermission(string permission)
            {
                // return true;
                var retVal = this.List.Any(x => x.ToLower() == permission.ToLower());
                return retVal;
            }

            public bool HasPermissions(IEnumerable<string> permissions)
            {
                // return true;
                return permissions.All(this.HasPermission);
            }

            public bool HasPermissionForPartner(string permission, string partnerId)
            {
                // return true;
                return this.HasPermission(permission);
            }

            public bool HasPermissionsForPartner(IEnumerable<string> permissions, string partnerId)
            {
                // return true;
                return permissions.All(this.HasPermission);
            }
        }

        public static Product NewPublicProduct(CreateProductParameters parameters)
        {
            var res = MockFactory.GetCatalogAdminService().CreateProduct(parameters);
            
            if (!res.Success)
            {
                throw new Exception(res.ResultDescription);
            }

            ActivateProduct(res.Product.ProductId);
            ModerateProduct(res.Product.ProductId);

            return res.Product;
        }

        public static void ActivateProduct(string productId)
        {
            var res = MockFactory.GetCatalogAdminService().ChangeProductsStatus(new ChangeStatusParameters()
            {
                UserId = TestDataStore.TestUserId,
                ProductIds = new string[] { productId },
                ProductStatus = ProductStatuses.Active
            });

            if (!res.Success)
            {
                throw new Exception(res.ResultDescription);
            }            
        }

        public static void ModerateProduct(string productId)
        {
            var res = MockFactory.GetCatalogAdminService().ChangeProductsModerationStatus(new ChangeModerationStatusParameters()
            {
                UserId = TestDataStore.TestUserId,
                ProductIds = new string[] { productId },
                ProductModerationStatus = ProductModerationStatuses.Applied
            });

            if (!res.Success)
            {
                throw new Exception(res.ResultDescription);
            }
        }

        public static void AssertResult<T>(T result) where T : ResultBase
        {
            Assert.IsNotNull(result, string.Format("Object {0} is null", result.GetType()));
            Assert.IsTrue(result.Success, result.ResultDescription);
        }
    }
}
