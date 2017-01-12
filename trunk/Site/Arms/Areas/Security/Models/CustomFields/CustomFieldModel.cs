using Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models;

namespace Vtb24.Arms.Security.Models.CustomFields
{
    public class CustomFieldModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static CustomFieldModel Map(CustomField original)
        {
            return new CustomFieldModel
            {
                Id = original.Id,
                Name = original.Name
            };
        }
    }
}
