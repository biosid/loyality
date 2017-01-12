using System;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Models
{
    public class BaseActionConditionalFactorsWithTargetException : Exception
    {
        public BaseActionConditionalFactorsWithTargetException()
            : base("Условные факторы базовой механики не должны содержать целевую аудиторию")
        {
        }
    }
}
