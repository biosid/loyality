namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AdminSecurityGroupNotFoundException : AdminSecurityServiceException
    {
        public string Name { get; private set; }

        public AdminSecurityGroupNotFoundException(string name)
            : base(ResultCodes.GroupNotFound, "Группа с именем \"" + name + "\" не найдена")
        {
            Name = name;
        }
    }
}
