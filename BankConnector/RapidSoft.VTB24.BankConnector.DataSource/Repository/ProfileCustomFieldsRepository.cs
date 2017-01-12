namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class ProfileCustomFieldsRepository : GenericRepository<ProfileCustomField>, IProfileCustomFieldsRepository
    {
        public ProfileCustomFieldsRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public ProfileCustomField[] GetAllFieldsInOrder()
        {
            return this.GetAll().OrderBy(f => f.Order).ToArray();
        }
    }
}
