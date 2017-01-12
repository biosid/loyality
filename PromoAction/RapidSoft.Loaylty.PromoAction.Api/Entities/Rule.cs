namespace RapidSoft.Loaylty.PromoAction.Api.Entities 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;

    /// <summary>
    /// ������� �������������� �����
    /// </summary>
    [DataContract]
    public class Rule : BaseEntity, ITraceableEntity<long>, IHistoryTraceable<RuleHistory>, IValidatableObject, IResettable<Rule>
    {
        /// <summary>
        /// ������� (��������) � ������� xml.
        /// </summary>
        private string predicate;

        /// <summary>
        /// ����������������� �������� (�������).
        /// </summary>
        private filter deserializedPredicate;

        /// <summary>
        /// ����� ������ � ������� ������ ������ �������.
        /// </summary>
        private RuleDomain ruleDomain;

        /// <summary>
        /// ������������� ������ ������ � ������� ������ ������ �������.
        /// </summary>
        private long ruleDomainId;

        /// <summary>
        /// ������� �� ������������ �������: <c>false</c> - ������� �����������, <c>true</c> - ������� �� �����������.
        /// </summary>
        private bool isNotExcludedBy;

        /// <summary>
        /// ����������������� ������ �������� �������������.
        /// </summary>
        private ConditionalFactor[] deserializedConditionalFactors;

        private ApproveStatus approved;

        /// <summary>
        /// ���������� ��������������.
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// ������������� ������ ������ � ������� ������ ������ �������.
        /// </summary>
        [DataMember]
        public long RuleDomainId
        {
            get
            {
                return this.ruleDomainId;
            }

            set
            {
                this.ruleDomainId = value;
                if (this.ruleDomain != null && this.ruleDomainId != this.ruleDomain.Id)
                {
                    this.ruleDomain = null;
                }
            }
        }

        /// <summary>
        /// �������� �������/����������
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// ����� ������ � ������� ������ ������ �������.
        /// </summary>
        [IgnoreDataMember]
        public RuleDomain RuleDomain
        {
            get
            {
                return this.ruleDomain;
            }

            set
            {
                this.ruleDomain = value;
                this.ruleDomainId = this.ruleDomain != null ? this.ruleDomain.Id : default(long);
            }
        }

        /// <summary>
        /// ��� ������� <see cref="RuleTypes"/>.
        /// </summary>
        [DataMember]
        public RuleTypes Type { get; set; }

        /// <summary>
        /// ����� � ���� ������ �������� �������.
        /// </summary>
        [DataMember]
        public DateTime? DateTimeFrom { get; set; }

        /// <summary>
        /// ���� � ����� �����, ������� ������� ���������� ��� ��������.
        /// </summary>
        [DataMember]
        public DateTime? DateTimeTo { get; set; }

        /// <summary>
        /// ������ ������
        /// </summary>
        [DataMember]
        public RuleStatuses Status { get; set; }

        /// <summary>
        /// ������� ������������ �������: <c>false</c> - �� �����������, <c>true</c> - ����������� ��� �������, ����� ������� � ���������� ��������� ��� ����������� �������.
        /// � ����������� ������ ���������� ���������.
        /// </summary>
        [DataMember]
        public bool IsExclusive { get; set; }

        /// <summary>
        /// ������� �� ������������ �������: <c>false</c> - ������� �����������, <c>true</c> - ������� �� �����������.
        /// </summary>
        [DataMember]
        public bool IsNotExcludedBy
        {
            get
            {
                if (this.Type == RuleTypes.BaseMultiplication || this.Type == RuleTypes.BaseAddition)
                {
                    // NOTE: ���� ������� �������, �� ��� ������ �� ����������!!!
                    return true;
                }

                return this.isNotExcludedBy;
            }

            set
            {
                this.isNotExcludedBy = value;
            }
        }

        /// <summary>
        /// ��������� �������, ������� ��������� ������ ������ ������ ������� ���� � ������ ������ ������. 
        /// �������� ������ ���� ���������� � ������ ���� ������ ������ ������ ������, ������� ��� �������� � 
        /// � ����������� ������ ������ ���� ���������� ���������.
        /// </summary>
        [DataMember]
        public int Priority { get; set; }

        /// <summary>
        /// ������� (��������) � ������� xml.
        /// </summary>
        [DataMember]
        public string Predicate
        {
            get
            {
                return this.predicate;
            }

            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("������ �������� �������� �� ���������");
                }

                this.deserializedPredicate = null;
                this.predicate = value;
            }
        }

        /// <summary>
        /// �������� ��������.
        /// ���� ��� ������� ��������, �� �������� �������� ��� ������� ������ ���� �������� �� �������� ����� ����.
        /// ���� ��� ������� ������������������, �� �������� �������� ��� ������� ������ ���� �������� �� �������� ����� ����.
        /// ���� ��� ������� �����������, �� �������� �������� ��� ������� ������ ���� �������������� �� ��������� ����� ����.
        /// </summary>
        [DataMember]
        public decimal Factor { get; set; }

        /// <summary>
        /// �������� ������������ � ������� xml.
        /// </summary>
        [DataMember]
        public string ConditionalFactors { get; set; }

        ///// <summary>
        ///// ������ ���������� ��������� �� �����, ��. ����-���������.
        ///// </summary>
        //[DataMember]
        //public string ExternalStatusId { get; set; }

        /// <summary>
        /// ������� ������������� �������
        /// </summary>
        [DataMember]
        public ApproveStatus Approved
        {
            get
            {
                // NOTE: ������� ������� ������ ������������
                if (this.IsBase())
                {
                    return ApproveStatus.Approved;
                }

                return this.approved;
            }

            set
            {
                this.approved = value;
            }
        }

        /// <summary>
        /// ������� ������������� �������
        /// </summary>
        [DataMember]
        public string ApproveDescription { get; set; }

        public static Rule BuildDefaultRule(decimal factor, RuleTypes type, RuleDomain domain)
        {
            return new Rule
                       {
                           Id = -1,
                           Factor = factor,
                           Type = type,
                           Priority = int.MinValue,
                           Predicate = null, // NOTE: ������ true!

                           ConditionalFactors = null,
                           DateTimeFrom = null,
                           DateTimeTo = null,
                           IsExclusive = false,
                           IsNotExcludedBy = false,
                           Name = "Default " + type + " rule",
                           Status = RuleStatuses.Active,
                           RuleDomain = domain,
                           InsertedDate = DateTime.MinValue,
                           UpdatedDate = null,
                           UpdatedUserId = "system generated"
                       };
        }

        /// <summary>
        /// ���������� ����������������� ��������.
        /// </summary>
        /// <returns>
        /// ��������� ��������� (<see cref="filter"/>).
        /// </returns>
        public filter GetDeserializedPredicate()
        {
            var retVal = this.deserializedPredicate ?? (this.deserializedPredicate = this.Predicate.Deserialize<filter>());
            return retVal;
        }

        /// <summary>
        /// ������������� ��������.
        /// </summary>
        /// <param name="filter">
        /// ��������� ��������� (<see cref="filter"/>).
        /// </param>
        public void SetDeserializedPredicate(filter filter)
        {
            filter.ThrowIfNull("filter");

            this.deserializedPredicate = filter;
            this.predicate = filter.Serialize();
        }

        /// <summary>
        /// ���������� ����������������� �������� ������������.
        /// </summary>
        /// <returns>
        /// ������ �������� �������������.
        /// </returns>
        public ConditionalFactor[] GetDeserializedConditionalFactors()
        {
            return this.deserializedConditionalFactors ?? (this.deserializedConditionalFactors = this.ConditionalFactors.Deserialize<ConditionalFactor[]>());
        }

        /// <summary>
        /// ������������� ������ �������� �������������.
        /// </summary>
        /// <param name="conditionalFactors">
        /// ������ �������� �������������.
        /// </param>
        public void SetConditionalFactors(ConditionalFactor[] conditionalFactors)
        {
            this.deserializedConditionalFactors = conditionalFactors;
            this.ConditionalFactors = this.deserializedConditionalFactors.Serialize();
        }

        /// <summary>
        /// ���������� <c>true</c> ���� ������� �������� �������.
        /// </summary>
        /// <returns>
        /// <c>true</c> ���� ������� �������� �������.
        /// </returns>
        public bool IsBase()
        {
            return this.Type.IsBase();
        }

        /// <summary>
        /// ����� ������� �� �������� ����� �������� ��� ���������� � �������.
        /// </summary>
        /// <param name="historyEvent">
        /// ��� ������� ����������� ������� �������� �������� �����������.
        /// </param>
        /// <returns>
        /// �������� �����������.
        /// </returns>
        public RuleHistory ToHistoryEntity(HistoryEvent historyEvent, string userId)
        {
            return new RuleHistory(this, historyEvent);
        }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        /// <param name="validationContext">The validation context.</param>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.IsBase() && !this.IsNotExcludedBy)
            {
                // NOTE: � ���������� �� 07.12.2012 �� �������� ������������ ���� ������.
                yield return new ValidationResult("���� ������� �������, �� ��� ������ ���� �� �����������.");
            }

            if (this.DateTimeFrom.HasValue && this.DateTimeTo.HasValue && this.DateTimeFrom > this.DateTimeTo)
            {
                yield return new ValidationResult("���� ������ �������� ������� �� ����� ���� ������ ���� ���������.");
            }

            if (this.GetDeserializedConditionalFactors() != null)
            {
                if (this.GetDeserializedConditionalFactors().Any(x => x.Predicate == null))
                {
                    yield return new ValidationResult("�������� ��������� ������������ �� ����� null.");
                }

                if (this.GetDeserializedConditionalFactors().GroupBy(x => x.Priority).Any(x => x.Count() > 1))
                {
                    yield return new ValidationResult("���������� �������� ������������ ������ ��������� � ������ �������.");
                }
            }
        }

        /// <summary>
        /// ����������� ������� �� <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">
        /// �������� �� ������� ����������� �����������.
        /// </param>
        public void ResetFrom(Rule entity)
        {
            // NOTE: Approved � ApproveDescription �� �����������������, ��� ��� ��� ��������� ���� � �������� ���� ����, ���� ���� ����������.
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.RuleDomainId = entity.RuleDomainId;
            this.Priority = entity.Priority;
            this.Predicate = entity.Predicate;
            this.ConditionalFactors = entity.ConditionalFactors;
            this.DateTimeFrom = entity.DateTimeFrom;
            this.DateTimeTo = entity.DateTimeTo;
            this.Status = entity.Status;
            this.Factor = entity.Factor;
            this.IsExclusive = entity.IsExclusive;
            this.IsNotExcludedBy = entity.IsNotExcludedBy;
            this.Type = entity.Type;
            this.UpdatedUserId = entity.UpdatedUserId;
            // this.ExternalStatusId = entity.ExternalStatusId;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var countOfConditionalFactors = this.deserializedConditionalFactors == null
                                                ? "0"
                                                : this.deserializedConditionalFactors.Length.ToString(CultureInfo.InvariantCulture);

            return
                string.Format(
                    "Id: {0}, RuleDomainId: {1}, Type: {2}, Priority: {3}, Factor: {4}, IsExclusive: {5}, IsNotExcludedBy: {6}, Predicate: {7}, Count of ConditionalFactors: {8}, DateTimeFrom: {9}, DateTimeTo: {10}",
                    this.Id,
                    this.RuleDomainId,
                    this.Type,
                    this.Priority,
                    this.Factor,
                    this.IsExclusive,
                    this.IsNotExcludedBy,
                    this.Predicate,
                    countOfConditionalFactors,
                    this.DateTimeFrom.HasValue ? this.DateTimeFrom.ToString() : "null",
                    this.DateTimeTo.HasValue ? this.DateTimeFrom.ToString() : "null");
        }

        public object ToHistoryEntity(HistoryEvent historyEvent)
        {
            return this.ToHistoryEntity(historyEvent, null);
        }
    }
}