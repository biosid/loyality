namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults 
{
    using System.Runtime.Serialization;

    /// <summary>
    /// ������-��������� ���������� ��������.
    /// </summary>
    [DataContract]
    public class CalculateResult : ResultBase
    {
        /// <summary>
        /// �����, ������������ ����� ���������� ���� ������� ������.
        /// ���� ������� ������� �� �����������, �� ������������ �������� �����.
        /// </summary>
        [DataMember]
        public decimal BaseResult { get; set; }

        /// <summary>
        /// ������������� ���������. �����, ������������ ����� ���������� ���� ������.
        /// ���� ������� ������� �� �����������, �� ������������ �������� �����.
        /// </summary>
        [DataMember]
        public decimal PromoResult { get; set; }

        /// <summary>
        /// ��� ����� ���������� ������
        /// </summary>
        [DataMember]
        public RuleApplyStatuses RuleApplyStatus { get; set; }

        [DataMember]
        public string[] TraceMessages { get; set; }

        /// <summary>
        /// ����������� ����������� �������-��������� � ������ ���������� ������ "�� �������".
        /// </summary>
        /// <param name="result">
        /// ���������� ��������� ��������.
        /// </param>
        /// <param name="description">
        /// �������� ����� ���������� ������.
        /// </param>
        /// <returns>
        /// ��������� ������-���������.
        /// </returns>
        public static CalculateResult BuildFail(decimal result, string description)
        {
            return new CalculateResult
                       {
                           ResultCode = ResultCodes.UNKNOWN_ERROR,
                           Success = true,
                           RuleApplyStatus = RuleApplyStatuses.RulesNotExecuted, 
                           ResultDescription = description
                       };
        }

        /// <summary>
        /// ����������� ����������� �������-��������� � ������ ���������� ������ "�������".
        /// </summary>
        /// <param name="retBaseVal">
        ///     ���������� ��������� ��������.
        /// </param>
        /// <param name="promoResult"></param>
        /// <param name="isBaseApplied">
        ///     ������� ����������� �� ������� �������.
        /// </param>
        /// <param name="isNotBaseApplied">
        ///     ������� ����������� �� �� ������� (���������) �������.
        /// </param>
        /// <returns>
        /// ��������� ������-���������.
        /// </returns>
        public static CalculateResult BuildSuccess(decimal retBaseVal, decimal promoResult, bool isBaseApplied, bool isNotBaseApplied, string[] traceMessages)
        {
            RuleApplyStatuses code;

            if (isBaseApplied)
            {
                code = isNotBaseApplied ? RuleApplyStatuses.RulesExecuted : RuleApplyStatuses.BaseOnlyRulesExecuted;
            }
            else
            {
                code = isNotBaseApplied ? RuleApplyStatuses.RulesExecuted : RuleApplyStatuses.RulesNotExecuted;
            }

            return new CalculateResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           BaseResult = retBaseVal,
                           PromoResult = promoResult,
                           RuleApplyStatus = code,
                           ResultDescription = null,
                           TraceMessages = traceMessages
                       };
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Result: {0}, RuleApplyStatus: {1}, ResultDescription: {2}", this.PromoResult, this.RuleApplyStatus, this.ResultDescription);
        }
    }
}