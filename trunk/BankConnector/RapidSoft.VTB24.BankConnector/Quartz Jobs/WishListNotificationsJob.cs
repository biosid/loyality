namespace RapidSoft.VTB24.BankConnector.Quartz_Jobs
{
    using System.Configuration;
    using System.Threading;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Quartz;
    using RapidSoft.VTB24.BankConnector.Processors;

    [GroupDisallowConcurrentExecution]
    public class WishListNotificationsJob : IInterruptableJob
    {
        #region IInterruptableJob Members

        public void Execute(IJobExecutionContext context)
        {
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

            var wishListProcessor = new UnityContainer().LoadConfiguration(section).Resolve<WishListProcessor>();

            wishListProcessor.SendWishListNotifications();
        }

        public void Interrupt()
        {
            Thread.CurrentThread.Abort();
        }

        #endregion
    }
}