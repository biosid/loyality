using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using WebGrease.Css;

namespace Vtb24.Arms.Helpers
{
    public static class PartnerHelpers
    {
        public static void ImportMatrixForPartner(this HttpPostedFileBase file, IGiftShopManagement catalog, int partnerId, Controller controller)
        {
            var url = SaveImportFile(file, "partnermatrix");

            if (!string.IsNullOrEmpty(url))
            {
                catalog.ImportDeliveryRatesFromHttp(partnerId, url);
            }
        }

        public static void ImportGifts(this HttpPostedFileBase file, IGiftShopManagement catalog, int partnerId)
        {
            var url = SaveImportFile(file, "gifts");

            if (!string.IsNullOrEmpty(url))
            {
                catalog.ImportProductsFromYml(partnerId, url);
            }
        }

        public static string SaveGiftImage(this HttpPostedFileBase file, string productId)
        {
            var basePath = ConfigurationManager.AppSettings["catalog_gift_images_save_path"];
            var baseUrl = ConfigurationManager.AppSettings["catalog_gift_images_base_url"];

            var imagesDirName = SanitizeFilename(productId);
            var imageExtension = Path.GetExtension(file.FileName);
            var imageName = Guid.NewGuid().ToString("N") + imageExtension;

            var imagesDirPath = Path.Combine(basePath, imagesDirName);
            var imagePath = Path.Combine(imagesDirPath, imageName);

            Directory.CreateDirectory(imagesDirPath);
            file.SaveAs(imagePath);

            // используем UrlPathEncode для имени каталога с изображениями товара,
            // так как в id продукта могут быть недопустимые в урл символы (как минимум пробел)
            var imageUrl = baseUrl + HttpUtility.UrlPathEncode(imagesDirName) + "/" + imageName;

            return imageUrl;
        }

        public static void DeleteGiftImage(string productId, string fileUrl)
        {
            var baseUrl = ConfigurationManager.AppSettings["catalog_gift_images_base_url"];

            if (fileUrl.StartsWith(baseUrl))
            {
                var filePath = fileUrl.Substring(baseUrl.Length);

                // получаем имя каталога с изображениями товара и декодируем его
                var slashIndex = filePath.IndexOf('/');
                if (slashIndex != -1)
                {
                    filePath = HttpUtility.UrlDecode(filePath.Substring(0, slashIndex)) + filePath.Substring(slashIndex);
                }

                var basePath = ConfigurationManager.AppSettings["catalog_gift_images_save_path"];

                var path = basePath + filePath;

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        private static string SaveImportFile(this HttpPostedFileBase file, string type)
        {
            if (file == null || file.ContentLength <= 0 || string.IsNullOrEmpty(type))
                return string.Empty;

            var fileName = Path.GetFileName(file.FileName);

            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            var fileFolderName = Guid.NewGuid().ToString();
            var folderPath = Path.Combine(ConfigurationManager.AppSettings["import_save_path"], type, fileFolderName);

            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            file.SaveAs(filePath);

            return string.Format("{0}{1}/{2}/{3}", ConfigurationManager.AppSettings["import_base_url"], type, fileFolderName, fileName);
        }

        private static string SanitizeFilename(string name)
        {
            return Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c, '_'));
        }
    }
}
