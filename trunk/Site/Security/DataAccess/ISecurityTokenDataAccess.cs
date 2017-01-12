using System;
using System.Data.Entity;
using Vtb24.Site.Security.SecurityTokenService.Models;

namespace Vtb24.Site.Security.DataAccess
{
    internal interface ISecurityTokenDataAccess : IDisposable
    {
        DbSet<SecurityToken> SecurityTokens { get; set; }
        int SaveChanges();
    }
}