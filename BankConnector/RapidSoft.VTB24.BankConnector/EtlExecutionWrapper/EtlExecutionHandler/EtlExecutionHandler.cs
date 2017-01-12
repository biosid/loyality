namespace RapidSoft.VTB24.BankConnector.EtlExecutionWrapper.EtlExecutionHandler
{
    using System;
    using System.Configuration;
    using System.Linq;

    using RapidSoft.Etl.Runtime;
    using RapidSoft.Etl.Runtime.Steps;
    using RapidSoft.VTB24.BankConnector.Configuration;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper.Email;

    internal class EtlExecutionHandler
    {
        public EtlExecutionAbortReason CheckAbortReason(EtlVariableAssignment[] assignments)
        {
            var checkNewEmails = assignments.SingleOrDefault(a => a.Name == EtlVariableKeys.CheckNewEmails).GetBool();

            if (checkNewEmails == true)
            {
                return this.CheckEmailReason(assignments);
            }

            return null;
        }

        private EtlExecutionAbortReason CheckEmailReason(EtlVariableAssignment[] assignments)
        {
            var subjectTerm = assignments.SingleOrDefault(a => a.Name == EtlVariableKeys.SubjectTerm);

            if (subjectTerm == null)
            {
                throw new Exception("SubjectTerm for email check not found");
            }

            var email = new EmailSearcher(this.GetLoyaltyServerSettings()).Find(subjectTerm.Value);

            if (email != null)
            {
                return null;
            }
            else
            {
                return new EtlExecutionAbortReason()
                       {
                           Message = string.Format("Email with subjectTerm:{0} not found.", subjectTerm.Value)
                       };
            }
        }

        private EtlEmailServer GetLoyaltyServerSettings()
        {
            return new EtlEmailServer
            {
                Host = EtlConfigSection.GetEtlVar("LoyaltyImapHost"),
                Port = EtlConfigSection.GetEtlVar("LoyaltyImapPort"),
                UseSSL = EtlConfigSection.GetEtlVarOrDefault("LoyaltyImapUseSSL"),
                UserName = EtlConfigSection.GetEtlVar("LoyaltyImapUserName"),
                Password = EtlConfigSection.GetEtlVar("LoyaltyImapPassword"),
            };
        }
    }
}