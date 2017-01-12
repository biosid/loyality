using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.PromoAction.Tests.Entities
{
    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Settings;

    /// <summary>
    /// The entities metadata tests.
    /// </summary>
    [TestClass]
    public class EntitiesMetadataTests
    {
        [TestMethod]
        public void ShouldSerialize()
        {
            var attrKladr = new Attribute
                                {
                                    Name = "KLADR",
                                    DisplayName = "Город присутствия",
                                    Id = Guid.NewGuid().ToString(),
                                    Type = AttributeTypes.Text
                                };
            var attrAudiences = new Attribute
                                    {
                                        Name = ApiSettings.PromoActionPropertyName,
                                        DisplayName = "Целевая аудитория",
                                        Id = Guid.NewGuid().ToString(),
                                        Type = AttributeTypes.Text
                                    };

            var clientMetadata = new EntityMetadata
                                     {
                                         EntityName = ApiSettings.ClientProfileObjectName,
                                         DisplayName = "Пользователь",
                                         Attributes = new[] { attrKladr, attrAudiences }
                                     };

            var attrPartnerId = new Attribute
                                    {
                                        Name = "PartnerId",
                                        DisplayName = "Партнёр",
                                        Id = Guid.NewGuid().ToString(),
                                        Type = AttributeTypes.Number
                                    };
            var attrCategoryId = new Attribute
                                     {
                                         Name = "CategoryId",
                                         DisplayName = "Категория",
                                         Id = Guid.NewGuid().ToString(),
                                         Type = AttributeTypes.Number
                                     };
            var attrProductId = new Attribute
                                    {
                                        Name = "ProductId",
                                        DisplayName = "Товар",
                                        Id = Guid.NewGuid().ToString(),
                                        Type = AttributeTypes.Number
                                    };

            var productMetadata = new EntityMetadata
                                      {
                                          EntityName = "p",
                                          DisplayName = "Вознаграждение",
                                          Attributes =
                                              new[] { attrPartnerId, attrCategoryId, attrProductId }
                                      };

            var entities = new EntitiesMetadata { Entities = new[] { productMetadata, clientMetadata } };

            var ser = entities.Serialize();

            Assert.IsNotNull(ser);

            Console.WriteLine(ser);
        }
    }
}
