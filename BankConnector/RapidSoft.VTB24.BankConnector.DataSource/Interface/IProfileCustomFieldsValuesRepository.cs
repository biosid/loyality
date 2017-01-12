namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System.Collections.Generic;

    using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IProfileCustomFieldsValuesRepository : IGenericRepository<ProfileCustomFieldsValue>
    {
        Dictionary<int, string> GetByClientId(string clientId);
    }
}
