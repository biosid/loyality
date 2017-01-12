namespace Vtb24.Site.Infrastructure.Caching
{
    public interface ICacheCleaner
    {
        void CleanBasket();

        void CleanWishList();
    }
}
