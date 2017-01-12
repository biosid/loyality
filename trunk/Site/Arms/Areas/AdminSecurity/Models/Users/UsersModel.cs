namespace Vtb24.Arms.AdminSecurity.Models.Users
{
    public class UsersModel
    {
        public UserModel[] Users { get; set; }

        public UsersQueryModel Query { get; set; }

        public int TotalPages { get; set; }
        
    }
}
