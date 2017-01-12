using Vtb24.Arms.Infrastructure;

namespace Vtb24.Arms.Security.Models.Users
{
    public class UsersModel
    {
        public bool NoResults { get; set; }

        [Mask("+7 (999) 999-9999")]
        public string Login { get; set; }
    }
}
