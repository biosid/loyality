using Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models;

namespace Vtb24.Arms.AdminServices.AdminVtbBankConnector.Stubs
{
    public class AdminVtbBankConnectorStub : IAdminVtbBankConnector
    {
        public void ChangeClientPhoneNumber(string clientId, string phoneNumber)
        {
        }

        public void ChangeClientEmail(string clientId, string email)
        {
            throw new System.NotImplementedException();
        }

        public CustomField[] GetAllCustomFields()
        {
            return new[]
            {
                new CustomField { Id = 1, Name = "Field1" },
                new CustomField { Id = 2, Name = "Field2" }
            };
        }

        public int AppendCustomField(string name)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveCustomField(int id)
        {
            throw new System.NotImplementedException();
        }

        public void RenameCustomField(int id, string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
