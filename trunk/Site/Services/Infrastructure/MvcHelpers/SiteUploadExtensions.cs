using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Vtb24.Site.Services.Infrastructure.MvcHelpers
{
    public static class SiteUploadExtensions
    {
        public static IEnumerable<SiteUploadsHelper.UploadInfo> SaveUploads(this HttpPostedFileBase[] files)
        {
            var uploads = new List<SiteUploadsHelper.UploadInfo>();

            if (files == null)
            {
                return uploads;
            }

            foreach (var file in files.Where(f => f != null))
            {
                var name = Path.GetFileName(file.FileName);
                uploads.Add(SiteUploadsHelper.SaveUpload(name, file.InputStream));
            }

            return uploads;
        }

        public static void ValidateFileSizes(this HttpPostedFileBase[] files, int maxFileSizeMb, Action<string, int, HttpPostedFileBase> action)
        {
            if (files == null)
            {
                return;
            }
            var maxContentLength = maxFileSizeMb * 1024 * 1024;
            for (var i = 0; i < files.Length; i++)
            {
                if (files[i] != null && files[i].ContentLength > maxContentLength)
                {
                    action(
                        string.Format("Превышен максимальный размер загружаемого файла {0}", Path.GetFileName(files[i].FileName)),
                        i,
                        files[i]
                    );
                }
            }
        }

        public static void ValidateTotalSize(this HttpPostedFileBase[] files, int maxTotalSizeMb, Action<string, int> action)
        {
            if (files == null)
            {
                return;
            }

            var maxContentLength = maxTotalSizeMb * 1024 * 1024;
            var totalSize = files.Where(f => f != null).Sum(f => f.ContentLength);

            if (totalSize > maxContentLength)
            {
                action(
                    "Превышен общий размер прикрепляемых файлов",
                    totalSize
                );
            }
        }
    }
}