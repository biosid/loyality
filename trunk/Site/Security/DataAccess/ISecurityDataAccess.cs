using System;
using System.Data.Entity;
using Vtb24.Site.Security.SecurityService.Models;
using Vtb24.Site.Security.SecurityService.Models.Outputs;

namespace Vtb24.Site.Security.DataAccess
{
    internal interface ISecurityDataAccess: IDisposable
    {
        DbSet<User> Users { get; set; }

        DbSet<PhoneNumberChange> PhoneNumberChangeHistory { get; set; }

        int SaveChanges();

        void ResetLastPasswordFailureDate(int userId);
    }
}
