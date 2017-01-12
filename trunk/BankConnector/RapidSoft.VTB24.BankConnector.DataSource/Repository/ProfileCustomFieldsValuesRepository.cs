namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class ProfileCustomFieldsValuesRepository : GenericRepository<ProfileCustomFieldsValue>, IProfileCustomFieldsValuesRepository
    {
        public ProfileCustomFieldsValuesRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public Dictionary<int, string> GetByClientId(string clientId)
        {
            return this.GetAll().Where(v => v.ClientId == clientId).ToDictionary(v => v.FieldId, v => v.Value);
        }
    }
}
