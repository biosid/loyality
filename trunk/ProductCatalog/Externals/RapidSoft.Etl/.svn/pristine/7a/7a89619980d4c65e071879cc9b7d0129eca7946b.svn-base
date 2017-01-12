using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Agents.FS
{
    public sealed class FileEtlAgent : ILocalEtlAgent
    {
        #region Constructors

        public FileEtlAgent(EtlAgentInfo agentInfo)
        {
            if (agentInfo == null)
            {
                throw new ArgumentNullException("agentInfo");
            }

            if (string.IsNullOrEmpty(agentInfo.ConnectionString))
            {
                throw new ArgumentException("Parameter \"connectionString\" cannot be empty", "connectionString");
            }

            _directoryPath = agentInfo.ConnectionString;
            _logsDirectoryName = DEFAULT_LOGS_DIRECTORY_NAME;
            _logFileExtension = DEFAULT_LOG_FILE_EXTENSION;
        }

        #endregion

        #region Constants

        private const string DEFAULT_PACKAGE_FILE_EXTENSION = ".etl.xml";

        private const string DEFAULT_LOGS_DIRECTORY_NAME = "Logs";
        private const string DEFAULT_LOG_FILE_EXTENSION = ".log.csv";
        private const int LOG_FILE_BUFFER_SIZE = 8;
        private const int LOG_FILE_CREATE_TRY_COUNT = 255;

        private readonly string LOG_DIRECTORY_NAME_FORMAT = "{0:yyyyMMdd}";
        private readonly string LOG_FILE_NAME_FORMAT = "{0:yyyyMMddHHmmssffff}_{1}";
        //private readonly string LOG_FILE_NAME_FORMAT = "{0:yyyyMMdd}_{1}";

        #endregion

        #region Fields

        private readonly string _directoryPath;
        private readonly string _logsDirectoryName = DEFAULT_LOGS_DIRECTORY_NAME;
        private readonly string _logFileExtension = DEFAULT_LOG_FILE_EXTENSION;
        private readonly List<IEtlLogger> _attachedLoggers = new List<IEtlLogger>();
        
        #endregion

        #region Properties

        public string DirectoryPath
        {
            [DebuggerStepThrough]
            get
            {
                return _directoryPath;
            }
        }

        public string PackageFileExtension
        {
            [DebuggerStepThrough]
            get
            {
                return DEFAULT_PACKAGE_FILE_EXTENSION;
            }
        }

        public string LogsDirectoryName
        {
            [DebuggerStepThrough]
            get
            {
                return _logsDirectoryName;
            }
        }

        public string LogFileExtension
        {
            [DebuggerStepThrough]
            get
            {
                return _logFileExtension;
            }
        }

        public Encoding LogFileEncoding
        {
            [DebuggerStepThrough]
            get
            {
                return Encoding.UTF8;
            }
        }

        #endregion

        #region IEtlAgent Members

        public EtlAgentInfo GetEtlAgentInfo()
        {
            return new EtlAgentInfo
            {
                EtlAgentType = this.GetType().AssemblyQualifiedName,
                ConnectionString = this.DirectoryPath,
                SchemaName = null,
            };
        }

        public void DeployEtlPackage(EtlPackage package, EtlPackageDeploymentOptions options)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            if (options == null)
            {
                options = new EtlPackageDeploymentOptions();
            }

            PrepareDirectory();

            if (options.Overwrite)
            {
                UpdateEtlPackage(package);
            }
            else
            {
                AddEtlPackage(package);
            }
        }

        public EtlPackage[] GetEtlPackages()
        {
            PrepareDirectory();

            var packages = SelectEtlPackages();
            return packages.ToArray();
        }

        public EtlPackage GetEtlPackage(string etlPackageId)
        {
            PrepareDirectory();

            var packageInfo = SelectEtlPackage(etlPackageId);
            return packageInfo;
        }

        public EtlSession InvokeEtlPackage(
            string etlPackageId,
            EtlVariableAssignment[] parameters,
            string parentSessionId,
            string user = null,
            string waitingEtlSessionId = null)
        {
            if (!string.IsNullOrEmpty(waitingEtlSessionId))
            {
                throw new NotSupportedException("Отложенные сессии не поддерживаются FileEtlAgent");
            }

            PrepareDirectory();

            var package = SelectEtlPackage(etlPackageId);
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            using (var defaultLogger = CreateDefaultLogger())
            {
                var loggers = new IEtlLogger[_attachedLoggers.Count + 1];
                loggers[0] = defaultLogger;
                for (var i = 0; i < _attachedLoggers.Count; i++)
                {
                    loggers[i + 1] = _attachedLoggers[i];
                }

                var session = package.Invoke(loggers, parameters, parentSessionId, user);
                return session;
            }
        }

        public IEtlLogParser GetEtlLogParser()
        {
            throw new NotSupportedException();
        }

        public EtlSession CreateWaitingEtlSession(string etlPackageId, string user = null)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region ILocalEtlAgent Members

        public void AttachLogger(IEtlLogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (!_attachedLoggers.Contains(logger))
            {
                _attachedLoggers.Add(logger);
            }
        }

        #endregion

        #region Methods

        private List<EtlPackage> SelectEtlPackages()
        {
            var packages = new List<EtlPackage>();

            if (!Directory.Exists(_directoryPath))
            {
                return packages;
            }

            var filePathes = Directory.GetFiles(_directoryPath, "*" + this.PackageFileExtension);

            foreach (var filePath in filePathes)
            {
                var xml = File.ReadAllText(filePath);
                var package = EtlPackageXmlSerializer.Deserialize(xml);
                if (package != null)
                {
                    packages.Add(package);
                }
            }

            return packages;
        }

        private void AddEtlPackage(EtlPackage package)
        {
            var filePath = Path.Combine(_directoryPath, package.Id.ToString() + this.PackageFileExtension);

            if (File.Exists(filePath))
            {
                throw new EtlPackageAlreadyExistsException(package.Id);
            }

            var xml = EtlPackageXmlSerializer.Serialize(package);
            File.WriteAllText(filePath, xml);
        }

        private void UpdateEtlPackage(EtlPackage package)
        {
            var filePath = Path.Combine(_directoryPath, package.Id.ToString() + this.PackageFileExtension);

            var xml = EtlPackageXmlSerializer.Serialize(package);
            File.WriteAllText(filePath, xml);
        }

        private EtlPackage SelectEtlPackage(string etlPackageId)
        {
            var packages = SelectEtlPackages();
            EtlPackage foundPackage = null;

            foreach (var package in packages)
            {
                if (string.Equals(package.Id, etlPackageId, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (foundPackage != null)
                    {
                        throw new InvalidOperationException(string.Format("There are more than one ETL packages with identifier \"{0}\" in directory \"{1}\"", etlPackageId, _directoryPath));
                    }

                    foundPackage = package;
                }
            }

            return foundPackage;
        }

        private IEtlLogger[] GetAllLoggers()
        {
            var defaultLogger = CreateDefaultLogger();

            var loggers = new IEtlLogger[_attachedLoggers.Count + 1];
            loggers[0] = defaultLogger;

            for (var i = 0; i < _attachedLoggers.Count; i++)
            {
                loggers[i + 1] = _attachedLoggers[i];
            }

            return loggers;
        }

        private IEtlLogger CreateDefaultLogger()
        {
            var currentLogDirectoryName = GetCurrentLogDirectoryName();
            var currentLogDirectoryPath = Path.Combine(Path.Combine(_directoryPath, _logsDirectoryName), currentLogDirectoryName);

            if (!Directory.Exists(currentLogDirectoryPath))
            {
                Directory.CreateDirectory(currentLogDirectoryPath);
            }

            var fileStream = CreateNewLogFileStream(currentLogDirectoryPath);
            var streamWriter = new StreamWriter(fileStream, this.LogFileEncoding);
            var logger = new CsvEtlLogger(streamWriter);
            logger.WriteHeaders();

            return logger;
        }

        private FileStream CreateNewLogFileStream(string directoryPath)
        {
            var fileNumber = 1;
            var tryCount = 0;
            var retry = false;
            string fileName = null;
            string filePath = null;
            Exception lastException = null;

            FileStream stream = null;

            while (tryCount < LOG_FILE_CREATE_TRY_COUNT)
            {
                retry = false;
                tryCount++;

                fileName = GetNewLogFileName(fileNumber);
                filePath = Path.Combine(directoryPath, fileName);

                try
                {
                    stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.Write, LOG_FILE_BUFFER_SIZE);
                }
                catch (IOException exc)
                {
                    lastException = exc;

                    if (File.Exists(filePath))
                    {
                        retry = true;
                    }
                }

                if (retry)
                {
                    fileNumber++;
                }
                else
                {
                    return stream;
                }
            }

            throw new IOException(string.Format("Cannot create log file \"{0}\" after {1} tries", filePath, tryCount), lastException);
        }

        private string GetCurrentLogDirectoryName()
        {
            var dt = DateTime.Now;
            var directoryName = string.Format(LOG_DIRECTORY_NAME_FORMAT, dt);
            return directoryName;
        }

        private string GetNewLogFileName(int fileNumber)
        {
            var dt = DateTime.Now;
            var fileName = string.Concat(string.Format(LOG_FILE_NAME_FORMAT, dt, fileNumber), this.LogFileExtension);
            return fileName;
        }

        private void PrepareDirectory()
        {
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
        }

        #endregion
    }
}