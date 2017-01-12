using System.ComponentModel.DataAnnotations;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Helpers;

namespace Vtb24.Arms.Actions.Models
{
    public class ConditionalFactorModel
    {
        [Required(ErrorMessage = "Необходимо указать приоритет")]
        public int Priority { get; set; }

        [Required(ErrorMessage = "Необходимо указать фактор")]
        public decimal Factor { get; set; }

        public string Predicate { get; set; }

        public static ConditionalFactorModel Map(ConditionalFactor original)
        {
            return new ConditionalFactorModel
            {
                Priority = original.Priority,
                Factor = original.Factor,
                Predicate = PredicateHelpers.PredicateToJson(original.Predicate)
            };
        }
    }
}
