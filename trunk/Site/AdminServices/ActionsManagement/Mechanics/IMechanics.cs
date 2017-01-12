using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics
{
    public interface IMechanics
    {
        Mechanic[] GetAllMechanics();

        Mechanic GetMechanic(long id);

        Metadata[] GetMetadataByMechanicId(long id);
    }
}
