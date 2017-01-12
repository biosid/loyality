namespace RapidSoft.VTB24.BankConnector.Quartz_Jobs
{
    using System;
    using System.Linq;

    using Quartz;

    using RapidSoft.Etl.Runtime;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;

    public class JobExecutionParametersBuilder
    {
        public static JobExecutionParameters BuildExecutionParameters(IJobExecutionContext context, EtlVariableAssignment[] addAssigments = null)
        {
            var assignments = context.MergedJobDataMap.GEtlVariableAssignments();

            if (addAssigments != null)
            {
                assignments = assignments.Union(addAssigments).ToArray();
            }

            var executionParameters = new JobExecutionParameters()
            {
                Assignments = assignments,
                JobName = context.JobDetail.Key.Name
            };

            GetPackageId(executionParameters);

            return executionParameters;
        }

        private static void GetPackageId(JobExecutionParameters executionParameters)
        {
            var packageId = executionParameters.Assignments.SingleOrDefault(a => a.Name == EtlVariableKeys.PackageId);

            if (packageId == null)
            {
                throw new Exception(string.Format("Package id for job:{0} not defined", executionParameters.JobName));
            }

            executionParameters.PackageId = packageId.Value;
        }
    }
}