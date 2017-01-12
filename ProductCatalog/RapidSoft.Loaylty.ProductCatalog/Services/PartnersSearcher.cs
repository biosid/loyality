namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System.Configuration;
    using System.Linq;

    using API.Entities;

    using DataSources.Interfaces;
    using DataSources.Repositories;

    using Interfaces;

    public class PartnersSearcher : IPartnersSearcher
    {
        private readonly IPartnerRepository partnersDataSource;

        public PartnersSearcher()
        {
            partnersDataSource = new PartnerRepository();
        }

        #region IPartnersSearcher Members

        public Partner[] Search()
        {
            return partnersDataSource.GetAllPartners().ToArray();
        }

        #endregion
    }
}