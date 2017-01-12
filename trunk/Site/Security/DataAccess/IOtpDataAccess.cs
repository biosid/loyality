using System;
using System.Data.Entity;
using Vtb24.Site.Security.OneTimePasswordService.Models;

namespace Vtb24.Site.Security.DataAccess
{
    internal interface IOtpDataAccess : IDisposable
    {
        DbSet<OtpToken> Tokens { get; set; }
        int SaveChanges();
    }
}