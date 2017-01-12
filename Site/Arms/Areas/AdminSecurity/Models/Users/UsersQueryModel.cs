using Vtb24.Arms.Helpers;

namespace Vtb24.Arms.AdminSecurity.Models.Users
{
    public class UsersQueryModel
    {
        // ReSharper disable InconsistentNaming

        public int? page { get; set; }

        // ReSharper restore InconsistentNaming

        public string ToQuery()
        {
            return QueryHelper.ToQuery(this);
        }

        public UsersQueryModel MixQuery(string query, bool overwrite = false)
        {
            return QueryHelper.MixQueryTo(this, query, overwrite);
        }
    }
}
