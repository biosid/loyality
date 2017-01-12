using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vtb24.Site.Content.MyPointImages.Models;
using Vtb24.Site.Content.DataAccess;
using Vtb24.Site.Content.Infrastructure;
using Vtb24.Site.Services.MyPointImagesService;

namespace Vtb24.Site.Content.MyPointImages
{
    class MyPointImagesService : IMyPointImagesService
    {
        public string[] GetMyPointImages(string[] descriptions)
        {
            using (var context = new ContentServiceDbContext())
            {
                var hashes = descriptions.Where(d => !String.IsNullOrEmpty(d)).Distinct().Select(d => d.GetHashString()).ToArray();

                var imageCatalog = context.ImageCatalogs
                                          .Where(i => hashes.Contains(i.HashCode)).ToDictionary(i => i.Description, i => i.ImagePath);

                var myPointImages =
                    descriptions.Select(d => !String.IsNullOrEmpty(d) && imageCatalog.ContainsKey(d) ? imageCatalog[d] : null).ToArray();

                return myPointImages;
            }
            
        }
    }
}
