using System.Collections.Generic;
using System.IO;
using Ionic.Zip;

namespace RapidSoft.VTB24.BankConnector.Tests.Helpers
{
    using System.Text;

    public static class FileReaderHelper
	{
		public static List<string> ReadAllLinesFromFolderFiles(string folder, out int fileCount, string searchPattern = null, bool uncompress = false)
		{
			if (!Directory.Exists(folder))
			{
                fileCount = 0;
                return new List<string>();
			}

			var filesList = searchPattern == null ? Directory.GetFiles(folder) : Directory.GetFiles(folder, searchPattern);
		    fileCount = filesList.Length;

			var result = new List<string>();
			foreach (var file in filesList)
			{
                if (uncompress)
                {
                    UncompressFile(file, folder);
                }

				using (StreamReader sr = new StreamReader(Path.Combine(folder, file), Encoding.GetEncoding(1251)))
				{
					string line;
					while (!string.IsNullOrEmpty(line = sr.ReadLine()))
					{
						result.Add(line);
					}
				}
			}

			return result;
		}

        public static void UncompressFile(string fileName, string folder)
        {
            var zipFileName = fileName + ".zip";
            File.Move(fileName, zipFileName);
            using (var zipFile = ZipFile.Read(zipFileName))
            {
                foreach (var entry in zipFile)
                {
                    entry.Extract(folder, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }

        public static bool IsFilesEquals(string file1, string file2)
        {
            int file1byte;
            int file2byte;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            using(var fs1 = new FileStream(file1, FileMode.Open))
            using (var fs2 = new FileStream(file2, FileMode.Open))
            {

                // Check the file sizes. If they are not the same, the files 
                // are not the same.
                if (fs1.Length != fs2.Length)
                {
                    // Close the file
                    fs1.Close();
                    fs2.Close();

                    // Return false to indicate files are different
                    return false;
                }

                // Read and compare a byte from each file until either a
                // non-matching set of bytes is found or until the end of
                // file1 is reached.
                do
                {
                    // Read one byte from each file.
                    file1byte = fs1.ReadByte();
                    file2byte = fs2.ReadByte();
                }
                while ((file1byte == file2byte) && (file1byte != -1));

                // Close the files.
                fs1.Close();
                fs2.Close();
            }

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return ((file1byte - file2byte) == 0);
        }
	}
}
