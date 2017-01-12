namespace RapidSoft.Etl.Runtime
{
    using System;

    public static class EtlVariableAssignmentExtensions
    {
         public static bool? GetBool(this EtlVariableAssignment assignment)
         {
             if (assignment == null || assignment.Value == null)
             {
                 return null;
             }

             return bool.Parse(assignment.Value);
         }
    }
}