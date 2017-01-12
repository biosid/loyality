using System;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Models
{
    public class BaseActionPredicateWithTargetException : Exception
    {
        public BaseActionPredicateWithTargetException()
            : base("Условие базовой механики не должно содержать целевую аудиторию")
        {
        }
    }
}
