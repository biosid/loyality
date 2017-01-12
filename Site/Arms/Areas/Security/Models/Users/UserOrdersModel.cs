namespace Vtb24.Arms.Security.Models.Users
{
    public class UserOrdersModel
    {
        public UserOrdersQueryModel Query { get; set; }

        public UserOrderModel[] Orders { get; set; }

        public int TotalPages { get; set; }
    }
}
