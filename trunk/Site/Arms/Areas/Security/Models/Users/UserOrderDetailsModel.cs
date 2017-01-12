namespace Vtb24.Arms.Security.Models.Users
{
    public class UserOrderDetailsModel
    {
        // ReSharper disable InconsistentNaming

        public string query { get; set; }

        // ReSharper restore InconsistentNaming

        public string Login { get; set; }

        public UserOrderModel Order { get; set; }

        public UserOrdersQueryModel UserOrdersQueryModel
        {
            get
            {
                return string.IsNullOrWhiteSpace(query)
                           ? new UserOrdersQueryModel()
                           : new UserOrdersQueryModel().MixQuery(query);
            }
        }
    }
}
