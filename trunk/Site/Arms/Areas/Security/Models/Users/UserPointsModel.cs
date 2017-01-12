namespace Vtb24.Arms.Security.Models.Users
{
    public class UserPointsModel
    {
        public UserPointsQueryModel Query { get; set; }

        public UserPointsOperationModel[] Operations { get; set; }

        public decimal Balance { get; set; }

        public decimal Total { get; set; }

        public int TotalPages { get; set; }
    }
}
