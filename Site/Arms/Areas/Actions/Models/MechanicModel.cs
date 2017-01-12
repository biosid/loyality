using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;

namespace Vtb24.Arms.Actions.Models
{
    public class MechanicModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public static MechanicModel Map(Mechanic original)
        {
            return new MechanicModel
            {
                Id = original.Id,
                Name = original.Name
            };
        }
    }
}
