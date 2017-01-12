
namespace RapidSoft.YML
{
    using System.Collections.Generic;
    using System.Linq;

    using Entities;

    using Extensions;

    public static class OfferCollectionExtensions
    {
        #region Фильры

        public static IEnumerable<GenericOffer> Availible(this IEnumerable<GenericOffer> col)
        {
            return col.Where(c => c.IsAvailible.IsNullOr(true));
        }

        public static IEnumerable<GenericOffer> WithPicture(this IEnumerable<GenericOffer> col)
        {
            return col.Where(c => c.Picture != null);
        }

        public static IEnumerable<GenericOffer> InCategories(this IEnumerable<GenericOffer> col, IEnumerable<string> categories)
        {
            return col.Where(c => c.Categories.IntersectsWith(categories));
        }

        #endregion

        #region Конвертация

        public static IEnumerable<IOffer> ToConcreteOffers(this IEnumerable<GenericOffer> col)
        {
            return col.Select(c => c.ToConcreteOffer());
        }

        #endregion
    }
}