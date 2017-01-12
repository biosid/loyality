using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Helpers;
using Action = Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Action;

namespace Vtb24.Arms.Actions.Models
{
    public class ActionEditModel
    {
        // ReSharper disable InconsistentNaming

        public string query { get; set; }

        // ReSharper restore InconsistentNaming

        public string SaveErrorText { get; set; }

        public bool IsNewAction { get; set; }

        public long Id { get; set; }

        [Required(ErrorMessage = "Необходимо указать наименование")]
        [StringLength(256, ErrorMessage = "Превышена допустимая длина наименования (256 символов)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо указать приоритет")]
        [Range(1, 1000, ErrorMessage = "Неверное значение приоритета, возможные значения: от {1} до {2} включительно")]
        public int Priority { get; set; }

        public bool IsExclusive { get; set; }

        public bool IsNotExcludedBy { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public Statuses Status { get; set; }

        public ApproveStatuses ApproveStatus { get; set; }

        public string ApproveMessage { get; set; }

        public string Predicate { get; set; }

        public string Metadata { get; set; }

        public long MechanicId { get; set; }

        public string MechanicName { get; set; }

        public Types Type { get; set; }

        [Required(ErrorMessage = "Необходимо указать фактор")]
        public decimal Factor { get; set; }

        public ConditionalFactorModel[] ConditionalFactors { get; set; }

        public ActionsQueryModel ActionsQueryModel
        {
            get
            {
                return string.IsNullOrWhiteSpace(query)
                           ? new ActionsQueryModel()
                           : new ActionsQueryModel().MixQuery(query);
            }
        }

        public static ActionEditModel Map(Action action, Metadata[] metadata, string query)
        {
            return new ActionEditModel
            {
                query = query,
                IsNewAction = false,
                Id = action.Id,
                Name = action.Name,
                Priority = action.Priority,
                IsExclusive = action.IsExclusive,
                IsNotExcludedBy = action.IsNotExcludedBy,
                From = action.From,
                To = action.To,
                Status = action.Status.Map(),
                ApproveStatus = action.ApproveStatus.Map(),
                ApproveMessage = action.ApproveMessage,
                Predicate = PredicateHelpers.PredicateToJson(action.Predicate),
                Metadata = MetadataHelpers.ToJson(metadata),
                MechanicId = action.MechanicId,
                Type = action.Type.Map(),
                Factor = action.Factor,
                ConditionalFactors = action.ConditionalFactors.Select(ConditionalFactorModel.Map).ToArray()
            };
        }
    }
}
