using Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models;

namespace Vtb24.Arms.AdminServices
{
    public interface IAdminVtbBankConnector
    {
        void ChangeClientPhoneNumber(string clientId, string phoneNumber);

        void ChangeClientEmail(string clientId, string email);

        CustomField[] GetAllCustomFields();

        int AppendCustomField(string name);

        void RemoveCustomField(int id);

        void RenameCustomField(int id, string name);
    }
}