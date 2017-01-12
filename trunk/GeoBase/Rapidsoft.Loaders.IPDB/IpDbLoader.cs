using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using Common;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Logging.Sql;
using RapidSoft.Loaders.IPDB.Entities;

namespace RapidSoft.Loaders.IPDB
{
    public class IpDbLoader
    {
        public void Load()
        {
            var SqlEtlLogger = new SqlEtlLogger("System.Data.SqlClient", LoaderConfiguration.ConnectionString, "dbo");
			var textLogger = new TextEtlLogger(Console.Out);
			var etlLogger = new MultiEtlLogger(textLogger, SqlEtlLogger);

            Guid EtlPackageId = LoaderConfiguration.PackID;

            EtlSession session = new EtlSession
			{
                EtlPackageId = EtlPackageId.ToString(),
                EtlSessionId = Guid.NewGuid().ToString(),
                StartDateTime = DateTime.Now,
                StartUtcDateTime = DateTime.UtcNow,
                StartMessage = "Начало загрузки базы IP-адресов",
                Status = EtlStatus.Started,
            };

            etlLogger.LogEtlSessionStart(session);

            try
            {
				LogEtlMessage(session,etlLogger,"Начало загрузки архива IP-адресов из внешней системы");
                string sDir = DownloadDatabase(new Guid(session.EtlSessionId));
				LogEtlMessage(session, etlLogger, "Загрузка архива IP-адресов завершена");

				LogEtlMessage(session, etlLogger, "Начало распаковки архива IP-адресов");
				List<IpDbRecord> list = ParseDB(sDir);
				LogEtlMessage(session, etlLogger, "Распаковка архива IP-адресов завершена");
				LogEtlMessage(session, etlLogger, "Начало загрузки данных IP-адресов в БД");
				XmlDocument xdList = GenerateXml(list.ToList());
                string sXml = xdList.InnerXml;
                using (SqlConnection connection = new SqlConnection(LoaderConfiguration.ConnectionString))
                {

                    connection.Open();

                    String sProcedureName = "[geopoints].[SyncIPRangesFromBuffer]";

                    SqlCommand command = new SqlCommand(sProcedureName, connection);
                    RepositoriesUtils.AddParameter(command, "@packID", LoaderConfiguration.PackID);
					RepositoriesUtils.AddParameter(command, "@sessionID", session.EtlSessionId);
                    RepositoriesUtils.AddParameter(command, "@xmlData", sXml);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 600000;
                    command.ExecuteNonQuery();

                }
				LogEtlMessage(session, etlLogger, "Загрузка данных IP-адресов в БД завершена");
				session.EndDateTime = DateTime.Now;
				session.EndMessage = "Загрузка базы IP-адресов завершена";
				session.EndUtcDateTime = DateTime.UtcNow;
				session.Status = EtlStatus.Succeeded;
				etlLogger.LogEtlSessionEnd(session);
			}
            catch (Exception ex)
            {
                EtlMessage message = new EtlMessage
                {
                    EtlPackageId = EtlPackageId.ToString(),
                    EtlSessionId = session.EtlSessionId.ToString(),
                    LogDateTime = DateTime.Now,
                    LogUtcDateTime = DateTime.UtcNow,
                    MessageType = EtlMessageType.CriticalError,
                    StackTrace = ex.StackTrace,
                    Text = ex.Message,
                };
                etlLogger.LogEtlMessage(message);
				session.EndDateTime = DateTime.Now;
				session.EndMessage = "Загрузка базы IP-адресов завершена с ошибкой";
				session.EndUtcDateTime = DateTime.UtcNow;
				session.Status = EtlStatus.Failed;
				etlLogger.LogEtlSessionEnd(session);
			}
        }

        private XmlDocument GenerateXml(List<IpDbRecord> p_list)
        {
            XmlDocument xd = new XmlDocument();
            XmlNode xnData = XML.AppendXmlNode(xd, "Data", "");

            foreach (IpDbRecord r in p_list)
            {
                XmlNode xnRec = XML.AppendXmlNode(xnData, "r", "");
                XML.AppendAttribute(xnRec, "fri", r.IPFromNumber.ToString());
                XML.AppendAttribute(xnRec, "toi", r.IPToNumber.ToString());
                XML.AppendAttribute(xnRec, "frs", r.IPFrom);
                XML.AppendAttribute(xnRec, "tos", r.IPTo);
                XML.AppendAttribute(xnRec, "ctr", r.Country);
                XML.AppendAttribute(xnRec, "cty", r.City);
                XML.AppendAttribute(xnRec, "reg", r.Region);
                XML.AppendAttribute(xnRec, "fed", r.FedRegion);
            }

            return xd;        
        }

        private List<IpDbRecord> ParseDB(string p_sDir)
        {
            List<IpDbRecord> res = new List<IpDbRecord>();

            var cities = ReadCities(Path.Combine(p_sDir, "cities.txt"));

            string sOutputFile = Path.Combine(p_sDir, "cidr_optim.txt");
            
            using (FileStream fs = new FileStream(sOutputFile, FileMode.Open))
            {
                byte[] aData = new byte[fs.Length];

                fs.Read(aData, 0, (int)(fs.Length));
                
                string sData = System.Text.Encoding.GetEncoding(1251).GetString(aData);

                //Разные файлы содержат или \r\n или \n просто
                sData = sData.Replace("\r", "");

                string [] aRows = sData.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sRow in aRows)
                {
                    string[] aValues = sRow.Split('\t');
                    IpDbRecord rec = new IpDbRecord();

                    rec.IPFromNumber = long.Parse(aValues[0]);
                    rec.IPToNumber = long.Parse(aValues[1]);

                    string[] aParts = aValues[2].Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

                    rec.IPFrom = aParts[0];
                    rec.IPTo = aParts[1];

                    rec.Country = aValues[3];

                    var cityString = aValues[4];
                    
                    if (cityString == "-")
                    {
                        rec.City = cityString;
                    }
                    else
                    {
                        var city = cities.Single(c => c.Key == cityString);

                        rec.City = city.Value.City;
                        rec.Region = city.Value.Region;
                        rec.FedRegion = city.Value.FedRegion;   
                    }                   
                    
                    res.Add(rec);
                }
            }
            return res;        
        }

        private static Dictionary<string, IpDbCity> ReadCities(string citiesFilePath)
        {
            var cities = new Dictionary<string, IpDbCity>();

            var fileLines = File.ReadAllLines(citiesFilePath, System.Text.Encoding.GetEncoding(1251));

            foreach (var line in fileLines)
            {
                var arr = line.Split('\t');

                cities.Add(arr[0], new IpDbCity()
                {
                    Id = arr[0],
                    City = arr[1],
                    Region = arr[2],
                    FedRegion = arr[3],
                    Latitude = arr[4],
                    Longitude = arr[5]
                });
            }

            return cities;
        }

        private string DownloadDatabase(Guid sessionID)
        {
            string sFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + sessionID.ToString("N");
            string sOutputPath = Path.Combine(LoaderConfiguration.TempFolderPath, sFolderName);


            DirectoryInfo outDir = new DirectoryInfo(sOutputPath);
            if (outDir.Exists == false)
                outDir.Create();

            string sOutputFile = Path.Combine(sOutputPath, "db_files.zip");

            WebClient wc = new WebClient();

            wc.DownloadFile(LoaderConfiguration.IpDbLink, sOutputFile); 
                        

            using (FileStream fs = new FileStream(sOutputFile, FileMode.Open, FileAccess.Read))
            {                                                    
                    using (ZipFile zipFile = new ZipFile(fs))
                    {
                        foreach (ZipEntry ent in zipFile)
                        {                                                                                    
                                using (Stream fileInZipStream = zipFile.GetInputStream(ent))
                                {
                                    string sFilePath = Path.Combine(sOutputPath, ent.Name);
                                    using (FileStream outFileStream = File.OpenWrite(sFilePath))
                                    {
                                        byte[] buf = new byte[4096];
                                        StreamUtils.Copy(fileInZipStream, outFileStream, buf);
                                    }
                                }                                
                            
                        }
                    }                                                                     
            }


            return sOutputPath;
        }

		private void LogEtlMessage(EtlSession session, IEtlLogger logger, string message)
		{
			var etlMessage = new EtlMessage
			{
				EtlPackageId = session.EtlPackageId.ToString(),
				EtlSessionId = session.EtlSessionId.ToString(),
				LogDateTime = DateTime.Now,
				LogUtcDateTime = DateTime.UtcNow,
				MessageType = EtlMessageType.Information,
				Text = message
			};
			logger.LogEtlMessage(etlMessage);
		}

    }

    internal class IpDbCity 
    {
        public string Id
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string Region
        {
            get;
            set;
        }

        public string FedRegion
        {
            get;
            set;
        }

        public string Latitude
        {
            get;
            set;
        }

        public string Longitude
        {
            get;
            set;
        }
    }
}
