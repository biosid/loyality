using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Vtb24.Site.Services.Infrastructure
{
    public static class SiteUploadsHelper
    {
        public static string SavePath
        {
            get { return ConfigurationManager.AppSettings["site_uploads_save_path"]; }
        }

        public static string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["site_uploads_base_url"]; }
        }

        public static string GetUploadUrl(Guid id, string fileName)
        {
            return string.Format("{0}{1}/{2}", BaseUrl, id, fileName);
        }

        public static UploadInfo FindUpload(Guid id)
        {
            var prefix = GetFilenamePrefix(id);
            var folder = Path.Combine(SavePath, id.ToString());
            var path = Directory
                .EnumerateFiles(folder)
                .FirstOrDefault(f => Path.GetFileName(f).StartsWith(prefix));

            if (path == null)
            {
                return null;
            }


            var fileName = Path.GetFileName(path).Replace(prefix, "");
            var size = new FileInfo(path).Length;

            return new UploadInfo
            {
                Id = id,
                FileName = fileName,
                FileSize = (int) size,
                Path = path
            };
        }

        public static UploadInfo SaveUpload(string fileName, Stream upload)
        {
            var id = Guid.NewGuid();
            fileName = string.IsNullOrEmpty(fileName) ? id.ToString() : fileName;
            var path = CreatePath(id, fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (var file = File.Create(path, (int) upload.Length))
            {
                upload.Seek(0, SeekOrigin.Begin);
                upload.CopyTo(file);
                file.Flush();
            }

            return new UploadInfo
            {
                Id = id,
                FileName = fileName,
                FileSize = (int) upload.Length,
                Path = path
            };
        }


        #region  Приватные методы

        private static string CreatePath(Guid id, string fileName)
        {
            var folder = Path.Combine(SavePath, id.ToString());
            var prefix = GetFilenamePrefix(id);
            var path = Path.Combine(folder, prefix+fileName);
            return path;
        }

        private static string GetFilenamePrefix(Guid id)
        {
            return id.ToString().Substring(9, 4) + "_";
        }

        #endregion


        public class UploadInfo
        {
            public Guid Id { get; set; }

            public string FileName { get; set; }

            public int FileSize { get; set; }

            public string Path { get; set; }
            
        }
    }
}