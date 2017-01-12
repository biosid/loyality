namespace Rapidsoft.Loyalty.NotificationSystem.Tests
{
    using System;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using API.Entities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class InitTests
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            try
            {
                NotificationDB.DeleteTestData();

                NotificationDB.CreateTestData();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
        }
    }
}