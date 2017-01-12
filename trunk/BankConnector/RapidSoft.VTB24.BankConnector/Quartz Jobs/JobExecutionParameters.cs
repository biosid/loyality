namespace RapidSoft.VTB24.BankConnector.EtlExecutionWrapper
{
    using RapidSoft.Etl.Runtime;

    public class JobExecutionParameters
    {
        public string JobName
        {
            get;
            set;
        }

        public EtlVariableAssignment[] Assignments
        {
            get;
            set;
        }

        public string PackageId
        {
            get;
            set;
        }
    }
}